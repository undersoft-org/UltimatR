using System;
using System.Collections.Generic;
using System.Instant;
using System.Linq;
using System.Linq.Expressions;
using System.Series;
using System.Uniques;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.OData.Client;
namespace UltimatR
{
    public partial class Repository<TEntity> 
    {
        #region Methods
       
        public virtual TEntity this[params object[] keys]
        {
            get
            {
                OnFinding.Publish(this);

                var item = dbSet.Find(keys);

                OnFindComplete.Publish(this, item);
                return item;
            }
            set
            {               
                object current = null;                
                var entity = dbSet.Find(keys);

                OnUpsert.Publish(this, entity);
                
                if (entity != null)
                  current = value.PatchTo(Stamp(entity));
                else
                 current = dbSet.Add(Sign(value)).Entity;

                OnUpsertComplete.Publish(this, current);
            }
        }
        public virtual TEntity this[object[] keys, Expression<Func<TEntity, object>>[] expanders]
        {
            get
            {
                var entity = this[keys];
                if(entity == null) return entity;
                if(expanders != null)
                {
                    var query = entity.ToQueryable();
                    if (expanders != null)
                    {
                        foreach (var expander in expanders)
                        {
                            query = (dbSet != null) ? query.Include(expander)
                            : ((DataServiceQuery<TEntity>)query).Expand(expander);
                        }
                    }
                    entity = query.FirstOrDefault();
                }
                return entity;
            }
            set
            {
                var entity = this[keys];
                if (entity != null)
                {
                    var query = entity.ToQueryable();
                    if (expanders != null)
                    {
                        foreach (var expander in expanders)
                        {
                            query = (dbSet != null) ? query.Include(expander)
                            : ((DataServiceQuery<TEntity>)query).Expand(expander);
                        }
                    }

                    OnPatching.Execute(this, entity);

                    var current = value.PatchTo(Stamp(entity));

                    OnSaveComplete.Execute(this, current);

                }                    
            }
        }
        public virtual object this[Expression<Func<TEntity, object>> selector, object[] keys, params Expression<Func<TEntity, object>>[] expanders]
        {
            get
            {
                var entity = this[keys];
                if (entity == null) return entity;
                var query = entity.ToQueryable();
                if (expanders != null)
                {
                    foreach (var expander in expanders)
                    {
                        query = (dbSet != null) ? query.Include(expander)
                        : ((DataServiceQuery<TEntity>)query).Expand(expander);
                    }
                }
                return query.Select(selector).FirstOrDefault();
            }
            set
            {
                var entity = this[keys];
                var query = entity.ToQueryable();
                if (expanders != null)
                {
                    foreach (var expander in expanders)
                    {
                        query = (dbSet != null) ? query.Include(expander)
                        : ((DataServiceQuery<TEntity>)query).Expand(expander);
                    }
                }
                var s = query.Select(selector).FirstOrDefault();
                if (s != null)
                {
                    OnPatching.Execute(this, s);

                    value.PatchTo(s);

                    OnSaveComplete.Execute(this, s);
                }
            }
        }

        public virtual TEntity this[bool reverse, SortExpression<TEntity> sortTerms]
            => (reverse) ? this[sortTerms].LastOrDefault() : this[sortTerms].FirstOrDefault();
        public virtual TEntity this[bool reverse, Expression<Func<TEntity, bool>> predicate] 
            =>  (reverse) ? this[predicate].LastOrDefault() : this[predicate].FirstOrDefault();
        public virtual TEntity this[bool reverse, Expression<Func<TEntity, object>>[] expanders]
            => (reverse) ? this[expanders].LastOrDefault() : this[expanders].FirstOrDefault();
        public virtual object  this[bool reverse, Expression<Func<TEntity, object>> selector]
           => (reverse) ? this[selector].LastOrDefault() : this[selector].FirstOrDefault();

        public virtual object  this[bool reverse, Expression<Func<TEntity, object>> selector, Expression<Func<TEntity, object>>[] expanders]
             => (reverse) ? this[selector, expanders].LastOrDefault() : this[selector, expanders].FirstOrDefault();
        public virtual object  this[bool reverse, Expression<Func<TEntity, object>> selector, Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] expanders]
              => (reverse) ? this[selector, predicate, expanders].LastOrDefault() : this[selector, predicate, expanders].FirstOrDefault();
        public virtual object  this[bool reverse, Expression<Func<TEntity, object>> selector, Expression<Func<TEntity, bool>> predicate, SortExpression<TEntity> sortTerms, params Expression<Func<TEntity, object>>[] expanders]
            => (reverse) ? this[selector, predicate, sortTerms, expanders].LastOrDefault() : this[selector, predicate, sortTerms, expanders].FirstOrDefault();

