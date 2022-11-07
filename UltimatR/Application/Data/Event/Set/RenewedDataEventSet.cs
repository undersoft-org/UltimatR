using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Logs;
using System.Series;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace UltimatR
{
    public class RenewedDataEventSet<TStore, TEntity, TDto>  : EventSet<DataCommand<TDto>> 
        where TEntity : Entity where TDto : Dto where TStore : IDataStore
    {
        [JsonIgnore] public Func<TEntity, Expression<Func<TEntity, bool>>> Predicate { get; }
        [JsonIgnore] public Func<TEntity, Expression<Func<TEntity, bool>>>[] Conditions { get; }

        public RenewedDataEventSet(RenewDataCommandSet<TStore, TEntity, TDto> commands) 
            : base(commands.PublishMode, commands.ForOnly(c => c.Entity != null, c => new RenewedDataEvent<TStore, TEntity, TDto>
            ((RenewDataCommand<TStore, TEntity, TDto>)c)).ToArray())
        {            
            Conditions = commands.Conditions;
            Predicate = commands.Predicate;
        }
    }

    public class DataRenewedEventsHandler<TStore, TEntity, TDto> : INotificationHandler<RenewedDataEventSet<TStore, TEntity, TDto>>
        where TEntity : Entity where TDto : Dto where TStore : IDataStore
    {
        protected readonly IHostRepository<TEntity> _repository;
        protected readonly IHostRepository<Event> _eventStore;

        public DataRenewedEventsHandler() { }
        public DataRenewedEventsHandler(IHostRepository<IReportStore, TEntity> repository,
                                        IHostRepository<IEventStore, Event> eventStore)
        {
            _repository = repository;
            _eventStore = eventStore;
        }

        public virtual Task Handle(RenewedDataEventSet<TStore, TEntity, TDto> request, CancellationToken cancellationToken)
        {
            return Task.Run(async () =>
            {
                try
                {
                    request.ForOnly(d => !d.Command.IsValid, d => { request.Remove(d); });

                    await _eventStore.AddAsync(request);

                    if (request.PublishMode == PublishMode.PropagateCommand)
                    {                     
                        IDeck<TEntity> entities;
                        if (request.Conditions != null)
                            entities = _repository.PutBy(request.Select(d => d.Command.Data), request.Predicate, request.Conditions).ToDeck();
                        else
                            entities = _repository.PutBy(request.Select(d => d.Command.Data), request.Predicate).ToDeck();

                        request.ForEach((r) =>
                        {
                            _ = (entities.ContainsKey(r.AggregateId))
                              ? r.PublishStatus = PublishStatus.Complete
                              : r.PublishStatus = PublishStatus.Uncomplete;
                        });
                    }
                }
                catch (Exception ex)
                {
                    this.Failure<Domainlog>(ex.Message, request.Select(r => r.Command.ErrorMessages).ToArray(), ex);
                    request.ForEach((r) => r.PublishStatus = PublishStatus.Error);
                }
            }, cancellationToken);
        }
    }
}
