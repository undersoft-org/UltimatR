//-----------------------------------------------------------------------
// <copyright file="DtoEventController.cs" company="Undersoft">
//     Author: Dariusz Hanc
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Json;
using System.Threading.Tasks;

namespace UltimatR
{
    [LinkedResult]
    [ApiController]
    public abstract class DtoEventController<TKey, TStore, TEntity, TDto> : ControllerBase where TEntity : Entity
        where TDto : Dto
        where TStore : IDataStore
    {
        protected readonly Func<TKey, Expression<Func<TEntity, bool>>> _keymatcher;
        protected readonly IUltimatr _ultimatr;

        protected DtoEventController(IUltimatr ultimatr) { _ultimatr = ultimatr; }

        protected DtoEventController(IUltimatr ultimatr, Func<TKey, Expression<Func<TEntity, bool>>> keymatcher)
        {
            _ultimatr = ultimatr;
            _keymatcher = keymatcher;
        }

        [HttpDelete]
        public virtual async Task<IActionResult> Delete([FromBody] TDto[] dtos)
        {
            bool isValid = false;

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            DtoCommandSet<TDto> result = await _ultimatr.Send(
                new DeleteDtoSet<TStore, TEntity, TDto>(PublishMode.PropagateCommand, dtos))
                .ConfigureAwait(false);

            object[] response = result.ForEach(c => (isValid = c.IsValid) ? (c.Id as object) : c.ErrorMessages)
                .ToArray();
            return (!isValid) ? UnprocessableEntity(response) : Ok(response);
        }

        [HttpDelete("{key}")]
        public virtual async Task<IActionResult> Delete(TKey key, [FromBody] TDto dto)
        {
            bool isValid = false;

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            DtoCommand<TDto> result = await _ultimatr.Send(
                new DeleteDto<TStore, TEntity, TDto>(PublishMode.PropagateCommand, dto, key))
                .ConfigureAwait(false);

            object response = result.IsValid ? (result.Id as object) : result.ErrorMessages;
            return (!isValid) ? UnprocessableEntity(response) : Ok(response);
        }

        [HttpGet]
        public virtual async Task<IActionResult> Get()
        { return Ok(await _ultimatr.Send(new GetDto<TStore, TEntity, TDto>()).ConfigureAwait(true)); }

        [HttpGet("{key}")]
        public virtual async Task<IActionResult> Get(TKey key)
        {
            Task<TDto> query = (_keymatcher == null)
                ? _ultimatr.Send(new FindDto<TStore, TEntity, TDto>(key))
                : _ultimatr.Send(new FindDto<TStore, TEntity, TDto>(_keymatcher(key)));

            return Ok(await query.ConfigureAwait(false));
        }

        [HttpPatch]
        public virtual async Task<IActionResult> Patch([FromBody] TDto[] dtos)
        {
            bool isValid = false;

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            DtoCommandSet<TDto> result = await _ultimatr.Send(
                new ChangeDtoSet<TStore, TEntity, TDto>(PublishMode.PropagateCommand, dtos))
                .ConfigureAwait(false);

            object[] response = result.ForEach(c => (isValid = c.IsValid) ? (c.Id as object) : c.ErrorMessages)
                .ToArray();
            return (!isValid) ? UnprocessableEntity(response) : Ok(response);
        }

        [HttpPatch("{key}")]
        public virtual async Task<IActionResult> Patch(TKey key, [FromBody] TDto dto)
        {
            bool isValid = false;

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            DtoCommand<TDto> result = await _ultimatr.Send(
                new ChangeDto<TStore, TEntity, TDto>(PublishMode.PropagateCommand, dto, key))
                .ConfigureAwait(false);

            object response = result.IsValid ? (result.Id as object) : result.ErrorMessages;
            return (!isValid) ? UnprocessableEntity(response) : Ok(response);
        }

        [HttpPost]
        public virtual async Task<IActionResult> Post([FromBody] TDto[] dtos)
        {
            bool isValid = false;

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            DtoCommandSet<TDto> result = await _ultimatr.Send(
                new CreateDtoSet<TStore, TEntity, TDto>(PublishMode.PropagateCommand, dtos))
                .ConfigureAwait(false);

            object[] response = result.ForEach(c => (isValid = c.IsValid) ? (c.Id as object) : c.ErrorMessages)
                .ToArray();
            return (!isValid) ? UnprocessableEntity(response) : Ok(response);
        }

        [HttpPost("query")]
        public virtual async Task<IActionResult> Post(QueryItems query)
        {
            query.Filter
                .ForEach(
                    (fi) => fi.Value =
                        JsonSerializer.Deserialize(
                            ((JsonElement)fi.Value).GetRawText(),
                            Type.GetType($"System.{fi.Type}", null, null, false, true)));

            return Ok(
                await _ultimatr.Send(
                    new FilterDto<TStore, TEntity, TDto>(
                        new FilterExpression<TEntity>(query.Filter).Create(),
                        new SortExpression<TEntity>(query.Sort)))
                    .ConfigureAwait(false));
        }

        [HttpPost("{Key}")]
        public virtual async Task<IActionResult> Post(TKey key, [FromBody] TDto dto)
        {
            bool isValid = false;

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            DtoCommand<TDto> result = await _ultimatr.Send(
                new CreateDto<TStore, TEntity, TDto>(PublishMode.PropagateCommand, dto, key))
                .ConfigureAwait(false);

            object response = result.IsValid ? (result.Id as object) : result.ErrorMessages;
            return (!isValid) ? UnprocessableEntity(response) : Ok(response);
        }

        [HttpPut]
        public virtual async Task<IActionResult> Put([FromBody] TDto[] dtos)
        {
            bool isValid = false;

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            DtoCommandSet<TDto> result = await _ultimatr.Send(
                new UpdateDtoSet<TStore, TEntity, TDto>(PublishMode.PropagateCommand, dtos))
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

            DtoCommand<TDto> result = await _ultimatr.Send(
                new UpdateDto<TStore, TEntity, TDto>(PublishMode.PropagateCommand, dto, key))
                .ConfigureAwait(false);

            object response = result.IsValid ? (result.Id as object) : result.ErrorMessages;
            return (!isValid) ? UnprocessableEntity(response) : Ok(response);
        }
    }
}
