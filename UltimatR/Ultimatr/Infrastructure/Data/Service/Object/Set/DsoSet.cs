using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Series;
using System.Threading.Tasks;
using System.Uniques;

using Microsoft.OData.Client;

namespace UltimatR
{
    public interface IDso<TStore, TEntity> : IDso<TEntity>
    {
    }

    public interface IDso<TEntity> : ICollection<TEntity>, IEnumerable<TEntity>, IEnumerable, IList<TEntity>
    {
        DataServiceContext Context { get; }
        
        void LoadAsync<TOrigin>(TOrigin origin, Func<TOrigin, Expression<Func<TEntity, bool>>> predicate);

        void Load<TOrigin>(TOrigin origin, Func<TOrigin, Expression<Func<TEntity, bool>>> predicate);

        void Load(Expression<Func<TEntity, bool>> predicate);

        void LoadAsync(Expression<Func<TEntity, bool>> predicate);

        void Load(Func<IQueryable<TEntity>, IQueryable<TEntity>> query);

        void LoadAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> query);

        event EventHandler<LoadCompletedEventArgs> LoadCompleted;

        Task SaveAsync();

        void Save();
    }

    public class DsoSet<TStore, TEntity> : DsoSet<TEntity>, IDso<TStore, TEntity> where TEntity : class, IIdentifiable
    {
        private IRemoteRepository<TEntity> _repository;

        public DsoSet(IRemoteRepository<TStore, TEntity> repository) : base(repository.Query) 
        {
            _repository = repository;
        }
    }

    public class DsoSet<TEntity> : DataServiceCollection<TEntity>, IDso<TEntity>, IFindable where TEntity : class, IIdentifiable
    {
        protected DataServiceContext context;
        protected IQueryable<TEntity> _query;
        protected object origin;

        public DataServiceContext Context => context;

        public object this[object key]
        {
            get => (key is long)
                ? this.FirstOrDefault(item => item.Id == (long)key)
                : this.FirstOrDefault(item => (ulong)item.Id == key.UniqueKey64());
            set => SetItem(IndexOf((TEntity)this[key]), (TEntity)value);
        }

        public DsoSet() : base()
        {
        }
        public DsoSet(DataServiceQuery<TEntity> query) : base(query.Context)
        {
            _query = query;
            this.context = query.Context;
        }
        public DsoSet(DataServiceContext context, IQueryable<TEntity> query) : base(context)
        {
            _query = query;
            this.context = context;
        }

        public DsoSet(DataServiceQuerySingle<TEntity> item) : base(item)
        {
            context = item.Context;
            _query = context.CreateQuery<TEntity>(typeof(TEntity).Name);
        }
        public DsoSet(IEnumerable<TEntity> items) : base(items)
        {
        }
        public DsoSet(TrackingMode trackingMode, DataServiceQuerySingle<TEntity> item) 
            : base(trackingMode, item)
        {
            context = item.Context;
            _query = context.CreateQuery<TEntity>(typeof(TEntity).Name);
        }
        public DsoSet(IEnumerable<TEntity> items, TrackingMode trackingMode) 
            : base(items, trackingMode)
        {
        }
        public DsoSet(DataServiceContext context) : base(context)
        {
            this.context = context;
            _query = context.CreateQuery<TEntity>(typeof(TEntity).Name);
        }
        public DsoSet(DataServiceContext context, string entitySetName, Func<EntityChangedParams, bool> entityChangedCallback, Func<EntityCollectionChangedParams, bool> collectionChangedCallback) 
            : base(context, entitySetName, entityChangedCallback, collectionChangedCallback)
        {
            this.context = context;
            _query = context.CreateQuery<TEntity>(typeof(TEntity).Name);
        }
        public DsoSet(IEnumerable<TEntity> items, TrackingMode trackingMode, string entitySetName, Func<EntityChangedParams, bool> entityChangedCallback, Func<EntityCollectionChangedParams, bool> collectionChangedCallback) 
            : base(items, trackingMode, entitySetName, entityChangedCallback, collectionChangedCallback)
        {
        }
        public DsoSet(DataServiceContext context, IEnumerable<TEntity> items, TrackingMode trackingMode, string entitySetName, Func<EntityChangedParams, bool> entityChangedCallback, Func<EntityCollectionChangedParams, bool> collectionChangedCallback) 
            : base(context, items, trackingMode, entitySetName, entityChangedCallback, collectionChangedCallback)
        {
            this.context = context;
            _query = context.CreateQuery<TEntity>(typeof(TEntity).Name);
        }

        public virtual IQueryable<TEntity> Query => _query;

        public virtual void Load()
        {
            base.Load(_query);
        }
        public virtual void Load(Func<IQueryable<TEntity>, IQueryable<TEntity>> query)
        {
            base.Load(query(_query));
        }
        public virtual void Load(Expression<Func<TEntity, bool>> predicate)
        {
            if (predicate != null)
                base.Load(_query.Where(predicate));
        }
        public virtual void Load<TOrigin>(TOrigin origin, Func<TOrigin, Expression<Func<TEntity, bool>>> predicate)
        {
            this.origin = origin;
            if (predicate != null)
                Load(predicate.Invoke(origin));
        }

        public virtual void LoadAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> query)
        {
            base.LoadAsync(query(_query));
        }
        public virtual void LoadAsync(Expression<Func<TEntity, bool>> predicate)
        {
            if (predicate != null)
                base.LoadAsync(_query.Where(predicate));
        }
        public virtual void LoadAsync<TOrigin>(TOrigin origin, Func<TOrigin, Expression<Func<TEntity, bool>>> predicate)
        {
            this.origin = origin;
            if (predicate != null)
                LoadAsync(predicate.Invoke(origin));
        }

        public virtual void Save()
        {
            context.SaveChanges(SaveChangesOptions.BatchWithSingleChangeset);
        }

        public virtual async Task SaveAsync()
        {
            await context.SaveChangesAsync(SaveChangesOptions.BatchWithSingleChangeset);
        }

        public virtual void AddRange(IEnumerable<TEntity> items)
        {
            items.DoEach(e => Add(e));
        }
    }
}