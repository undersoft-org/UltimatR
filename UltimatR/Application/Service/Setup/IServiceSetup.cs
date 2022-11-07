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
// <copyright file="IServiceSetup.cs" company="UltimatR.Framework">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

/// <summary>
/// The UltimatR namespace.
/// </summary>
namespace UltimatR
{
    /// <summary>
    /// Interface IServiceSetup
    /// </summary>
    public interface IServiceSetup
    {
        /// <summary>
        /// Adds the mapper.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        /// <returns>IServiceSetup.</returns>
        IServiceSetup AddMapper(IRepositoryMapper mapper);
        /// <summary>
        /// Adds the mapper.
        /// </summary>
        /// <param name="profiles">The profiles.</param>
        /// <returns>IServiceSetup.</returns>
        IServiceSetup AddMapper(params Profile[] profiles);
        /// <summary>
        /// Adds the mapper.
        /// </summary>
        /// <typeparam name="TProfile">The type of the t profile.</typeparam>
        /// <returns>IServiceSetup.</returns>
        IServiceSetup AddMapper<TProfile>() where TProfile : Profile;
        /// <summary>
        /// Adds the data service.
        /// </summary>
        /// <param name="mvc">The MVC.</param>
        /// <param name="contextType">Type of the context.</param>
        /// <param name="routePrefix">The route prefix.</param>
        /// <param name="pageLimit">The page limit.</param>
        /// <returns>IServiceSetup.</returns>
        IServiceSetup AddDataService(IMvcBuilder mvc, Type contextType, string routePrefix = null, int? pageLimit = null);
        /// <summary>
        /// Adds the data service.
        /// </summary>
        /// <typeparam name="TContext">The type of the t context.</typeparam>
        /// <param name="mvc">The MVC.</param>
        /// <param name="routePrefix">The route prefix.</param>
        /// <param name="pageLimit">The page limit.</param>
        /// <returns>IServiceSetup.</returns>
        IServiceSetup AddDataService<TContext>(IMvcBuilder mvc, string routePrefix = null, int? pageLimit = null) where TContext : DbContext;
        /// <summary>
        /// Configures the data services.
        /// </summary>
        /// <param name="mvc">The MVC.</param>
        /// <param name="pageLimit">The page limit.</param>
        /// <returns>IServiceSetup.</returns>
        IServiceSetup ConfigureDataServices(IMvcBuilder mvc, int? pageLimit = null);
        /// <summary>
        /// Adds the caching.
        /// </summary>
        /// <returns>IServiceSetup.</returns>
        IServiceSetup AddCaching();
        /// <summary>
        /// Configures the services.
        /// </summary>
        /// <param name="assemblies">The assemblies.</param>
        /// <returns>IServiceSetup.</returns>
        IServiceSetup ConfigureServices(Assembly[] assemblies = null);
        /// <summary>
        /// Configures the endpoints.
        /// </summary>
        /// <param name="assemblies">The assemblies.</param>
        /// <returns>IServiceSetup.</returns>
        IServiceSetup ConfigureEndpoints(Assembly[] assemblies = null);
        /// <summary>
        /// Configures the clients.
        /// </summary>
        /// <param name="assemblies">The assemblies.</param>
        /// <returns>IServiceSetup.</returns>
        IServiceSetup ConfigureClients(Assembly[] assemblies = null);
        /// <summary>
        /// Adds the implementations.
        /// </summary>
        /// <param name="assemblies">The assemblies.</param>
        /// <returns>IServiceSetup.</returns>
        IServiceSetup AddImplementations(Assembly[] assemblies = null);
        IServiceSetup ConfigureApiVersions(string[] apiVersions);
    }
}