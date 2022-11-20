//-----------------------------------------------------------------------
// <copyright file="DtoCommandController.cs" company="Undersoft">
//     Author: Dariusz Hanc
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace UltimatR
{
    [LinkedResult]
    [ApiController]
    public abstract class DtoCommandController<TKey, TStore, TEntity, TDto> : ControllerBase where TEntity : Entity
        where TDto : Dto
        where TStore : IDataStore
    {
        protected Func<TKey, Expression<Func<TEntity, bool>>> _keymatcher = k => e => k.Equals(e.Id);
        protected Func<TDto, Expression<Func<TEntity, bool>>> _predicate = p => e => p.Id == e.Id;
        protected IUltimateService _ultimatr;

        protected DtoCommandController(IUltimatr ultimatr) { _ultimatr = ultimatr; }

        protected DtoCommandController(IUltimatr ultimatr, Func<TKey, Expression<Func<TEntity, bool>>> keymatcher) 
            : this(ultimatr) { _keymatcher = keymatcher; }

        protected DtoCommandController(
            IUltimatr ultimatr,
            Func<TDto, Expression<Func<TEntity, bool>>> predicate,
            Func<TKey, Expression<Func<TEntity, bool>>> keymatcher) : this(ultimatr, keymatcher) { _predicate = predicate; }

        [HttpDelete]
        public virtual async Task<IActionResult> Delete([FromBody] TDto[] dtos)
        {
            bool isValid = false;

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            DtoCommandSet<TDto> result = await _ultimatr.Send(new DeleteDtoSet<TStore, TEntity, TDto>
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

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _ultimatr.Send(new DeleteDtoSet<TStore, TEntity, TDto>
                                                                 (PublishMode.PropagateCommand, dto, key))
                                                                        .ConfigureAwait(false);

            var response = result.ForEach(c => (isValid = c.IsValid)
                                                   ? c.Id as object
                                                   : c.ErrorMessages).ToArray();
            return (!isValid)
                   ? UnprocessableEntity(response)
                   : Ok(response);
        }

        [HttpPatch]
        public virtual async Task<IActionResult> Patch([FromBody] TDto[] dtos)
        {
            bool isValid = false;

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _ultimatr.Send(new ChangeDtoSet<TStore, TEntity, TDto>
                                                                    (PublishMode.PropagateCommand, dtos))
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

            var result = await _ultimatr.Send(new ChangeDtoSet<TStore, TEntity, TDto>
                                                  (PublishMode.PropagateCommand, dto, key))
                                                     .ConfigureAwait(false);

            var response = result.ForEach(c => (isValid = c.IsValid)
                                                  ? c.Id as object
                                                  : c.ErrorMessages).ToArray();
            return (!isValid)
                   ? UnprocessableEntity(response)
                   : Ok(response);
        }

        [HttpPost]
        public virtual async Task<IActionResult> Post([FromBody] TDto[] dtos)
        {
            bool isValid = false;

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            DtoCommandSet<TDto> result = await _ultimatr.Send(new CreateDtoSet<TStore, TEntity, TDto>
                                                        (PublishMode.PropagateCommand, dtos)).ConfigureAwait(false);

            object[] response = result.ForEach(c => (isValid = c.IsValid) ? (c.Id as object) : c.ErrorMessages)
                .ToArray();
            return (!isValid) ? UnprocessableEntity(response) : Ok(response);
        }

        [HttpPost("{Key}")]
        public virtual async Task<IActionResult> Post(TKey key, [FromBody] TDto dto)
        {
            bool isValid = false;

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _ultimatr.Send(new CreateDtoSet<TStore, TEntity, TDto>
                                                    (PublishMode.PropagateCommand, dto, key))
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

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            DtoCommandSet<TDto> result = await _ultimatr.Send(new UpdateDtoSet<TStore, TEntity, TDto>
                                                                        (PublishMode.PropagateCommand, dtos))
                                                                                    .ConfigureAwait(false);

            object[] response = result.ForEach(c => (isValid = c.IsValid) ? (c.Id as object) : c.ErrorMessages)
                .ToArray();
            return (!isValid) ? UnprocessableEntity(response) : Ok(response);
        }

        [HttpPut("{key}")]
        public virtual async Task<IActionResult> Put(TKey key, [FromBody] TDto dto)
        {
            bool isValid = false;

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _ultimatr.Send(new UpdateDtoSet<TStore, TEntity, TDto>
                                                        (PublishMode.PropagateCommand, dto, key))
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
