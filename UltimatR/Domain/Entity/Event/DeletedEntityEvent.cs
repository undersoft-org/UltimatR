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
    public class DeletedEntityEvent<TStore, TEntity> : Event<EntityCommand<TEntity>> where TEntity : Entity where TStore : IDataStore
    {
        [JsonIgnore] public Func<TEntity, Expression<Func<TEntity, bool>>> PredicateFunction { get; }
        [JsonIgnore] public Expression<Func<TEntity, bool>> PredicateExpression { get; }

        public DeletedEntityEvent(DeleteEntityCommand<TStore, TEntity> command) : base(command)
        {
            PredicateExpression = command.PredicateExpression;
            PredicateFunction = command.PredicateFunction;
        }
    }

    public class EntityDeletedEventHandler<TStore, TEntity> : INotificationHandler<DeletedEntityEvent<TStore, TEntity>> where TEntity : Entity where TStore : IDataStore
    { 
        protected readonly IHostRepository<TEntity> _repository;
        protected readonly IHostRepository<Event> _eventStore;

        public EntityDeletedEventHandler(IHostRepository<IReportStore, TEntity> repository,
                                       IHostRepository<IEventStore, Event> eventStore)
        {
            _repository = repository;
            _eventStore = eventStore;
        }

        public virtual Task Handle(DeletedEntityEvent<TStore, TEntity> request, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                try
                {
                    if (_eventStore.Add(request) == null)
                        throw new Exception($"{ GetType().Name } " +
                               $"for entity { typeof(TEntity).Name } unable add event");

                    var cmd = request.Command;
                    if (cmd.PublishMode == PublishMode.PropagateCommand)
                    {                     
                        if (cmd.Data == null)
                            cmd.Entity = _repository.Delete(request.PredicateExpression);
                        else
                            cmd.Entity = _repository.Delete((TEntity)cmd.Data, request.PredicateFunction);

                        if (cmd.Entity == null)
                            throw new Exception($"{ this.GetType().Name } " +
                                 $"for entity { typeof(TEntity).Name } unable delete report");

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
