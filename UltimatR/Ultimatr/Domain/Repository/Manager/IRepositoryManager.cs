using System;
using System.Collections.Generic;
using System.Series;
using System.Threading.Tasks;

namespace UltimatR
{
    public interface IRepositoryManager
    {
        IDataMapper Mapper { get; }

        Task AddClientPools();
        Task AddEndpointPools();
        
        ILinkedRepository<TEntity> load<TEntity>() where TEntity : Entity;
        ILinkedRepository<TEntity> Load<TEntity>() where TEntity : Entity;
        ILinkedRepository<TEntity> Load<TEntity>(Type contextType) where TEntity : Entity;
        ILinkedRepository<TEntity> load<TStore, TEntity>() where TStore : IDataStore where TEntity : Entity;
        ILinkedRepository<TEntity> Load<TStore, TEntity>() where TStore : IDataStore where TEntity : Entity;
        
        IRepositoryClient GetClient<TStore, TEntity>() where TEntity : Entity;
        IEnumerable<IRepositoryClient> GetClients();
        IRepositoryEndpoint GetEndpoint<TStore, TEntity>() where TEntity : Entity;
        IEnumerable<IRepositoryEndpoint> GetEndpoints();
        IDataRepository<TEntity> use<TEntity>() where TEntity : Entity;
        IDataRepository<TEntity> Use<TEntity>() where TEntity : Entity;
        IDataRepository<TEntity> Use<TEntity>(Type contextType) where TEntity : Entity;
        IDataRepository<TEntity> use<TStore, TEntity>()
            where TStore : IDataStore
            where TEntity : Entity;
        IDataRepository<TEntity> Use<TStore, TEntity>()
            where TStore : IDataStore
            where TEntity : Entity;
    }
}