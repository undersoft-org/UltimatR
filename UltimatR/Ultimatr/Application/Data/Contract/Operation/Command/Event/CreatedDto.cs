//-----------------------------------------------------------------------
// <copyright file="CreatedDtoEvent.cs" company="Undersoft">
//     Author: Dariusz Hanc
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Linq.Expressions;
using System.Text.Json.Serialization;

namespace UltimatR
{
    public class CreatedDto<TStore, TEntity, TDto> : CommandEvent<DtoCommand<TDto>> where TEntity : Entity
        where TDto : Dto
        where TStore : IDataStore
    {
        public CreatedDto(CreateDto<TStore, TEntity, TDto> command) : base(command)
        { Predicate = command.Predicate; }

        [JsonIgnore] public Func<TEntity, Expression<Func<TEntity, bool>>> Predicate { get; }
    }
}
