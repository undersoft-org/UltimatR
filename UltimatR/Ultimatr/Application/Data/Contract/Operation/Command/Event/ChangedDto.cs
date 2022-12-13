//-----------------------------------------------------------------------
// <copyright file="ChangedDtoEvent.cs" company="Undersoft">
//     Author: Dariusz Hanc
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Linq.Expressions;
using System.Text.Json.Serialization;

namespace UltimatR
{
    public class ChangedDto<TStore, TEntity, TDto> : CommandEvent<DtoCommand<TDto>> where TEntity : Entity
        where TDto : Dto
        where TStore : IDataStore
    {
        public ChangedDto(DtoCommand<TDto> command) : base(command)
        {
        }

        public ChangedDto(ChangeDto<TStore, TEntity, TDto> command) : base(command)
        { Predicate = command.Predicate; }

        [JsonIgnore] public Func<TDto, Expression<Func<TEntity, bool>>> Predicate { get; }
    }
}
