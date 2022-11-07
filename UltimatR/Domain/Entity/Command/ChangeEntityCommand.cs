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
    public class ChangeEntityCommand<TStore, TEntity> : EntityCommand<TEntity> where TEntity : Entity where TStore : IDataStore
    {
        [JsonIgnore] public Delta<TEntity> Delta { get; }
        [JsonIgnore] public Func<TEntity, Expression<Func<TEntity, bool>>> Predicate { get; }

        public ChangeEntityCommand(PublishMode publishPattern, TEntity input)
            : base(input, CommandMode.Change, publishPattern)
        {            
        }
        public ChangeEntityCommand(PublishMode publishPattern, TEntity input, Func<TEntity, Expression<Func<TEntity, bool>>> predicate) 
            : base(input, CommandMode.Change, publishPattern)
        {
            Predicate = predicate;
        }
        public ChangeEntityCommand(PublishMode publishPattern, Delta<TEntity> input, Func<TEntity, Expression<Func<TEntity, bool>>> predicate) 
            : base(input.GetInstance(), CommandMode.Change, publishPattern)
        {
            Delta = input;
            Predicate = predicate;
        }
    }

    public class ChangeEntityCommandHandler<TStore, TEntity> : IRequestHandler<ChangeEntityCommand<TStore, TEntity>, TEntity> where TEntity : Entity where TStore : IDataStore
    {
        protected readonly IHostRepository<TEntity> _repository;        
        protected readonly IUltimatr _ultimatr;

        public ChangeEntityCommandHandler(IUltimatr ultimatr, IHostRepository<TStore, TEntity> repository)
        {
            _ultimatr = ultimatr;
            _repository = repository;
        }

        public Task<TEntity> Handle(ChangeEntityCommand<TStore, TEntity> request, CancellationToken cancellationToken)
        {
            return Task.Run(async () =>
            {
                if (!request.Result.IsValid)
                    return request.Data;
                try
                {                    
                    if(request.Predicate == null)
                        request.Entity = await _repository.Patch(request.Data).ConfigureAwait(false);
                    else if (request.Delta != null)
                        request.Entity = await _repository.Patch(request.Delta, request.Predicate).ConfigureAwait(false);
                    else
                        request.Entity = await _repository.Patch(request.Data, request.Predicate).ConfigureAwait(false); 

                    if (request.Entity == null) throw new Exception($"{ this.GetType().Name } for entity " +
                                                                    $"{ typeof(TEntity).Name } failed");   
                    
                    _ = _ultimatr.Publish(new ChangedEntityEvent<TStore, TEntity>(request)).ConfigureAwait(false);

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
