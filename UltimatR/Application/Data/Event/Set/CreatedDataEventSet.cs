using MediatR;
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
    public class CreatedDataEventSet<TStore, TEntity, TDto>  : EventSet<DataCommand<TDto>> 
        where TEntity : Entity where TDto : Dto where TStore : IDataStore
    {  
        [JsonIgnore] public Func<TEntity, Expression<Func<TEntity, bool>>> Predicate { get; }

        public CreatedDataEventSet(CreateDataCommandSet<TStore, TEntity, TDto> commands) 
            : base(commands.PublishMode, commands.ForOnly(c => c.Entity != null, c => new CreatedDataEvent<TStore, TEntity, TDto>
            ((CreateDataCommand<TStore, TEntity, TDto>)c)).ToArray())
        {
            Predicate = commands.Predicate;           
        }
    }

    public class DataCreatedEventsHandler<TStore, TEntity, TDto> : INotificationHandler<CreatedDataEventSet<TStore, TEntity, TDto>>
        where TEntity : Entity where TDto : Dto where TStore : IDataStore
    {
        protected readonly IHostRepository<TEntity> _repository;
        protected readonly IHostRepository<Event> _eventStore;

        public DataCreatedEventsHandler() { }
        public DataCreatedEventsHandler(IHostRepository<IReportStore, TEntity> repository,
                                        IHostRepository<IEventStore, Event> eventStore)
        {
            _repository = repository;
            _eventStore = eventStore;
        }

        public virtual Task Handle(CreatedDataEventSet<TStore, TEntity, TDto> request, CancellationToken cancellationToken)
        {
            return Task.Run(async () =>
            {
                try
                {
                    request.ForOnly(d => !d.Command.IsValid, d => { request.Remove(d); });

                    await _eventStore.AddAsync(request);

                    if (request.PublishMode == PublishMode.PropagateCommand)
                    {
                        var entities = _repository.Add(request.Select(d => d.Command.Entity).Cast<TEntity>(), request.Predicate).ToDeck();

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
