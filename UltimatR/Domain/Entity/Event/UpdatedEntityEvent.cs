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
    public class UpdatedEntityEvent<TStore, TEntity> : Event<EntityCommand<TEntity>> where TEntity : Entity where TStore : IDataStore
    {
        [JsonIgnore] public Func<TEntity, Expression<Func<TEntity, bool>>> Predicate { get; }
        [JsonIgnore] public Func<TEntity, Expression<Func<TEntity, bool>>>[] Conditions { get; }

        public UpdatedEntityEvent(UpdateEntityCommand<TStore, TEntity> command) : base(command)
        { 
            Predicate = command.Predicate;
            Conditions = command.Conditions;
        }
    }

    public class EntityUpdatedEventHandler<TStore, TEntity> : INotificationHandler<UpdatedEntityEvent<TStore, TEntity>> where TEntity : Entity where TStore : IDataStore
    {
        protected readonly IHostRepository<TEntity> _repository;
        protected readonly IHostRepository<Event> _eventStore;

        public EntityUpdatedEventHandler(IHostRepository<IReportStore, TEntity> repository,
                                       IHostRepository<IEventStore, Event> eventStore)
        {
            _repository = repository;
            _eventStore = eventStore;
        }

        public virtual Task Handle(UpdatedEntityEvent<TStore, TEntity> request, CancellationToken cancellationToken)
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
                        var cmd = request.Command;
                        if (request.Conditions != null)
                            cmd.Entity = await _repository.Set((TEntity)request.Command.Data, request.Predicate, request.Conditions);
                        else
                            cmd.Entity = await _repository.Set((TEntity)request.Command.Data, request.Predicate);

                        if (cmd.Entity == null)
                            throw new Exception($"{ this.GetType().Name } " +
                                 $"for entity { typeof(TEntity).Name } unable update report");

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
