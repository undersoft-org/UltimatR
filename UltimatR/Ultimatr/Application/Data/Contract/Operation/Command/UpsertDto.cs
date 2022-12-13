using System;
using System.Linq.Expressions;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace UltimatR
{
    public class UpsertDto<TStore, TEntity, TDto>  : DtoCommand<TDto> where TEntity : Entity where TDto : Dto where TStore : IDataStore
    {
        [JsonIgnore] public Func<TEntity, Expression<Func<TEntity, bool>>> Predicate { get; }
        [JsonIgnore] public Func<TEntity, Expression<Func<TEntity, bool>>>[] Conditions { get; }

        public UpsertDto(PublishMode publishPattern, TDto input, Func<TEntity, Expression<Func<TEntity, bool>>> predicate) 
            : base(CommandMode.Upsert, publishPattern, input)
        {
            Predicate = predicate;
        }
        public UpsertDto(PublishMode publishPattern, TDto input, Func<TEntity, Expression<Func<TEntity, bool>>> predicate, params Func<TEntity, Expression<Func<TEntity, bool>>>[] conditions)
            : base(CommandMode.Upsert, publishPattern, input)
        {
            Predicate = predicate;
            Conditions = conditions;
        }
    }
}
