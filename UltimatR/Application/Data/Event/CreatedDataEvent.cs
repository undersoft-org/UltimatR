using System;
using System.Logs;
using System.Text.Json.Serialization;
using System.Threading;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FluentValidation.Results;
using MediatR;

namespace UltimatR
{
    public class CreatedDataEvent<TStore, TEntity, TDto>  : Event<DataCommand<TDto>> where TEntity : Entity where TDto : Dto where TStore : IDataStore
    {
        [JsonIgnore] public Func<TEntity, Expression<Func<TEntity, bool>>> Predicate { get; }

        public CreatedDataEvent(CreateDataCommand<TStore, TEntity, TDto> command) : base(command)
        {
            Predicate = command.Predicate;
        }
    }

    public class DataCreatedEventHandler<TStore, TEntity, TDto> : INotificationHandler<CreatedDataEvent<TStore, TEntity, TDto> >
        where TEntity : Entity where TDto : Dto where TStore : IDataStore
    {
        protected readonly IHostRepository<TEntity> _repository;
        protected readonly IHostRepository<Event> _eventStore;

        public DataCreatedEventHandler() { }
        public DataCreatedEventHandler(IHostRepository<IReportStore, TEntity> repository,
                                       IHostRepository<IEventStore, Event> eventStore)
        {
            _repository = repository;
            _eventStore = eventStore;
        }

        public virtual Task Handle(CreatedDataEvent<TStore, TEntity, TDto>  request, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                try
                {
                    if (_eventStore.Add(request) == null)
                        throw new Exception($"{ GetType().Name } " +
                               $"for entity { typeof(TEntity).Name } unable add event");

                    if (request.Command.PublishMode == PublishMode.PropagateCommand)
                    {
                        if (_repository.Add((TEntity)request.Command.Entity, request.Predicate) == null)
                            throw new Exception($"{ this.GetType().Name } " +
                                 $"for entity { typeof(TEntity).Name } unable create report");

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
