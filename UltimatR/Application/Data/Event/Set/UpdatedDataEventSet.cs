using System;
using System.Logs;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Linq;
using MediatR;
using System.Series;

namespace UltimatR
{
    public class UpdatedDataEventSet<TStore, TEntity, TDto> : EventSet<DataCommand<TDto>> 
        where TEntity : Entity where TDto : Dto where TStore : IDataStore
    {
        [JsonIgnore] public Func<TDto, Expression<Func<TEntity, bool>>> Predicate { get; }
        [JsonIgnore] public Func<TDto, Expression<Func<TEntity, bool>>>[] Conditions { get; }

        public UpdatedDataEventSet(UpdateDataCommandSet<TStore, TEntity, TDto> commands)
              : base(commands.PublishMode, commands.ForOnly(c => c.Entity != null, 
                  c => new UpdatedDataEvent<TStore, TEntity, TDto>
            ((UpdateDataCommand<TStore, TEntity, TDto>)c)).ToArray())
        {
            Predicate = commands.Predicate;
            Conditions = commands.Conditions;
        }
    }

    public class DataUpdatedEventsHandler<TStore, TEntity, TDto> 
        : INotificationHandler<UpdatedDataEventSet<TStore, TEntity, TDto> > 
        where TEntity : Entity where TDto : Dto where TStore : IDataStore
    {
        protected readonly IHostRepository<TEntity> _repository;
        protected readonly IHostRepository<Event> _eventStore;

        public DataUpdatedEventsHandler() { }
        public DataUpdatedEventsHandler(IHostRepository<IReportStore, TEntity> repository,
                                        IHostRepository<IEventStore, Event> eventStore)
        {
            _repository = repository;
            _eventStore = eventStore;
        }

        public virtual Task Handle(UpdatedDataEventSet<TStore, TEntity, TDto> request, 
                                   CancellationToken cancellationToken)
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
                        if (request.Predicate == null)
                            entities = _repository.SetBy(request.Select(d => d.Command.Data)).ToDeck();
                        else if (request.Conditions == null)
                            entities = _repository.SetBy(request.Select(d => d.Command.Data), request.Predicate).ToDeck();
                        else
                            entities = _repository.SetBy(request.Select(d => d.Command.Data), request.Predicate, request.Conditions).ToDeck();

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
