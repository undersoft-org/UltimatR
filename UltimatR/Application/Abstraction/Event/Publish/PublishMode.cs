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
// Last Modified On : 12-29-2021
// ***********************************************************************
// <copyright file="PublishMode.cs" company="UltimatR.Framework">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using FluentValidation.Results;

/// <summary>
/// The UltimatR namespace.
/// </summary>
namespace UltimatR
{
    /// <summary>
    /// Enum PublishMode
    /// </summary>
    public enum PublishMode
   {
        /// <summary>
        /// The propagate command
        /// </summary>
        PropagateCommand,
        /// <summary>
        /// The suppress command
        /// </summary>
        SuppressCommand
    }
}