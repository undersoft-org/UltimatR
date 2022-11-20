﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Attributes;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Uniques;

namespace UltimatR
{
    [LinkedResult]
    [ODataAttributeRouting]
    public abstract class DsoController<TKey, TStore, TEntity> : ODataController, IDsoController<TKey, TStore, TEntity> where TEntity : Entity where TStore : IDataStore
    {
        protected readonly Func<TKey, Expression<Func<TEntity, bool>>> _keymatcher;
        protected readonly IUltimatr _ultimatr;

        protected DsoController() { }
        protected DsoController(IUltimatr ultimatr) : this(ultimatr, k => e => k.Equals(e.Id))
        {
        }
        protected DsoController(IUltimatr ultimatr, Func<TKey, Expression<Func<TEntity, bool>>> keymatcher)
        {
            _keymatcher = keymatcher;
            _ultimatr = ultimatr;
        }

        [EnableQuery]
        [IgnoreApi]
        [HttpGet]
        public virtual IQueryable<TEntity> Get()
        {
            return _ultimatr.Use<TStore, TEntity>().AsQueryable();
        }

        [EnableQuery]
        [IgnoreApi]
        [HttpGet]
        public virtual UniqueOne<TEntity> Get([FromODataUri] TKey key)
        {
            return _ultimatr.Use<TStore, TEntity>()[_keymatcher(key)].AsUniqueOne();
        }

        [IgnoreApi]
        [HttpPost]
        public virtual async Task<IActionResult> Post(TEntity entity)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Created(await _ultimatr.Send(new CreateDso<TStore, TEntity>
                                                    (PublishMode.PropagateCommand, entity))
                                                    .ConfigureAwait(false));
        }

        [IgnoreApi]
        [HttpPatch]
        public virtual async Task<IActionResult> Patch([FromODataUri] TKey key, TEntity entity)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Updated(await _ultimatr.Send(new ChangeDso<TStore, TEntity>
                                                    (PublishMode.PropagateCommand, entity, key))
                                                    .ConfigureAwait(false));
        }

        [IgnoreApi]
        [HttpPut]
        public virtual async Task<IActionResult> Put([FromODataUri] TKey key, TEntity entity)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Updated(await _ultimatr.Send(new UpdateDso<TStore, TEntity>
                                                     (PublishMode.PropagateCommand, entity, key))
                                                    .ConfigureAwait(false));
        }

        [IgnoreApi]
        [HttpDelete]
        public virtual async Task<IActionResult> Delete([FromODataUri] TKey key)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(await _ultimatr.Send(new DeleteDso<TStore, TEntity>
                                                    (PublishMode.PropagateCommand, key))
                                                    .ConfigureAwait(false));
        }
    }
}