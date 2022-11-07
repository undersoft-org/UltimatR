using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.OData.Deltas;
using System;
using System.Linq.Expressions;
using System.Logs;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace UltimatR
{
    public class ChangedEntityEvent<TStore, TEntity>  : Event<EntityCommand<TEntity>> where TEntity : Entity where TStore : IDataStore
    {        
        [JsonIgnore] public Delta<TEntity> Delta { get; }
        [JsonIgnore] public Func<TEntity, Expression<Func<TEntity, bool>>> Predicate { get; }

        public ChangedEntityEvent(ChangeEntityCommand<TStore, TEntity> command) : base(command)
        {
            Delta = command.Delta;
            Predicate = command.Predicate;
        }
    }

    public class EntityChangedEventHandler<TStore, TEntity> : INotificationHandler<ChangedEntityEvent<TStore, TEntity> > where TEntity : Entity where TStore : IDataStore
    {
        protected readonly IHostRepository<TEntity> _repository;
        protected readonly IHostRepository<Event> _eventStore;

        public EntityChangedEventHandler(IHostRepository<IReportStore, TEntity> repository,
                                         IHostRepository<IEventStore, Event> eventStore)
        {
            _repository = repository;
            _eventStore = eventStore;
        }

        public virtual Task Handle(ChangedEntityEvent<TStore, TEntity> request, CancellationToken cancellationToken)
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
                        if (request.Delta != null)
                            result = await _repository.Patch(request.Delta, request.Predicate);
                        else
                            result = await _repository.Patch(request.Command.Data, request.Predicate);

                        if (result == null)
                            throw new Exception($"{ this.GetType().Name } " +
                                 $"for entity { typeof(TEntity).Name } unable change report");

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
