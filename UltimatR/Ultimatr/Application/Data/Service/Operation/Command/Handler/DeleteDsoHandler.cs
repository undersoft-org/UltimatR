using System;
using System.Logs;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation.Results;
using MediatR;

namespace UltimatR
{
    public class DeleteDsoHandler<TStore, TEntity> : IRequestHandler<DeleteDso<TStore, TEntity>, TEntity> where TEntity : Entity where TStore : IDataStore
    {
        protected readonly IDataRepository<TEntity> _repository;        
        protected readonly IUltimatr _ultimatr;

        public DeleteDsoHandler(IUltimatr ultimatr, IDataRepository<TStore, TEntity> repository)
        {
            _ultimatr = ultimatr;
            _repository = repository;            
        }

        public async Task<TEntity> Handle(DeleteDso<TStore, TEntity> request, CancellationToken cancellationToken)
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

                    _ = _ultimatr.Publish(new DeletedDso<TStore, TEntity>(request)).ConfigureAwait(false); ;

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
