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
// Last Modified On : 12-30-2021
// ***********************************************************************
// <copyright file="IServiceConfiguration.cs" company="UltimatR.Framework">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;

/// <summary>
/// The UltimatR namespace.
/// </summary>
namespace UltimatR
{
    /// <summary>
    /// Interface IServiceConfiguration
    /// Implements the <see cref="Microsoft.Extensions.Configuration.IConfiguration" />
    /// </summary>
    /// <seealso cref="Microsoft.Extensions.Configuration.IConfiguration" />
    public interface IServiceConfiguration : IConfiguration
    {
        /// <summary>
        /// Gets the version.
        /// </summary>
        /// <value>The version.</value>
        string Version { get; }

        /// <summary>
        /// Clients the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>IConfigurationSection.</returns>
        IConfigurationSection Client(string name);
        /// <summary>
        /// Clients the size of the pool.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        /// <returns>System.Int32.</returns>
        int ClientPoolSize(IConfigurationSection endpoint);
        /// <summary>
        /// Clients the connection string.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <returns>System.String.</returns>
        string ClientConnectionString(IConfigurationSection client);
        /// <summary>
        /// Clients the connection string.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>System.String.</returns>
        string ClientConnectionString(string name);
        /// <summary>
        /// Clients the provider.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <returns>ClientProvider.</returns>
        ClientProvider ClientProvider(IConfigurationSection client);
        /// <summary>
        /// Clients the provider.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>ClientProvider.</returns>
        ClientProvider ClientProvider(string name);
        /// <summary>
        /// Clientses this instance.
        /// </summary>
        /// <returns>IEnumerable&lt;IConfigurationSection&gt;.</returns>
        IEnumerable<IConfigurationSection> Clients();
        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>The description.</value>
        string Description { get; }
        /// <summary>
        /// Dses the route prefix.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>System.String.</returns>
        string DsRoutePrefix(string name);
        /// <summary>
        /// Endpoints the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>IConfigurationSection.</returns>
        IConfigurationSection Endpoint(string name);
        /// <summary>
        /// Endpoints the size of the pool.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        /// <returns>System.Int32.</returns>
        int EndpointPoolSize(IConfigurationSection endpoint);
        /// <summary>
        /// Endpoints the connection string.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        /// <returns>System.String.</returns>
        string EndpointConnectionString(IConfigurationSection endpoint);
        /// <summary>
        /// Endpoints the connection string.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>System.String.</returns>
        string EndpointConnectionString(string name);
        /// <summary>
        /// Endpoints the provider.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        /// <returns>EndpointProvider.</returns>
        EndpointProvider EndpointProvider(IConfigurationSection endpoint);
        /// <summary>
        /// Endpoints the provider.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>EndpointProvider.</returns>
        EndpointProvider EndpointProvider(string name);
        /// <summary>
        /// Endpointses this instance.
        /// </summary>
        /// <returns>IEnumerable&lt;IConfigurationSection&gt;.</returns>
        IEnumerable<IConfigurationSection> Endpoints();
        /// <summary>
        /// Gets the title.
        /// </summary>
        /// <value>The title.</value>
        string Title { get; }
        /// <summary>
        /// Ultimates the service.
        /// </summary>
        /// <returns>IConfigurationSection.</returns>
        IConfigurationSection UltimateService();
        /// <summary>
        /// Identities the server.
        /// </summary>
        /// <returns>IConfigurationSection.</returns>
        IConfigurationSection IdentityServer();
        /// <summary>
        /// Identities the server address.
        /// </summary>
        /// <returns>System.String.</returns>
        string IdentityServerAddress();
        /// <summary>
        /// Identities the name of the server API.
        /// </summary>
        /// <returns>System.String.</returns>
        string IdentityServerApiName();
        /// <summary>
        /// Identities the server scopes.
        /// </summary>
        /// <returns>IEnumerable&lt;IConfigurationSection&gt;.</returns>
        IEnumerable<IConfigurationSection> IdentityServerScopes();

    }
}