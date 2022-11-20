﻿using IdentityModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.OData.Client;
using SharpCompress.Common;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Instant;
using System.Linq;
using System.Linq.Expressions;
using System.Logs;
using System.Series;
using System.Threading;
using System.Threading.Tasks;
using System.Uniques;

namespace UltimatR
{
    public abstract class Repository : RepositoryEvents, IRepository
    {
        #region Fields

        protected RelatedType relatedtype = RelatedType.None;
        protected Ussn serialcode;
        protected IDataMapper mapper;
        protected bool loaded;
        protected bool audited;

        private bool disposedValue;
        private Type contextType;

        #endregion

        #region Constructors

        protected Repository() { }
        protected Repository(object context)
        {
            InnerContext = context;
        }
        protected Repository(IRepositoryContext context)
        {        
            Site = context.Site;
            context.Lease(this);
        }

        #endregion

        #region Properties

        public Type ElementType { get; set; }

        public string Name => ElementType.ProxyRetypeName();

        public string Fullname => ElementType.ProxyRetypeFullName();

        public object InnerContext { get; set; }

        public DataSite Site { get; set; }

        public Type ContextType => contextType ??= InnerContext.GetType();

        public Expression Expression { get; set; }

        public IQueryProvider Provider { get; set; }

        public IDataMapper Mapper => mapper ??= RepositoryManager.GetMapper();

        public IUnique Empty => Ussn.Empty;

        public CancellationToken Cancellation { get; set; } = new(false);

        public ulong UniqueKey
        {
            get => (serialcode.UniqueKey == 0) ? serialcode.UniqueKey = Unique.New : serialcode.UniqueKey;
            set => serialcode.UniqueKey = value;
        }

        public ulong UniqueSeed
        {
            get => (serialcode.UniqueSeed == 0)
                ? serialcode.UniqueSeed = ContextType.UniqueKey32()
                : serialcode.UniqueSeed;
            set => serialcode.UniqueSeed = value;
        }

        public IRepositoryContext ContextLease { get; set; }

        public IRepositoryContextPool ContextPool { get; set; }

        public bool Pooled => ContextPool != null;

        public bool Leased => ContextLease != null;

        public IEnumerable<ILinkedObject> LinkedObjects { get; set; }

        public virtual int LinkedCount { get; set; }

        public virtual Towards Towards { get; set; }

        #endregion

        #region Methods

        public byte[] GetBytes()
        {
            return serialcode.GetBytes();
        }

        public byte[] GetUniqueBytes()
        {
            return serialcode.GetUniqueBytes();
        }

        public bool Equals(IUnique other)
        {
            return serialcode.Equals(other);
        }

        public int CompareTo(IUnique other)
        {
            return serialcode.CompareTo(other);
        }

        protected virtual async void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    await Save(true);

                    _ = ReleaseAsync().ConfigureAwait(false);

                    ElementType = null;
                    Expression = null;
                    Provider = null;
                    serialcode.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public virtual async ValueTask DisposeAsyncCore()
        {
            await new ValueTask(Task.Run(async () =>
            {
                await Save(true).ConfigureAwait(false);

                await ReleaseAsync().ConfigureAwait(false);

                ElementType = null;
                Expression = null;
                Provider = null;
                serialcode.Dispose();
            })).ConfigureAwait(false);
        }

        public async ValueTask DisposeAsync()
        {
            await DisposeAsyncCore().ConfigureAwait(false);

            Dispose(false);

            GC.SuppressFinalize(this);
        }

        public virtual void ResetState()
        {
            if (Leased)
                ContextLease.ResetState();
        }

        public virtual Task ResetStateAsync(CancellationToken cancellationToken = default)
        {
            return Task.Run(() => ResetState());
        }

        public virtual Task<int> Save(bool asTransaction, CancellationToken token = default(CancellationToken))
        {
            if (Leased)
            {
                return ContextLease.Save(asTransaction, token);
            }
            else
            {
                switch (asTransaction)
                {
                    case true:
                        return saveAsTransaction();
                    default:
                        return saveChanges();
                }
            }
        }

        public virtual bool Release()
        {
            if (Leased)
                return ContextLease.Release();
            return false;
        }

        public virtual Task<bool> ReleaseAsync(CancellationToken token = default)
        {
            return ContextLease.ReleaseAsync();
        }

        public virtual void Lease(IRepositoryContext rentContext)
        {
            rentContext.Lease(this);
        }

        public virtual Task LeaseAsync(IRepositoryContext destContext, CancellationToken token = default)
        {
            return Task.Run(() => Lease(destContext), token);
        }

        public virtual void SnapshotConfiguration()
        {
            throw new NotImplementedException();
        }

        public virtual void LoadLinked(object entity)
        {
            LinkedObjects.DoEach((o) => o.Load(entity));
        }

        public virtual async Task LoadLinkedAsync(object entity)
        {
            await Task.WhenAll(LinkedObjects.DoEach((o) => o.LoadAsync(entity)));
        }

        public virtual void LoadRelated(EntityEntry entry, RelatedType relatedType)
        {
            var modes = (int)(relatedType
                              & (RelatedType.Reference
                                 | RelatedType.Collection));

            if (modes < 1) return;

            if (modes.IsOdd())
                entry.References
                    .ForOnly(a => !a.IsLoaded, (e) =>
                    {
                        e.Load();
                    });
            if (modes > 1)
                entry.Collections
                    .ForOnly(a => !a.IsLoaded, (e) =>
                    {
                        e.Load();
                    });
        }

        public virtual void LoadRelatedAsync(EntityEntry entry, RelatedType relatedType, CancellationToken token)
        {
            var modes = (int)(relatedType
                              & (RelatedType.Reference
                                 | RelatedType.Collection));

            if (modes < 1) return;

            if (modes.IsOdd())
                entry.References
                    .ForOnly(a => !a.IsLoaded, async (e) =>
                    {
                        await e.LoadAsync();
                    }).Commit();
            if (modes > 1)
                entry.Collections
                    .ForOnly(a => !a.IsLoaded, async (e) =>
                    {
                        await e.LoadAsync();
                    }).Commit();
        }

        public virtual void LinkTrigger(object sender, EntityEntryEventArgs e)
        {
            var entry = e.Entry;
            var entity = entry.Entity;
            var type = entity.ProxyRetype();

            if (type.IsAssignableTo(typeof(IEntity)) && type == ElementType)
            {
                LinkedObjects.DoEach(async (o) =>
                {
                    await o.LoadAsync(entity);
                });
            }
        }

        protected virtual void AuditTrigger(object sender, EntityEntryEventArgs e)
        {
            var entity = e.Entry.Entity as Entity;

            switch (e.Entry.State)
            {
                case EntityState.Deleted:
                    entity.Stamp();
                    entity.ModificationTime = DateTime.UtcNow;
                    entity.Inactive = true;
                    break;
                case EntityState.Modified:
                    entity.Stamp();
                    entity.ModificationTime = DateTime.UtcNow;
                    break;
                case EntityState.Added:
                    entity.Stamp();
                    var _entity = entity as IEntity;
                    entity.CreationTime = DateTime.UtcNow;
                    break;
            }

            audited = true;
        }

        protected abstract Task<int> saveAsTransaction(CancellationToken token = default(CancellationToken));

        protected abstract Task<int> saveChanges(CancellationToken token = default(CancellationToken));

        #endregion
    }
}