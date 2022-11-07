using IdentityModel;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Instant;
using System.Linq;
using System.Linq.Expressions;
using System.Logs;
using System.Series;
using System.Threading;
using System.Threading.Tasks;

namespace UltimatR
{
    public partial class Repository<TEntity>
    {
        #region Methods

        public virtual TEntity NewEntry(params object[] parameters)
        {
            return dbSet.Add(Sign(typeof(TEntity).New<TEntity>(parameters))).Entity;
        }

        public virtual TEntity Add(TEntity entity)
        {
            return dbSet.Add(Sign(entity)).Entity;
        }

        public virtual IEnumerable<TEntity> Add(IEnumerable<TEntity> entity)
        {
            foreach (var e in entity)
                yield return Add(e);
        }
        public virtual IEnumerable<TEntity> Add(IEnumerable<TEntity> entities, Func<TEntity, Expression<Func<TEntity, bool>>> predicate)
        {
            var addins = entities;
            if (predicate != null)
                addins = entities.Where(e => !Query.Any(predicate.Invoke(e)));
            if (addins.Any())
            {
                foreach (var addin in addins)
                {
                    yield return Add(addin);
                }
            }
        }

        public virtual Task    AddAsync(IEnumerable<TEntity> entity)
        {
            return Task.Run(() => dbSet.AddRange(entity.ForEach(e => Sign(e))));
        }
        public virtual Task    AddAsync(IEnumerable<TEntity> entities, Func<TEntity, Expression<Func<TEntity, bool>>> predicate)
        {
            return Task.Run(() =>
            {
                var addin = entities;
                if (predicate != null)
                    addin = entities.Where(e => !Query.Any(predicate.Invoke(e)));
                if (addin.Any())
                    dbSet.AddRange(addin.ForEach(e => Sign(e)));
            });
        }
        public virtual TEntity Add(TEntity entity, Func<TEntity, Expression<Func<TEntity, bool>>> predicate)
        {
            if (predicate != null && Query.Any(predicate.Invoke(entity))) return null;
            return dbSet.Add(Sign(entity)).Entity;
        }

        public virtual TEntity              Delete(TEntity entity)
        {
            var entry = dbContext.Entry(entity);
            if (entry == null ||
                entry.State == EntityState.Detached)            
                entry = dbSet.Attach(entity);
            
            entry.State = EntityState.Deleted;
            return entry.Entity;
        }
        public virtual IEnumerable<TEntity> Delete(IEnumerable<TEntity> entity)
        {
            foreach (var e in entity)
                yield return Delete(e);
        }
        public virtual IEnumerable<TEntity> Delete(long[] ids)
        {            
            var deck = Query.WhereIn(p => p.Id, ids).ToDeck();
            foreach (TEntity model in deck)
            {
                yield return Delete(model);
            }            
        }
        public virtual async Task<TEntity>  Delete(params object[] key)
        {
            var toDelete = await Find(key);
            if (toDelete != null)
                return Delete(toDelete);
            return null;
        }
        public virtual TEntity              Delete(Expression<Func<TEntity, bool>> predicate)
        {
            var toDelete = this[false, predicate];
            if (toDelete != null)
                return Delete(toDelete);
            return null;
        }
        public virtual TEntity              Delete(TEntity entity, Func<TEntity, Expression<Func<TEntity, bool>>> predicate)
        {
            if (predicate != null)
                return Delete(predicate.Invoke(entity));
            return null;
        }
        public virtual IEnumerable<TEntity> Delete(IEnumerable<TEntity> entities, Func<TEntity, Expression<Func<TEntity, bool>>> predicate)
        {
            if (predicate != null)
                foreach (var entity in entities)
                    yield return Delete(predicate.Invoke(entity));
            yield return null;
        }

        protected virtual TEntity InnerSet(TEntity entity)
        {
            return Stamp(entity);
        }

        public virtual TEntity Update(TEntity entity)
        {
            return InnerSet(entity);
        }

