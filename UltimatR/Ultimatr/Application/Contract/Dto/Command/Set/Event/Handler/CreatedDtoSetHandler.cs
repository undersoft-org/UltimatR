using MediatR;
using System;
using System.Linq;
using System.Logs;
using System.Threading;
using System.Threading.Tasks;

namespace UltimatR
{
    public class CreatedDtoSetHandler<TStore, TEntity, TDto> : INotificationHandler<CreatedDtoSet<TStore, TEntity, TDto>>
        where TEntity : Entity where TDto : Dto where TStore : IDataStore
    {
        protected readonly IDataRepository<TEntity> _repository;
        protected readonly IDataRepository<Event> _eventStore;

        public CreatedDtoSetHandler() { }
        public CreatedDtoSetHandler(IDataRepository<IReportStore, TEntity> repository,
                                        IDataRepository<IEventStore, Event> eventStore)
        {
            _repository = repository;
            _eventStore = eventStore;
        }

        public virtual Task Handle(CreatedDtoSet<TStore, TEntity, TDto> request, CancellationToken cancellationToken)
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
