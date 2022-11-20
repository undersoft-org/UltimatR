using System;
using System.Linq.Expressions;
using System.Text.Json.Serialization;

namespace UltimatR
{
    public class UpdateDto<TStore, TEntity, TDto>  : DtoCommand<TDto> where TEntity : Entity where TDto : Dto where TStore : IDataStore
    {
        [JsonIgnore] public Func<TEntity, Expression<Func<TEntity, bool>>> Predicate { get; }
        [JsonIgnore] public Func<TEntity, Expression<Func<TEntity, bool>>>[] Conditions { get; }

        public UpdateDto(PublishMode publishPattern, TDto input, params object[] keys)
           : base(CommandMode.Update, publishPattern, input, keys)
        {
        }
        
        public UpdateDto(PublishMode publishPattern, TDto input, Func<TEntity, 
                                Expression<Func<TEntity, bool>>> predicate) 
            : base(CommandMode.Update, publishPattern, input)
        {
            Predicate = predicate;
        }
        
        public UpdateDto(PublishMode publishPattern, TDto input, Func<TEntity, Expression<Func<TEntity, bool>>> predicate, 
                                params Func<TEntity, Expression<Func<TEntity, bool>>>[] conditions) 
            : base(CommandMode.Update, publishPattern, input)
        {
            Predicate = predicate;
            Conditions = conditions;
        }
    }
}
