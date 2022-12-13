﻿//-----------------------------------------------------------------------
// <copyright file="CreateDsoCommand.cs" company="Undersoft">
//     Author: Dariusz Hanc
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using FluentValidation.Results;
using MediatR;
using System;
using System.Logs;
using System.Threading;
using System.Threading.Tasks;

namespace UltimatR
{
    public class CreateDsoHandler<TStore, TEntity> : IRequestHandler<CreateDso<TStore, TEntity>, TEntity>
        where TEntity : Entity
        where TStore : IDataStore
    {
        protected readonly IDataRepository<TEntity> _repository;
        protected readonly IUltimatr _ultimatr;

        public CreateDsoHandler(IUltimatr ultimatr, IDataRepository<TStore, TEntity> repository)
        {
            _ultimatr = ultimatr;
            _repository = repository;
        }

        public Task<TEntity> Handle(CreateDso<TStore, TEntity> request, CancellationToken cancellationToken)
        {
            return Task.Run(
                () =>
                {
                    if(!request.Result.IsValid)
                        return request.Data;

                    try
                    {
                        request.Entity = _repository.Add(request.Data, request.Predicate);

                        if(request.Entity == null)
                            throw new Exception(
                                $"{($"{GetType().Name} for entity ")}{($"{typeof(TEntity).Name} unable create entry")}");

                        _ = _ultimatr.Publish(new CreatedDso<TStore, TEntity>(request)).ConfigureAwait(false);

                        return request.Entity as TEntity;
                    } catch(Exception ex)
                    {
                        request.Result.Errors.Add(new ValidationFailure(string.Empty, ex.Message));
                        this.Failure<Applog>(ex.Message, request.ErrorMessages, ex);
                    }

                    return null;
                },
                cancellationToken);
        }
    }
}
