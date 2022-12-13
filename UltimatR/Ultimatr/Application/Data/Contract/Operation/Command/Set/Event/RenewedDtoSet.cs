using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Json.Serialization;

namespace UltimatR
{
    public class RenewedDtoSet<TStore, TEntity, TDto>  : CommandEventSet<DtoCommand<TDto>> 
        where TEntity : Entity where TDto : Dto where TStore : IDataStore
    {
        [JsonIgnore] public Func<TEntity, Expression<Func<TEntity, bool>>> Predicate { get; }
        [JsonIgnore] public Func<TEntity, Expression<Func<TEntity, bool>>>[] Conditions { get; }

        public RenewedDtoSet(UpsertDtoSet<TStore, TEntity, TDto> commands) 
            : base(commands.PublishMode, commands.ForOnly(c => c.Entity != null, c => new UpsertedDto<TStore, TEntity, TDto>
            ((UpsertDto<TStore, TEntity, TDto>)c)).ToArray())
        {            
            Conditions = commands.Conditions;
            Predicate = commands.Predicate;
        }
    }
}
