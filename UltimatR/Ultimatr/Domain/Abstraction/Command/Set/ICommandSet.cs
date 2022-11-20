// ***********************************************************************
// Assembly         : UltimatR.Framework
// Authors          : darisuz.hanc < undersoft.org >
// Participants
// Patronate        : m.krzetowski (architect), k.reszka (team-leader)
// Contribution     : d.hanc (r&d.soft.developer), p.grys (senior.soft.engineer)
// Development      : p.gasowski (jr.soft.developer)
// Business         : k.golos (po) m.rafalski (pm), m.korzeniewski (analyst) 
// QA               : a.urbanek
// DevOps           : k.manikowski        
// Created          : 02-05-2022
//
// Last Modified By : darisuz.hanc < undersoft.org >
// Last Modified On : 01-16-2022
// ***********************************************************************
// <copyright file="ICommandSet.cs" company="UltimatR.Framework">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>

using FluentValidation.Results;
using System.Collections.Generic;

/// <summary>
/// The UltimatR namespace.
/// </summary>
namespace UltimatR
{
    /// <summary>
    /// Interface ICommandSet
    /// Implements the <see cref="UltimatR.ICommandSet" />
    /// </summary>
    /// <typeparam name="TDto">The type of the t dto.</typeparam>
    /// <seealso cref="UltimatR.ICommandSet" />
    public interface ICommandSet<TDto> : ICommandSet where TDto : Dto
    {
        /// <summary>
        /// Gets the commands.
        /// </summary>
        /// <value>The commands.</value>
        public new IEnumerable<DtoCommand<TDto>> Commands { get; }
    }

    /// <summary>
    /// Interface ICommandSet
    /// Implements the <see cref="UltimatR.ICommandSet" />
    /// </summary>
    /// <seealso cref="UltimatR.ICommandSet" />
    public interface ICommandSet : IDataIO
    {
        /// <summary>
        /// Gets the commands.
        /// </summary>
        /// <value>The commands.</value>
        public IEnumerable<ICommand> Commands { get; }

        /// <summary>
        /// Gets or sets the result.
        /// </summary>
        /// <value>The result.</value>
        public ValidationResult Result { get; set; }
    }
}