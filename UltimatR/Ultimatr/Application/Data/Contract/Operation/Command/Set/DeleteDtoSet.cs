using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Json.Serialization;

namespace UltimatR
{
    public class DeleteDtoSet<TStore, TEntity, TDto>  : DtoCommandSet<TDto> where TEntity : Entity where TDto : Dto where TStore : IDataStore
    {        
        [JsonIgnore] public Func<TEntity, Expression<Func<TEntity, bool>>> Predicate { get; }

        public DeleteDtoSet(PublishMode publishPattern, TDto input, object key)
        : base(CommandMode.Create, publishPattern, new[] { new DeleteDto<TStore, TEntity, TDto>(publishPattern, input, key) }) { }


        public DeleteDtoSet(PublishMode publishPattern, TDto[] inputs) 
            : base(CommandMode.Delete, publishPattern, inputs.Select(input => new DeleteDto<TStore, TEntity, TDto>(publishPattern, input)).ToArray())
        {
        }
        public DeleteDtoSet(PublishMode publishPattern, TDto[] inputs, Func<TEntity, Expression<Func<TEntity, bool>>> predicate)
           : base(CommandMode.Delete, publishPattern, inputs.Select(input => new DeleteDto<TStore, TEntity, TDto>(publishPattern, input, predicate)).ToArray())
        {
            Predicate = predicate;
        }
    }
}
