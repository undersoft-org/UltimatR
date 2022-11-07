using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace UltimatR
{
    public partial class ServiceSetup
    {
        public static void AddDsRuntimeImplementations(IApplicationBuilder app)
        {
            var sm = ServiceManager.GetManager();
            var service = sm.Catalog;
            var duplicateCheck = new HashSet<Type>();
            Type[] stores = new Type[] { typeof(IEntryStore), typeof(IReportStore) };

          /**************************************** DataService Entity Type Routines ***************************************/
            foreach (var contextEntityTypes in DsCatalog.Entities)
            {
                foreach (var _entityType in contextEntityTypes)
                {
                    var entityType = DsCatalog.Mappings[_entityType.Name];
                   
                    if (duplicateCheck.Add(entityType))
                    {
                        var callerType = DbCatalog.Callers[entityType.FullName];

                        /*****************************************************************************************/
                        foreach (var store in stores)
                        {
                            if (entityType != null && DsCatalog.GetContext(store, entityType) != null)
                            {
                                /*****************************************************************************************/
                                service.AddScoped(typeof(ITeleRepository<,>).MakeGenericType(store, entityType),
                                    typeof(TeleRepository<,>).MakeGenericType(store, entityType));

                                service.AddScoped(typeof(IDataCache<,>).MakeGenericType(store, entityType),
                                    typeof(DataCache<,>).MakeGenericType(store, entityType));
                                /*****************************************************************************************/
                                if(callerType != null)
                                {
                                  /*********************************************************************************************/
                                    service.AddScoped(typeof(IRepositoryLink<,,>).MakeGenericType(store, callerType, entityType),
                                        typeof(RepositoryLink<,,>).MakeGenericType(store, callerType, entityType));

                                    service.AddScoped(typeof(ILinkedObject<,>).MakeGenericType(store, callerType),
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