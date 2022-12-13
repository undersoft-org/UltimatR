using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Json.Serialization;

namespace UltimatR
{
    public class CreateDtoSet<TStore, TEntity, TDto>  : DtoCommandSet<TDto> where TEntity : Entity where TDto : Dto where TStore : IDataStore
    {       
        [JsonIgnore] public Func<TEntity, Expression<Func<TEntity, bool>>> Predicate { get; }

        public CreateDtoSet(PublishMode publishPattern, TDto input, object key)
       : base(CommandMode.Create, publishPattern, new[] { new CreateDto<TStore, TEntity, TDto>(publishPattern, input, key) }) { }

        public CreateDtoSet(PublishMode publishPattern, TDto[] inputs) 
            : base(CommandMode.Create, publishPattern, inputs.Select(input => new CreateDto<TStore, TEntity, TDto>(publishPattern, input)).ToArray())
        {
        }
        public CreateDtoSet(PublishMode publishPattern, TDto[] inputs, Func<TEntity, Expression<Func<TEntity, bool>>> predicate)
           : base(CommandMode.Create, publishPattern, inputs.Select(input => new CreateDto<TStore, TEntity, TDto>(publishPattern, input, predicate)).ToArray())
        {
            Predicate = predicate;
        }
    }
}
