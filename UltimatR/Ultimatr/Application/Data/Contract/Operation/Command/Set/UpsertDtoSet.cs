using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Json.Serialization;

namespace UltimatR
{
    public class UpsertDtoSet<TStore, TEntity, TDto>  : DtoCommandSet<TDto> where TEntity : Entity where TDto : Dto where TStore : IDataStore
    {
        [JsonIgnore] public Func<TEntity, Expression<Func<TEntity, bool>>> Predicate { get; }
        [JsonIgnore] public Func<TEntity, Expression<Func<TEntity, bool>>>[] Conditions { get; }

        public UpsertDtoSet(PublishMode publishPattern, TDto input, object key)
        : base(CommandMode.Change, publishPattern, new[] { new UpsertDto<TStore, TEntity, TDto>(publishPattern, input, e => e => e.Id == (long)key) }) { }

        public UpsertDtoSet(PublishMode publishPattern, TDto[] inputs, Func<TEntity, Expression<Func<TEntity, bool>>> predicate) 
            : base(CommandMode.Upsert, publishPattern, inputs.Select(input => new UpsertDto<TStore, TEntity, TDto>(publishPattern, input, predicate)).ToArray())
        {
            Predicate = predicate;
        }
        public UpsertDtoSet(PublishMode publishPattern, TDto[] inputs, Func<TEntity, Expression<Func<TEntity, bool>>> predicate, params Func<TEntity, Expression<Func<TEntity, bool>>>[] conditions)
           : base(CommandMode.Upsert, publishPattern, inputs.Select(input => new UpsertDto<TStore, TEntity, TDto>(publishPattern, input, predicate, conditions)).ToArray())
        {
            Predicate = predicate;
            Conditions = conditions;
        }
    }
}
