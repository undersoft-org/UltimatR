using System;
using System.Linq.Expressions;
using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

namespace UltimatR
{
    [LinkedResult]
    [ApiController]
    public abstract class QueryDbController<TKey, TStore, TEntity, TDto> : ControllerBase where TEntity : Entity where TDto : Dto where TStore : IDataStore
    {
        protected readonly IUltimatr _ultimatr;
        protected readonly Func<TKey, Expression<Func<TEntity, bool>>> _keymatcher;

        protected QueryDbController(IUltimatr ultimatr)
        {
            _ultimatr = ultimatr;    
        }

        protected QueryDbController(Func<TKey, Expression<Func<TEntity, bool>>> keymatcher, IUltimatr ultimatr)
        {
            _ultimatr = ultimatr;
            _keymatcher = keymatcher;
        }

        [HttpGet]
        public virtual async Task<IActionResult> Get()
        {
            return Ok(await _ultimatr.Send(new GetDataQuery<TStore, TEntity, TDto>())
                                                              .ConfigureAwait(true));
        }

        [HttpGet("{key}")]
        public virtual async Task<IActionResult> Get(TKey key)
        {
            Task<TDto> query = (_keymatcher == null)
                ? _ultimatr.Send(new FindDataQuery<TStore, TEntity, TDto>(key))
                : _ultimatr.Send(new FindDataQuery<TStore, TEntity, TDto>(_keymatcher(key)));
            
            return Ok(await query.ConfigureAwait(false));
        }

        [HttpPost("query")]
        public virtual async Task<IActionResult> Post(QueryItems query)
        {
            query.Filter.ForEach((fi) => fi.Value = JsonSerializer.Deserialize((
                                                    (JsonElement)fi.Value).GetRawText(), 
                                                    Type.GetType("System."+ fi.Type, 
                                                        null, null, false, true)));

            return Ok(await _ultimatr.Send(new FilterDataQuery<TStore, TEntity, TDto>(
                                           new FilterExpression<TEntity>(query.Filter).Create(), 
                                           new SortExpression<TEntity>(query.Sort)))
                                               .ConfigureAwait(false));           
        }
    }
}
