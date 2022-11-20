using FluentValidation.Results;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Json.Serialization;

namespace UltimatR
{
    public class ChangeDtoSet<TStore, TEntity, TDto>  : DtoCommandSet<TDto> where TEntity : Entity where TDto : Dto where TStore : IDataStore
    {       
        [JsonIgnore] public Func<TDto, Expression<Func<TEntity, bool>>> Predicate { get; }

        public ChangeDtoSet(PublishMode publishPattern, TDto input, object key)
         : base(CommandMode.Change, publishPattern, new[] { new ChangeDto<TStore, TEntity, TDto>(publishPattern, input, key) }) { }

        public ChangeDtoSet(PublishMode publishPattern, TDto[] inputs) 
            : base(CommandMode.Change, publishPattern, inputs.Select(c => new ChangeDto<TStore, TEntity, TDto>(publishPattern, c, c.Id)).ToArray()) { }
       
        public ChangeDtoSet(PublishMode publishPattern, TDto[] inputs, Func<TDto, Expression<Func<TEntity, bool>>> predicate)
           : base(CommandMode.Change, publishPattern, inputs.Select(c => new ChangeDto<TStore, TEntity, TDto>(publishPattern, c, predicate)).ToArray())
        {
            Predicate = predicate;
        }
    }
}
