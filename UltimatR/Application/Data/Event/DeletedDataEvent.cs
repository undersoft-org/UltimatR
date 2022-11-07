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
    public class DeletedDataEvent<TStore, TEntity, TDto>  : Event<DataCommand<TDto>> where TEntity : Entity where TDto : Dto where TStore : IDataStore
    {
        [JsonIgnore] public Func<TEntity, Expression<Func<TEntity, bool>>> Predicate { get; }

        public DeletedDataEvent(DeleteDataCommand<TStore, TEntity, TDto> command) : base(command)
        {
            Predicate = command.Predicate;
        }
    }

    public class DataDeletedEventHandler<TStore, TEntity, TDto> : INotificationHandler<DeletedDataEvent<TStore, TEntity, TDto> >
        where TEntity : Entity where TDto : Dto where TStore : IDataStore
    {
        protected readonly IHostRepository<TEntity> _repository;
        protected readonly IHostRepository<Event> _eventStore;

        public DataDeletedEventHandler() { }
        public DataDeletedEventHandler(IHostRepository<IReportStore, TEntity> repository,
                                       IHostRepository<IEventStore, Event> eventStore)
        {
            _repository = repository;
            _eventStore = eventStore;
        }

        public virtual Task Handle(DeletedDataEvent<TStore, TEntity, TDto>  request, CancellationToken cancellationToken)
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
                    if(request.Command.Keys != null)
                        result = await _repository.Delete(request.Command.Keys);
                    else if(request.Data == null && request.Predicate != null)
                        result = await _repository.Delete(request.Predicate);
                    else
                        result = await _repository.DeleteBy(request.Data, request.Predicate);                   

                    if(result == null)
                        throw new Exception($"{ this.GetType().Name } " +
                             $"for entity { typeof(TEntity).Name } unable delete report");

                    request.PublishStatus = PublishStatus.Complete;
                }
            }
            catch(Exception ex)
            {
                request.Command.Result.Errors.Add(new ValidationFailure(string.Empty, ex.Message));
                this.Failure<Domainlog>(ex.Message, request.Command.ErrorMessages, ex);
                request.PublishStatus = PublishStatus.Error;
            }
            }, cancellationToken);
        }
    }
}
