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
// Last Modified On : 01-05-2022
// ***********************************************************************
// <copyright file="CommandMode.cs" company="UltimatR.Framework">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>

using FluentValidation.Results;
using System;
using System.Text.Json.Serialization;

/// <summary>
/// The UltimatR namespace.
/// </summary>
namespace UltimatR
{
    /// <summary>
    /// Enum CommandMode
    /// </summary>
    [Flags]
   public enum CommandMode
    {
        /// <summary>
        /// Any
        /// </summary>
        Any = 31,
        /// <summary>
        /// The create
        /// </summary>
        Create = 1,
        /// <summary>
        /// The change
        /// </summary>
        Change = 2,
        /// <summary>
        /// The update
        /// </summary>
        Update = 4,
        /// <summary>
        /// The delete
        /// </summary>
        Delete = 8,
        /// <summary>
        /// The renew
        /// </summary>
        Renew = 16 
    }    
}