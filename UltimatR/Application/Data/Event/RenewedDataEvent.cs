using System;
using System.Logs;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Linq.Expressions;
using FluentValidation.Results;
using MediatR;

namespace UltimatR
{
    public class RenewedDataEvent<TStore, TEntity, TDto>  : Event<DataCommand<TDto>> where TEntity : Entity where TDto : Dto where TStore : IDataStore
    {
        [JsonIgnore] public Func<TEntity, Expression<Func<TEntity, bool>>> Predicate { get; }
        [JsonIgnore] public Func<TEntity, Expression<Func<TEntity, bool>>>[] Conditions { get; }

        public RenewedDataEvent(RenewDataCommand<TStore, TEntity, TDto> command) : base(command)
        {
            Conditions = command.Conditions;
            Predicate = command.Predicate;
        }
    }

    public class DataRenewedEventHandler<TStore, TEntity, TDto> : INotificationHandler<RenewedDataEvent<TStore, TEntity, TDto> >
          where TEntity : Entity where TDto : Dto where TStore : IDataStore
    {
        protected readonly IHostRepository<TEntity> _repository;
        protected readonly IHostRepository<Event> _eventStore;

        public DataRenewedEventHandler() { }
        public DataRenewedEventHandler(IHostRepository<IReportStore, TEntity> repository,
                                       IHostRepository<IEventStore, Event> eventStore)
        {
            _repository = repository;
            _eventStore = eventStore;
        }

        public virtual Task Handle(RenewedDataEvent<TStore, TEntity, TDto> request, CancellationToken cancellationToken)
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
