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
// <copyright file="PublishStatus.cs" company="UltimatR.Framework">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>

/// <summary>
/// The UltimatR namespace.
/// </summary>
namespace UltimatR
{
    /// <summary>
    /// Enum PublishStatus
    /// </summary>
    public enum PublishStatus
    {
        /// <summary>
        /// The none
        /// </summary>
        None = 0,
        /// <summary>
        /// The ready
        /// </summary>
        Ready = 1,
        /// <summary>
        /// The in progress
        /// </summary>
        InProgress = 2,
        /// <summary>
        /// The complete
        /// </summary>
        Complete = 3,
        /// <summary>
        /// The uncomplete
        /// </summary>
        Uncomplete = 4,
        /// <summary>
        /// The canceled
        /// </summary>
        Canceled = 8,
        /// <summary>
        /// The error
        /// </summary>
        Error = 9
    }
}
