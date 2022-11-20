//-----------------------------------------------------------------------
// <copyright file="UpdatedDtoEventSet.cs" company="Undersoft">
//     Author: Dariusz Hanc
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Json.Serialization;

namespace UltimatR
{
    public class UpdatedDtoSet<TStore, TEntity, TDto> : CommandEventSet<DtoCommand<TDto>> where TEntity : Entity
        where TDto : Dto
        where TStore : IDataStore
    {
        public UpdatedDtoSet(UpdateDtoSet<TStore, TEntity, TDto> commands) : base(
            commands.PublishMode,
            commands.ForOnly(
                c => c.Entity != null,
                c => new UpdatedDto<TStore, TEntity, TDto>((UpdateDto<TStore, TEntity, TDto>)c))
                .ToArray())
        {
            Predicate = commands.Predicate;
            Conditions = commands.Conditions;
        }

        [JsonIgnore] public Func<TDto, Expression<Func<TEntity, bool>>>[] Conditions { get; }

        [JsonIgnore] public Func<TDto, Expression<Func<TEntity, bool>>> Predicate { get; }
    }
}
