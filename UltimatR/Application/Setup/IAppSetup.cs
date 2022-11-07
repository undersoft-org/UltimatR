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
// Last Modified On : 01-03-2022
// ***********************************************************************
// <copyright file="IAppSetup.cs" company="UltimatR.Framework">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>

using System.Collections.Generic;
/// <summary>
/// The UltimatR namespace.
/// </summary>
namespace UltimatR
{
    /// <summary>
    /// Interface IAppSetup
    /// </summary>
    public interface IAppSetup
    {
        /// <summary>
        /// Uses the standard setup.
        /// </summary>
        /// <returns>IAppSetup.</returns>
        IAppSetup UseStandardSetup(string[] apiVersions);

        /// <summary>
        /// Uses the data clients.
        /// </summary>
        /// <returns>IAppSetup.</returns>
        IAppSetup UseDataClients();

        /// <summary>
        /// Uses the external provider.
        /// </summary>
        /// <returns>IAppSetup.</returns>
        IAppSetup UseExternalProvider();

        /// <summary>
        /// Uses the internal provider.
        /// </summary>
        /// <returns>IAppSetup.</returns>
        IAppSetup UseInternalProvider();

        /// <summary>
        /// Uses the data migrations.
        /// </summary>
        /// <returns>IAppSetup.</returns>
        IAppSetup UseDataMigrations();

        /// <summary>
        /// Uses the swagger setup.
        /// </summary>
        /// <returns>IAppSetup.</returns>
        IAppSetup UseSwaggerSetup(string[] apiVersions);
    }
}