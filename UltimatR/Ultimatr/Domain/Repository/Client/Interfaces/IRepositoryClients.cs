using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Series;

namespace UltimatR
{
    public interface IRepositoryClients : IDeck<IRepositoryClient>
    {
        IRepositoryClient this[DsContext context] { get; set; }
        IRepositoryClient this[string contextName] { get; set; }
        IRepositoryClient this[Type contextType] { get; set; }

        IRepositoryClient<TContext> Add<TContext>(IRepositoryClient<TContext> repoSource) where TContext : DsContext;
        IRepositoryClient Get(Type contextType);
        IRepositoryClient<TContext> Get<TContext>() where TContext : DsContext;
        ulong GetKey(IRepositoryClient item);
        IRepositoryClient New(Type contextType, Uri route);
        IRepositoryClient<TContext> New<TContext>(Uri route) where TContext : DsContext;
        IRepositoryClient<TContext> Put<TContext>(IRepositoryClient<TContext> repoSource) where TContext : DsContext;
        bool Remove<TContext>() where TContext : DsContext;
        bool TryAdd(Type contextType, IRepositoryClient repoSource);
        bool TryAdd<TContext>(IRepositoryClient<TContext> repoSource) where TContext : DsContext;
        bool TryGet(Type contextType, out IRepositoryClient repoSource);
        bool TryGet<TContext>(out IRepositoryClient<TContext> repoSource) where TContext : DsContext;
    }
}