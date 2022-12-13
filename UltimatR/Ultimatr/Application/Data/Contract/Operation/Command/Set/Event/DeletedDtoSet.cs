using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Json.Serialization;

namespace UltimatR
{
    public class DeletedDtoSet<TStore, TEntity, TDto>  : CommandEventSet<DtoCommand<TDto>> 
        where TEntity : Entity where TDto : Dto where TStore : IDataStore
    {  
        [JsonIgnore] public Func<TEntity, Expression<Func<TEntity, bool>>> Predicate { get; }

        public DeletedDtoSet(DeleteDtoSet<TStore, TEntity, TDto> commands) 
            : base(commands.PublishMode, commands.ForOnly(c => c.Entity != null, c => new DeletedDto<TStore, TEntity, TDto>
                                                        ((DeleteDto<TStore, TEntity, TDto>)c)).ToArray())
        {
            Predicate = commands.Predicate;           
        }
    }
}
