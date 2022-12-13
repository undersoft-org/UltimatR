using System;
using System.Linq.Expressions;
using System.Text.Json.Serialization;

namespace UltimatR
{
    public class DeleteDto<TStore, TEntity, TDto>  : DtoCommand<TDto> where TEntity : Entity where TDto : Dto where TStore : IDataStore
    {
        [JsonIgnore] public Func<TEntity,  Expression<Func<TEntity, bool>>> Predicate { get; }

        public DeleteDto(PublishMode publishPattern, TDto input) 
            : base(CommandMode.Delete, publishPattern, input)
        {
        }
        public DeleteDto(PublishMode publishPattern, TDto input, Func<TEntity, Expression<Func<TEntity, bool>>> predicate)
            : base(CommandMode.Delete, publishPattern, input)
        {
            Predicate = predicate;
        }
        public DeleteDto(PublishMode publishPattern, Func<TEntity, Expression<Func<TEntity, bool>>> predicate) : base(CommandMode.Delete, publishPattern)
        {
            Predicate = predicate;
        }
        public DeleteDto(PublishMode publishPattern, params object[] keys) 
            : base(CommandMode.Delete, publishPattern, keys)
        {
        }
    }
}
