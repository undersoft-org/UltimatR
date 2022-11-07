using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OData.Edm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Series;
using System.Threading.Tasks;

namespace UltimatR
{
    public class RepositoryManager : Board<IDataContext>, IDisposable, IAsyncDisposable, IRepositoryManager
    {       
        private new bool disposedValue;
        protected IRepositoryMapper mapper;

        protected static IRepositoryEndpoints Endpoints { get; set; }
        protected static IRepositoryClients Clients { get; set; }

        protected IServiceManager Services { get; init; }

        public IRepositoryMapper Mapper
        {
            get => mapper ??= GetMapper();
        }

        static RepositoryManager()
        {
            Endpoints = new RepositoryEndpoints();
            Clients = new RepositoryClients();
        }
        public RepositoryManager() : base()
        {
        }

        public IHostRepository<TEntity> use<TStore, TEntity>() where TEntity : Entity where TStore : IDataStore
        {
            return Use<TStore, TEntity>();
        }
        public IHostRepository<TEntity> use<TEntity>() where TEntity : Entity
        {
            return Use<TEntity>();
        }

        public IHostRepository<TEntity> Use<TEntity>()
        where TEntity : Entity
        {
            return Use<TEntity>(DbCatalog.GetContexts<TEntity>().FirstOrDefault());
        }
        public IHostRepository<TEntity> Use<TEntity>(Type contextType)
            where TEntity : Entity
        {
            return (IHostRepository<TEntity>)Services.GetService(typeof(IHostRepository<,>)
                                                     .MakeGenericType(DbCatalog
                                                     .Stores[contextType],
                                                      typeof(TEntity)));
        }
        public IHostRepository<TEntity> Use<TStore, TEntity>()
           where TEntity : Entity where TStore : IDataStore
        {
            return Services.GetService<IHostRepository<TStore, TEntity>>();
        }

        public ITeleRepository<TEntity> call<TStore, TEntity>() where TEntity : Entity where TStore : IDataStore
        {
            return Call<TStore, TEntity>();
        }
        public ITeleRepository<TEntity> call<TEntity>() where TEntity : Entity
        {
            return Call<TEntity>();
        }

        public ITeleRepository<TEntity> Call<TEntity>() where TEntity : Entity
        {
            return Call<TEntity>(DsCatalog.GetContexts<TEntity>().FirstOrDefault());
        }
        public ITeleRepository<TEntity> Call<TEntity>(Type contextType)
           where TEntity : Entity
        {
            return (ITeleRepository<TEntity>)Services.GetService(typeof(ITeleRepository<,>)
                                                     .MakeGenericType(DsCatalog
                                                     .Stores[contextType],
                                                      typeof(TEntity)));
        }
        public ITeleRepository<TEntity> Call<TStore, TEntity>() where TEntity : Entity where TStore : IDataStore
        {
            var result= Services.GetService<ITeleRepository<TStore, TEntity>>();
            return result;
        }

        public IRepositoryEndpoint GetEndpoint<TStore, TEntity>()
        where TEntity : Entity
        {
            var contextType = DbCatalog.GetContext<TStore, TEntity>();
            return Endpoints.Get(contextType);
        }

        public IRepositoryClient GetClient<TStore, TEntity>()
        where TEntity : Entity
        {
            var contextType = DsCatalog.GetContext<TStore, TEntity>();

            return Clients.Get(contextType);
        }

        public static void AddClientPool(Type contextType, int poolSize, int minSize = 1)
        {
            if (TryGetClient(contextType, out IRepositoryClient client))
            {
                client.PoolSize = poolSize;
                client.CreatePool();
            }
        }

        public Task AddClientPools()
        {
            return Task.Run(() =>
            {
                foreach (var client in GetClients())
                {
                    client.CreatePool();
                }
            });
        }

        public static IRepositoryClient CreateClient(IRepositoryClient client)
        {
            Type repotype = typeof(RepositoryClient<>).MakeGenericType(client.ContextType);
            return (IRepositoryClient)repotype.New(client);
        }
        public static IRepositoryClient<TContext> CreateClient<TContext>(IRepositoryClient<TContext> client)
            where TContext : DsContext
        {
            return new RepositoryClient<TContext>(client);
        }
        public static IRepositoryClient<TContext> CreateClient<TContext>(Uri serviceRoot) where TContext : DsContext
        {
            return new RepositoryClient<TContext>(serviceRoot);
        }        
        public static IRepositoryClient CreateClient(Uri serviceRoot)
        {
            return new RepositoryClient(serviceRoot);
        }

