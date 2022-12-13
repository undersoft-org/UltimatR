//-----------------------------------------------------------------------
// <copyright file="LinkedRepository.cs" company="Undersoft">
//     Author: Dariusz Hanc
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using Microsoft.OData.Client;
using System;
using System.Collections.Generic;
using System.Instant;
using System.Linq;
using System.Linq.Expressions;
using System.Logs;
using System.Series;
using System.Threading;
using System.Threading.Tasks;

namespace UltimatR
{
    public class RemoteRepository<TStore, TEntity> : RemoteRepository<TEntity>, IRemoteRepository<TStore, TEntity>
        where TEntity : class, IIdentifiable
        where TStore : IDataStore
    {
        public RemoteRepository(IRepositoryContextPool<DsContext<TStore>> pool, IEntityCache<TStore, TEntity> cache) : base(
            pool.ContextPool)
        {
            mapper = cache.Mapper;
            this.cache = cache;
        }

        public override Task<int> Save(bool asTransaction, CancellationToken token = default)
        { return ContextLease.Save(asTransaction, token); }
    }
}