        public virtual TEntity this[bool reverse, Expression<Func<TEntity, bool>> predicate, SortExpression<TEntity> sortTerms]
            => (reverse) ? this[predicate, sortTerms].LastOrDefault() : this[predicate, sortTerms].FirstOrDefault();
        public virtual TEntity this[bool reverse, Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] expanders]
            => (reverse) ? this[predicate, expanders].LastOrDefault() : this[predicate, expanders].FirstOrDefault();
        public virtual TEntity this[bool reverse, Expression<Func<TEntity, bool>> predicate, SortExpression<TEntity> sortTerms, params Expression<Func<TEntity, object>>[] expanders]
            => (reverse) ? this[predicate, sortTerms, expanders].LastOrDefault() : this[predicate, sortTerms, expanders].FirstOrDefault();
        
        public virtual TEntity this[bool reverse, SortExpression<TEntity> sortTerms, params Expression<Func<TEntity, object>>[] expanders]
            => (reverse) ? this[sortTerms, expanders].LastOrDefault() : this[sortTerms, expanders].FirstOrDefault();

        public virtual IQueryable<TEntity> this[int skip, int take, IQueryable<TEntity> query]
           => (take > 0) ? query.Skip(skip).Take(take) : query;

        public virtual IDeck<TEntity> this[int skip, int take, SortExpression<TEntity> sortTerms]
            => (take > 0) ? this[sortTerms].Skip(skip).Take(take).ToAlbum() : this[sortTerms].ToAlbum();
        public virtual IDeck<TEntity> this[int skip, int take, Expression<Func<TEntity, bool>> predicate]
            => (take > 0) ? this[predicate].Skip(skip).Take(take).ToAlbum() : this[predicate].ToAlbum();
        public virtual IList<object>  this[int skip, int take, Expression<Func<TEntity, object>> selector]
          => (take > 0) ? this[selector].Skip(skip).Take(take).ToArray() : this[selector].ToArray();
        public virtual IDeck<TEntity> this[int skip, int take, Expression<Func<TEntity, object>>[] expanders] 
            => (take > 0) ? this[expanders].Skip(skip).Take(take).ToAlbum() : this[expanders].ToAlbum();

        public virtual IList<object> this[int skip, int take, Expression<Func<TEntity, object>> selector, Expression<Func<TEntity, object>>[] expanders]
           => (take > 0) ? this[selector, expanders].Skip(skip).Take(take).ToArray() : this[selector, expanders].ToArray();
        public virtual IList<object> this[int skip, int take, Expression<Func<TEntity, object>> selector, Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] expanders]
            => (take > 0) ? this[selector, predicate, expanders].Skip(skip).Take(take).ToArray() : this[selector, predicate, expanders].ToArray();
        public virtual IList<object> this[int skip, int take, Expression<Func<TEntity, object>> selector, Expression<Func<TEntity, bool>> predicate, SortExpression<TEntity> sortTerms,  params Expression<Func<TEntity, object>>[] expanders]
            => (take > 0) ? this[selector, predicate, sortTerms, expanders].Skip(skip).Take(take).ToArray() : this[selector, predicate, sortTerms, expanders].ToArray();

        public virtual IDeck<TEntity> this[int skip, int take, Expression<Func<TEntity, bool>> predicate, SortExpression<TEntity> sortTerms]
            => (take > 0) ? this[predicate, sortTerms].Skip(skip).Take(take).ToAlbum() : this[predicate, sortTerms].ToAlbum();
        public virtual IDeck<TEntity> this[int skip, int take, Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] expanders]
            => (take > 0) ? this[predicate, expanders].Skip(skip).Take(take).ToAlbum(true) : this[predicate, expanders].ToAlbum();
        public virtual IDeck<TEntity> this[int skip, int take, Expression<Func<TEntity, bool>> predicate, SortExpression<TEntity> sortTerms, params Expression<Func<TEntity, object>>[] expanders]
            => (take > 0) ? this[predicate, sortTerms, expanders].Skip(skip).Take(take).ToAlbum() : this[predicate, sortTerms, expanders].ToAlbum();
       
        public virtual IDeck<TEntity> this[int skip, int take, SortExpression<TEntity> sortTerms, Expression<Func<TEntity, object>>[] expanders]
            => (take > 0) ? this[sortTerms, expanders].Skip(skip).Take(take).ToAlbum() : this[sortTerms, expanders].ToAlbum();
               
        public virtual IQueryable<TEntity> this[IQueryable<TEntity> query, params Expression<Func<TEntity, object>>[] expanders]
        {
            get
            {                
                IQueryable<TEntity> _query  = query;
                if (expanders != null)
                {
                    foreach (var expander in expanders)
                    {
                        _query = (dbSet != null) ? _query.Include(expander) 
                                                 : ((DataServiceQuery<TEntity>)_query).Expand(expander);
                    }
                }

                return _query;
            }
        }
        public virtual IQueryable<TEntity> this[IQueryable<TEntity> query, Expression<Func<TEntity, bool>> predicate] => query.Where(predicate).AsQueryable();
        public virtual IQueryable<object>  this[IQueryable<TEntity> query, Expression<Func<TEntity, object>> selector] => query.Select(selector).AsQueryable();
        public virtual IQueryable<object>  this[IQueryable<TEntity> query, Expression<Func<TEntity, int, object>> selector] => query.Select(selector).AsQueryable();
        public virtual IQueryable<TEntity> this[IQueryable<TEntity> query, Expression<Func<TEntity, object>> selector, IEnumerable<object> values] => query.WhereIn(selector, values);

        public virtual IQueryable<TEntity> this[Expression<Func<TEntity, bool>> predicate] 
            => Query.Where(predicate).AsQueryable();
        public virtual IQueryable<TEntity> this[SortExpression<TEntity> sortTerms] 
            => Sort(Query, sortTerms);
        public virtual IQueryable<object>  this[Expression<Func<TEntity, object>> selector]
           => Query.Select(selector).AsQueryable();
        public virtual IQueryable<TEntity> this[Expression<Func<TEntity, object>>[] expanders]
        {
            get
            {                
                IQueryable<TEntity> query = Query;
                if (expanders != null)
                {
                    foreach (var expander in expanders)
                    {
                        query = (dbSet != null) ? query.Include(expander) 
                                                : ((DataServiceQuery<TEntity>)query).Expand(expander);
                    }
                }

                return query;
            }
        }
        
        public virtual IQueryable<TEntity>  this[Expression<Func<TEntity, object>> selector, IEnumerable<object> values]
            => Query.WhereIn(selector, values);
        public virtual IQueryable<TEntity> this[Expression<Func<TEntity, object>> selector, IEnumerable<object> values, params Expression<Func<TEntity, object>>[] expanders]
             => this[this[expanders], selector, values];

        public virtual IQueryable<object> this[Expression<Func<TEntity, object>> selector, Expression<Func<TEntity, object>>[] expanders]
            => this[this[expanders], selector];
        public virtual IQueryable<object> this[Expression<Func<TEntity, object>> selector, Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] expanders]
          => this[this[predicate, expanders], selector];
        public virtual IQueryable<object> this[Expression<Func<TEntity, object>> selector, Expression<Func<TEntity, bool>> predicate, SortExpression<TEntity> sortTerms, params Expression<Func<TEntity, object>>[] expanders]
          => this[Sort(this[predicate, expanders], sortTerms), selector];

        public virtual IQueryable<object>  this[Expression<Func<TEntity, object>> selector, Expression<Func<TEntity, bool>> predicate]
            =>  this[this[predicate], selector];
        public virtual IQueryable<TEntity> this[Expression<Func<TEntity, bool>> predicate, SortExpression<TEntity> sortTerms]
            => Sort(this[predicate], sortTerms);
        public virtual IQueryable<TEntity> this[Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] expanders]
            => this[this[predicate], expanders];

        public virtual IQueryable<TEntity> this[SortExpression<TEntity> sortTerms, Expression<Func<TEntity, object>>[] expanders]
            => Sort(this[expanders], sortTerms);
        public virtual IQueryable<TEntity> this[Expression<Func<TEntity, bool>> predicate, SortExpression<TEntity> sortTerms, params Expression<Func<TEntity, object>>[] expanders]
            => Sort(this[predicate, expanders], sortTerms);

        public virtual IGrouping<dynamic, TEntity> this[Func<IQueryable<TEntity>, IGrouping<dynamic, TEntity>> groupByObject, Expression<Func<TEntity, bool>> predicate] => groupByObject(Query.Where(predicate).AsQueryable());

        #endregion
    }
}