        public static IRepositoryClient AddClient(IRepositoryClient client)
        {
            if (Clients == null)
                Clients =  ServiceManager.GetObject<IRepositoryClients>();
            Clients.Add(client);
            return client;
        }

        public static bool TryGetClient<TContext>(out IRepositoryClient<TContext> endpoint) where TContext : DsContext
        {
            return Clients.TryGet(out endpoint);
        }
        public static bool TryGetClient(Type contextType, out IRepositoryClient endpoint)
        {
            return Clients.TryGet(contextType, out endpoint);
        }

        public Task AddEndpointPools()
        {
            return Task.Run(() =>
            {
                foreach (var endpoint in Endpoints)
                {
                    endpoint.CreatePool();
                }
            });
        }

        public static void AddEndpointPool(Type contextType, int poolSize, int minSize = 1)
        {
            if (TryGetEndpoint(contextType, out IRepositoryEndpoint endpoint))
            {
                endpoint.PoolSize = poolSize;
                endpoint.CreatePool();
            }
        }

        public static IRepositoryEndpoint<TContext> CreateEndpoint<TContext>(DbContextOptions<TContext> options) where TContext : DbContext
        {
            return new RepositoryEndpoint<TContext>(options);
        }
        public static IRepositoryEndpoint CreateEndpoint(IRepositoryEndpoint endpoint)
        {
            Type repotype = typeof(RepositoryEndpoint<>).MakeGenericType(endpoint.ContextType);
            return (IRepositoryEndpoint)repotype.New(endpoint);
        }
        public static IRepositoryEndpoint<TContext> CreateEndpoint<TContext>(IRepositoryEndpoint<TContext> endpoint)
            where TContext : DbContext
        {
            return typeof(RepositoryEndpoint<TContext>).New<IRepositoryEndpoint<TContext>>(endpoint);
        }
        public static IRepositoryEndpoint CreateEndpoint(DbContextOptions options)
        {
            return new RepositoryEndpoint(options);
        }

        public static IRepositoryEndpoint AddEndpoint(IRepositoryEndpoint endpoint)
        {
            if (Endpoints == null)
                Endpoints = ServiceManager.GetObject<IRepositoryEndpoints>();
            Endpoints.Add(endpoint);
            return endpoint;
        }

        public static bool TryGetEndpoint<TContext>(out IRepositoryEndpoint<TContext> endpoint) where TContext : DbContext
        {
            return Endpoints.TryGet(out endpoint);
        }
        public static bool TryGetEndpoint(Type contextType, out IRepositoryEndpoint endpoint)
        {
            return Endpoints.TryGet(contextType, out endpoint);
        }

        public IEnumerable<IRepositoryEndpoint> GetEndpoints()
        {
            return Endpoints;
        }

        public IEnumerable<IRepositoryClient> GetClients()
        {
            return Clients;
        }

        public static IRepositoryMapper CreateMapper(params Profile[] profiles)
        {
            RepositoryMapper.AddProfiles(profiles);            
            return ServiceManager.GetObject<IRepositoryMapper>();
        }
        public static IRepositoryMapper CreateMapper<TProfile>() where TProfile : Profile
        {            
            RepositoryMapper.AddProfiles(typeof(TProfile).New<TProfile>());
            return ServiceManager.GetObject<IRepositoryMapper>();
        }

        public static IRepositoryMapper GetMapper()
        {                        
            return ServiceManager.GetObject<IRepositoryMapper>();
        }

        public static async Task LoadClientEdms(IApplicationBuilder app)
        {
            await Task.Run(() =>
            {
                Clients.ForEach((client) =>
                {
                    client.GetClientEdm();
                });

                ServiceSetup.AddDsRuntimeImplementations(app);
            });
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

        public override async ValueTask DisposeAsyncCore()
        {
            await base.DisposeAsyncCore().ConfigureAwait(false);
        }
    }
}
