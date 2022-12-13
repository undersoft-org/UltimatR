//-----------------------------------------------------------------------
// <copyright file="UpdatedDtoEvent.cs" company="Undersoft">
//     Author: Dariusz Hanc
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Linq.Expressions;
using System.Text.Json.Serialization;

namespace UltimatR
{
    public class UpdatedDto<TStore, TEntity, TDto> : CommandEvent<DtoCommand<TDto>> where TEntity : Entity
        where TDto : Dto
        where TStore : IDataStore
    {
        public UpdatedDto(UpdateDto<TStore, TEntity, TDto> command) : base(command)
        {
            Predicate = command.Predicate;
            Conditions = command.Conditions;
        }

        [JsonIgnore] public Func<TEntity, Expression<Func<TEntity, bool>>>[] Conditions { get; }

        [JsonIgnore] public Func<TEntity, Expression<Func<TEntity, bool>>> Predicate { get; }
    }
}
