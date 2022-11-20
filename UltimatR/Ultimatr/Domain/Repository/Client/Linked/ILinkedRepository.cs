using Microsoft.OData.Client;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UltimatR
{
    public interface ILinkedRepository<TStore, TEntity> : ILinkedRepository<TEntity> where TEntity : class, IIdentifiable
    {        
    }

    public interface ILinkedRepository<TEntity> : IRepository<TEntity> where TEntity : class, IIdentifiable
    {
        DsContext Context { get; }

        new DataServiceQuery<TEntity> Query { get; }

        DataServiceQuerySingle<TEntity> QuerySingle(params object[] keys);

        Task<IEnumerable<TEntity>> FindMany(params object[] keys);

        new Task<TEntity> Find(params object[] keys);
    }
} 