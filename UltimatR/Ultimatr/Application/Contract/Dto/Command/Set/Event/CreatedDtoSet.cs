using System;
using System.Linq;
using System.Linq.Expressions;
using System.Series;
using System.Text.Json.Serialization;

namespace UltimatR
{
    public class CreatedDtoSet<TStore, TEntity, TDto>  : CommandEventSet<DtoCommand<TDto>> 
        where TEntity : Entity where TDto : Dto where TStore : IDataStore
    {  
        [JsonIgnore] public Func<TEntity, Expression<Func<TEntity, bool>>> Predicate { get; }

        public CreatedDtoSet(CreateDtoSet<TStore, TEntity, TDto> commands) 
            : base(commands.PublishMode, commands.ForOnly(c => c.Entity != null, c => new CreatedDto<TStore, TEntity, TDto>
            ((CreateDto<TStore, TEntity, TDto>)c)).ToArray())
        {
            Predicate = commands.Predicate;           
        }
    }
}
