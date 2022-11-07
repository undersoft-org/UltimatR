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
    public class CreatedEntityEvent<TStore, TEntity> : Event<EntityCommand<TEntity>> where TEntity : Entity where TStore : IDataStore
    {
        [JsonIgnore] public Func<TEntity, Expression<Func<TEntity, bool>>> Predicate { get; }

        public CreatedEntityEvent(CreateEntityCommand<TStore, TEntity> command) : base(command)
        {
            Predicate = command.Predicate;
        }
    }

    public class EntityCreatedEventHandler<TStore, TEntity> : INotificationHandler<CreatedEntityEvent<TStore, TEntity>> where TEntity : Entity where TStore : IDataStore
    {
        protected  IHostRepository<TEntity> _repository;
        protected readonly IHostRepository<Event> _eventStore;

        public EntityCreatedEventHandler() { }
        public EntityCreatedEventHandler(IHostRepository<IReportStore, TEntity> repository,
                                       IHostRepository<IEventStore, Event> eventStore)
        {
            _repository = repository;
            _eventStore = eventStore;
        }

        public virtual Task Handle(CreatedEntityEvent<TStore, TEntity> request, CancellationToken cancellationToken)
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
                        var result = _repository.Add(request.Command.Data, request.Predicate);

                        if (result == null)
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
