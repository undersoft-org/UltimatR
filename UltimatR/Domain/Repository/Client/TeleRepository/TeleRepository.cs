using Microsoft.OData.Client;
using System;
using System.Collections.Generic;
using System.Instant;
using System.Linq;
using System.Linq.Expressions;
using System.Series;
using System.Threading;
using System.Threading.Tasks;

namespace UltimatR
{
    public class TeleRepository<TStore, TEntity> : TeleRepository<TEntity>, ITeleRepository<TStore, TEntity> 
        where TEntity : Entity where TStore : IDataStore
    {
        public TeleRepository(IDataContextPool<DsContext<TStore>> pool,
                              IDataCache<TStore, TEntity> cache) 
                              : base(pool.ContextPool)
        {
            mapper = cache.Mapper;
            this.cache = cache;                 
        }

        public override Task<int> Save(bool asTransaction, CancellationToken token = default)
        {
            return ContextLease.Save(asTransaction, token);    
        }
    }
    
    public class TeleRepository<TEntity> : Repository<TEntity>, ITeleRepository<TEntity> 
        where TEntity : Entity
    {
        private IDeck<DataServiceRequest> _batchset;

        #region Constructors

        public TeleRepository()
        {
        }
        public TeleRepository(IRepositoryClient repositorySource) : base(repositorySource)
        {
            Expression = Expression.Constant(this);
            Provider = new TeleRepositoryQueryProvider<TEntity>(Query);
        }
        public TeleRepository(DsContext dsContext) : base(dsContext)
        {
            if (dsContext != null)
            {
                Expression = Expression.Constant(this);
                Provider = new TeleRepositoryQueryProvider<TEntity>(Query);
            }
        }
        public TeleRepository(IDataContextPool context) : base(context)
        {                        
        }
        public TeleRepository(IQueryProvider provider, Expression expression)
        {
            ElementType = typeof(TEntity);
            Provider = provider;
            Expression = expression;                       
        }

        #endregion

        public DsContext Context => dsContext;

        public new DataServiceQuery<TEntity> Query => dsContext.CreateQuery<TEntity>(Name, true);

        public DataServiceQuerySingle<TEntity> QuerySingle(params object[] keys)
        {
            if (keys != null)
            {
                string keyedName =
                    $"{Name}({((keys.Length > 1) ? keys.Aggregate("", (a, b) => $"{a},{b}") : keys[0])})";
                return dsContext.CreateFunctionQuerySingle<TEntity>("", keyedName, true);
            }
            return null;
        }

        public override Task<TEntity> Find(params object[] keys)
        {
            return findAsync(keys);
        }

        public async Task<IEnumerable<TEntity>> FindMany(params object[] keys)
        {
            foreach (var key in keys)
            {
                if (key.GetType().IsAssignableTo(typeof(object[])))
                    findOne(((object[])key));
                else
                    findOne(key);
            }
            return (await Context.ExecuteBatchAsync(_batchset.ToArray()))
                .SelectMany(o => (o as QueryOperationResponse<TEntity>));
        }

        public override TEntity this[params object[] keys]
        {
            get
            {
                return find(keys);
            }
            set
            {
                var entity = find(keys);
                if (entity != null)
                {
                    value.PatchTo(entity);
                    dsContext.UpdateObject(Stamp(entity), keys);
                }
                else
                {
                    dsContext.AddObject(Name, Sign(value));
                }
            }
        }

        public override TEntity NewEntry(params object[] parameters)
        {
            var entity = Sign(typeof(TEntity).New<TEntity>(parameters));
            dsContext.AddObject(Name, entity);
            return entity;
        }

        public override TEntity Add(TEntity entity)
        {
            var _entity = Sign(entity);
            dsContext.AddObject(Name, _entity);
            return entity;
        }
        public override IEnumerable<TEntity> Add(IEnumerable<TEntity> entity)
        {
            foreach (var e in entity)
            {
                yield return Add(e);
            }
        }

        public override TEntity Delete(TEntity entity)
        {
            dsContext.DeleteObject(entity);
            return entity;
        }

        protected override TEntity             InnerSet(TEntity entity)
        {
            dsContext.UpdateObject(Stamp(entity));
            return entity;
        }    

        private TEntity find(params object[] keys)
        {
            if (keys != null)
            {
                string keyedName =
                    $"{Name}({((keys.Length > 1) ? keys.Aggregate("", (a, b) => $"{a},{b}") : keys[0])})";
                return dsContext.CreateFunctionQuerySingle<TEntity>("", keyedName, true).GetValue();
            }
            return null;
        }

        private Task<TEntity> findAsync(params object[] keys)
        {
            if (keys != null)
            {
                string keyedName =
                    $"{Name}({((keys.Length > 1) ? keys.Aggregate("", (a, b) => $"{a},{b}") : keys[0])})";
                return dsContext.CreateFunctionQuerySingle<TEntity>("", keyedName, true).GetValueAsync();
            }
            return null;
        }

        private DataServiceRequest findOne(params object[] keys)
        {
            if (keys != null)
            {
                if (_batchset == null)
                    _batchset = new Catalog<DataServiceRequest>();

                string keyedName =
                    $"{Name}({((keys.Length > 1) ? keys.Aggregate("", (a, b) => $"{a},{b}") : keys[0])})";
                return _batchset.Put(keys, dsContext.CreateQuery<TEntity>(keyedName, true)).Value;
            }
            return null;
        }

    }
}
