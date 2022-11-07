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
    public class DeleteEntityCommand<TStore, TEntity> : EntityCommand<TEntity> where TEntity : Entity where TStore : IDataStore
    {       
        [JsonIgnore] public Func<TEntity, Expression<Func<TEntity, bool>>> PredicateFunction { get; }
        [JsonIgnore] public Expression<Func<TEntity, bool>> PredicateExpression { get; }

        public DeleteEntityCommand(PublishMode publishPattern, TEntity input) 
            : base(input, CommandMode.Delete, publishPattern)
        {
        }

        public DeleteEntityCommand(PublishMode publishPattern, params object[] keys)
         : base(CommandMode.Delete, publishPattern, keys)
        {
        }

        public DeleteEntityCommand(PublishMode publishPattern, Expression<Func<TEntity, bool>> predicate) : base(CommandMode.Delete, publishPattern)
        {
            PredicateExpression = predicate;
        }

        public DeleteEntityCommand(PublishMode publishPattern, TEntity input, Func<TEntity, Expression<Func<TEntity, bool>>> predicate) 
            : base(input, CommandMode.Delete, publishPattern)
        {
            PredicateFunction = predicate;
        }
    }

    public class DeleteEntityCommandHandler<TStore, TEntity> : IRequestHandler<DeleteEntityCommand<TStore, TEntity>, TEntity> where TEntity : Entity where TStore : IDataStore
    {
        protected readonly IHostRepository<TEntity> _repository;        
        protected readonly IUltimatr _ultimatr;

        public DeleteEntityCommandHandler(IUltimatr ultimatr, IHostRepository<TStore, TEntity> repository)
        {
            _ultimatr = ultimatr;
            _repository = repository;            
        }

        public async Task<TEntity> Handle(DeleteEntityCommand<TStore, TEntity> request, CancellationToken cancellationToken)
        {
            return await Task.Run(async () =>
            {
                if (!request.Result.IsValid)
                    return request.Data;

                try
                {                    
                    if (request.Keys != null)
                        request.Entity = await _repository.Delete(request.Keys);
                    else if(request.Data == null)
                        request.Entity = _repository.Delete(request.PredicateExpression);
                    else
                        request.Entity = _repository.Delete(request.Data, request.PredicateFunction);

                    if (request.Entity == null) throw new Exception($"{ GetType().Name } for entity " +
                                                                    $"{ typeof(TEntity).Name } failed");

                    _ = _ultimatr.Publish(new DeletedEntityEvent<TStore, TEntity>(request)).ConfigureAwait(false); ;

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
