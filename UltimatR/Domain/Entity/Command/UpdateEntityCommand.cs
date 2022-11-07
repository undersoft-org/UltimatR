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
    public class UpdateEntityCommand<TStore, TEntity> : EntityCommand<TEntity>, IRequest<TEntity> where TEntity : Entity where TStore : IDataStore
    {
        [JsonIgnore] public Func<TEntity, Expression<Func<TEntity, bool>>> Predicate { get; }
        [JsonIgnore] public Func<TEntity, Expression<Func<TEntity, bool>>>[] Conditions { get; }

        public UpdateEntityCommand(PublishMode publishPattern, TEntity input)
           : base(input, CommandMode.Update, publishPattern)
        {            
        }
        public UpdateEntityCommand(PublishMode publishPattern, TEntity input, Func<TEntity, Expression<Func<TEntity, bool>>> predicate)
             : base(input, CommandMode.Update, publishPattern)
        {            
            Predicate = predicate;
        }
        public UpdateEntityCommand(PublishMode publishPattern, TEntity input, Func<TEntity, Expression<Func<TEntity, bool>>> predicate, params Func<TEntity, Expression<Func<TEntity, bool>>>[] conditions)
            : base(input, CommandMode.Update, publishPattern)
        {         
            Predicate = predicate;
            Conditions = conditions;
        }
    }

    public class UpdateEntityCommandHandler<TStore, TEntity> : IRequestHandler<UpdateEntityCommand<TStore, TEntity>, TEntity> where TEntity : Entity where TStore : IDataStore
    {
        protected readonly IHostRepository<TEntity> _repository;
        protected readonly IUltimatr _ultimatr;

        public UpdateEntityCommandHandler(IUltimatr ultimatr, IHostRepository<TStore, TEntity> repository)
        {
            _ultimatr = ultimatr;
            _repository = repository;
        }

        public async Task<TEntity> Handle(UpdateEntityCommand<TStore, TEntity> request, CancellationToken cancellationToken)
        {
            return await Task.Run(async () =>
            {
                if (!request.Result.IsValid)
                    return request.Data;

                try
                {                    
                    if (request.Predicate == null)
                        request.Entity = await _repository.Set(request.Data);
                    else if (request.Conditions == null)
                        request.Entity = await _repository.Set(request.Data, request.Predicate);
                    else
                        request.Entity = await _repository.Set(request.Data, request.Predicate, request.Conditions);

                    if (request.Entity == null) throw new Exception($"{ this.GetType().Name } for entity " +
                                                $"{ typeof(TEntity).Name } failed");
                                        
                    _ = _ultimatr.Publish(new UpdatedEntityEvent<TStore, TEntity>(request)).ConfigureAwait(false); ;

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
