using System;
using System.Linq.Expressions;
using System.Text.Json.Serialization;

namespace UltimatR
{
    public class ChangeDto<TStore, TEntity, TDto>  : DtoCommand<TDto> where TEntity : Entity where TDto : Dto where TStore : IDataStore
    {       
        [JsonIgnore] public Func<TDto, Expression<Func<TEntity, bool>>> Predicate { get; }

        public ChangeDto(PublishMode publishMode, TDto input, params object[] keys) 
            : base(CommandMode.Change, publishMode, input, keys)
        {
        }
        public ChangeDto(PublishMode publishMode, TDto input, Func<TDto, Expression<Func<TEntity, bool>>> predicate) 
            : base(CommandMode.Change, publishMode, input)
        {
            Predicate = predicate;
        }
    }
}
