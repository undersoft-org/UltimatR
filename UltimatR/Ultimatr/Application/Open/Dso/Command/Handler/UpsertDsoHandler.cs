﻿using System;
using System.Logs;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation.Results;
using MediatR;

namespace UltimatR
{
    public class UpsertDsoHandler<TStore, TEntity> : IRequestHandler<UpsertDso<TStore, TEntity>, TEntity> where TEntity : Entity where TStore : IDataStore
    {
        protected readonly IDataRepository<TEntity> _repository;        
        protected readonly IUltimateService _ultimatr;

        public UpsertDsoHandler(IUltimateService ultimatr, IDataRepository<TStore, TEntity> repository)
        {
            _ultimatr = ultimatr;
            _repository = repository;
        }

        public async Task<TEntity> Handle(UpsertDso<TStore, TEntity> request, CancellationToken cancellationToken)
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
                    
                    _ = _ultimatr.Publish(new UpsertedDso<TStore, TEntity>(request)).ConfigureAwait(false); ;

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