        public virtual async Task<TEntity>  Set<TModel>(TModel entity, params object[] key) where TModel : class
        {
            if (key == null) return null;
            var _entity = await Find(key);
            if (_entity != null)
            {
                return InnerSet((TEntity)entity.PutTo(_entity.Valuator).Devisor);
            }
            return null;
        }
        public virtual async Task<TEntity>  Set<TModel>(TModel entity, object key, Func<TEntity, Expression<Func<TEntity, bool>>> condition) where TModel : class, IIdentifiable
        {
            if (key == null) return null;
            var _entity = await Find(key);
            if (_entity != null)
            {
                if (condition != null &&
                    !Query.Any(condition
                          .Invoke(_entity)))
                            return null;

                return InnerSet((TEntity)entity.PutTo(_entity.Valuator).Devisor);
            }
            return _entity;
        }
        public virtual async Task<TEntity>  Set<TModel>(TModel entity) where TModel : class, IIdentifiable
        {
            if (entity.Id == 0) return null;
            var _entity = await Find(entity.Id);
            if (_entity != null)            
                return InnerSet((TEntity)entity.PutTo(_entity.Valuator).Devisor);
            return null;
        }
        public virtual IEnumerable<TEntity> Set<TModel>(IEnumerable<TModel> models) where TModel : class, IIdentifiable
        {
            var ids = models.Select(e => e.Id).ToArray();
            var deck = Query.WhereIn(p => p.Id, ids).ToDeck();
            foreach (var model in models)
            {
                var entity = deck[model];
                if (entity != null)
                {
                    yield return InnerSet((TEntity)model.PutTo(entity.Valuator).Devisor);
                }
            }
        }
        public virtual async Task<TEntity>  Set<TModel>(TModel entity, Func<TModel, Expression<Func<TEntity, bool>>> predicate, params Func<TModel, Expression<Func<TEntity, bool>>>[] conditions) where TModel : class, IIdentifiable
        {
            return await Task.Run(async () =>
            {
                TEntity _entity = null;
                if (predicate != null)
                    _entity = Query.FirstOrDefault(predicate
                                   .Invoke(entity));
                if (_entity == null)
                    _entity = await Find(entity.Id);
              
                if (_entity == null)
                    return null;

                if (conditions != null)                
                    foreach (var condition in conditions)                    
                        if (!Query.Any(condition.Invoke(entity)))
                            return null;

                return InnerSet((TEntity)entity.PutTo(_entity.Valuator).Devisor);
            });
        }
        public virtual IEnumerable<TEntity> Set<TModel>(IEnumerable<TModel> entities, Func<TModel, Expression<Func<TEntity, bool>>> predicate, params Func<TModel, Expression<Func<TEntity, bool>>>[] conditions) where TModel : class, IIdentifiable
        {
            IDeck<TEntity> setters = null;
            if (predicate != null)
                setters = entities.Select(e => Query
                                  .FirstOrDefault(predicate.Invoke(e)))
                                  .Where(e => e != null).ToDeck();
            if (setters == null)
            {
                var ids = entities.Select(i => i.Id).ToArray();
                setters = Query.WhereIn(q => q.Id, ids).ToDeck();
            }
            if (setters == null)
                yield return null;

            foreach (var entity in entities)
            {
                if (conditions != null)
                {
                    foreach (var condition in conditions)
                    {
                        if (!Query.Any(condition.Invoke(entity)))
                            yield return null;
                    }
                }
                yield return InnerSet(((TEntity)entity.PutTo(setters.Get(entity).Valuator).Devisor));
            }
        }

        public virtual Task<TEntity>            Patch(Delta<TEntity> delta, params object[] key)
        {
            return Task.Run(async () =>
            {
                if (key == null) return null;
                var entity = await Find(key);
                if (entity == null) return null;
                delta.Patch(entity);
                return InnerSet(entity);
            });
        }
        public virtual Task<TEntity>            Patch(Delta<TEntity> delta, Func<TEntity, Expression<Func<TEntity, bool>>> predicate) 
        {
            return Task.Run(() =>
            {
                TEntity entity = null;
                if (predicate != null)                
                    entity = this[false, predicate.Invoke(
                                         delta.GetInstance())];
                
                if (entity != null)
                {
                    delta.Patch(entity);
                    return InnerSet(entity);
                }
                return default;
            });
        }
        public virtual async Task<TEntity>      Patch<TModel>(TModel delta) where TModel : class, IIdentifiable
        {
            if (delta.Id == 0) return null;            
            var entity = await Find(delta.Id);
            if (entity != null)
            {
                return InnerSet((TEntity)delta.PatchTo(entity.Valuator).Devisor);
            }
            return null;
        }
       
