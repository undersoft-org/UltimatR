using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace UltimatR
{
    public class HostRepository<TStore, TEntity> : HostRepository<TEntity>, IHostRepository<TStore, TEntity> where TEntity : Entity where TStore : IDataStore
    {
        public HostRepository(IDataContextPool<DbContext<TStore>> pool,
                              IDataCache<TStore, TEntity> cache,
                              IEnumerable<ILinkedObject<TStore, TEntity>> linked,
                              ILinkSynchronizer synchronizer) 
                              : base(pool.ContextPool)
        { 
            mapper = cache.Mapper; 
            this.cache = cache;
            synchronizer.AddRepository(this);
            LinkedObjects = linked.DoEach((o) => 
            { 
                o.Host = this;                    
                return o; 
            });
        }

        public override Task<int> Save(bool asTransaction, CancellationToken token = default(CancellationToken))
        {
            return ContextLease.Save(asTransaction, token);
        }
    }

    public partial class HostRepository<TEntity> : Repository<TEntity>, IHostRepository<TEntity> where TEntity : class, IIdentifiable, IEntity
    {       
        #region Constructors

        public HostRepository()
        {
        }
        public HostRepository(IRepositoryClient repositorySource) : base(repositorySource)
        {               
        }
        public HostRepository(DsContext dsContext) : base(dsContext)
        {
        }
        public HostRepository(IRepositoryEndpoint repositorySource) : base(repositorySource)
        {               
        }
        public HostRepository(DbContext dbContext) : base(dbContext)
        {
        }
        public HostRepository(IDataContextPool context) : base(context)
        {                 
        }
        public HostRepository(IQueryProvider provider, Expression expression)
        {
            ElementType = typeof(TEntity);
            Provider = provider;
            Expression = expression;
        }

        #endregion

        #region Properties

        public int IndexFrom { get; set; }

        public IList<TEntity> Items { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public int TotalPages { get; set; }

        public bool HasNextPage { get; set; }

        public bool HasPreviousPage { get; set; }

        #endregion
    }
}
