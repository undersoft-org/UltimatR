using System;
using System.Linq.Expressions;
using System.Logs;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation.Results;
using MediatR;

namespace UltimatR
{
    public class ChangedDataEvent<TStore, TEntity, TDto>  : Event<DataCommand<TDto>> 
        where TEntity : Entity where TDto : Dto where TStore : IDataStore
    {  
        [JsonIgnore] public Func<TDto, Expression<Func<TEntity, bool>>> Predicate { get;  }

        public ChangedDataEvent(DataCommand<TDto> command) : base(command) { }

        public ChangedDataEvent(ChangeDataCommand<TStore, TEntity,  TDto> command) : base(command)
        {
            Predicate = command.Predicate;
        }
    }

    public class DataChangedEventHandler<TStore, TEntity, TCommand> : INotificationHandler<ChangedDataEvent<TStore, TEntity, TCommand>>
        where TEntity : Entity where TCommand : Dto where TStore : IDataStore
    {
        protected readonly IHostRepository<TEntity> _repository;
        protected readonly IHostRepository<Event> _eventStore;

        public DataChangedEventHandler() { }
        public DataChangedEventHandler(IHostRepository<IReportStore, TEntity> repository,
                                       IHostRepository<IEventStore, Event> eventStore)
        {
            _repository = repository;
            _eventStore = eventStore;
        }

        public virtual Task Handle(ChangedDataEvent<TStore, TEntity, TCommand> request, CancellationToken cancellationToken)
        {
            return Task.Run(async () =>
            {
                try
                {                  
                    if (_eventStore.Add(request) == null) 
                        throw new Exception($"{ GetType().Name } for entity " +
                              $"{ typeof(TEntity).Name } unable add event");

                    if (request.Command.PublishMode == PublishMode.PropagateCommand)
                    {
                        TEntity entity;
                        if (request.Command.Keys != null)
                            entity = await _repository.PatchBy(request.Command.Data, request.Command.Keys);
                        else
                            entity = await _repository.PatchBy(request.Command.Data, request.Predicate);

                        if (entity == null)
                            throw new Exception($"{ GetType().Name } for entity " +
                                  $"{ typeof(TEntity).Name } unable change report");

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
