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
// Last Modified On : 12-28-2021
// ***********************************************************************
// <copyright file="EventSet.cs" company="UltimatR.Framework">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>

using MediatR;
using System.Series;

/// <summary>
/// The UltimatR namespace.
/// </summary>
namespace UltimatR
{
    /// <summary>
    /// Class EventSet.
    /// Implements the <see cref="System.Series.Deck{UltimatR.CommandEvent{TCommand}}" />
    /// Implements the <see cref="MediatR.INotification" />
    /// </summary>
    /// <typeparam name="TCommand">The type of the t command.</typeparam>
    /// <seealso cref="System.Series.Deck{UltimatR.CommandEvent{TCommand}}" />
    /// <seealso cref="MediatR.INotification" />
    public abstract class CommandEventSet<TCommand> : Deck<CommandEvent<TCommand>>, INotification where TCommand : Command
    {
        /// <summary>
        /// Gets or sets the publish mode.
        /// </summary>
        /// <value>The publish mode.</value>
        public PublishMode PublishMode { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandEventSet{TCommand}"/> class.
        /// </summary>
        /// <param name="publishPattern">The publish pattern.</param>
        /// <param name="commands">The commands.</param>
        protected CommandEventSet(PublishMode publishPattern, CommandEvent<TCommand>[] commands) : base(commands)
        {
            PublishMode = publishPattern;
        }
    }
}