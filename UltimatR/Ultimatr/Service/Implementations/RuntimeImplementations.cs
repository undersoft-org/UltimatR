//-----------------------------------------------------------------------
// <copyright file="RuntimeImplementations.cs" company="Undersoft">
//     Author: Dariusz Hanc
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OData.Edm;
using System;
using System.Collections.Generic;
using System.Series;

namespace UltimatR
{
    public partial class AppSetup
    {
        public static void AddRuntimeImplementations(IApplicationBuilder app)
        {
            IServiceManager sm = ServiceManager.GetManager();
            IServiceRegistry service = sm.Registry;
            HashSet<Type> duplicateCheck = new HashSet<Type>();
            Type[] stores = new Type[] { typeof(IEntryStore), typeof(IReportStore) };

          /**************************************** DataService Entity Type Routines ***************************************/
            foreach(IDeck<IEdmEntityType> contextEntityTypes in DsRegistry.Entities)
            {
                foreach(IEdmEntityType _entityType in contextEntityTypes)
                {
                    Type entityType = DsRegistry.Mappings[_entityType.Name];

                    if(duplicateCheck.Add(entityType))
                    {
                        Type callerType = DbRegistry.Callers[entityType.FullName];

                        /*****************************************************************************************/
                        foreach(Type store in stores)
                        {
                            if((entityType != null) && (DsRegistry.GetContext(store, entityType) != null))
                            {
                                /*****************************************************************************************/
                                service.AddScoped(
                                    typeof(ILinkedRepository<,>).MakeGenericType(store, entityType),
                                    typeof(LinkedRepository<,>).MakeGenericType(store, entityType));

                                service.AddScoped(
                                    typeof(IEntityCache<,>).MakeGenericType(store, entityType),
                                    typeof(EntityCache<,>).MakeGenericType(store, entityType));
                                /*****************************************************************************************/
                                if(callerType != null)
                                {
                                    /*********************************************************************************************/
                                    service.AddScoped(
                                        typeof(IRepositoryLink<,,>).MakeGenericType(store, callerType, entityType),
                                        typeof(RepositoryLink<,,>).MakeGenericType(store, callerType, entityType));

                                    service.AddScoped(
                                        typeof(ILinkedObject<,>).MakeGenericType(store, callerType),
                                        typeof(RepositoryLink<,,>).MakeGenericType(store, callerType, entityType));
                                    /*********************************************************************************************/
                                }
                            }
                        }
                    }
                }
            }
            app.RebuildProviders();
        }
    }
}