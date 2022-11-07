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

        public virtual Task<TEntity> Map<TDto>(TDto model, TEntity entity)
        {
            return Task.Run(() =>
                Mapper.Map(model, entity), Cancellation);
        }
        public virtual Task<TDto> Map<TDto>(TEntity entity, TDto model)
        {
            return Task.Run(() =>
                Mapper.Map(entity, model), Cancellation);
        }
        public virtual Task<IList<TEntity>> Map<TDto>(IEnumerable<TDto> model, IEnumerable<TEntity> entity)
        {
            return Task.Run(() => (IList<TEntity>)
                Mapper.Map(model, entity).ToList(), Cancellation);
        }
        public virtual Task<IList<TDto>> Map<TDto>(IEnumerable<TEntity> entity, IEnumerable<TDto> model)
        {
            return Task.Run(() =>
              (IList<TDto>)(Mapper.Map(entity, model).ToList()), Cancellation);
        }

        public virtual Task<IDeck<TEntity>> HashMap<TDto>(IEnumerable<TDto> model, IEnumerable<TEntity> entity)
        {
            return Task.Run(() => (IDeck<TEntity>)
                Mapper.Map(model, entity).ToAlbum(), Cancellation);
        }
        public virtual Task<IDeck<TDto>> HashMap<TDto>(IEnumerable<TEntity> entity, IEnumerable<TDto> model)
        {
            return Task.Run(() =>
              (IDeck<TDto>)(Mapper.Map(entity, model).ToAlbum()), Cancellation);
        }

        public virtual Task<TDto> MapTo<TDto>(TEntity entity)
        {
            return Task.Run(() =>
                Mapper.Map<TEntity, TDto>(entity), Cancellation);
        }
        public virtual Task<TDto> MapTo<TDto>(object entity)
        {
            return Task.Run(() =>
                Mapper.Map<TDto>(entity), Cancellation);
        }
        public virtual Task<TEntity> MapFrom<TDto>(TDto model)
        {
            return Task.Run(() =>
                Mapper.Map<TDto, TEntity>(model), Cancellation);
        }
        public virtual Task<TDto> MapFrom<TDto>(object model)
        {
            return Task.Run(() =>
                Mapper.Map<TDto>(model), Cancellation);
        }
        public virtual Task<IList<TDto>> MapTo<TDto>(IEnumerable<object> entity)
        {
            return Task.Run(() =>
                (Mapper.Map<IList<TDto>>(entity.Commit())), Cancellation);
        }
        public virtual Task<IList<TDto>> MapTo<TDto>(IEnumerable<TEntity> entity)
        {
            return Task.Run(() =>
                Mapper.Map<IList<TDto>>(entity.Commit()), Cancellation);
        }
        public virtual Task<IList<TEntity>> MapFrom<TDto>(IEnumerable<TDto> model)
        {
            return Task.Run(() =>
                Mapper.Map<TDto[], IList<TEntity>>(model.Commit()), Cancellation);
        }

        public virtual Task<IDeck<TDto>> HashMapTo<TDto>(IEnumerable<object> entity)
        {
            return Task.Run(() =>
                (IDeck<TDto>)(Mapper.Map<IEnumerable<TDto>>(entity.ToArray())).ToAlbum(), Cancellation);
        }
        public virtual Task<IDeck<TDto>> HashMapTo<TDto>(IEnumerable<TEntity> entity)
        {
            return Task.Run(() =>
                (IDeck<TDto>)(Mapper.Map<IEnumerable<TDto>>(entity.ToArray())).ToAlbum(), Cancellation);
        }
        public virtual Task<IDeck<TEntity>> HashMapFrom<TDto>(IEnumerable<TDto> model)
        {
            return Task.Run(() =>
                (IDeck<TEntity>)(Mapper.Map<IEnumerable<TDto>, IEnumerable<TEntity>>(model.ToArray())).ToAlbum(), Cancellation);
        }

        #endregion

    }
}
