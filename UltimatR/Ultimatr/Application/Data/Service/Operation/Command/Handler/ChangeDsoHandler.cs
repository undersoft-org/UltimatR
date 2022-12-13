﻿using FluentValidation.Results;
using MediatR;
using System;
using System.Logs;
using System.Threading;
using System.Threading.Tasks;

namespace UltimatR
{
    public class ChangeDsoHandler<TStore, TEntity> : IRequestHandler<ChangeDso<TStore, TEntity>, TEntity> where TEntity : Entity where TStore : IDataStore
    {
        protected readonly IDataRepository<TEntity> _repository;        
        protected readonly IUltimatr _ultimatr;

        public ChangeDsoHandler(IUltimatr ultimatr, IDataRepository<TStore, TEntity> repository)
        {
            _ultimatr = ultimatr;
            _repository = repository;
        }

        public Task<TEntity> Handle(ChangeDso<TStore, TEntity> request, CancellationToken cancellationToken)
        {
            return Task.Run(async () =>
            {
                if (!request.Result.IsValid)
                    return request.Data;
                try
                {                    
                    if(request.Keys != null)
                        request.Entity = await _repository.Patch(request.Data, request.Keys).ConfigureAwait(false);
                    else if (request.Delta != null)
                        request.Entity = await _repository.Patch(request.Delta, request.Predicate).ConfigureAwait(false);
                    else
                        request.Entity = await _repository.Patch(request.Data, request.Predicate).ConfigureAwait(false); 

                    if (request.Entity == null) throw new Exception($"{ this.GetType().Name } for entity " +
                                                                    $"{ typeof(TEntity).Name } failed");   
                    
                    _ = _ultimatr.Publish(new ChangedDso<TStore, TEntity>(request)).ConfigureAwait(false);

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
