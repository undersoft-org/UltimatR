using Microsoft.OData.Client;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UltimatR
{
    public interface IRemoteRepository<TStore, TEntity> : IRemoteRepository<TEntity> where TEntity : class, IIdentifiable
    {        
    }
} 