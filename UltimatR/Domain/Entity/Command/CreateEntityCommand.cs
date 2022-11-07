using FluentValidation.Results;
using MediatR;
using System;
using System.Linq.Expressions;
using System.Logs;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace UltimatR
{
    public class CreateEntityCommand<TStore, TEntity> : EntityCommand<TEntity>, IRequest<TEntity> where TEntity : Entity where TStore : IDataStore
    {      
        [JsonIgnore] public Func<TEntity,  Expression<Func<TEntity, bool>>> Predicate { get; }

        public CreateEntityCommand(PublishMode publishPattern, TEntity input) 
            : base(input, CommandMode.Create, publishPattern)
        {
            input.AutoId();
        }
        public CreateEntityCommand(PublishMode publishPattern, TEntity input, Func<TEntity, Expression<Func<TEntity, bool>>> predicate) 
            : base(input, CommandMode.Create, publishPattern)
        {
            input.AutoId();
            Predicate = predicate;
        }
    }

    public class CreateEntityCommandHandler<TStore, TEntity> : IRequestHandler<CreateEntityCommand<TStore, TEntity>, TEntity> where TEntity : Entity where TStore : IDataStore
    {
        protected readonly IHostRepository<TEntity> _repository;        
        protected readonly IUltimatr _ultimatr;

        public CreateEntityCommandHandler(IUltimatr ultimatr, IHostRepository<TStore, TEntity> repository)
        {
            _ultimatr = ultimatr;
            _repository = repository;
        }

        public Task<TEntity> Handle(CreateEntityCommand<TStore, TEntity> request, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                if (!request.Result.IsValid)
                    return request.Data;

                try
                {
                    request.Entity = _repository.Add(request.Data, request.Predicate);

                    if (request.Entity == null) throw new Exception($"{ GetType().Name } for entity " +
                                                                    $"{ typeof(TEntity).Name } unable create entry");

                    _ = _ultimatr.Publish(new CreatedEntityEvent<TStore, TEntity>(request)).ConfigureAwait(false);

                    return request.Entity as TEntity;
                }
                catch (Exception ex)
                {
                    request.Result.Errors.Add(new ValidationFailure(string.Empty, ex.Message));
                    this.Failure<Applog>(ex.Message, request.ErrorMessages, ex);
                }

                return null;
            }, cancellationToken);
        }
    }
}
