using System;
using System.Linq;
using System.Logs;
using System.Series;
using System.Threading;
using System.Threading.Tasks;
using System.Uniques;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace UltimatR
{
    public class RepositoryEndpoint : Board<IDataContext>, IRepositoryEndpoint
    {
        protected DbContextConfigurationSnapshot _configurationSnapshot;
        protected ODataConventionModelBuilder odataBuilder;
        protected DbContextOptionsBuilder optionsBuilder;
        protected IEdmModel edmModel;
        protected DbContextOptions options;        
        protected Ussn servicecode;
        protected new bool disposedValue;
        protected Type contextType;

        private const int WAIT_PUBLISH_TIMEOUT = 30 * 1000;
        private ManualResetEventSlim _access = new ManualResetEventSlim(true, 128);
        private SemaphoreSlim _pass = new SemaphoreSlim(1);

        public void AcquireAccess()
        {
            do
            {
                if (!_access.Wait(WAIT_PUBLISH_TIMEOUT))
                    throw new TimeoutException("Wait write Timeout");
                _access.Reset();
            }
            while (!_pass.Wait(0));
        }
        public void ReleaseAccess()
        {
            _pass.Release();
            _access.Set();
        }

        public RepositoryEndpoint()
        {
            odataBuilder = new ODataConventionModelBuilder();
            Site = DataSite.Endpoint;            
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
            var endpoint = config.Endpoint(contextType.FullName);
            var connectionString = config.EndpointConnectionString(contextType.FullName);
            var provider = config.EndpointProvider(contextType.FullName);
            ContextPool = this;
            PoolSize = config.EndpointPoolSize(endpoint);
            optionsBuilder = RepositoryEndpointOptions.BuildOptions(provider, connectionString);
            InnerContext = CreateContext(optionsBuilder.Options).GetDbEntities();
        }
        public RepositoryEndpoint(Type contextType, EndpointProvider provider, string connectionString) : this()
        {
            ContextPool = this;
            optionsBuilder = RepositoryEndpointOptions.BuildOptions(provider, connectionString);
            InnerContext = CreateContext(contextType, optionsBuilder.Options);
            Context.GetDbEntities();
        }
        public RepositoryEndpoint(Type contextType, DbContextOptions options) : this()
        {
            ContextPool = this;
            InnerContext = CreateContext(contextType, options);
            Context.GetDbEntities();
        }
        public RepositoryEndpoint(DbContextOptions options) : this()
        {
            ContextPool = this;
            InnerContext =  CreateContext(options);
            Context.GetDbEntities();
        }
        public RepositoryEndpoint(IDataContextPool pool) : this()
        {
            PoolSize = pool.PoolSize;
            ContextPool = pool;            
            InnerContext = pool.CreateContext();           
        }

        public DataSite Site { get; set; }

        public object InnerContext { get; set; }

        public int PoolSize { get; set; }

        public Type ContextType => contextType ??= InnerContext.GetType();

        public virtual DbContextOptions Options => options;

        public virtual DbContext Context => (DbContext)InnerContext;

        public virtual object CreateContext()
        {
            return (DbContext)ContextType.New(options);            
        }     
        public virtual DbContext CreateContext(DbContextOptions options)
        {
            contextType ??= options.ContextType;
            this.options ??= options;
            return (DbContext)ContextType.New(options);            
        }
        public virtual DbContext CreateContext(Type contextType, DbContextOptions options)
        {
            this.contextType ??= contextType;
            this.options ??= options;
            return (DbContext)ContextType.New(options);                      
        }

        public TContext GetContext<TContext>() where TContext : DbContext
        {
            return (TContext)Context;
        }

        public virtual TContext CreateContext<TContext>() where TContext : class
        {
            contextType ??= typeof(TContext);
            return typeof(TContext).New<TContext>(options);            
        }
        public virtual TContext CreateContext<TContext>(DbContextOptions options) where TContext : DbContext
{
            this.options ??= options;
            contextType ??= typeof(TContext);
            return typeof(TContext).New<TContext>(options);            
        }

        public EntitySetConfiguration<TEntity> EntitySet<TEntity>() where TEntity : Entity
        {
            return odataBuilder.EntitySet<TEntity>(typeof(TEntity).Name);           
        }
        public EntitySetConfiguration EntitySet(Type entityType)
        {
            var entitySetName = entityType.Name;
            if(entityType.IsGenericType && entityType.IsAssignableTo(typeof(Identifier)))
                entitySetName = entityType.GetGenericArguments().FirstOrDefault().Name + "Identifier";

            var etc = odataBuilder.AddEntityType(entityType);
            etc.Name = entitySetName;
            var ets = odataBuilder.AddEntitySet(entitySetName, etc);
            ets.EntityType.HasKey(entityType.GetProperty("Id"));
            return ets;
        }
           
        public ODataConventionModelBuilder AutoBuildEdmModel()
        {
            var entityTypes = Context.Model.GetEntityTypes();
            odataBuilder = new ODataConventionModelBuilder();

            foreach (var entityType in entityTypes)
            {
                var type = entityType.ClrType;
                var setType = EntitySet(type).EntityType;
                setType.HasKey(type.GetProperty("Id"));
            }

            return odataBuilder;
        }

        public IEdmModel CreateEdmModel()
        {
            return AutoBuildEdmModel().GetEdmModel();
        }
        public IEdmModel GetEdmModel()
        {
            var model = edmModel ??= odataBuilder.GetEdmModel();
            odataBuilder.ValidateModel(model);
            return model;
        }

        public int ExecuteSql(string sql, params object[] parameters) 
            => Context.Database.ExecuteSqlRaw(sql, parameters);

        public IQueryable<TEntity> FromSql<TEntity>(string sql, params object[] parameters) 
        where TEntity : class => (Microsoft.EntityFrameworkCore.RelationalQueryableExtensions.FromSqlRaw(((DbContext)InnerContext).Set<TEntity>(), sql, parameters));       

        public override ulong UniqueKey
        {
            get => (servicecode.UniqueKey == 0) ? servicecode.UniqueKey = Unique.New : servicecode.UniqueKey;
            set => servicecode.UniqueKey = value;
        }

        public override ulong UniqueSeed
        {
            get => (servicecode.UniqueSeed == 0) ? servicecode.UniqueSeed = ContextType.ProxyRetypeKey32() : servicecode.UniqueSeed;
            set => servicecode.UniqueSeed = value;
        }

        public IDataContext ContextLease { get; set; }

        public IDataContextPool ContextPool { get; set; }

        public bool Pooled => ContextPool != null;

        public bool Leased => ContextLease != null;

        protected override async void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    await Save(true);

                    await ReleaseAsync();

                    InnerContext = null;
                    contextType = null;
                    options = null;                  
                    servicecode.Dispose();                    
                }            

                disposedValue = true;
            }
        }

        public new ValueTask DisposeAsync()
        {
            return new ValueTask(Task.Run(() => Dispose(true)));
        }

        public virtual IDataContext Rent()
        {
            if (TryDequeue(out IDataContext context))
            {
                return context;
            }
           
            return (IDataContext)typeof(IRepositoryEndpoint<>)
                                        .MakeGenericType(ContextType)
                                        .New(this);                        
        }

        public virtual void Return()
        {
            ResetState();
            ContextPool.Add((IDataContext)this);
        }

        public virtual Task ReturnAsync(CancellationToken token = default)
        {
            return Task.Run(() =>
            {
                ResetStateAsync();
                ContextPool.Add((IDataContext)this);
            }, token);
        }

        public void CreatePool()
        {
            SnapshotConfiguration();
            Type repotype = typeof(RepositoryEndpoint<>)
                            .MakeGenericType(ContextType);
            int size = PoolSize - Count;
            for (int i = 0; i < size; i++)
            {
                var repo = repotype.New<IDataContext>(this);                
                Add(repo);
            }
        }
        public void CreatePool<TContext>()
        {
            SnapshotConfiguration();
            Type repotype = typeof(RepositoryEndpoint<>)
                            .MakeGenericType(typeof(TContext));
            var size = PoolSize - Count;
            for (int i = 0; i < size; i++)
            {
                var repo = repotype.New<IDataContext>(this);                
                Add(repo);
            }
        }        

        public virtual void ResetState()
        {
            ((IDbContextDependencies)Context).StateManager.ResetState();
            ((IResettableService)Context.ChangeTracker).ResetState();            
        }

        public virtual Task ResetStateAsync(CancellationToken token = default)
        {
            return Task.Run(async () =>
            {
               await ((IDbContextDependencies)Context).StateManager.ResetStateAsync(token);
               await ((IResettableService)Context.ChangeTracker).ResetStateAsync(token);
            }, token);
        }

        public virtual bool Release()
        {
            if (Leased)
            {
                var destContext = ContextLease;                
                destContext.ContextLease = null;
                destContext.InnerContext = null;
                destContext = null;
                ContextLease = null;

                 Return();

                return true;
            }
            return false;
        }

        public virtual Task<bool> ReleaseAsync(CancellationToken token = default)
        {
            return Task.Run(() =>
            {
                if (Leased)
                {
                    var destContext = ContextLease;
                    destContext.ContextLease = null;
                    destContext.InnerContext = null;
                    destContext = null;
                    ContextLease = null;

                    _ = ReturnAsync();

                    return true;
                }
                return false;
            }, token);
        }

        public virtual void Lease(IDataContext destContext)
        {
            if (Pooled)
            {
                var rentContext = Rent();
                var dbcontext = (DbContext)rentContext.InnerContext;
                var changeTracker = dbcontext.ChangeTracker;                

                rentContext.ContextLease = destContext;
                destContext.ContextLease = rentContext;
                destContext.InnerContext = dbcontext;              

                if (_configurationSnapshot?.AutoDetectChangesEnabled != null)
                {
                    changeTracker.AutoDetectChangesEnabled = _configurationSnapshot.AutoDetectChangesEnabled.Value;
                    changeTracker.QueryTrackingBehavior = _configurationSnapshot.QueryTrackingBehavior.Value;
                    changeTracker.LazyLoadingEnabled = _configurationSnapshot.LazyLoadingEnabled.Value;
                    changeTracker.CascadeDeleteTiming = _configurationSnapshot.CascadeDeleteTiming.Value;
                    changeTracker.DeleteOrphansTiming = _configurationSnapshot.DeleteOrphansTiming.Value;
                }
                else
                {
                    ((IResettableService)changeTracker)?.ResetState();
                }

                var database = dbcontext?.Database;

                if (database != null)
                {
                    database.AutoTransactionsEnabled
                        = _configurationSnapshot?.AutoTransactionsEnabled == null
                        || _configurationSnapshot.AutoTransactionsEnabled.Value;
                }
            }
            else
            {

                if (Count > 0)
                    destContext.ContextPool = this;
                destContext.InnerContext = CreateContext();
            }
            disposedValue = false;
        }

        public virtual Task LeaseAsync(IDataContext lease, CancellationToken token = default)
        {
            return Task.Run(() => Lease(lease), token);
        }

        public virtual Task<int> Save(bool asTransaction, CancellationToken token = default(CancellationToken))
        {
            return saveEndpoint(asTransaction, token);
        }

        private async Task<int> saveEndpoint(bool asTransaction, CancellationToken token = default(CancellationToken))
        {
            if (Context.ChangeTracker.HasChanges())
            {
                if (asTransaction)
                    return await saveAsTransaction(Context, token);
                else
                    return await saveChanges(Context, token);
            }
            return 0;
        }

        private async Task<int> saveAsTransaction(DbContext context, CancellationToken token = default(CancellationToken))
        {
            await using var tr = await context.Database.BeginTransactionAsync(token);
            try
            {
                var changes = await context.SaveChangesAsync(token);

                await tr.CommitAsync(token);

                return changes;
            }
            catch (DbUpdateException e)
            {
                if (e is DbUpdateConcurrencyException)
                    tr.Warning<Datalog>($"Concurrency update exception data changed by: { e.Source }, " +
                                        $"entries involved in detail data object", e.Entries, e);
                else
                    tr.Failure<Datalog>(
                        $"Fail on update database transaction Id:{tr.TransactionId}, using context:{context.GetType().Name}," +
                        $" context Id:{context.ContextId}, TimeStamp:{DateTime.Now.ToString()}");

                await tr.RollbackAsync(token);

                tr.Warning<Datalog>($"Transaction Id:{tr.TransactionId} Rolling Back !!");
            }
            return -1;
        }

        private async Task<int> saveChanges(DbContext context, CancellationToken token = default(CancellationToken))
        {
            try
            {
                return await context.SaveChangesAsync(token);
            }
            catch (DbUpdateException e)
            {
                if (e is DbUpdateConcurrencyException)
                    context.Warning<Datalog>($"Concurrency update exception data changed by: { e.Source }, " +
                                             $"entries involved in detail data object", e.Entries, e);
                else
                    context.Failure<Datalog>(
                        $"Fail on update database, using context:{context.GetType().Name}, " +
                        $"context Id: {context.ContextId}, TimeStamp: {DateTime.Now.ToString()}");
            }

            return -1;
        }

        public void SnapshotConfiguration()
        {
            var _changeTracker = Context.ChangeTracker;
             _configurationSnapshot = new DbContextConfigurationSnapshot(
              _changeTracker?.AutoDetectChangesEnabled,
              _changeTracker?.QueryTrackingBehavior,
              Context.Database?.AutoTransactionsEnabled,
              _changeTracker?.LazyLoadingEnabled,
              _changeTracker?.CascadeDeleteTiming,
              _changeTracker?.DeleteOrphansTiming);                      
        }
    }

    public class RepositoryEndpoint<TContext> : RepositoryEndpoint, IRepositoryEndpoint<TContext> where TContext : DbContext
    {
        protected new DbContextOptionsBuilder<TContext> optionsBuilder;

        public RepositoryEndpoint() : base()
        {
        
        }
        public RepositoryEndpoint(IServiceConfiguration config) : base()
        {
            contextType = typeof(TContext);
            var endpoint = config.Endpoint(contextType.FullName);
            var connectionString = config.EndpointConnectionString(contextType.FullName);
            var provider = config.EndpointProvider(contextType.FullName);
            ContextPool = this;
            PoolSize = config.EndpointPoolSize(endpoint);
            optionsBuilder = RepositoryEndpointOptions.BuildOptions<TContext>(provider, connectionString);
            InnerContext = CreateContext(optionsBuilder.Options);
            Context.GetDbEntities();
        }
        public RepositoryEndpoint(EndpointProvider provider, string connectionString) : base()
        {
            ContextPool = this;
            contextType = typeof(TContext);
            optionsBuilder = RepositoryEndpointOptions.BuildOptions<TContext>(provider, connectionString);
            InnerContext = CreateContext(optionsBuilder.Options);
            Context.GetDbEntities();
        }        
        public RepositoryEndpoint(DbContextOptions<TContext> options) : base()
        {
            ContextPool = this;
            contextType = options.ContextType;
            InnerContext = CreateContext(options);
            Context.GetDbEntities();
        }
        public RepositoryEndpoint(IRepositoryEndpoint pool) : base(pool)
        {
        }

        public override DbContextOptions<TContext> Options => (DbContextOptions<TContext>)options;

        public override TContext Context => (TContext)InnerContext;

        public override TContext CreateContext()
        {
            return (typeof(TContext).New<TContext>(options));
        }      
        public TContext CreateContext(DbContextOptions<TContext> options)
        {
            this.options ??= options;
            Type type = typeof(TContext);
            contextType ??= type;
            return type.New<TContext>(options);                     
        }

        public TContext CreateDbContext(string[] args)
        {
            return CreateContext();
        }
        public TContext CreateDbContext()
        {
            return CreateContext();
        }     
    }

    public static class RepositoryEndpointOptions
    {
        public static DbContextOptionsBuilder<TContext> BuildOptions<TContext>(EndpointProvider provider, string connectionString) where TContext : DbContext
        {            
            return (DbContextOptionsBuilder<TContext>)BuildOptions(new DbContextOptionsBuilder<TContext>(), provider, connectionString)
                .ConfigureWarnings(w => w.Ignore(CoreEventId.DetachedLazyLoadingWarning));
        }

        public static DbContextOptionsBuilder BuildOptions(EndpointProvider provider, string connectionString)
        {
            return BuildOptions(new DbContextOptionsBuilder(), provider, connectionString)
                .ConfigureWarnings(w => w.Ignore(CoreEventId.DetachedLazyLoadingWarning)); ;
        }

        public static DbContextOptionsBuilder BuildOptions(DbContextOptionsBuilder builder, EndpointProvider provider, string connectionString)
        {
            switch (provider)
            {
                case EndpointProvider.SqlServer:
                    return builder
                        .UseSqlServer(connectionString)
                        .UseLazyLoadingProxies();
                case EndpointProvider.AzureSql:
                    return builder
                        .UseSqlServer(connectionString)
                        .UseLazyLoadingProxies();

                case EndpointProvider.PostgreSql:
                    return builder
                        .UseNpgsql(connectionString)
                        .UseLazyLoadingProxies();
                  
                case EndpointProvider.SqlLite:
                    return builder
                        .UseSqlite(connectionString)
                        .UseLazyLoadingProxies();

                case EndpointProvider.MariaDb:
                    return builder
                        .UseMySql(ServerVersion
                            .AutoDetect(connectionString))
                        .UseLazyLoadingProxies();

                case EndpointProvider.MySql:
                    return builder
                        .UseMySql(ServerVersion
                            .AutoDetect(connectionString))
                        .UseLazyLoadingProxies();

                case EndpointProvider.Oracle:
                    return builder
                        .UseOracle(connectionString)
                        .UseLazyLoadingProxies();

                case EndpointProvider.CosmosDb:
                    return builder
                        .UseCosmos(connectionString.Split('#')[0],
                            connectionString.Split('#')[1],
                            connectionString.Split('#')[2])
                        .UseLazyLoadingProxies();

                case EndpointProvider.MemoryDb:
                    return builder
                        .UseInternalServiceProvider(new ServiceCollection()
                            .AddEntityFrameworkInMemoryDatabase()
                            .BuildServiceProvider())
                        .UseInMemoryDatabase(connectionString)
                        .UseLazyLoadingProxies()
                        .ConfigureWarnings(w
                            => w.Ignore((InMemoryEventId
                                .TransactionIgnoredWarning)));
                default:
                    break;
            }

            return builder;
        }

        private static IServiceCatalog AddEntityServicesForDb(EndpointProvider provider)
        {
            var sm = ServiceManager.GetManager();
            if (!DbCatalog.Providers.ContainsKey((int)provider))
            {                
                switch(provider)
                {
                    case EndpointProvider.SqlServer:
                        sm.Catalog.AddEntityFrameworkSqlServer();
                        break;
                    case EndpointProvider.AzureSql:
                        sm.Catalog.AddEntityFrameworkSqlServer();
                        break;
                    case EndpointProvider.PostgreSql:
                        sm.Catalog.AddEntityFrameworkNpgsql();
                        break;
                    case EndpointProvider.SqlLite:
                        sm.Catalog.AddEntityFrameworkSqlite();
                        break;
                    case EndpointProvider.MariaDb:
                        sm.Catalog.AddEntityFrameworkMySql();
                        break;
                    case EndpointProvider.MySql:
                        sm.Catalog.AddEntityFrameworkMySql();
                        break;
                    case EndpointProvider.Oracle:
                        sm.Catalog.AddEntityFrameworkOracle();
                        break;
                    case EndpointProvider.CosmosDb:
                        sm.Catalog.AddEntityFrameworkCosmos();
                        break;
                    case EndpointProvider.MemoryDb:
                        sm.Catalog.AddEntityFrameworkInMemoryDatabase();
                        break;
                    default:
                        break;
                }
                sm.Catalog.AddEntityFrameworkProxies();
                DbCatalog.Providers.Add((int)provider, provider);
            }
            return sm.Catalog;
        }
    }

    public enum EndpointProvider
    {
        None,
        SqlServer,
        OData,
        RabbitMQ,
        MemoryDb,
        AzureSql,
        PostgreSql,
        SqlLite,
        MySql,
        MariaDb,
        Oracle,
        CosmosDb,
        MongoDb,
        FileSystem
    }

    public enum ClientProvider
    {
        None,
        OData,
        RabbitMQ
    }


}
