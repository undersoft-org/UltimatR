using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.OData.Client;

namespace UltimatR
{
    public partial class Repository<TEntity> : Repository, IRepository<TEntity> where TEntity : class, IIdentifiable, IEntity
    {
        #region Fields

        private DbSet<TEntity> _dbSet;
        protected DbSet<TEntity> dbSet
        {
            get
            {
                ThreadSpot.Set(this);
                return _dbSet;
            }
        }
        private DataServiceQuery<TEntity> _dsQuery;
        protected DataServiceQuery<TEntity> dsQuery
        {
            get
            {
                ThreadSpot.Set(this);
                return _dsQuery;
            }
        }
        protected IQueryable<TEntity> query;
        protected IDataCache cache;      
               
        #endregion

        #region Constructors

        public Repository()
        {
            ElementType = typeof(TEntity).ProxyRetype();
            Expression = Expression.Constant(this);
        }
        public Repository(IRepositoryClient repositorySource) : base(repositorySource)
        {
            ElementType = typeof(TEntity).ProxyRetype();
            _dsQuery = dsContext.CreateQuery<TEntity>(typeof(TEntity).Name, false);
            query = _dsQuery;
            Expression = Expression.Constant(this.AsEnumerable());
            Provider = new TeleRepositoryQueryProvider<TEntity>(dsQuery);
        }
        public Repository(DsContext dsContext) : base(dsContext)
        {
            ElementType = typeof(TEntity).ProxyRetype();
            _dsQuery = dsContext.CreateQuery<TEntity>(typeof(TEntity).Name, false);
            query = _dsQuery;
            Expression = Expression.Constant(this.AsEnumerable());
            Provider = new TeleRepositoryQueryProvider<TEntity>(dsQuery);
        }
        public Repository(IRepositoryEndpoint repositorySource) : base(repositorySource)
        {
            ElementType = typeof(TEntity).ProxyRetype();
            _dbSet = dbContext.GetDbSet<TEntity>();
            query = _dbSet;
            Expression = Expression.Constant(this.AsEnumerable());
            Provider = new HostRepositoryQueryProvider<TEntity>(dbSet);
        }
        public Repository(DbContext dbContext) : base(dbContext)
        {
            ElementType = typeof(TEntity).ProxyRetype();
            _dbSet = dbContext.GetDbSet<TEntity>();
            query = _dbSet;
            Expression = Expression.Constant(this.AsEnumerable());
            Provider = new HostRepositoryQueryProvider<TEntity>(dbSet);
        }
        public Repository(IDataContext context) : base(context)
        {
            ElementType = typeof(TEntity).ProxyRetype();
            if (context.Site == DataSite.Endpoint)
            {
                _dbSet = dbContext.Set<TEntity>();
                Provider = new HostRepositoryQueryProvider<TEntity>(dbSet);
                query = _dbSet;
            }
            else
            {
                _dsQuery = dsContext.CreateQuery<TEntity>(typeof(TEntity).Name, false);
                Provider = new TeleRepositoryQueryProvider<TEntity>(dsQuery);
                query = _dsQuery;
            }
            Expression = Expression.Constant(this.AsEnumerable());


        }
        public Repository(IQueryProvider provider, Expression expression)
        {
            ElementType = typeof(TEntity).ProxyRetype();
            Provider = provider;
            Expression = expression;
        }

        #endregion 

        #region Properties        

        public virtual IQueryable<TEntity> Query
        {
            get
            {
                ThreadSpot.Set(this);                 
                return query;
            }
        }
      
        #endregion

        #region Methods

        public virtual IQueryable<TEntity> AsQueryable()
        {
            return Query;
        }

        public virtual Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return dbContext.Database.BeginTransactionAsync(Cancellation);
        }
        public virtual       IDbContextTransaction BeginTransaction()
        {
            return dbContext.Database.BeginTransaction();
        }
        
        public virtual async Task CommitTransaction(Task<IDbContextTransaction> transaction)
        {
            await (await transaction).CommitAsync(Cancellation);
        }
        public virtual       void CommitTransaction(IDbContextTransaction transaction)
        {
            transaction.Commit();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<TEntity> GetEnumerator() =>
            Provider.Execute<IQueryable<TEntity>>(Expression).GetEnumerator();

        public TEntity Sign(TEntity entity)
        {
            entity.Sign();
            cache?.MemorizeAsync(entity);
            return entity;
        }

        public TEntity Stamp(TEntity entity)
        {
            entity.Stamp();
            cache?.MemorizeAsync(entity);
            return entity;
        }

        public override void LinkTrigger(object sender, EntityEntryEventArgs e)
        {
            var entry = e.Entry;
            var entity = entry.Entity;
            var type = entity.ProxyRetype();
            
            if (type.IsAssignableTo(typeof(IEntity)) && type == ElementType)
            {
                LinkedObjects.DoEach(async (o) =>
                {
                    if (o.Towards != Towards.SetToSet)
                        await o.LoadAsync(entity);
                    else if (lastEntry != null)
                        await o.LoadAsync(lastEntry.Entity);
                    lastEntry = entry;
                });
            }
        }     

        #endregion
    }

    public enum RelatedType
    {
        None = 0,
        Reference = 1,
        Collection = 2,
        Any = 3
    } 
}
