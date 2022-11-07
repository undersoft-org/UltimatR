using System;
using System.Linq;
using System.Logs;
using System.Security.Policy;
using System.Series;
using System.Threading;
using System.Threading.Tasks;
using System.Uniques;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OData.Client;
using RTools_NTS.Util;

namespace UltimatR
{
    public class RepositoryClient : Board<IDataContext>, IRepositoryClient
    {
        protected Uri uri;        
        protected Ussn servicecode;        
        private new bool disposedValue;
        protected Type contextType;

        public RepositoryClient()
        {
            Site = DataSite.Client;
        }
        public RepositoryClient(Type contextType, IServiceConfiguration config) : this()
        {
            var endpoint = config.Client(contextType.FullName);
            var connectionString = config.ClientConnectionString(contextType.FullName);
            var provider = config.ClientProvider(contextType.FullName);
            PoolSize = config.ClientPoolSize(endpoint);
            InnerContext = CreateContext(contextType, new Uri(connectionString));
        }
        public RepositoryClient(Type contextType, Uri serviceRoot) : this()
        {
            InnerContext = CreateContext(contextType, serviceRoot);
        }
        public RepositoryClient(Type contextType, ClientProvider provider, string connectionString) : this()
        {
            InnerContext = CreateContext(contextType, new Uri(connectionString));
        }
        public RepositoryClient(ClientProvider provider, string connectionString) : this()
        {
            InnerContext = CreateContext(new Uri(connectionString));
        }
        public RepositoryClient(Uri serviceRoot) : this()
        {
            InnerContext = CreateContext(uri);
        }
        public RepositoryClient(IRepositoryClient pool) : this()
        {
            PoolSize = pool.PoolSize;
            ContextPool = pool;
            InnerContext = pool.CreateContext();
        }

        public object InnerContext { get; set; }

        public DataSite Site { get; set; }

        public int PoolSize { get; set; }

        public Type ContextType => contextType ??= InnerContext.GetType();

        public virtual Uri Route => uri;

        public virtual DsContext Context => (DsContext)InnerContext;

        public virtual object    CreateContext()
        {
            return ContextType.New<DsContext>(uri);         
        }
        public virtual DsContext CreateContext(Uri serviceRoot)
        {
            uri ??= serviceRoot;
            contextType ??= typeof(DsContext);
            return typeof(DsContext).New<DsContext>(uri);            
        }
        public virtual DsContext CreateContext(Type contextType, Uri serviceRoot)
        {
            uri ??= serviceRoot;
            this.contextType ??= contextType;
            return (DsContext)contextType.New(uri);                        
        }

        public virtual TContext GetContext<TContext>() where TContext : DsContext
        {
           return (TContext)InnerContext;
        }
      
        public virtual TContext CreateContext<TContext>() where TContext : class
        {
            contextType ??= typeof(TContext);
            return typeof(TContext).New<TContext>(uri);            
        }
        public virtual TContext CreateContext<TContext>(Uri serviceRoot) where TContext : DsContext
        {
            uri = serviceRoot;
            contextType ??= typeof(TContext);
            return typeof(TContext).New<TContext>(uri);            
        }

        public void GetClientEdm()
        {
            Context.CreateServiceModel();
            Context.GetDsEntityTypes();
        }

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
                    uri = null;
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

