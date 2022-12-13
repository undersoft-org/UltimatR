//-----------------------------------------------------------------------
// <copyright file="RepositoryEndpoint.cs" company="Undersoft">
//     Author: Dariusz Hanc
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Linq;
using System.Series;
using System.Uniques;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace UltimatR
{
    public class RepositoryEndpoint : Board<IRepositoryContext>, IRepositoryEndpoint
    {
        protected DbContextConfigurationSnapshot _configurationSnapshot;
        protected Type contextType;
        protected new bool disposedValue;
        protected DbContextOptionsBuilder optionsBuilder;
        protected Ussn servicecode;
        const int WAIT_PUBLISH_TIMEOUT = 30 * 1000;
        ManualResetEventSlim _access = new ManualResetEventSlim(true, 128);
        SemaphoreSlim _pass = new SemaphoreSlim(1);

        public RepositoryEndpoint() { Site = DataSite.Endpoint; }

        public RepositoryEndpoint(DbContextOptions options) : this()
        {
            ContextPool = this;
            InnerContext = CreateContext(options);
            Context.GetDbEntities();
        }

        public RepositoryEndpoint(IRepositoryContextPool pool) : this()
        {
            PoolSize = pool.PoolSize;
            ContextPool = pool;
            InnerContext = pool.CreateContext();
        }

        public RepositoryEndpoint(EndpointProvider provider, string connectionString) : this()
        {
            ContextPool = this;
            optionsBuilder = RepositoryEndpointOptions.BuildOptions(provider, connectionString);
            InnerContext = CreateContext(optionsBuilder.Options);
            Context.GetDbEntities();
        }

        public RepositoryEndpoint(Type contextType, IServiceConfiguration config) : this()
        {
            IConfigurationSection endpoint = config.Endpoint(contextType.FullName);
            string connectionString = config.EndpointConnectionString(contextType.FullName);
            EndpointProvider provider = config.EndpointProvider(contextType.FullName);
            ContextPool = this;
            PoolSize = config.EndpointPoolSize(endpoint);
            optionsBuilder = RepositoryEndpointOptions.BuildOptions(provider, connectionString);
            InnerContext = CreateContext(optionsBuilder.Options);
            Context.GetDbEntities();
        }

        public RepositoryEndpoint(Type contextType, DbContextOptions options) : this()
        {
            ContextPool = this;
            InnerContext = CreateContext(contextType, options);
            Context.GetDbEntities();
        }

        public RepositoryEndpoint(Type contextType, EndpointProvider provider, string connectionString) : this()
        {
            ContextPool = this;
            optionsBuilder = RepositoryEndpointOptions.BuildOptions(provider, connectionString);
            InnerContext = CreateContext(contextType, optionsBuilder.Options);
            Context.GetDbEntities();
        }

        public virtual IDataContext Context => (IDataContext)InnerContext;

        public IRepositoryContext ContextLease { get; set; }

        public IRepositoryContextPool ContextPool { get; set; }

        public Type ContextType => contextType ??= InnerContext.GetType();

        public object InnerContext { get; set; }

        public bool Leased => ContextLease != null;

        public virtual DbContextOptions Options { get; protected set; }

        public bool Pooled => ContextPool != null;

        public int PoolSize { get; set; }

        public DataSite Site { get; set; }

        public override ulong UniqueKey
        {
            get => (servicecode.UniqueKey == 0) ? (servicecode.UniqueKey = Unique.New) : servicecode.UniqueKey;
            set => servicecode.UniqueKey = value;
        }

        public override ulong UniqueSeed
        {
            get => (servicecode.UniqueSeed == 0)
                ? (servicecode.UniqueSeed = ContextType.ProxyRetypeKey32())
                : servicecode.UniqueSeed;
            set => servicecode.UniqueSeed = value;
        }

        public void AcquireAccess()
        {
            do
            {
                if(!_access.Wait(WAIT_PUBLISH_TIMEOUT))
                {
                    throw new TimeoutException("Wait write Timeout");
                }

                _access.Reset();
            } while (!_pass.Wait(0));
        }

        public virtual object CreateContext() { return ContextType.New(Options); }

        public virtual TContext CreateContext<TContext>() where TContext : class
        {
            contextType ??= typeof(TContext);
            return typeof(TContext).New<TContext>(Options);
        }

        public virtual IDataContext CreateContext(DbContextOptions options)
        {
            contextType ??= options.ContextType;
            Options ??= options;
            return (IDataContext)ContextType.New(options);
        }

        public virtual TContext CreateContext<TContext>(DbContextOptions options) where TContext : DbContext
        {
            Options ??= options;
            contextType ??= typeof(TContext);
            return typeof(TContext).New<TContext>(options);
        }

        public virtual IDataContext CreateContext(Type contextType, DbContextOptions options)
        {
            this.contextType ??= contextType;
            Options ??= options;
            return (IDataContext)ContextType.New(options);
        }

        public void CreatePool()
        {
            SnapshotConfiguration();
            Type repotype = typeof(RepositoryEndpoint<>)
                            .MakeGenericType(ContextType);
            int size = PoolSize - Count;
            for(int i = 0; i < size; i++)
            {
                IRepositoryContext repo = repotype.New<IRepositoryContext>(this);
                Add(repo);
            }
        }

        public void CreatePool<TContext>()
        {
            SnapshotConfiguration();
            Type repotype = typeof(RepositoryEndpoint<>)
                            .MakeGenericType(typeof(TContext));
            int size = PoolSize - Count;
            for(int i = 0; i < size; i++)
            {
                IRepositoryContext repo = repotype.New<IRepositoryContext>(this);
                Add(repo);
            }
        }

        public TModel CreateServiceModel<TModel>() { return Context.CreateServiceModel<TModel>(); }
        public new ValueTask DisposeAsync() { return new ValueTask(Task.Run(() => Dispose(true))); }
        public TContext GetContext<TContext>() where TContext : DbContext { return (TContext)Context; }
        public TModel GetServiceModel<TModel>() { return Context.GetServiceModel<TModel>(); }

        public virtual void Lease(IRepositoryContext destContext)
        {
            if(Pooled)
            {
                IRepositoryContext rentContext = Rent();
                DbContext dbcontext = (DbContext)rentContext.InnerContext;
                ChangeTracker changeTracker = dbcontext.ChangeTracker;

                rentContext.ContextLease = destContext;
                destContext.ContextLease = rentContext;
                destContext.InnerContext = dbcontext;

                if(_configurationSnapshot?.AutoDetectChangesEnabled != null)
                {
                    changeTracker.AutoDetectChangesEnabled = _configurationSnapshot.AutoDetectChangesEnabled.Value;
                    changeTracker.QueryTrackingBehavior = _configurationSnapshot.QueryTrackingBehavior.Value;
                    changeTracker.LazyLoadingEnabled = _configurationSnapshot.LazyLoadingEnabled.Value;
                    changeTracker.CascadeDeleteTiming = _configurationSnapshot.CascadeDeleteTiming.Value;
                    changeTracker.DeleteOrphansTiming = _configurationSnapshot.DeleteOrphansTiming.Value;
                } else
                {
                    ((IResettableService)changeTracker)?.ResetState();
                }

                DatabaseFacade database = dbcontext?.Database;

                if(database != null)
                {
                    database.AutoTransactionsEnabled
                        = (_configurationSnapshot?.AutoTransactionsEnabled == null) ||
                        _configurationSnapshot.AutoTransactionsEnabled.Value;
                }
            } else
            {
                if(Count > 0)
                {
                    destContext.ContextPool = this;
                }

                destContext.InnerContext = CreateContext();
            }
            disposedValue = false;
        }

        public virtual Task LeaseAsync(IRepositoryContext lease, CancellationToken token = default)
        { return Task.Run(() => Lease(lease), token); }

        public virtual bool Release()
        {
            if(Leased)
            {
                IRepositoryContext destContext = ContextLease;
                destContext.ContextLease = null;
                destContext.InnerContext = null;
                destContext = null;
                ContextLease = null;

                Return();

                return true;
            }
            return false;
        }

        public void ReleaseAccess()
        {
            _pass.Release();
            _access.Set();
        }

        public virtual Task<bool> ReleaseAsync(CancellationToken token = default)
        {
            return Task.Run(
                () =>
                {
                    if(Leased)
                    {
                        IRepositoryContext destContext = ContextLease;
                        destContext.ContextLease = null;
                        destContext.InnerContext = null;
                        destContext = null;
                        ContextLease = null;

                        _ = ReturnAsync();

                        return true;
                    }
                    return false;
                },
                token);
        }

        public virtual IRepositoryContext Rent()
        {
            if(TryDequeue(out IRepositoryContext context))
            {
                return context;
            }

            return (IRepositoryContext)typeof(IRepositoryEndpoint<>)
                                        .MakeGenericType(ContextType)
                .New(this);
        }

        public virtual void ResetState()
        {
            ((IDbContextDependencies)Context).StateManager.ResetState();
            ((IResettableService)((DataContext)Context).ChangeTracker).ResetState();
        }

        public virtual Task ResetStateAsync(CancellationToken token = default)
        {
            return Task.Run(
                async () =>
                {
                    await ((IDbContextDependencies)Context).StateManager.ResetStateAsync(token);
                    await ((IResettableService)((DataContext)Context).ChangeTracker).ResetStateAsync(token);
                },
                token);
        }

        public virtual void Return()
        {
            ResetState();
            ContextPool.Add((IRepositoryContext)this);
        }

        public virtual Task ReturnAsync(CancellationToken token = default)
        {
            return Task.Run(
                () =>
                {
                    ResetStateAsync();
                    ContextPool.Add((IRepositoryContext)this);
                },
                token);
        }

        public virtual Task<int> Save(bool asTransaction, CancellationToken token = default(CancellationToken))
        { return Context.Save(asTransaction, token); }
        
        public object ServiceEntitySet<TEntity>() where TEntity : class, IIdentifiable
        { return Context.ServiceEntitySet<TEntity>(); }
        public object ServiceEntitySet(Type entityType) { return Context.ServiceEntitySet(entityType); }

        public void SnapshotConfiguration()
        {
            ChangeTracker _changeTracker = ((DataContext)Context).ChangeTracker;
            _configurationSnapshot = new DbContextConfigurationSnapshot(
                _changeTracker?.AutoDetectChangesEnabled,
                _changeTracker?.QueryTrackingBehavior,
                ((DataContext)Context).Database?.AutoTransactionsEnabled,
                _changeTracker?.LazyLoadingEnabled,
                _changeTracker?.CascadeDeleteTiming,
                _changeTracker?.DeleteOrphansTiming);
        }

        protected override async void Dispose(bool disposing)
        {
            if(!disposedValue)
            {
                if(disposing)
                {
                    await Save(true);

                    await ReleaseAsync();

                    InnerContext = null;
                    contextType = null;
                    Options = null;
                    servicecode.Dispose();
                }

                disposedValue = true;
            }
        }
    }
}
