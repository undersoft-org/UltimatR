using MediatR;
using System;
using System.Text.Json;
using System.Uniques;
using System.Logs;

namespace UltimatR
{
    public abstract class CommandEvent<TCommand> : Event, INotification where TCommand : Command
    {
        public TCommand Command { get; }

        protected CommandEvent(TCommand command)
        {
            var aggregateTypeFullName = command.Entity.ProxyRetypeFullName();
            var eventTypeFullName = GetType().FullName;

            Command = command;            
            Id = (long)Unique.New;
            AggregateId = command.Id;
            AggregateType = aggregateTypeFullName;       
            EventType = eventTypeFullName;           
            var entity = (Entity)command.Entity;
            OriginId = entity.OriginId;
            OriginName = entity.OriginName;
            Modifier = entity.Modifier;
            ModificationTime = entity.ModificationTime;
            Creator = entity.Creator;
            CreationTime = entity.CreationTime;
            PublishStatus = PublishStatus.Ready;
            PublishTime = Log.Clock;          

            Data = JsonSerializer.Serialize((Command)command);
        }
    }
}