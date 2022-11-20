using System;
using System.Linq.Expressions;
using System.Text.Json.Serialization;

namespace UltimatR
{
    public class CreateDto<TStore, TEntity, TDto>  : DtoCommand<TDto> 
        where TEntity : Entity where TDto : Dto where TStore : IDataStore
    {
        [JsonIgnore] public Func<TEntity, Expression<Func<TEntity, bool>>> Predicate { get; }

        public CreateDto(PublishMode publishPattern, TDto input) 
            : base(CommandMode.Create, publishPattern, input)
        {
            input.AutoId();
        }
        public CreateDto(PublishMode publishPattern, TDto input, object key)
          : base(CommandMode.Create, publishPattern, input)
        {
            input.SetId(key);
        }
        public CreateDto(PublishMode publishPattern, TDto input, Func<TEntity, Expression<Func<TEntity, bool>>> predicate) 
            : base(CommandMode.Create, publishPattern, input)
        {
            input.AutoId();
            Predicate = predicate;
        }
    }
}
