using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson.IO;
using MongoDB.EntityFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Series;
using System.Uniques;

namespace UltimatR
{
    public class RepositoryClients : Catalog<IRepositoryClient>, IRepositoryClients
    {
        public RepositoryClients() : base(false, 17) { }

        public IRepositoryClient this[string contextName]
        {
            get => base[contextName.UniqueKey64()];
            set => base.Set(contextName.UniqueKey64(), value);
        }
        public IRepositoryClient this[DsContext context]
        {
            get => base[context.GetType()];
            set => base.Set(context.GetType(), value);
        }
        public IRepositoryClient this[Type contextType]
        {
            get => base[contextType];
            set => base.Set(contextType, value);
        }

        public IRepositoryClient Get(Type contextType)
        {
            return base[contextType];
        }
        public IRepositoryClient<TContext> Get<TContext>() where TContext : DsContext
        {
            return (IRepositoryClient<TContext>)base[typeof(TContext)];
        }

        public bool TryGet(Type contextType, out IRepositoryClient repoSource)
        {
            return base.TryGet(contextType, out repoSource);
        }
        public bool TryGet<TContext>(out IRepositoryClient<TContext> repoSource) where TContext : DsContext
        {
            if (!TryGet(typeof(TContext), out IRepositoryClient _repo))
            {
                repoSource = (IRepositoryClient<TContext>)_repo;
                return false;
            }
            repoSource = null;
            return false;
        }

        public bool TryAdd(Type contextType, IRepositoryClient repoSource)
        {
            return base.Add(contextType, repoSource);
        }
        public override bool TryAdd(IRepositoryClient repoSource)
        {
            return base.Add(repoSource.ContextType, repoSource);
        }
        public bool TryAdd<TContext>(IRepositoryClient<TContext> repoSource) where TContext : DsContext
        {
            return base.Add(typeof(TContext), repoSource);
        }

        public IRepositoryClient<TContext> Add<TContext>(IRepositoryClient<TContext> repoSource) where TContext : DsContext
        {
            return (IRepositoryClient<TContext>)base.Put(typeof(TContext), repoSource).Value;
        }
        public override void Add(IRepositoryClient repoSource)
        {
            base.Put(repoSource.ContextType, repoSource);
        }

        public bool Remove<TContext>() where TContext : DsContext
        {
            return TryRemove(typeof(TContext));
        }

        public int PoolCount(Type contextType)
        {
            return  Get(contextType).Count;
        }
        public int PoolCount<TContext>() where TContext : DsContext
        {
            return Get<TContext>().Count;
        }

        public IRepositoryClient<TContext> Put<TContext>(IRepositoryClient<TContext> repoSource)
            where TContext : DsContext
        {
            return (IRepositoryClient<TContext>)base.Put(typeof(TContext), repoSource).Value;
        }

        public IRepositoryClient<TContext> New<TContext>(Uri route) where TContext : DsContext
        {
            return Add(new RepositoryClient<TContext>(route));
        }
        public IRepositoryClient New(Type contextType, Uri route)
        {
            return Put(new RepositoryClient(contextType, route)).Value;
        }

        public ulong GetKey(IRepositoryClient item)
        {
            return item.UniqueKey;
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    base.Dispose(true);
                }
                disposedValue = true;
            }
        }
    }
}
