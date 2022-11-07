using System.Linq;

using Microsoft.OData.Client;

namespace UltimatR
{
    public class DsoToSet<TStore, TEntity> : DsoSet<TEntity>, IDso<TStore, TEntity> where TEntity : class, IIdentifiable
    {
        public DsoToSet(ITeleRepository<TStore, TEntity> repository) : base(repository) { }
    }

    public class DsoToSet<TEntity> : DsoSet<TEntity> where TEntity : class, IIdentifiable
    {
        public DsoToSet() : base()
        {            
        }
        public DsoToSet(DataServiceQuery<TEntity> query) : base(query)
        {       
        }
        public DsoToSet(DsContext context, IQueryable<TEntity> query) : base(context, query)
        {
        }
        public DsoToSet(DsContext context) : base(context)
        {
        }
    }
}