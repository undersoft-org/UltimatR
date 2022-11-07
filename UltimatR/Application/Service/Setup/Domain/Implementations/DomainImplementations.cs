using MediatR;
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
            var service = catalog;
            var duplicateCheck = new HashSet<Type>();            
            Type[] stores = new Type[] { typeof(IEntryStore), typeof(IReportStore), typeof(IEventStore) };

            service.AddScoped<ILinkSynchronizer, LinkSynchronizer>();

            foreach (var contextEntityTypes in DbCatalog.Entities)
            { 
                foreach (var _entityType in contextEntityTypes)
                { 
                    var entityType = _entityType.ClrType;
                    if (duplicateCheck.Add(entityType))
                    { 
                        foreach (var store in stores)
                        {                             
                            service.AddScoped(typeof(IHostRepository<,>).MakeGenericType(store, entityType),
                                typeof(HostRepository<,>).MakeGenericType(store, entityType));
                            
                            service.AddSingleton(typeof(IDataCache<,>).MakeGenericType(store, entityType),
                              typeof(DataCache<,>).MakeGenericType(store, entityType));                      
                            service.AddTransient(typeof(IRequest<>).MakeGenericType(entityType),
                                typeof(FilterEntityQuery<,>).MakeGenericType(store, entityType));

                            service.AddTransient(typeof(IRequestHandler<,>).MakeGenericType(
                                typeof(FilterEntityQuery<,>).MakeGenericType(store, entityType),
                                typeof(IDeck<>).MakeGenericType(entityType)),
                                typeof(FilterEntityQueryHandler<,>).MakeGenericType(store, entityType));

                            service.AddTransient(typeof(IRequest<>).MakeGenericType(entityType),
                                typeof(FindEntityQuery<,>).MakeGenericType(store, entityType));

                            service.AddTransient(typeof(IRequestHandler<,>).MakeGenericType(
                                typeof(FindEntityQuery<,>).MakeGenericType(store, entityType), entityType
                            ), typeof(FindEntityQueryHandler<,>).MakeGenericType(store, entityType));

                            service.AddTransient(typeof(IRequest<>).MakeGenericType(entityType),
                                typeof(GetEntityQuery<,>).MakeGenericType(store, entityType));

                            service.AddTransient(typeof(IRequestHandler<,>).MakeGenericType(
                                typeof(GetEntityQuery<,>).MakeGenericType(store, entityType),
                                typeof(IDeck<>).MakeGenericType(entityType)
                            ), typeof(GetEntityQueryHandler<,>).MakeGenericType(store, entityType));
                            service.AddTransient(typeof(IRequest<>).MakeGenericType(entityType),
                                typeof(CreateEntityCommand<,>).MakeGenericType(store, entityType));

                            service.AddTransient(typeof(IRequestHandler<,>).MakeGenericType(new[]
                            {   typeof(CreateEntityCommand<,>).MakeGenericType(store, entityType), entityType
                            }), typeof(CreateEntityCommandHandler<,>).MakeGenericType(store, entityType));                      

                            service.AddTransient(typeof(IRequestHandler<,>).MakeGenericType(new[]
                            {   typeof(RenewEntityCommand<,>).MakeGenericType(store, entityType), entityType
                            }), typeof(RenewEntityCommandHandler<,>).MakeGenericType(store, entityType));                     

                            service.AddTransient(typeof(IRequestHandler<,>).MakeGenericType(new[]
                            {   typeof(UpdateEntityCommand<,>).MakeGenericType(store, entityType), entityType
                            }), typeof(UpdateEntityCommandHandler<,>).MakeGenericType(store, entityType));                          

                            service.AddTransient(typeof(IRequestHandler<,>).MakeGenericType(new[]
                            {   typeof(ChangeEntityCommand<,>).MakeGenericType(store, entityType), entityType
                            }), typeof(ChangeEntityCommandHandler<,>).MakeGenericType(store, entityType));             

                            service.AddTransient(typeof(IRequestHandler<,>).MakeGenericType(new[]
                            {   typeof(DeleteEntityCommand<,>).MakeGenericType(store, entityType), entityType
                            }), typeof(DeleteEntityCommandHandler<,>).MakeGenericType(store, entityType));
                            service.AddTransient(typeof(INotificationHandler<>).MakeGenericType(
                                typeof(ChangedEntityEvent<,>).MakeGenericType(store, entityType)),
                                typeof(EntityChangedEventHandler<,>).MakeGenericType(store, entityType));

                            service.AddTransient(typeof(INotificationHandler<>).MakeGenericType(
                                typeof(CreatedEntityEvent<,>).MakeGenericType(store, entityType)),
                                typeof(EntityCreatedEventHandler<,>).MakeGenericType(store, entityType));

                            service.AddTransient(typeof(INotificationHandler<>).MakeGenericType(
                                typeof(DeletedEntityEvent<,>).MakeGenericType(store, entityType)),
                                typeof(EntityDeletedEventHandler<,>).MakeGenericType(store, entityType));

                            service.AddTransient(typeof(INotificationHandler<>).MakeGenericType(
                                typeof(RenewedEntityEvent<,>).MakeGenericType(store, entityType)),
                                typeof(EntityRenewedEventHandler<,>).MakeGenericType(store, entityType));

                            service.AddTransient(typeof(INotificationHandler<>).MakeGenericType(
                                typeof(UpdatedEntityEvent<,>).MakeGenericType(store, entityType)),
                                typeof(EntityUpdatedEventHandler<,>).MakeGenericType(store, entityType));
                        } 
                    }
                }
            }
            return this;
        }
    }
}