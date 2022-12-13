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
        protected IDataMapper mapper;

        protected static IRepositoryEndpoints Endpoints { get; set; }
        protected static IRepositoryClients Clients { get; set; }

        protected IServiceManager Services { get; init; }

        public IDataMapper Mapper
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

        public IDataRepository<TEntity> use<TStore, TEntity>() where TEntity : Entity where TStore : IDataStore
        {
            return Use<TStore, TEntity>();
        }
        public IDataRepository<TEntity> use<TEntity>() where TEntity : Entity
        {
            return Use<TEntity>();
        }

        public IDataRepository<TEntity> Use<TEntity>()
        where TEntity : Entity
        {
            return Use<TEntity>(DbRegistry.GetContexts<TEntity>().FirstOrDefault());
        }
        public IDataRepository<TEntity> Use<TEntity>(Type contextType)
            where TEntity : Entity
        {
            return (IDataRepository<TEntity>)Services.GetService(typeof(IDataRepository<,>)
                                                     .MakeGenericType(DbRegistry
                                                     .Stores[contextType],
                                                      typeof(TEntity)));
        }
        public IDataRepository<TEntity> Use<TStore, TEntity>()
           where TEntity : Entity where TStore : IDataStore
        {
            return Services.GetService<IDataRepository<TStore, TEntity>>();
        }

        public IRemoteRepository<TEntity> load<TStore, TEntity>() where TEntity : Entity where TStore : IDataStore
        {
            return Load<TStore, TEntity>();
        }
        public IRemoteRepository<TEntity> load<TEntity>() where TEntity : Entity
        {
            return Load<TEntity>();
        }

        public IRemoteRepository<TEntity> Load<TEntity>() where TEntity : Entity
        {
            return Load<TEntity>(DsRegistry.GetContexts<TEntity>().FirstOrDefault());
        }
        public IRemoteRepository<TEntity> Load<TEntity>(Type contextType)
           where TEntity : Entity
        {
            return (IRemoteRepository<TEntity>)Services.GetService(typeof(IRemoteRepository<,>)
                                                     .MakeGenericType(DsRegistry
                                                     .Stores[contextType],
                                                      typeof(TEntity)));
        }
        public IRemoteRepository<TEntity> Load<TStore, TEntity>() where TEntity : Entity where TStore : IDataStore
        {
            var result= Services.GetService<IRemoteRepository<TStore, TEntity>>();
            return result;
        }

        public IRepositoryEndpoint GetEndpoint<TStore, TEntity>()
        where TEntity : Entity
        {
            var contextType = DbRegistry.GetContext<TStore, TEntity>();
            return Endpoints.Get(contextType);
        }

        public IRepositoryClient GetClient<TStore, TEntity>()
        where TEntity : Entity
        {
            var contextType = DsRegistry.GetContext<TStore, TEntity>();

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
        public static IRepositoryClient CreateClient(Type contextType, Uri serviceRoot)
        {
            return new RepositoryClient(contextType, serviceRoot);
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

        public static IRepositoryEndpoint<TContext> CreateEndpoint<TContext>(DbContextOptions<TContext> options) where TContext : DataContext
        {
            return new RepositoryEndpoint<TContext>(options);
        }
        public static IRepositoryEndpoint CreateEndpoint(IRepositoryEndpoint endpoint)
        {
            Type repotype = typeof(RepositoryEndpoint<>).MakeGenericType(endpoint.ContextType);
            return (IRepositoryEndpoint)repotype.New(endpoint);
        }
        public static IRepositoryEndpoint<TContext> CreateEndpoint<TContext>(IRepositoryEndpoint<TContext> endpoint)
            where TContext : DataContext
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

        public static IDataMapper CreateMapper(params Profile[] profiles)
        {
            DataMapper.AddProfiles(profiles);            
            return ServiceManager.GetObject<IDataMapper>();
        }
        public static IDataMapper CreateMapper<TProfile>() where TProfile : Profile
        {            
            DataMapper.AddProfiles(typeof(TProfile).New<TProfile>());
            return ServiceManager.GetObject<IDataMapper>();
        }

        public static IDataMapper GetMapper()
        {                        
            return ServiceManager.GetObject<IDataMapper>();
        }

        public static async Task LoadClientEdms(IApplicationBuilder app)
        {
            await Task.Run(() =>
            {
                Clients.ForEach((client) =>
                {
                    client.BuildMetadata();
                });

                AppSetup.AddRuntimeImplementations(app);
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
