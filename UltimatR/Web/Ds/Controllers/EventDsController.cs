using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Attributes;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Uniques;

namespace UltimatR
{
    [LinkedResult]
    [ODataAttributeRouting]
    public abstract class EventDsController<TKey, TStore, TEntity> : ODataController where TEntity : Entity where TStore : IDataStore
    {
        protected readonly Func<TKey, Expression<Func<TEntity, bool>>> _keymatcher;
        protected readonly IUltimateService _uservice;

        protected EventDsController() { }
        protected EventDsController(IUltimateService uservice) : this(uservice, k => e => k.Equals(e.Id))
        {
        }
        protected EventDsController(IUltimateService uservice, Func<TKey, Expression<Func<TEntity, bool>>> keymatcher)
        {
            _keymatcher = keymatcher;
            _uservice = uservice;
        }

        [EnableQuery]
        [IgnoreApi]
        [HttpGet]
        public virtual IQueryable<TEntity> Get()
        {
            return _uservice.Use<TStore, TEntity>().AsQueryable();
        }

        [EnableQuery]
        [IgnoreApi]
        [HttpGet]
        public virtual UniqueOne<TEntity> Get([FromODataUri] TKey key)
        {
            return _uservice.Use<TStore, TEntity>()[_keymatcher(key)].AsUniqueOne();
        }

        [IgnoreApi]
        [HttpPost]
        public virtual async Task<IActionResult> Post(TEntity entity)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Created(await _uservice.Send(new CreateEntityCommand<TStore, TEntity>
                                                    (PublishMode.PropagateCommand, entity))
                                                    .ConfigureAwait(false));
        }

        [IgnoreApi]
        [HttpPatch]
        public virtual async Task<IActionResult> Patch([FromODataUri] TKey key, TEntity entity)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Updated(await _uservice.Send(new ChangeEntityCommand<TStore, TEntity>
                                                    (PublishMode.PropagateCommand,
                                                    entity.Sign<TEntity>(key)))
                                                    .ConfigureAwait(false));
        }

        [IgnoreApi]
        [HttpPut]
        public virtual async Task<IActionResult> Put([FromODataUri] TKey key, TEntity entity)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Updated(await _uservice.Send(new UpdateEntityCommand<TStore, TEntity>
                                                    (PublishMode.PropagateCommand,
                                                    entity.Sign<TEntity>(key)))
                                                    .ConfigureAwait(false));
        }

        [IgnoreApi]
        [HttpDelete]
        public virtual async Task<IActionResult> Delete([FromODataUri] TKey key)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(await _uservice.Send(new DeleteEntityCommand<TStore, TEntity>
                                                    (PublishMode.PropagateCommand, key))
                                                    .ConfigureAwait(false));
        }
    }
}
