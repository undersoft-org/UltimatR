using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UltimatR
{
    [LinkedResult]
    [ApiController]
    public abstract class EventDbController<TKey, TStore, TEntity, TDto> 
        : ControllerBase where TEntity : Entity where TDto : Dto where TStore : IDataStore
    {
        protected readonly IUltimatr _ultimatr;
        protected readonly Func<TKey, Expression<Func<TEntity, bool>>> _keymatcher;

        protected EventDbController(IUltimatr ultimatr)
        {
            _ultimatr = ultimatr;            
        }

        protected EventDbController(IUltimatr ultimatr, Func<TKey, Expression<Func<TEntity, bool>>> keymatcher)
        {
            _ultimatr = ultimatr;
            _keymatcher = keymatcher;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _ultimatr.Send(new GetDataQuery<TStore, TEntity, TDto>())
                                                              .ConfigureAwait(false));
        }

        [HttpGet("{key}")]
        public async Task<IActionResult> Get(TKey key)
        {
            Task<TDto> query = (_keymatcher == null)
               ? _ultimatr.Send(new FindDataQuery<TStore, TEntity, TDto>(key))
               : _ultimatr.Send(new FindDataQuery<TStore, TEntity, TDto>(_keymatcher(key)));

            return Ok(await query.ConfigureAwait(false));
        }

        [HttpPost]
        public virtual async Task<IActionResult> Post([FromBody] TDto[] dtos)
        {
            bool isValid = false;

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _ultimatr.Send(new CreateDataCommandSet<TStore, TEntity, TDto>
                                                  (PublishMode.PropagateCommand, dtos))
                                                  .ConfigureAwait(false);

            var response = result.ForEach(c => (isValid = c.IsValid)
                                                  ? c.Id as object
                                                  : c.ErrorMessages).ToArray();
            return (!isValid)
                   ? UnprocessableEntity(response)
                   : Ok(response);
        }

        [HttpPost("{key}")]
        public virtual async Task<IActionResult> Post(TKey key, [FromBody] TDto dto)
        {
            bool isValid = false;

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _ultimatr.Send(new CreateDataCommandSet<TStore, TEntity, TDto>
                                                  (PublishMode.PropagateCommand, new[] { dto }))
                                                  .ConfigureAwait(false);

            var response = result.ForEach(c => (isValid = c.IsValid)
                                                  ? c.Id as object
                                                  : c.ErrorMessages).ToArray();
            return (!isValid)
                   ? UnprocessableEntity(response)
                   : Ok(response);
        }

        [HttpPatch("{key}")]
        public virtual async Task<IActionResult> Patch(TKey key, [FromBody] TDto dto)
        {
            bool isValid = false;

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _ultimatr.Send(new ChangeDataCommandSet<TStore, TEntity, TDto>
                                                 (PublishMode.PropagateCommand, new[] { dto }))
                                                  .ConfigureAwait(false);

            var response = result.ForEach(c => (isValid = c.IsValid)
                                                  ? c.Id as object
                                                  : c.ErrorMessages).ToArray();
            return (!isValid)
                   ? UnprocessableEntity(response)
                   : Ok(response);
        }

        [HttpPut]
        public virtual async Task<IActionResult> Put([FromBody] TDto[] dtos)
        {
            bool isValid = false;

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _ultimatr.Send(new UpdateDataCommandSet<TStore, TEntity, TDto>
                                                  (PublishMode.PropagateCommand, dtos))
                                                  .ConfigureAwait(false);

            var response = result.ForEach(c => (isValid = c.IsValid)
                                                  ? c.Id as object
                                                  : c.ErrorMessages).ToArray();
            return (!isValid)
                   ? UnprocessableEntity(response)
                   : Ok(response);
        }

        [HttpPut("{key}")]
        public virtual async Task<IActionResult> Put(TKey key, [FromBody] TDto dto)
        {
            bool isValid = false;

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _ultimatr.Send(new UpdateDataCommandSet<TStore, TEntity, TDto>
                                                    (PublishMode.PropagateCommand, new[] { dto }))
                                                    .ConfigureAwait(false);

            var response = result.ForEach(c => (isValid = c.IsValid)
                                                  ? c.Id as object
                                                  : c.ErrorMessages).ToArray();
            return (!isValid)
                   ? UnprocessableEntity(response)
                   : Ok(response);
        }

        [HttpDelete]
        public virtual async Task<IActionResult> Delete([FromBody] TDto[] dtos)
        {
            bool isValid = false;

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _ultimatr.Send(new DeleteDataCommandSet<TStore, TEntity, TDto>
                                                  (PublishMode.PropagateCommand, dtos))
                                                  .ConfigureAwait(false);

            var response = result.ForEach(c => (isValid = c.IsValid)
                                                  ? c.Id as object
                                                  : c.ErrorMessages).ToArray();
            return (!isValid)
                   ? UnprocessableEntity(response)
                   : Ok(response);
        }

        [HttpDelete("{key}")]
        public virtual async Task<IActionResult> Delete(TKey key, [FromBody] TDto dto)
        {
            bool isValid = false;

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _ultimatr.Send(new DeleteDataCommandSet<TStore, TEntity, TDto>
                                                    (PublishMode.PropagateCommand, new[] { dto }))
                                                    .ConfigureAwait(false);

            var response = result.ForEach(c => (isValid = c.IsValid)
                                                  ? c.Id as object
                                                  : c.ErrorMessages).ToArray();
            return (!isValid)
                   ? UnprocessableEntity(response)
                   : Ok(response);
        }
    }
}
