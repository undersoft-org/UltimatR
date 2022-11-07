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
// Last Modified On : 11-03-2021
// ***********************************************************************
// <copyright file="IOrigin.cs" company="UltimatR.Framework">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>

using System;

/// <summary>
/// The UltimatR namespace.
/// </summary>
namespace UltimatR
{
    /// <summary>
    /// Interface IOrigin
    /// </summary>
    public interface IOrigin
    {
        /// <summary>
        /// Gets or sets the creation time.
        /// </summary>
        /// <value>The creation time.</value>
        DateTime CreationTime { get; set; }
        /// <summary>
        /// Gets or sets the creator.
        /// </summary>
        /// <value>The creator.</value>
        string Creator { get; set; }
        /// <summary>
        /// Gets or sets the modification time.
        /// </summary>
        /// <value>The modification time.</value>
        DateTime ModificationTime { get; set; }
        /// <summary>
        /// Gets or sets the modifier.
        /// </summary>
        /// <value>The modifier.</value>
        string Modifier { get; set; }
        /// <summary>
        /// Gets or sets the origin identifier.
        /// </summary>
        /// <value>The origin identifier.</value>
        short OriginId { get; set; }
        /// <summary>
        /// Gets or sets the name of the origin.
        /// </summary>
        /// <value>The name of the origin.</value>
        string OriginName { get; set; }
    }
}