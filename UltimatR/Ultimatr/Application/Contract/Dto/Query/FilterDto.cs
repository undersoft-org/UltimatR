using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Series;
using System.Text.Json.Serialization;

namespace UltimatR
{
    public class FilterDto<TStore, TEntity, TDto> : Query<TStore, TEntity, IDeck<TDto>>
        where TEntity : Entity where TStore : IDataStore
    {
        public FilterDto(Expression<Func<TEntity, bool>> predicate) : base(predicate) { }
        public FilterDto(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] expanders) : base(predicate, expanders) { }
        public FilterDto(Expression<Func<TEntity, bool>> predicate, SortExpression<TEntity> sortTerms,
                                params Expression<Func<TEntity, object>>[] expanders) : base(predicate, sortTerms, expanders) { }
    }
}
    