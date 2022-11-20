using System;
using System.Linq.Expressions;
using System.Text.Json.Serialization;

namespace UltimatR
{
    public class FindDto<TStore, TEntity, TDto> : Query<TStore, TEntity, TDto>
        where TEntity : Entity where TStore : IDataStore
    {
        public FindDto(params object[] keys) : base(keys) { }
        public FindDto(object[] keys, params Expression<Func<TEntity, object>>[] expanders) : base(keys, expanders) { }
        public FindDto(Expression<Func<TEntity, bool>> predicate) : base(predicate) { }
        public FindDto(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] expanders) : base(predicate, expanders) { }
    }
}
