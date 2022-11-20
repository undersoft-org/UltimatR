using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Logs;
using System.Series;

namespace UltimatR
{
    public class GetDto<TStore, TEntity, TDto> : Query<TStore, TEntity, IDeck<TDto>>
        where TEntity : Entity where TStore : IDataStore
    {
        public GetDto(params Expression<Func<TEntity, object>>[] expanders) : base(expanders) { }

        public GetDto(SortExpression<TEntity> sortTerms, params Expression<Func<TEntity, object>>[] expanders) : base(sortTerms, expanders) { }
    }
}
