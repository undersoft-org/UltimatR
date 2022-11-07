using System;
using System.Linq;
using System.Reflection;
using System.Series;

using FluentValidation;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

namespace UltimatR
{
    public partial class ServiceSetup
    {
        public IServiceSetup AddAppImplementations(Assembly[] assemblies)
        {
            var service = catalog;
            assemblies ??= AppDomain.CurrentDomain.GetAssemblies();
            service.AddValidatorsFromAssemblies(assemblies);
            service.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
            service.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            service.AddMediatR(assemblies);
            var dtos = assemblies.SelectMany(a => a.DefinedTypes
                                 .Where(t => t.ImplementedInterfaces
                                 .Contains(typeof(IDto))))
                                 .Select(t => t.UnderlyingSystemType)
                                 .ToArray();
            var mapper = catalog.GetObject<IRepositoryMapper>();
            var   deck = service.AddTransient<IDeck<IEntity>,  Catalog<IEntity>>()
                                .AddTransient<IDeck<IDto>,     Deck<IDto>>()
                                .AddScoped<IMassDeck<IEntity>, MassCatalog<IEntity>>()
                                .AddScoped<IMassDeck<IDto>,    MassDeck<IDto>>();
            var duplicateCheck = new Deck<Type>();
            Type[] stores = new Type[] { typeof(IEntryStore), typeof(IReportStore), typeof(IEventStore) };
            foreach (var contextEntityTypes in DbCatalog.Entities)
            { 
                foreach (var _entityType in contextEntityTypes)
                { 
                    var entityType = _entityType.ClrType;
                    if (duplicateCheck.TryAdd(entityType))
                    { 
                        foreach (var _dto in dtos)
                        { 
                            var dto = _dto;
                            if (!dto.Name.Contains($"{entityType.Name}Dto"))                            
                                if (entityType.IsGenericType && entityType.IsAssignableTo(typeof(Identifier)) &&                                    
                                    dto.Name.Contains($"{entityType.GetGenericArguments().FirstOrDefault().Name}Dto"))
                                    dto = typeof(IdentifierDto<>).MakeGenericType(_dto);
                                else
                                    continue;                            
                            service.AddTransient(typeof(IRequest<>).MakeGenericType(
                                                 typeof(DataCommand<>).MakeGenericType(dto)),
                                                 typeof(DataCommand<>).MakeGenericType(dto));

                            service.AddTransient(typeof(CommandValidator<>).MakeGenericType(
                                                 typeof(DataCommand<>).MakeGenericType(dto)),
                                                 typeof(DataCommandValidator<>).MakeGenericType(dto));
                            foreach (var store in stores)
                            { 
                                service.AddTransient(typeof(IRequestHandler<,>).MakeGenericType(new[]
                                {   typeof(FilterDataQuery<,,>).MakeGenericType(store, entityType, dto),
                                    typeof(IDeck<>).MakeGenericType(dto)
                                }), typeof(FilterDataQueryHandler<,,>).MakeGenericType(store, entityType, dto));

                                service.AddTransient(typeof(IRequestHandler<,>).MakeGenericType(
                                    typeof(FindDataQuery<,,>).MakeGenericType(store, entityType, dto), dto
                                ),  typeof(FindDataQueryHandler<,,>).MakeGenericType(store, entityType, dto));

                                service.AddTransient(typeof(IRequestHandler<,>).MakeGenericType(
                                    typeof(GetDataQuery<,,>).MakeGenericType(store, entityType, dto),
                                    typeof(IDeck<>).MakeGenericType(dto)
                                ),  typeof(GetDataQueryHandler<,,>).MakeGenericType(store, entityType, dto));
                                service.AddTransient(typeof(IRequestHandler<,>).MakeGenericType(new[]
                                {   typeof(CreateDataCommand<,,>).MakeGenericType(store, entityType, dto),
                                    typeof(DataCommand<>).MakeGenericType(dto)
                                }), typeof(CreateDataCommandHandler<,,>).MakeGenericType(store, entityType, dto));

                                service.AddTransient(typeof(IRequestHandler<,>).MakeGenericType(new[]
                                {   typeof(RenewDataCommand<,,>).MakeGenericType(store, entityType, dto),
                                    typeof(DataCommand<>).MakeGenericType(dto)
                                }), typeof(RenewDataCommandHandler<,,>).MakeGenericType(store, entityType, dto));

                                service.AddTransient(typeof(IRequestHandler<,>).MakeGenericType(new[]
                                {   typeof(UpdateDataCommand<,,>).MakeGenericType(store, entityType, dto),
                                    typeof(DataCommand<>).MakeGenericType(dto)
                                }), typeof(UpdateDataCommandHandler<,,>).MakeGenericType(store, entityType, dto));

                                service.AddTransient(typeof(IRequestHandler<,>).MakeGenericType(new[]
                                {   typeof(ChangeDataCommand<,,>).MakeGenericType(store, entityType, dto),
                                    typeof(DataCommand<>).MakeGenericType(dto)
                                }), typeof(ChangeDataCommandHandler<,,>).MakeGenericType(store, entityType, dto));

                                service.AddTransient(typeof(IRequestHandler<,>).MakeGenericType(new[]
                                {   typeof(DeleteDataCommand<,,>).MakeGenericType(store, entityType, dto),
                                    typeof(DataCommand<>).MakeGenericType(dto)
                                }), typeof(DeleteDataCommandHandler<,,>).MakeGenericType(store, entityType, dto));

                                service.AddScoped(typeof(IRequestHandler<,>).MakeGenericType(new[]
                                {   typeof(ChangeDataCommandSet<,,>).MakeGenericType(store, entityType, dto),
                                    typeof(CommandSet<>).MakeGenericType(dto)
                                }), typeof(ChangeDataCommandSetHandler<,,>).MakeGenericType(store, entityType, dto));

                                service.AddScoped(typeof(IRequestHandler<,>).MakeGenericType(new[]
                                {   typeof(UpdateDataCommandSet<,,>).MakeGenericType(store, entityType, dto),
                                    typeof(CommandSet<>).MakeGenericType(dto)
                                }), typeof(UpdateDataCommandSetHandler<,,>).MakeGenericType(store, entityType, dto));

                                service.AddScoped(typeof(IRequestHandler<,>).MakeGenericType(new[]
                                {   typeof(CreateDataCommandSet<,,>).MakeGenericType(store, entityType, dto),
                                    typeof(CommandSet<>).MakeGenericType(dto)
                                }), typeof(CreateDataCommandSetHandler<,,>).MakeGenericType(store, entityType, dto));

                                service.AddScoped(typeof(IRequestHandler<,>).MakeGenericType(new[]
                                {   typeof(RenewDataCommandSet<,,>).MakeGenericType(store, entityType, dto),
                                    typeof(CommandSet<>).MakeGenericType(dto)
                                }), typeof(RenewDataCommandSetHandler<,,>).MakeGenericType(store, entityType, dto));

                                service.AddScoped(typeof(IRequestHandler<,>).MakeGenericType(new[]
                                {   typeof(DeleteDataCommandSet<,,>).MakeGenericType(store, entityType, dto),
                                    typeof(CommandSet<>).MakeGenericType(dto)
                                }), typeof(DeleteDataCommandSetHandler<,,>).MakeGenericType(store, entityType, dto));
                                service.AddScoped(typeof(INotificationHandler<>).MakeGenericType(
                                    typeof(DeletedDataEventSet<,,>).MakeGenericType(store, entityType, dto)),
                                    typeof(DataDeletedEventsHandler<,,>).MakeGenericType(store, entityType, dto));

                                service.AddScoped(typeof(INotificationHandler<>).MakeGenericType(
                                    typeof(RenewedDataEventSet<,,>).MakeGenericType(store, entityType, dto)),
                                    typeof(DataRenewedEventsHandler<,,>).MakeGenericType(store, entityType, dto));

                                service.AddScoped(typeof(INotificationHandler<>).MakeGenericType(
                                    typeof(UpdatedDataEventSet<,,>).MakeGenericType(store, entityType, dto)),
                                    typeof(DataUpdatedEventsHandler<,,>).MakeGenericType(store, entityType, dto));

                                service.AddScoped(typeof(INotificationHandler<>).MakeGenericType(
                                    typeof(CreatedDataEventSet<,,>).MakeGenericType(store, entityType, dto)),
                                    typeof(DataCreatedEventsHandler<,,>).MakeGenericType(store, entityType, dto));

                                service.AddScoped(typeof(INotificationHandler<>).MakeGenericType(
                                    typeof(ChangedDataEventSet<,,>).MakeGenericType(store, entityType, dto)),
                                    typeof(DataChangedEventsHandler<,,>).MakeGenericType(store, entityType, dto));

                                service.AddTransient(typeof(INotificationHandler<>).MakeGenericType(
                                    typeof(ChangedDataEvent<,,>).MakeGenericType(store, entityType, dto)),
                                    typeof(DataChangedEventHandler<,,>).MakeGenericType(store, entityType, dto));

                                service.AddTransient(typeof(INotificationHandler<>).MakeGenericType(
                                    typeof(CreatedDataEvent<,,>).MakeGenericType(store, entityType, dto)),
                                    typeof(DataCreatedEventHandler<,,>).MakeGenericType(store, entityType, dto));

                                service.AddTransient(typeof(INotificationHandler<>).MakeGenericType(
                                    typeof(DeletedDataEvent<,,>).MakeGenericType(store, entityType, dto)),
                                    typeof(DataDeletedEventHandler<,,>).MakeGenericType(store, entityType, dto));

                                service.AddTransient(typeof(INotificationHandler<>).MakeGenericType(
                                    typeof(RenewedDataEvent<,,>).MakeGenericType(store, entityType, dto)),
                                    typeof(DataRenewedEventHandler<,,>).MakeGenericType(store, entityType, dto));

                                service.AddTransient(typeof(INotificationHandler<>).MakeGenericType(
                                    typeof(UpdatedDataEvent<,,>).MakeGenericType(store, entityType, dto)),
                                    typeof(DataUpdatedEventHandler<,,>).MakeGenericType(store, entityType, dto));
                            } 
                            mapper.TryCreateMap(entityType, dto);
                        }
                    }
                }
            }       
            mapper.Build();
            return this;
        }
    }
}