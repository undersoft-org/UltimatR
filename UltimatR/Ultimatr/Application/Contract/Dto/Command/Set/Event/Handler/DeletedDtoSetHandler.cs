using MediatR;
using System;
using System.Linq;
using System.Logs;
using System.Series;
using System.Threading;
using System.Threading.Tasks;

namespace UltimatR
{
    public class DeletedDtoSetHandler<TStore, TEntity, TDto> : INotificationHandler<DeletedDtoSet<TStore, TEntity, TDto>>
        where TEntity : Entity where TDto : Dto where TStore : IDataStore
    {
        protected readonly IDataRepository<TEntity> _repository;
        protected readonly IDataRepository<Event> _eventStore;

        public DeletedDtoSetHandler() { }
        public DeletedDtoSetHandler(IDataRepository<IReportStore, TEntity> repository,
                                        IDataRepository<IEventStore, Event> eventStore)
        {
            _repository = repository;
            _eventStore = eventStore;
        }

        public virtual Task Handle(DeletedDtoSet<TStore, TEntity, TDto> request, CancellationToken cancellationToken)
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
