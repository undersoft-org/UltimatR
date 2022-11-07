using System;
using System.Collections.Generic;
using System.Series;
using System.Threading.Tasks;

namespace UltimatR
{
    public interface IRepositoryManager
    {
        IRepositoryMapper Mapper { get; }

        Task AddClientPools();
        Task AddEndpointPools();
        
        ITeleRepository<TEntity> call<TEntity>() where TEntity : Entity;
        ITeleRepository<TEntity> Call<TEntity>() where TEntity : Entity;
        ITeleRepository<TEntity> Call<TEntity>(Type contextType) where TEntity : Entity;
        ITeleRepository<TEntity> call<TStore, TEntity>() where TStore : IDataStore where TEntity : Entity;
        ITeleRepository<TEntity> Call<TStore, TEntity>() where TStore : IDataStore where TEntity : Entity;
        
        IRepositoryClient GetClient<TStore, TEntity>() where TEntity : Entity;
        IEnumerable<IRepositoryClient> GetClients();
        IRepositoryEndpoint GetEndpoint<TStore, TEntity>() where TEntity : Entity;
        IEnumerable<IRepositoryEndpoint> GetEndpoints();
        IHostRepository<TEntity> use<TEntity>() where TEntity : Entity;
        IHostRepository<TEntity> Use<TEntity>() where TEntity : Entity;
        IHostRepository<TEntity> Use<TEntity>(Type contextType) where TEntity : Entity;
        IHostRepository<TEntity> use<TStore, TEntity>()
            where TStore : IDataStore
            where TEntity : Entity;
        IHostRepository<TEntity> Use<TStore, TEntity>()
            where TStore : IDataStore
            where TEntity : Entity;
    }
}