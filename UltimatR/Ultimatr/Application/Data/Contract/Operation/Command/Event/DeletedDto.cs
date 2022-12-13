using System;
using System.Text.Json.Serialization;
using System.Linq.Expressions;

namespace UltimatR
{
    public class DeletedDto<TStore, TEntity, TDto>  : CommandEvent<DtoCommand<TDto>> where TEntity : Entity where TDto : Dto where TStore : IDataStore
    {
        [JsonIgnore] public Func<TEntity, Expression<Func<TEntity, bool>>> Predicate { get; }

        public DeletedDto(DeleteDto<TStore, TEntity, TDto> command) : base(command)
        {
            Predicate = command.Predicate;
        }
    }
}
