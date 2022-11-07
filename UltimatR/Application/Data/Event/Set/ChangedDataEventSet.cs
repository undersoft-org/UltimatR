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
    public class ChangedDataEventSet<TStore, TEntity, TDto>  : EventSet<DataCommand<TDto>> 
        where TEntity : Entity where TDto : Dto where TStore : IDataStore
    {  
        [JsonIgnore] public Func<TDto, Expression<Func<TEntity, bool>>> Predicate { get;   }

        public ChangedDataEventSet(ChangeDataCommandSet<TStore, TEntity, TDto> commands) 
            : base(commands.PublishMode, commands.ForOnly(c => c.Entity != null, c => new ChangedDataEvent<TStore, TEntity, TDto>
            ((ChangeDataCommand<TStore, TEntity, TDto>)c)).ToArray())
        {
            Predicate = commands.Predicate;           
        }
    }

    public class DataChangedEventsHandler<TStore, TEntity, TDto> : INotificationHandler<ChangedDataEventSet<TStore, TEntity, TDto>>
        where TEntity : Entity where TDto : Dto where TStore : IDataStore
    {
        protected readonly IHostRepository<TEntity> _repository;
        protected readonly IHostRepository<Event> _eventStore;

        public DataChangedEventsHandler() { }
        public DataChangedEventsHandler(IHostRepository<IReportStore, TEntity> repository,
                                        IHostRepository<IEventStore, Event> eventStore)
        {
            _repository = repository;
            _eventStore = eventStore;
        }

        public virtual async Task Handle(ChangedDataEventSet<TStore, TEntity, TDto> request, CancellationToken cancellationToken)
        {
            await Task.Run(async () =>
            {
                try
                {
                    request.ForOnly(d => !d.Command.IsValid, d => { request.Remove(d); });

                    await _eventStore.AddAsync(request);

                    if (request.PublishMode == PublishMode.PropagateCommand)
                    {
                        IDeck<TEntity> entities;
                        if (request.Predicate == null)
                            entities = _repository.PatchBy(request.Select(d => d.Command.Data).ToArray()).ToDeck();
                        else
                            entities = _repository.PatchBy(request.Select(d => d.Command.Data).ToArray(), request.Predicate).ToDeck();

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