        public virtual IEnumerable<TEntity>     Patch<TModel>(IEnumerable<TModel> entities) where TModel : class, IIdentifiable
        {
            var ids = entities.Select(e => e.Id).ToArray();
            var deck = query.WhereIn(p => p.Id, ids).ToDeck();
            foreach (var entity in entities)
            {
                var _entity = deck.Get(entity);
                if (_entity != null)
                {
                    entity.PatchTo(_entity.Valuator);
                    yield return InnerSet(_entity);
                }
            }
        }
        
        public virtual Task<TEntity>            Patch<TModel>(TModel delta, params object[] key) where TModel : class, IIdentifiable
        {
            return Task.Run(async () =>
            {
                if (key == null) return null;
                var entity = await Find(key);

                if (entity != null)
                {
                    return InnerSet((TEntity)delta.PatchTo(entity.Valuator, OnPatchComplete).Devisor);
                }

                return null;
            });
        }
        public virtual Task<TEntity>            Patch<TModel>(TModel delta, Func<TModel, Expression<Func<TEntity, bool>>> predicate) where TModel : class, IIdentifiable
        {
            return Task.Run(() =>
            {
                TEntity entity = null;
                if (predicate != null)
                {
                    entity = this[false, predicate.Invoke(delta)];
                }
                if (entity != null)
                {
                    return InnerSet((TEntity)delta.PatchTo(entity.Valuator).Devisor);
                }
                return default;
            });
        }
        public virtual IEnumerable<TEntity>     Patch<TModel>(IEnumerable<TModel> entities, Func<TModel, Expression<Func<TEntity, bool>>> predicate) where TModel : class, IIdentifiable
        {
            IDeck<TEntity> setters = null;
            if (predicate != null)
                setters = entities.Select( e => this[false, predicate
                                                .Invoke(e)]).ToDeck();
            if (setters == null)
            {
                var ids = entities.Select(i => i.Id).ToArray();
                setters = Query.WhereIn(q => q.Id, ids).ToDeck();
            }
            if (setters == null)
                yield return null;

            foreach (var entity in entities)
            {
                yield return InnerSet(((TEntity)entity.PutTo(setters[entity].Valuator).Devisor));
            }
        }

        public virtual Task<TEntity>           Put(TEntity entity, Func<TEntity, Expression<Func<TEntity, bool>>> predicate, params Func<TEntity, Expression<Func<TEntity, bool>>>[] conditions)
        {
            return Task.Run(async () =>
            {
                TEntity _entity = null;
                if (predicate != null)
                    _entity = Query.FirstOrDefault(
                              predicate.Invoke(entity));

                if (_entity == null)
                    _entity = await Find(entity.Id);

                if (conditions != null)
                    foreach (var condition in conditions)
                        if (!Query.Any(condition.Invoke(entity)))
                            return null;

                if (_entity == null)
                    return Add(entity);

                return InnerSet((TEntity)entity.PutTo(_entity.Valuator).Devisor);
            });
        }
        public virtual IEnumerable<TEntity>    Put(IEnumerable<TEntity> entities, Func<TEntity, Expression<Func<TEntity, bool>>> predicate, params Func<TEntity, Expression<Func<TEntity, bool>>>[] conditions)
        {
            IDeck<TEntity> setters = null;
            if (predicate != null)
                setters = entities.Select(e => 
                             Query.FirstOrDefault(
                                   predicate(e)))
                                  .ToDeck();
            if (setters == null)
            {
                var ids = entities.Select(i => i.Id).ToArray();
                setters = Query.WhereIn(q => q.Id, ids).ToDeck();
            }

            foreach (var entity in entities)
            { 
                foreach (var condition in conditions)                
                    if (!Query.Any(condition(entity)))
                        yield return null;                

                var settin = setters[entity];              
                if (settin != null)
                {
                    yield return InnerSet((TEntity)entity.PutTo(settin.Valuator).Devisor);
                }
                else
                    yield return Add(entity);
            }
        }

        #endregion
    }

}
