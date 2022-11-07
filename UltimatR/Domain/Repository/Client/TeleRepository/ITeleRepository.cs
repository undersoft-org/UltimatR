using Microsoft.OData.Client;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UltimatR
{
    public interface ITeleRepository<TStore, TEntity> : ITeleRepository<TEntity> where TEntity : class, IIdentifiable
    {        
    }

    public interface  ITeleRepository<TEntity> : IRepository<TEntity> where TEntity : class, IIdentifiable
    {
        DsContext Context { get; }

        new DataServiceQuery<TEntity> Query { get; }

        DataServiceQuerySingle<TEntity> QuerySingle(params object[] keys);

        Task<IEnumerable<TEntity>> FindMany(params object[] keys);

        new Task<TEntity> Find(params object[] keys);
    }
} 