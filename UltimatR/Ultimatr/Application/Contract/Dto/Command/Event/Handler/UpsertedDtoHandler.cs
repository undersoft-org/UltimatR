using System;
using System.Logs;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation.Results;
using MediatR;

namespace UltimatR
{
    public class UpsertedDtoHandler<TStore, TEntity, TDto> : INotificationHandler<RenewedDto<TStore, TEntity, TDto> >
          where TEntity : Entity where TDto : Dto where TStore : IDataStore
    {
        protected readonly IDataRepository<TEntity> _repository;
        protected readonly IDataRepository<Event> _eventStore;

        public UpsertedDtoHandler() { }
        public UpsertedDtoHandler(IDataRepository<IReportStore, TEntity> repository,
                                       IDataRepository<IEventStore, Event> eventStore)
        {
            _repository = repository;
            _eventStore = eventStore;
        }

        public virtual Task Handle(RenewedDto<TStore, TEntity, TDto> request, CancellationToken cancellationToken)
        {
            return Task.Run(async () =>
            {
                try
                {
                    if (_eventStore.Add(request) == null)
                        throw new Exception($"{ GetType().Name } " +
                               $"for entity { typeof(TEntity).Name } unable add event");

                    if (request.Command.PublishMode == PublishMode.PropagateCommand)
                    {
                        TEntity result = null;
                        if (request.Conditions != null)
                            result = await _repository.PutBy(request.Command.Data, request.Predicate, request.Conditions);
                        else
                            result = await _repository.PutBy(request.Command.Data, request.Predicate);

                        if (result == null)
                            throw new Exception($"{ GetType().Name } " +
                                 $"for entity { typeof(TEntity).Name } unable renew report");

                        request.PublishStatus = PublishStatus.Complete;
                    }
                }
                catch (Exception ex)
                {
                    request.Command.Result.Errors.Add(new ValidationFailure(string.Empty, ex.Message));
                    this.Failure<Domainlog>(ex.Message, request.Command.ErrorMessages, ex);
                    request.PublishStatus = PublishStatus.Error;
                }
            }, cancellationToken);
        }
    }
}
