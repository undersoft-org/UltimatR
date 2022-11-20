﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Uniques;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace UltimatR
{
    public class RabbitMqEventBus : EventBus
    {
        protected RabbitMqEventBusOptions rabbitMqEventBusOptions { get; }
        protected IConnectionPool ConnectionPool { get; }
        protected IRabbitMqSerializer Serializer { get; }

        //TODO: Accessing to the List<IEventHandlerFactory> may not be thread-safe!
        protected ConcurrentDictionary<Type, List<IEventHandlerFactory>> HandlerFactories { get; }
        protected ConcurrentDictionary<string, Type> EventTypes { get; }
        protected IRabbitMqMessageConsumerFactory MessageConsumerFactory { get; }
        protected IRabbitMqMessageConsumer Consumer { get; private set; }

        private bool _exchangeCreated;

        public RabbitMqEventBus(
            IOptions<RabbitMqEventBusOptions> options,
            IConnectionPool connectionPool,
            IRabbitMqSerializer serializer,
            IServiceScopeFactory serviceScopeFactory,
            IOptions<EventBusOptions> eventBusOptions,
            IRabbitMqMessageConsumerFactory messageConsumerFactory,
            IEventHandlerInvoker eventHandlerInvoker)
            : base(eventBusOptions,
                serviceScopeFactory,      
                eventHandlerInvoker)
        {
            ConnectionPool = connectionPool;
            Serializer = serializer;
            MessageConsumerFactory = messageConsumerFactory;
            rabbitMqEventBusOptions = options.Value;

            HandlerFactories = new ConcurrentDictionary<Type, List<IEventHandlerFactory>>();
            EventTypes = new ConcurrentDictionary<string, Type>();
        }

        public void Initialize()
        {
            Consumer = MessageConsumerFactory.Create(
                new ExchangeDeclareConfiguration(
                    rabbitMqEventBusOptions.ExchangeName,
                    type: rabbitMqEventBusOptions.GetExchangeTypeOrDefault(),
                    durable: true
                ),
                new QueueDeclareConfiguration(
                    rabbitMqEventBusOptions.ClientName,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    prefetchCount: rabbitMqEventBusOptions.PrefetchCount
                ),
                rabbitMqEventBusOptions.ConnectionName
            );

            Consumer.OnMessageReceived(ProcessEventAsync);

            SubscribeHandlers(new EventBusOptions().Handlers);
        }

        private async Task ProcessEventAsync(IModel channel, BasicDeliverEventArgs ea)
        {
            var eventName = ea.RoutingKey;
            var eventType = EventTypes.GetValueOrDefault(eventName);
            if (eventType == null)
            {
                return;
            }

            var eventBytes = ea.Body.ToArray();

            var eventData = Serializer.Deserialize(eventBytes, eventType);

            await TriggerHandlersAsync(eventType, eventData);
        }

        public override IDisposable Subscribe(Type eventType, IEventHandlerFactory factory)
        {
            var handlerFactories = GetOrCreateHandlerFactories(eventType);

            if (factory.IsInFactories(handlerFactories))
            {
                return null;
            }

            handlerFactories.Add(factory);

            if (handlerFactories.Count == 1) //TODO: Multi-threading!
            {
                Consumer.BindAsync(EventNameAttribute.GetNameOrDefault(eventType));
            }

            return new EventHandlerFactoryUnregistrar(this, eventType, factory);
        }

        /// <inheritdoc/>
        public override void Unsubscribe<TEvent>(Func<TEvent, Task> action)
        {
            GetOrCreateHandlerFactories(typeof(TEvent)).RemoveAll(
                        factory =>
                        {
                            var singleInstanceFactory = factory as SingleInstanceHandlerFactory;
                            if (singleInstanceFactory == null)
                            {
                                return false;
                            }

                            var actionHandler = singleInstanceFactory.HandlerInstance as ActionEventHandler<TEvent>;
                            if (actionHandler == null)
                            {
                                return false;
                            }

                            return actionHandler.Action == action;
                        });
        }

        /// <inheritdoc/>
        public override void Unsubscribe(Type eventType, IEventHandler handler)
        {
            GetOrCreateHandlerFactories(eventType).RemoveAll(
                        factory =>
                            factory is SingleInstanceHandlerFactory &&
                            (factory as SingleInstanceHandlerFactory).HandlerInstance == handler
                    );
            ;
        }

        /// <inheritdoc/>
        public override void Unsubscribe(Type eventType, IEventHandlerFactory factory)
        {
            GetOrCreateHandlerFactories(eventType).Remove(factory);
        }

        /// <inheritdoc/>
        public override void UnsubscribeAll(Type eventType)
        {
            GetOrCreateHandlerFactories(eventType).Clear();
        }

        protected async override Task PublishToEventBusAsync(Type eventType, object eventData)
        {
            await PublishAsync(eventType, eventData, null);
        }

        protected byte[] Serialize(object eventData)
        {
            return Serializer.Serialize(eventData);
        }

        public Task PublishAsync(
            Type eventType,
            object eventData,
            IBasicProperties properties,
            Dictionary<string, object> headersArguments = null)
        {
            var eventName = EventNameAttribute.GetNameOrDefault(eventType);
            var body = Serializer.Serialize(eventData);

            return PublishAsync(eventName, body, properties, headersArguments);
        }

        protected virtual Task PublishAsync(
            string eventName,
            byte[] body,
            IBasicProperties properties,
            Dictionary<string, object> headersArguments = null,
            ulong? eventId = null)
        {
            using (var channel = ConnectionPool.Get(rabbitMqEventBusOptions.ConnectionName).CreateModel())
            {
                return PublishAsync(channel, eventName, body, properties, headersArguments, eventId);
            }
        }

        protected virtual Task PublishAsync(
            IModel channel,
            string eventName,
            byte[] body,
            IBasicProperties properties,
            Dictionary<string, object> headersArguments = null,
            ulong? eventId = null)
        {
            EnsureExchangeExists(channel);

            if (properties == null)
            {
                properties = channel.CreateBasicProperties();
                properties.DeliveryMode = RabbitMqConsts.DeliveryModes.Persistent;
            }

            if (properties.MessageId == null)
            {
                properties.MessageId = (eventId ?? Unique.New).ToString("N");
            }

            SetEventMessageHeaders(properties, headersArguments);

            channel.BasicPublish(
                exchange: rabbitMqEventBusOptions.ExchangeName,
                routingKey: eventName,
                mandatory: true,
                basicProperties: properties,
                body: body
            );

            return Task.CompletedTask;
        }

        private void EnsureExchangeExists(IModel channel)
        {
            if (_exchangeCreated)
            {
                return;
            }

            try
            {
                channel.ExchangeDeclarePassive(rabbitMqEventBusOptions.ExchangeName);
            }
            catch (Exception)
            {
                channel.ExchangeDeclare(
                    rabbitMqEventBusOptions.ExchangeName,
                    rabbitMqEventBusOptions.GetExchangeTypeOrDefault(),
                    durable: true
                );
            }
            _exchangeCreated = true;
        }

        private void SetEventMessageHeaders(IBasicProperties properties, Dictionary<string, object> headersArguments)
        {
            if (headersArguments == null)
            {
                return;
            }

            properties.Headers ??= new Dictionary<string, object>();

            foreach (var header in headersArguments)
            {
                properties.Headers[header.Key] = header.Value;
            }
        }

        private List<IEventHandlerFactory> GetOrCreateHandlerFactories(Type eventType)
        {
            return HandlerFactories.GetOrAdd(
                eventType,
                type =>
                {
                    var eventName = EventNameAttribute.GetNameOrDefault(type);
                    EventTypes[eventName] = type;
                    return new List<IEventHandlerFactory>();
                }
            );
        }

        protected override IEnumerable<EventTypeWithEventHandlerFactories> GetHandlerFactories(Type eventType)
        {
            var handlerFactoryList = new List<EventTypeWithEventHandlerFactories>();

            foreach (var handlerFactory in
                     HandlerFactories.Where(hf => ShouldTriggerEventForHandler(eventType, hf.Key)))
            {
                handlerFactoryList.Add(
                    new EventTypeWithEventHandlerFactories(handlerFactory.Key, handlerFactory.Value));
            }

            return handlerFactoryList.ToArray();
        }

        private static bool ShouldTriggerEventForHandler(Type targetEventType, Type handlerEventType)
        {
            //Should trigger same type
            if (handlerEventType == targetEventType)
            {
                return true;
            }

            //TODO: Support inheritance? But it does not support on subscription to RabbitMq!
            //Should trigger for inherited types
            if (handlerEventType.IsAssignableFrom(targetEventType))
            {
                return true;
            }

            return false;
        }
    }
}
