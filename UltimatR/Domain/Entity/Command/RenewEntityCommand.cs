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
    public class RenewEntityCommand<TStore, TEntity> : EntityCommand<TEntity>, IRequest<TEntity> where TEntity : Entity where TStore : IDataStore
    {      
        [JsonIgnore] public Func<TEntity, Expression<Func<TEntity, bool>>> Predicate { get; }
        [JsonIgnore] public Func<TEntity, Expression<Func<TEntity, bool>>>[] Conditions { get; }

        public RenewEntityCommand(PublishMode publishPattern, TEntity input, Func<TEntity, Expression<Func<TEntity, bool>>> predicate) 
            : base(input, CommandMode.Renew, publishPattern)
        {
            Predicate = predicate;
        }
        public RenewEntityCommand(PublishMode publishPattern, TEntity input, Func<TEntity, Expression<Func<TEntity, bool>>> predicate, params Func<TEntity, Expression<Func<TEntity, bool>>>[] conditions)
           : base(input, CommandMode.Renew, publishPattern)
        {
            Predicate = predicate;
            Conditions = conditions;
        }
    }

    public class RenewEntityCommandHandler<TStore, TEntity> : IRequestHandler<RenewEntityCommand<TStore, TEntity>, TEntity> where TEntity : Entity where TStore : IDataStore
    {
        protected readonly IHostRepository<TEntity> _repository;        
        protected readonly IUltimateService _ultimatr;

        public RenewEntityCommandHandler(IUltimateService ultimatr, IHostRepository<TStore, TEntity> repository)
        {
            _ultimatr = ultimatr;
            _repository = repository;
        }

        public async Task<TEntity> Handle(RenewEntityCommand<TStore, TEntity> request, CancellationToken cancellationToken)
        {
            return await Task.Run(async () =>
            {
                if (!request.Result.IsValid)
                    return request.Data;

                try
                {                    
                    if (request.Conditions != null)
                        request.Entity = await _repository.Put(request.Data, request.Predicate, request.Conditions);
                    else
                        request.Entity = await _repository.Put(request.Data, request.Predicate);

                    if (request.Entity == null) throw new Exception($"{ this.GetType().Name } for entity " +
                                                                    $"{ typeof(TEntity).Name } failed");
                    
                    _ = _ultimatr.Publish(new RenewedEntityEvent<TStore, TEntity>(request)).ConfigureAwait(false); ;

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
