using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Json.Serialization;

namespace UltimatR
{
    public class ChangedDtoSet<TStore, TEntity, TDto>  : CommandEventSet<DtoCommand<TDto>> 
        where TEntity : Entity where TDto : Dto where TStore : IDataStore
    {  
        [JsonIgnore] public Func<TDto, Expression<Func<TEntity, bool>>> Predicate { get;   }

        public ChangedDtoSet(ChangeDtoSet<TStore, TEntity, TDto> commands) 
            : base(commands.PublishMode, commands.ForOnly(c => c.Entity != null, c => new ChangedDto<TStore, TEntity, TDto>
            ((ChangeDto<TStore, TEntity, TDto>)c)).ToArray())
        {
            Predicate = commands.Predicate;           
        }
    }
}
