//-----------------------------------------------------------------------
// <copyright file="DtoQueryController.cs" company="Undersoft">
//     Author: Dariusz Hanc
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq.Expressions;
using System.Text.Json;
using System.Threading.Tasks;

namespace UltimatR
{
    [LinkedResult]
    [ApiController]
    public abstract class DtoQueryController<TKey, TStore, TEntity, TDto> : ControllerBase where TEntity : Entity
        where TDto : Dto
        where TStore : IDataStore
    {
        protected readonly Func<TKey, Expression<Func<TEntity, bool>>> _keymatcher;
        protected readonly IUltimatr _ultimatr;

        protected DtoQueryController(IUltimatr ultimatr) { _ultimatr = ultimatr; }

        protected DtoQueryController(Func<TKey, Expression<Func<TEntity, bool>>> keymatcher, IUltimatr ultimatr)
        {
            _ultimatr = ultimatr;
            _keymatcher = keymatcher;
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
    }
}
