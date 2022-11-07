using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Series;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace UltimatR
{
    public partial class HostRepository<TEntity>
    {
        #region Methods

        public virtual async Task<TEntity> AddBy<TDto>(TDto model)
        {
            return await Task.Run(async () => base.Add(await MapFrom(model)));
        }
        public virtual async Task<IEnumerable<TEntity>> AddBy<TDto>(IEnumerable<TDto> model)
        {            
            return base.Add(await MapFrom(model));            
        }
        public virtual async Task<TEntity> AddBy<TDto>(TDto model,Func<TEntity, Expression<Func<TEntity, bool>>> predicate)
        {
            return base.Add(await MapFrom(model), predicate);
        }
        public virtual async Task<IEnumerable<TEntity>> AddBy<TDto>(IEnumerable<TDto> models, Func<TEntity, Expression<Func<TEntity, bool>>> predicate)
        {
            return await base.Add(await MapFrom(models), predicate).CommitAsync();         
        }

        public virtual async Task<TEntity>  DeleteBy<TDto>(TDto model)
        {
            return base.Delete(await MapFrom(model));
        }
        public virtual IEnumerable<TEntity> DeleteBy<TDto>(IEnumerable<TDto> model)
        {
            foreach (var m in model)
            {
                var a = MapFrom(m);
                a.Wait();
                yield return Delete(a.Result);
            }
        }
        public virtual async Task<TEntity>  DeleteBy<TDto>(TDto model, Func<TEntity, Expression<Func<TEntity, bool>>> predicate)
        {
            return base.Delete(predicate.Invoke(await MapFrom(model)));
        }
        public virtual IEnumerable<TEntity> DeleteBy<TDto>(IEnumerable<TDto> model, Func<TEntity, Expression<Func<TEntity, bool>>> predicate)
        {
            foreach (var m in model)
                yield return DeleteBy(m, predicate).Result;
        }

        public virtual async Task<TEntity>  SetBy<TDto>(TDto model) where TDto : class, IIdentifiable
        {
            return await base.Set(model);
        }
        public virtual IEnumerable<TEntity> SetBy<TDto>(IEnumerable<TDto> entity) where TDto : class, IIdentifiable
        {
            return base.Set(entity).Commit();
        }
        public virtual async Task<TEntity>  SetBy<TDto>(TDto model, params object[] keys) where TDto : class, IIdentifiable
        {
            return await base.Set(model, keys);
        }
        public virtual async Task<TEntity>  SetBy<TDto>(TDto model, Func<TDto, Expression<Func<TEntity, bool>>> predicate, params Func<TDto, Expression<Func<TEntity, bool>>>[] conditions) where TDto : class, IIdentifiable
        {
            return await base.Set(model, predicate(model), conditions.ForEach(c => c(model)));
        }
        public virtual IEnumerable<TEntity> SetBy<TDto>(IEnumerable<TDto> models, Func<TDto, Expression<Func<TEntity, bool>>> predicate, params Func<TDto, Expression<Func<TEntity, bool>>>[] conditions) where TDto : class, IIdentifiable
        {
            return base.Set(models, predicate, conditions).Commit();
        }
        
        public virtual async Task<TEntity>  PutBy<TDto>(TDto model, Func<TEntity, Expression<Func<TEntity, bool>>> predicate, params Func<TEntity, Expression<Func<TEntity, bool>>>[] conditions)
        {
            return await base.Put(await MapFrom(model), predicate, conditions); 
        }
        public virtual IEnumerable<TEntity> PutBy<TDto>(IEnumerable<TDto> model, Func<TEntity, Expression<Func<TEntity, bool>>> predicate, params Func<TEntity, Expression<Func<TEntity, bool>>>[] conditions)
        {
            var map = MapFrom(model);
            map.Wait(); 
            return base.Put(map.Result , predicate, conditions).Commit();
        }

        public virtual async Task<TEntity>  PatchBy<TDto>(TDto model) where TDto : class, IIdentifiable
        {
            return await base.Patch(model);
        }
        public virtual IEnumerable<TEntity> PatchBy<TDto>(IEnumerable<TDto> entity) where TDto : class, IIdentifiable
        {
            return base.Patch(entity).Commit();
        }
        public virtual async Task<TEntity>  PatchBy<TDto>(TDto model, params object[] keys) where TDto : class, IIdentifiable
        {
            return await base.Patch(model, keys);
        }
        public virtual async Task<TEntity>  PatchBy<TDto>(TDto model, Func<TDto, Expression<Func<TEntity, bool>>> predicate) where TDto : class, IIdentifiable
        {
            return await base.Patch(model, predicate);
        }
        public virtual IEnumerable<TEntity> PatchBy<TDto>(IEnumerable<TDto> models, Func<TDto, Expression<Func<TEntity, bool>>> predicate) where TDto : class, IIdentifiable
        {
            return base.Patch(models, predicate).Commit();
        }
        #endregion

    }
}
