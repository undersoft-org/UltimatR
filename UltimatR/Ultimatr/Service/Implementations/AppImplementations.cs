//-----------------------------------------------------------------------
// <copyright file="AppImplementations.cs" company="Undersoft">
//     Author: Dariusz Hanc
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;
using System;

using System.Linq;

using System.Reflection;

using System.Series;

namespace UltimatR
{
    public partial class ServiceSetup
    {
        public IServiceSetup AddAppImplementations(Assembly[] assemblies)
        {
            IServiceRegistry service = registry;

            assemblies ??= AppDomain.CurrentDomain.GetAssemblies();
            service.AddValidatorsFromAssemblies(assemblies);
            service.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
            service.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            service.AddMediatR(assemblies);
            Type[] dtos = assemblies.SelectMany(
                a => a.DefinedTypes.Where(t => t.ImplementedInterfaces.Contains(typeof(IDto))))
                .Select(t => t.UnderlyingSystemType)
                .ToArray();
            IDataMapper mapper = registry.GetObject<IDataMapper>();
            IServiceCollection deck = service.AddTransient<IDeck<IEntity>, Catalog<IEntity>>()
                .AddTransient<IDeck<IDto>, Deck<IDto>>()
                .AddScoped<IMassDeck<IEntity>, MassCatalog<IEntity>>()
                .AddScoped<IMassDeck<IDto>, MassDeck<IDto>>();
            Deck<Type> duplicateCheck = new Deck<Type>();
            Type[] stores = new Type[] { typeof(IEntryStore), typeof(IReportStore), typeof(IEventStore) };
            foreach(IDeck<IEntityType> contextEntityTypes in DbRegistry.Entities)
            {
                foreach(IEntityType _entityType in contextEntityTypes)
                {
                    Type entityType = _entityType.ClrType;
                    if(duplicateCheck.TryAdd(entityType))
                    {
                        foreach(Type _dto in dtos)
                        {
                            Type dto = _dto;
                            if(!dto.Name.Contains($"{entityType.Name}Dto"))
                                if(entityType.IsGenericType &&
                                    entityType.IsAssignableTo(typeof(Identifier)) &&
                                    dto.Name.Contains($"{entityType.GetGenericArguments().FirstOrDefault().Name}Dto"))
                                    dto = typeof(DtoIdentifier<>).MakeGenericType(_dto);
                                else
                                    continue;
                            service.AddTransient(
                                typeof(IRequest<>).MakeGenericType(typeof(DtoCommand<>).MakeGenericType(dto)),
                                typeof(DtoCommand<>).MakeGenericType(dto));

                            service.AddTransient(
                                typeof(CommandValidator<>).MakeGenericType(typeof(DtoCommand<>).MakeGenericType(dto)),
                                typeof(DtoCommandValidator<>).MakeGenericType(dto));
                            foreach(Type store in stores)
                            {
                                service.AddTransient(
                                    typeof(IRequestHandler<,>).MakeGenericType(
                                        new[]
                                    {
                                        typeof(FilterDto<,,>).MakeGenericType(store, entityType, dto),
                                        typeof(IDeck<>).MakeGenericType(dto)
                                    }),
                                    typeof(FilterDtoHandler<,,>).MakeGenericType(store, entityType, dto));

                                service.AddTransient(
                                    typeof(IRequestHandler<,>).MakeGenericType(
                                        typeof(FindDto<,,>).MakeGenericType(store, entityType, dto),
                                        dto),
                                    typeof(FindDtoHandler<,,>).MakeGenericType(store, entityType, dto));

                                service.AddTransient(
                                    typeof(IRequestHandler<,>).MakeGenericType(
                                        typeof(GetDto<,,>).MakeGenericType(store, entityType, dto),
                                        typeof(IDeck<>).MakeGenericType(dto)),
                                    typeof(GetDtoHandler<,,>).MakeGenericType(store, entityType, dto));
                                service.AddTransient(
                                    typeof(IRequestHandler<,>).MakeGenericType(
                                        new[]
                                    {
                                        typeof(CreateDto<,,>).MakeGenericType(store, entityType, dto),
                                        typeof(DtoCommand<>).MakeGenericType(dto)
                                    }),
                                    typeof(CreateDtoHandler<,,>).MakeGenericType(store, entityType, dto));

                                service.AddTransient(
                                    typeof(IRequestHandler<,>).MakeGenericType(
                                        new[]
                                    {
                                        typeof(RenewDto<,,>).MakeGenericType(store, entityType, dto),
                                        typeof(DtoCommand<>).MakeGenericType(dto)
                                    }),
                                    typeof(RenewDtoHandler<,,>).MakeGenericType(store, entityType, dto));

                                service.AddTransient(
                                    typeof(IRequestHandler<,>).MakeGenericType(
                                        new[]
                                    {
                                        typeof(UpdateDto<,,>).MakeGenericType(store, entityType, dto),
                                        typeof(DtoCommand<>).MakeGenericType(dto)
                                    }),
                                    typeof(UpdateDtoHandler<,,>).MakeGenericType(store, entityType, dto));

                                service.AddTransient(
                                    typeof(IRequestHandler<,>).MakeGenericType(
                                        new[]
                                    {
                                        typeof(ChangeDto<,,>).MakeGenericType(store, entityType, dto),
                                        typeof(DtoCommand<>).MakeGenericType(dto)
                                    }),
                                    typeof(ChangeDtoHandler<,,>).MakeGenericType(store, entityType, dto));

                                service.AddTransient(
                                    typeof(IRequestHandler<,>).MakeGenericType(
                                        new[]
                                    {
                                        typeof(DeleteDto<,,>).MakeGenericType(store, entityType, dto),
                                        typeof(DtoCommand<>).MakeGenericType(dto)
                                    }),
                                    typeof(DeleteDtoHandler<,,>).MakeGenericType(store, entityType, dto));

                                service.AddScoped(
                                    typeof(IRequestHandler<,>).MakeGenericType(
                                        new[]
                                    {
                                        typeof(ChangeDtoSet<,,>).MakeGenericType(store, entityType, dto),
                                        typeof(DtoCommandSet<>).MakeGenericType(dto)
                                    }),
                                    typeof(ChangeDtoSetHandler<,,>).MakeGenericType(store, entityType, dto));

                                service.AddScoped(
                                    typeof(IRequestHandler<,>).MakeGenericType(
                                        new[]
                                    {
                                        typeof(UpdateDtoSet<,,>).MakeGenericType(store, entityType, dto),
                                        typeof(DtoCommandSet<>).MakeGenericType(dto)
                                    }),
                                    typeof(UpdateDtoSetHandler<,,>).MakeGenericType(store, entityType, dto));

                                service.AddScoped(
                                    typeof(IRequestHandler<,>).MakeGenericType(
                                        new[]
                                    {
                                        typeof(CreateDtoSet<,,>).MakeGenericType(store, entityType, dto),
                                        typeof(DtoCommandSet<>).MakeGenericType(dto)
                                    }),
                                    typeof(CreateDtoSetHandler<,,>).MakeGenericType(store, entityType, dto));

                                service.AddScoped(
                                    typeof(IRequestHandler<,>).MakeGenericType(
                                        new[]
                                    {
                                        typeof(UpsertDtoSet<,,>).MakeGenericType(store, entityType, dto),
                                        typeof(DtoCommandSet<>).MakeGenericType(dto)
                                    }),
                                    typeof(UpsertDtoSetHandler<,,>).MakeGenericType(store, entityType, dto));

                                service.AddScoped(
                                    typeof(IRequestHandler<,>).MakeGenericType(
                                        new[]
                                    {
                                        typeof(DeleteDtoSet<,,>).MakeGenericType(store, entityType, dto),
                                        typeof(DtoCommandSet<>).MakeGenericType(dto)
                                    }),
                                    typeof(DeleteDtoSetHandler<,,>).MakeGenericType(store, entityType, dto));
                                service.AddScoped(
                                    typeof(INotificationHandler<>).MakeGenericType(
                                        typeof(DeletedDtoSet<,,>).MakeGenericType(store, entityType, dto)),
                                    typeof(DeletedDtoSetHandler<,,>).MakeGenericType(store, entityType, dto));

                                service.AddScoped(
                                    typeof(INotificationHandler<>).MakeGenericType(
                                        typeof(RenewedDtoSet<,,>).MakeGenericType(store, entityType, dto)),
                                    typeof(RenewedDtoSetHandler<,,>).MakeGenericType(store, entityType, dto));

                                service.AddScoped(
                                    typeof(INotificationHandler<>).MakeGenericType(
                                        typeof(UpdatedDtoSet<,,>).MakeGenericType(store, entityType, dto)),
                                    typeof(UpdatedDtoSetHandler<,,>).MakeGenericType(store, entityType, dto));

                                service.AddScoped(
                                    typeof(INotificationHandler<>).MakeGenericType(
                                        typeof(CreatedDtoSet<,,>).MakeGenericType(store, entityType, dto)),
                                    typeof(CreatedDtoSetHandler<,,>).MakeGenericType(store, entityType, dto));

                                service.AddScoped(
                                    typeof(INotificationHandler<>).MakeGenericType(
                                        typeof(ChangedDtoSet<,,>).MakeGenericType(store, entityType, dto)),
                                    typeof(ChangedDtoSetHandler<,,>).MakeGenericType(store, entityType, dto));

                                service.AddTransient(
                                    typeof(INotificationHandler<>).MakeGenericType(
                                        typeof(ChangedDto<,,>).MakeGenericType(store, entityType, dto)),
                                    typeof(ChangedDtoHandler<,,>).MakeGenericType(store, entityType, dto));

                                service.AddTransient(
                                    typeof(INotificationHandler<>).MakeGenericType(
                                        typeof(CreatedDto<,,>).MakeGenericType(store, entityType, dto)),
                                    typeof(CreatedDtoHandler<,,>).MakeGenericType(store, entityType, dto));

                                service.AddTransient(
                                    typeof(INotificationHandler<>).MakeGenericType(
                                        typeof(DeletedDto<,,>).MakeGenericType(store, entityType, dto)),
                                    typeof(DeletedDtoHandler<,,>).MakeGenericType(store, entityType, dto));

                                service.AddTransient(
                                    typeof(INotificationHandler<>).MakeGenericType(
                                        typeof(RenewedDto<,,>).MakeGenericType(store, entityType, dto)),
                                    typeof(UpsertedDtoHandler<,,>).MakeGenericType(store, entityType, dto));

                                service.AddTransient(
                                    typeof(INotificationHandler<>).MakeGenericType(
                                        typeof(UpdatedDto<,,>).MakeGenericType(store, entityType, dto)),
                                    typeof(UpdatedDtoHandler<,,>).MakeGenericType(store, entityType, dto));
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