//-----------------------------------------------------------------------
// <copyright file="DomainImplementations.cs" company="Undersoft">
//     Author: Dariusz Hanc
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using MediatR;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Series;

namespace UltimatR
{
    public partial class ServiceSetup
    {
        public IServiceSetup AddDomainImplementations()
        {
            IServiceRegistry service = registry;
            service.AddScoped<DataContext>();
            service.AddScoped<IDataContext, DataContext>();
            service.AddScoped<IUltimateService, UltimateService>();
            service.AddScoped<IUltimatr, Ultimatr>();
            service.AddHttpContextAccessor();

            HashSet<Type> duplicateCheck = new HashSet<Type>();
            Type[] stores = new Type[] { typeof(IEntryStore), typeof(IReportStore), typeof(IEventStore) };

            service.AddScoped<ILinkSynchronizer, LinkSynchronizer>();

            foreach(IDeck<IEntityType> contextEntityTypes in DbRegistry.Entities)
            {
                foreach(IEntityType _entityType in contextEntityTypes)
                {
                    Type entityType = _entityType.ClrType;
                    if(duplicateCheck.Add(entityType))
                    {
                        foreach(Type store in stores)
                        {
                            service.AddScoped(
                                typeof(IDataRepository<,>).MakeGenericType(store, entityType),
                                typeof(DataRepository<,>).MakeGenericType(store, entityType));

                            service.AddSingleton(
                                typeof(IEntityCache<,>).MakeGenericType(store, entityType),
                                typeof(EntityCache<,>).MakeGenericType(store, entityType));

                            service.AddTransient(
                                typeof(IRequest<>).MakeGenericType(entityType),
                                typeof(FilterDso<,>).MakeGenericType(store, entityType));

                            service.AddTransient(
                                typeof(IRequestHandler<,>).MakeGenericType(
                                    typeof(FilterDso<,>).MakeGenericType(store, entityType),
                                    typeof(IDeck<>).MakeGenericType(entityType)),
                                typeof(FilterDsoHandler<,>).MakeGenericType(store, entityType));

                            service.AddTransient(
                                typeof(IRequest<>).MakeGenericType(entityType),
                                typeof(FindDso<,>).MakeGenericType(store, entityType));

                            service.AddTransient(
                                typeof(IRequestHandler<,>).MakeGenericType(
                                    typeof(FindDso<,>).MakeGenericType(store, entityType),
                                    entityType),
                                typeof(FindDsoHandler<,>).MakeGenericType(store, entityType));

                            service.AddTransient(
                                typeof(IRequest<>).MakeGenericType(entityType),
                                typeof(GetDso<,>).MakeGenericType(store, entityType));

                            service.AddTransient(
                                typeof(IRequestHandler<,>).MakeGenericType(
                                    typeof(GetDso<,>).MakeGenericType(store, entityType),
                                    typeof(IDeck<>).MakeGenericType(entityType)),
                                typeof(GetDsoHandler<,>).MakeGenericType(store, entityType));

                            service.AddTransient(
                                typeof(IRequestHandler<,>).MakeGenericType(
                                    new[] { typeof(CreateDso<,>).MakeGenericType(store, entityType), entityType }),
                                typeof(CreateDsoHandler<,>).MakeGenericType(store, entityType));

                            service.AddTransient(
                                typeof(IRequestHandler<,>).MakeGenericType(
                                    new[] { typeof(UpsertDso<,>).MakeGenericType(store, entityType), entityType }),
                                typeof(UpsertDsoHandler<,>).MakeGenericType(store, entityType));

                            service.AddTransient(
                                typeof(IRequestHandler<,>).MakeGenericType(
                                    new[] { typeof(UpdateDso<,>).MakeGenericType(store, entityType), entityType }),
                                typeof(UpdateDsoHandler<,>).MakeGenericType(store, entityType));

                            service.AddTransient(
                                typeof(IRequestHandler<,>).MakeGenericType(
                                    new[] { typeof(ChangeDso<,>).MakeGenericType(store, entityType), entityType }),
                                typeof(ChangeDsoHandler<,>).MakeGenericType(store, entityType));

                            service.AddTransient(
                                typeof(IRequestHandler<,>).MakeGenericType(
                                    new[] { typeof(DeleteDso<,>).MakeGenericType(store, entityType), entityType }),
                                typeof(DeleteDsoHandler<,>).MakeGenericType(store, entityType));

                            service.AddTransient(
                                typeof(INotificationHandler<>).MakeGenericType(
                                    typeof(ChangedDso<,>).MakeGenericType(store, entityType)),
                                typeof(ChangedDsoHandler<,>).MakeGenericType(store, entityType));

                            service.AddTransient(
                                typeof(INotificationHandler<>).MakeGenericType(
                                    typeof(CreatedDso<,>).MakeGenericType(store, entityType)),
                                typeof(CreatedDsoHandler<,>).MakeGenericType(store, entityType));

                            service.AddTransient(
                                typeof(INotificationHandler<>).MakeGenericType(
                                    typeof(DeletedDso<,>).MakeGenericType(store, entityType)),
                                typeof(DeletedDsoHandler<,>).MakeGenericType(store, entityType));

                            service.AddTransient(
                                typeof(INotificationHandler<>).MakeGenericType(
                                    typeof(UpsertedDso<,>).MakeGenericType(store, entityType)),
                                typeof(UpsertedDsoHandler<,>).MakeGenericType(store, entityType));

                            service.AddTransient(
                                typeof(INotificationHandler<>).MakeGenericType(
                                    typeof(UpdatedDso<,>).MakeGenericType(store, entityType)),
                                typeof(UpdatedDsoHandler<,>).MakeGenericType(store, entityType));
                        }
                    }
                }
            }
            return this;
        }
    }
}