using System;
using System.Text.Json.Serialization;
using System.Linq.Expressions;

namespace UltimatR
{
    public class RenewedDto<TStore, TEntity, TDto>  : CommandEvent<DtoCommand<TDto>> where TEntity : Entity where TDto : Dto where TStore : IDataStore
    {
        [JsonIgnore] public Func<TEntity, Expression<Func<TEntity, bool>>> Predicate { get; }
        [JsonIgnore] public Func<TEntity, Expression<Func<TEntity, bool>>>[] Conditions { get; }

        public RenewedDto(RenewDto<TStore, TEntity, TDto> command) : base(command)
        {
            Conditions = command.Conditions;
            Predicate = command.Predicate;
        }
    }
}
