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
    public class DeletedDataEventSet<TStore, TEntity, TDto>  : EventSet<DataCommand<TDto>> 
        where TEntity : Entity where TDto : Dto where TStore : IDataStore
    {  
        [JsonIgnore] public Func<TEntity, Expression<Func<TEntity, bool>>> Predicate { get; }

        public DeletedDataEventSet(DeleteDataCommandSet<TStore, TEntity, TDto> commands) 
            : base(commands.PublishMode, commands.ForOnly(c => c.Entity != null, c => new DeletedDataEvent<TStore, TEntity, TDto>
                                                        ((DeleteDataCommand<TStore, TEntity, TDto>)c)).ToArray())
        {
            Predicate = commands.Predicate;           
        }
    }

    public class DataDeletedEventsHandler<TStore, TEntity, TDto> : INotificationHandler<DeletedDataEventSet<TStore, TEntity, TDto>>
        where TEntity : Entity where TDto : Dto where TStore : IDataStore
    {
        protected readonly IHostRepository<TEntity> _repository;
        protected readonly IHostRepository<Event> _eventStore;

        public DataDeletedEventsHandler() { }
        public DataDeletedEventsHandler(IHostRepository<IReportStore, TEntity> repository,
                                        IHostRepository<IEventStore, Event> eventStore)
        {
            _repository = repository;
            _eventStore = eventStore;
        }

        public virtual Task Handle(DeletedDataEventSet<TStore, TEntity, TDto> request, CancellationToken cancellationToken)
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
                            entities = _repository.Delete(request.Select(d => (TEntity)d.Command.Entity)).ToDeck();
                        else
                            entities = _repository.Delete(request.Select(d => (TEntity)d.Command.Entity), request.Predicate).ToDeck();

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