            return context = (IDataContext)typeof(IRepositoryClient<>)
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
                ResetStateAsync(token);
                ContextPool.Add((IDataContext)this);
            }, token);
        }

        public virtual void CreatePool()
        {            
            Type repotype = typeof(RepositoryClient<>)
                            .MakeGenericType(ContextType);
            var size = PoolSize - Count;
            for (int i = 0; i < size; i++)
            {
                var repo = repotype.New<IDataContext>(this);                
                Add(repo);
            }
        }
        public virtual void CreatePool<TContext>()
        {            
            Type repotype = typeof(RepositoryClient<>)
                            .MakeGenericType(typeof(TContext));
            var size = PoolSize - Count;
            for (int i = 0; i < size; i++)
            {
                var repo = repotype.New<IDataContext>(this);
                repo.UniqueKey = Unique.New;
                Add(repo);
            }
        }
        
        public virtual void ResetState()
        {
            Context.Entities.ForEach((e) => Context.Detach(e.Entity));
        }

        public virtual Task ResetStateAsync(CancellationToken token = default)
        {
            return Task.Run(() => ResetState(), token);
        }

        public virtual Task<int> Save(bool asTransaction, CancellationToken token = default(CancellationToken))
        {
            return saveClient(asTransaction);
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

                    _ = ReturnAsync(token);

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

                rentContext.ContextLease = destContext;
                destContext.ContextLease = rentContext;
                destContext.InnerContext = rentContext.InnerContext;
            }
            else
            {                
                if(Count > 0)
                    destContext.ContextPool = this;
                destContext.InnerContext = CreateContext();
            }
            disposedValue = false;
        }

        public virtual Task LeaseAsync(IDataContext destContext, CancellationToken token = default)
        {
            return Task.Run(() => Lease(destContext), token);
        }

        private async Task<int> saveClient(bool asTransaction, CancellationToken token = default(CancellationToken))
        {
            if (Context.Entities.Any(s => s.State != EntityStates.Unchanged))
            {
                if (asTransaction)
                    return await saveAsTransaction(Context, token);
                else
                    return await saveChanges(Context, token);
            }
            return 0;
        }

        private async Task<int> saveAsTransaction(DsContext context, CancellationToken token = default(CancellationToken))
        {
            try
            {
                return (await context.SaveChangesAsync(SaveChangesOptions.BatchWithSingleChangeset, token)).Count();
            }
            catch (Exception e)
            {
                context.Failure<Datalog>(
                    $"Fail on update dataservice as singlechangeset, using context:{context.GetType().Name}, " +
                    $"TimeStamp: {DateTime.Now.ToString()}");
            }

            return -1;
        }

        private async Task<int> saveChanges(DsContext context, CancellationToken token = default(CancellationToken))
        {
            try
            {
                return (await context.SaveChangesAsync(SaveChangesOptions.BatchWithIndependentOperations | SaveChangesOptions.ContinueOnError, token)).Count();
            }
            catch (Exception e)
            {
                context.Failure<Datalog>(
                    $"Fail on update dataservice as independent operations, using context:{context.GetType().Name}, " +
                    $"TimeStamp: {DateTime.Now.ToString()}");
            }

            return -1;
        }

        public void SnapshotConfiguration()
        {
            throw new NotImplementedException();
        }
    }

    public class RepositoryClient<TContext> : RepositoryClient, IRepositoryClient<TContext> where TContext : DsContext
    {
        public RepositoryClient() { }
        public RepositoryClient(IServiceConfiguration config) : this()
        {
            var contextType = typeof(TContext);
            var endpoint = config.Client(contextType.FullName);
            var connectionString = config.ClientConnectionString(contextType.FullName);
            var provider = config.ClientProvider(contextType.FullName);
            PoolSize = config.ClientPoolSize(endpoint);
            InnerContext = CreateContext(contextType, new Uri(connectionString));
        }
        public RepositoryClient(ClientProvider provider, string connectionString)
        {
            ContextPool = this;
            contextType = typeof(TContext);
            InnerContext = CreateContext<TContext>(new Uri(connectionString));            
        }
        public RepositoryClient(Uri serviceRoot)
        {
            ContextPool = this;
            contextType = typeof(TContext);
            InnerContext = CreateContext<TContext>(uri);
        }
        public RepositoryClient(IRepositoryClient client) : base(client)
        {  
        }

        public override TContext Context => (TContext)InnerContext;          

        public override TContext CreateContext()
        {  
            return typeof(TContext).New<TContext>(uri);            
        }

        public override TContext CreateContext(Uri serviceRoot)
        {
            uri = serviceRoot;            
            return typeof(TContext).New<TContext>(uri);            
        }        
    }

}
