using Microsoft.OData.Client;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Series;
using System.Linq;
using System.Uniques;
using System.Threading.Tasks;
using NetTopologySuite.Index.HPRtree;
using System.Linq.Expressions;

namespace UltimatR
{      

    public class DsoSetToSet<TStore, TEntity> : DsoSet<TEntity>, IDso<TStore, TEntity> where TEntity : class, IIdentifiable
    {
        public DsoSetToSet(ITeleRepository<TStore, TEntity> repository) : base(repository) { }
    }

    public class DsoSetToSet<TEntity> : DsoSet<TEntity> where TEntity : class, IIdentifiable
    {
        public DsoSetToSet() : base()
        {                 
        }
        public DsoSetToSet(DataServiceQuery<TEntity> query) : base(query)
        {         
        }
        public DsoSetToSet(DsContext context, IQueryable<TEntity> query) : base(context, query)
        {
        }
        public DsoSetToSet(DsContext context) : base(context)
        {
        }
    }
}