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
    public class UpdatedDataEvent<TStore, TEntity, TDto>  : Event<DataCommand<TDto>> where TEntity : Entity where TDto : Dto where TStore : IDataStore
    {
        [JsonIgnore] public Func<TEntity, Expression<Func<TEntity, bool>>> Predicate { get; }

        [JsonIgnore] public Func<TEntity, Expression<Func<TEntity, bool>>>[] Conditions { get; }

        public UpdatedDataEvent(UpdateDataCommand<TStore, TEntity, TDto> command) : base(command)
        {
            Predicate = command.Predicate;
            Conditions = command.Conditions;
        }
    }

    public class DataUpdatedEventHandler<TStore, TEntity, TDto> : INotificationHandler<UpdatedDataEvent<TStore, TEntity, TDto> >
        where TEntity : Entity where TDto : Dto where TStore : IDataStore
    {
        protected readonly IHostRepository<TEntity> _repository;
        protected readonly IHostRepository<Event> _eventStore;

        public DataUpdatedEventHandler() { }
        public DataUpdatedEventHandler(IHostRepository<IReportStore, TEntity> repository,
                                       IHostRepository<IEventStore, Event> eventStore)
        {
            _repository = repository;
            _eventStore = eventStore;
        }

        public virtual Task Handle(UpdatedDataEvent<TStore, TEntity, TDto> request, CancellationToken cancellationToken)
        {
            return Task.Run(async () =>
            {
                try
                {
                    if (_eventStore.Add(request) == null)
                        throw new Exception($"{ GetType().Name } or entity " +
                            $"{ typeof(TEntity).Name } unable add event");

                    if (request.Command.PublishMode == PublishMode.PropagateCommand)
                    {
                        TEntity result;
                        if (request.Predicate == null)
                            result = await _repository.SetBy(request.Command.Data);
                        else if (request.Conditions == null)
                            result = await _repository.SetBy(request.Command.Data, request.Predicate);
                        else
                            result = await _repository.SetBy(request.Command.Data, request.Predicate, request.Conditions);

                        if (result == null) 
                            throw new Exception($"{ GetType().Name } for entity " +
                                   $"{typeof(TEntity).Name } unable update report");

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
