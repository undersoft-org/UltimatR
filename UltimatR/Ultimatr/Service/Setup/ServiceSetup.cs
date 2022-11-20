//-----------------------------------------------------------------------
// <copyright file="ServiceSetup.cs" company="Undersoft">
//     Author: Dariusz Hanc
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using AutoMapper;
using FluentValidation;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using Microsoft.OData.Edm;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace UltimatR
{
    public partial class ServiceSetup : IServiceSetup
    {
        string[] apiVersions = new string[1] { "1" };
        Assembly[] Assemblies;

        public ServiceSetup(IServiceCollection services)
        {
            manager = new ServiceManager(services);
            registry = manager.Registry;
        }

        public ServiceSetup(IServiceCollection services, IConfiguration configuration) : this(services)
        { manager.Configuration = new ServiceConfiguration(configuration); }

        void AddDbContextConfiguration(IDataContext context)
        {
            DataContext _context = context as DataContext;
            _context.ChangeTracker.AutoDetectChangesEnabled = true;
            _context.ChangeTracker.LazyLoadingEnabled = true;
            _context.Database.AutoTransactionsEnabled = false;
        }

        string AddDsClientPrefix(Type contextType, string routePrefix = null)
        {
            Type iface = DsRegistry.GetDsStore(contextType);
            return GetDsoControllerRoute(iface, routePrefix);
        }

        string AddDsEndpointPrefix(Type contextType, string routePrefix = null)
        {
            Type iface = DbRegistry.GetDbStore(contextType);
            return GetDsoControllerRoute(iface, routePrefix);
        }

        IRepositoryEndpoint<TContext> AddEntitySets<TContext>() where TContext : DbContext
        { return (IRepositoryEndpoint<TContext>)AddEntitySets(typeof(TContext)); }

        IRepositoryEndpoint AddEntitySets(Type contextType)
        {
            if(!RepositoryManager.TryGetEndpoint(contextType, out IRepositoryEndpoint endpoint))
            {
                return endpoint;
            }

            Assembly[] asm = Assemblies;

            Type[] otypes = asm
                .SelectMany(
                    a => a.GetTypes()
                        .Where(
                            type => typeof(ODataController)
                .IsAssignableFrom(type))
                        .ToArray())
                .Where(b => !b.IsAbstract && b.BaseType.IsGenericType && (b.BaseType.GenericTypeArguments.Length == 3))
                .Select(a => a.BaseType)
                .ToArray();

            foreach(Type types in otypes)
            {
                Type[] genTypes = types.GenericTypeArguments;
                if(DsRegistry.GetDsStore(contextType) == genTypes[1])
                {
                    endpoint.ServiceEntitySet(genTypes[2]);
                }
            }

            return endpoint;
        }

        IServiceSetup AddSwaggerDsSupport()
        {
            registry.GetObject<IMvcBuilder>()
                .AddMvcOptions(
                    options =>
                    {
                        foreach(OutputFormatter outputFormatter in options.OutputFormatters
                            .OfType<OutputFormatter>()
                            .Where(x => x.SupportedMediaTypes.Count == 0))
                        {
                            outputFormatter.SupportedMediaTypes
                                .Add(new MediaTypeHeaderValue("application/prs.odatatestxx-odata"));
                        }

                        foreach(InputFormatter inputFormatter in options.InputFormatters
                            .OfType<InputFormatter>()
                            .Where(x => x.SupportedMediaTypes.Count == 0))
                        {
                            inputFormatter.SupportedMediaTypes
                                .Add(new MediaTypeHeaderValue("application/prs.odatatestxx-odata"));
                        }
                    });
            return this;
        }

        string GetDsoControllerRoute(Type iface, string routePrefix = null)
        {
            if(iface == typeof(IEntryStore))
            {
                return (routePrefix != null) ? (DsoControllerRoute.EntryStore = routePrefix) : DsoControllerRoute.EntryStore;
            } else if(iface == typeof(IEventStore))
            {
                return (routePrefix != null) ? (DsoControllerRoute.EventStore = routePrefix) : DsoControllerRoute.EventStore;
            } else if(iface == typeof(IReportStore))
            {
                return (routePrefix != null) ? (DsoControllerRoute.ReportStore = routePrefix) : DsoControllerRoute.ReportStore;
            } else if(iface == typeof(IConfigStore))
            {
                return (routePrefix != null) ? (DsoControllerRoute.ConfigStore = routePrefix) : DsoControllerRoute.ConfigStore;
            } else
            {
                return (routePrefix != null) ? (DsoControllerRoute.StateStore = routePrefix) : DsoControllerRoute.StateStore;
            }
        }

        IServiceConfiguration configuration => manager.Configuration;

        IServiceManager manager { get; }

        IServiceRegistry registry { get; set; }

        IServiceCollection services => registry.Services;

        public IServiceSetup AddCaching()
        {
            registry.AddObject(GlobalCache.Catalog);

            Type[] stores = new Type[] { typeof(IEntryStore), typeof(IReportStore), typeof(IEventStore) };
            foreach(Type item in stores)
            {
                AddDataCache(item);
            }

            return this;
        }

        public IServiceSetup AddDataCache(Type tstore)
        {
            Type idatacache = typeof(IStoreCache<>).MakeGenericType(tstore);
            Type datacache = typeof(StoreCache<>).MakeGenericType(tstore);

            object cache = datacache.New(registry.GetObject<IDataCache>());
            registry.AddObject(idatacache, cache);
            registry.AddObject(datacache, cache);

            return this;
        }

        public IServiceSetup AddDataService<TContext>(IMvcBuilder mvcBuilder, string routePrefix, int? pageLimit = null)
            where TContext : DbContext
        {
            IRepositoryEndpoint<TContext> endpoint = AddEntitySets<TContext>();

            routePrefix = AddDsEndpointPrefix(typeof(TContext), routePrefix);

            mvcBuilder.AddOData(
                (opt) =>
                {
                    opt.RouteOptions.EnableQualifiedOperationCall = true;
                    opt.RouteOptions.EnableUnqualifiedOperationCall = false;
                    opt.RouteOptions.EnableKeyInParenthesis = true;
                    opt.RouteOptions.EnableKeyAsSegment = false;
                    opt.RouteOptions.EnableControllerNameCaseInsensitive = true;
                    opt.EnableQueryFeatures(pageLimit);
                    opt.AddRouteComponents(routePrefix, endpoint.GetServiceModel<IEdmModel>());
                });

            return this;
        }

        public IServiceSetup AddDataService(
            IMvcBuilder mvcBuilder,
            Type contextType,
            string routePrefix,
            int? pageLimit = null)
        {
            IRepositoryEndpoint endpoint = AddEntitySets(contextType);

            routePrefix = AddDsEndpointPrefix(contextType, routePrefix);

            mvcBuilder.AddOData(
                (opt) =>
                {
                    opt.RouteOptions.EnableQualifiedOperationCall = true;
                    opt.RouteOptions.EnableUnqualifiedOperationCall = false;
                    opt.RouteOptions.EnableKeyInParenthesis = true;
                    opt.RouteOptions.EnableKeyAsSegment = false;
                    opt.RouteOptions.EnableControllerNameCaseInsensitive = true;
                    opt.EnableQueryFeatures(pageLimit);
                    opt.AddRouteComponents(routePrefix, endpoint.GetServiceModel<IEdmModel>());
                });

            return this;
        }

        public IServiceSetup AddIdentityServer()
        {
            registry.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(
                    options =>
                    {
                        options.Authority = configuration.IdentityServerAddress();
                        options.ApiName = configuration.IdentityServerApiName();
                        options.EnableCaching = true;
                        options.CacheDuration = TimeSpan.FromMinutes(10);
                    });

            registry.AddCors(
                options => options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                    }));

            return this;
        }

        public IServiceSetup AddImplementations(Assembly[] assemblies = null)
        {
            AddAppImplementations(assemblies);

            AddDomainImplementations();

            return this;
        }

        public IServiceSetup AddMapper<TProfile>() where TProfile : Profile
        {
            AddMapper(new DataMapper(typeof(TProfile).New<TProfile>()));

            return this;
        }

        public IServiceSetup AddMapper(params Profile[] profiles)
        {
            AddMapper(new DataMapper(profiles));

            return this;
        }

        public IServiceSetup AddMapper(IDataMapper mapper)
        {
            registry.AddObject(mapper);
            registry.AddObject<IDataMapper>(mapper);
            registry.AddScoped<IMapper, DataMapper>();

            return this;
        }

        public IServiceSetup AddPolicies()
        {
            registry.AddAuthorization(
                options => configuration.IdentityServerScopes()
                    .Select(s => s.GetValue<string>("Name"))
                    .ForEach(s => options.AddPolicy(s, policy => policy.RequireScope(s))));
            return this;
        }

        public IServiceSetup AddSwagger()
        {
            string ver = configuration.Version;
            Dictionary<string, string> scopes = configuration.IdentityServerScopes()
                .ToDictionary(key => key.GetValue<string>("Name"), value => value.GetValue<string>("Description"));
            registry.AddSwaggerGen(
                s =>
                {
                    s.DocumentFilter<IgnoreApiDocument>();
                    foreach(string version in apiVersions)
                    {
                        s.SwaggerDoc(
                            $"{version}",
                            new OpenApiInfo
                            {
                                Version = ver,
                                Title = configuration.Title,
                                Description = configuration.Description
                            });
                    }
                    s.AddSecurityDefinition(
                        "oauth2",
                        new OpenApiSecurityScheme
                        {
                            Type = SecuritySchemeType.OAuth2,
                            Flows =
                                new OpenApiOAuthFlows
                                        {
                                            ClientCredentials =
                                                new OpenApiOAuthFlow
                                                            {
                                                                AuthorizationUrl =
                                                                    new Uri(configuration.IdentityServerAddress()),
                                                                TokenUrl =
                                                                    new Uri(
                                                                                        $"{configuration.IdentityServerAddress()}/connect/token"),
                                                                Scopes = scopes
                                                            }
                                        }
                        });
                    s.AddSecurityRequirement(
                        new OpenApiSecurityRequirement
                        {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" }
                            },
                            new string[] { }
                        }
                        });
                });
            return this;
        }

        public IServiceSetup ConfigureApiVersions(string[] apiVersions)
        {
            this.apiVersions = apiVersions;
            return this;
        }

        public IServiceSetup ConfigureClients(Assembly[] assemblies = null)
        {
            IServiceConfiguration config = configuration;
            assemblies ??= AppDomain.CurrentDomain.GetAssemblies();
            TypeInfo[] definedTypes = assemblies.SelectMany(a => a.DefinedTypes).ToArray();
            IEnumerable<IConfigurationSection> clients = config.Clients();
            RepositoryClients repoClients = new RepositoryClients();

            services.AddSingleton(registry.AddObject<IRepositoryClients>(repoClients).Value);

            foreach(IConfigurationSection client in clients)
            {
                ClientProvider provider = config.ClientProvider(client);
                string connectionString = config.ClientConnectionString(client).Trim();
                int poolsize = config.ClientPoolSize(client);
                Type preContextType = definedTypes.Where(t => t.FullName.Contains(client.Key))
                    .Select(t => t.UnderlyingSystemType)
                    .FirstOrDefault();
                if((provider == ClientProvider.None) || (connectionString == null) || (preContextType == null))
                {
                    continue;
                }

                Type[] contextTypes = new Type[]
                {
                    preContextType.MakeGenericType(typeof(IEntryStore)),
                    preContextType.MakeGenericType(typeof(IReportStore))
                };

                foreach(Type contextType in contextTypes)
                {
                    string routePrefix = AddDsClientPrefix(contextType).Trim();
                    if(!connectionString.EndsWith('/'))
                    {
                        connectionString += "/";
                    }

                    if(routePrefix.StartsWith('/'))
                    {
                        routePrefix = routePrefix.Substring(1);
                    }

                    string _connectionString = $"{connectionString}{routePrefix}";

                    Type        iRepoType = typeof(IRepositoryClient<>).MakeGenericType(contextType);
                    Type         repoType = typeof(RepositoryClient<>).MakeGenericType(contextType);

                    IRepositoryClient repoClient = (IRepositoryClient)repoType.New(provider, _connectionString);

                    Type      storeDbType = typeof(DsContext<>).MakeGenericType(DsRegistry.GetDsStore(contextType));
                    Type    storeRepoType = typeof(RepositoryClient<>).MakeGenericType(storeDbType);

                    IRepositoryClient storeClient = (IRepositoryClient)storeRepoType.New(repoClient);

                    Type   istoreRepoType = typeof(IRepositoryClient<>).MakeGenericType(storeDbType);
                    Type    ipoolRepoType = typeof(IRepositoryContextPool<>).MakeGenericType(storeDbType);
                    Type ifactoryRepoType = typeof(IRepositoryContextFactory<>).MakeGenericType(storeDbType);
                    Type    idataRepoType = typeof(IRepositoryContext<>).MakeGenericType(storeDbType);

                    repoClient.PoolSize = poolsize;

                    IRepositoryClient globalClient = RepositoryManager.AddClient(repoClient);

                    registry.AddObject(iRepoType, globalClient);
                    registry.AddObject(repoType, globalClient);

                    registry.AddObject(istoreRepoType, storeClient);
                    registry.AddObject(ipoolRepoType, storeClient);
                    registry.AddObject(ifactoryRepoType, storeClient);
                    registry.AddObject(idataRepoType, storeClient);
                    registry.AddObject(storeRepoType, storeClient);

                    RepositoryManager.AddClientPool(globalClient.ContextType, poolsize);
                }
            }

            return this;
        }

        public IServiceSetup ConfigureDataServices(IMvcBuilder mvcBuilder, int? pageLimit = null)
        {
            IEnumerable<IRepositoryEndpoint> endpoints = manager.GetEndpoints();

            foreach(IRepositoryEndpoint endpoint in endpoints)
            {
                AddDataService(mvcBuilder, endpoint.ContextType, null, pageLimit);
            }

            return this;
        }

        public IServiceSetup ConfigureEndpoints(Assembly[] assemblies = null)
        {
            IServiceConfiguration config = configuration;
            assemblies ??= AppDomain.CurrentDomain.GetAssemblies();
            TypeInfo[] definedTypes = assemblies.SelectMany(a => a.DefinedTypes).ToArray();
            IEnumerable<IConfigurationSection> endpoints = config.Endpoints();

            RepositoryEndpoints repoEndpoints = new RepositoryEndpoints();
            services.AddSingleton(registry.AddObject<IRepositoryEndpoints>(repoEndpoints).Value);

            foreach(IConfigurationSection endpoint in endpoints)
            {
                string connectionString = config.EndpointConnectionString(endpoint);
                EndpointProvider provider = config.EndpointProvider(endpoint);
                int poolsize = config.EndpointPoolSize(endpoint);
                Type contextType = definedTypes.Where(t => t.FullName == endpoint.Key)
                    .Select(t => t.UnderlyingSystemType)
                    .FirstOrDefault();

                if((provider == EndpointProvider.None) || (connectionString == null) || (contextType == null))
                {
                    continue;
                }

                Type        iRepoType = typeof(IRepositoryEndpoint<>).MakeGenericType(contextType);
                Type         repoType = typeof(RepositoryEndpoint<>).MakeGenericType(contextType);
                Type  repoOptionsType = typeof(DbContextOptions<>).MakeGenericType(contextType);

                IRepositoryEndpoint repoEndpoint = (IRepositoryEndpoint)repoType.New(provider, connectionString);

                Type      storeDbType = typeof(DataContext<>).MakeGenericType(DbRegistry.GetDbStore(contextType));
                Type storeOptionsType = typeof(DbContextOptions<>).MakeGenericType(storeDbType);
                Type    storeRepoType = typeof(RepositoryEndpoint<>).MakeGenericType(storeDbType);

                IRepositoryEndpoint storeEndpoint = (IRepositoryEndpoint)storeRepoType.New(repoEndpoint);

                Type   istoreRepoType = typeof(IRepositoryEndpoint<>).MakeGenericType(storeDbType);
                Type    ipoolRepoType = typeof(IRepositoryContextPool<>).MakeGenericType(storeDbType);
                Type ifactoryRepoType = typeof(IRepositoryContextFactory<>).MakeGenericType(storeDbType);
                Type    idataRepoType = typeof(IRepositoryContext<>).MakeGenericType(storeDbType);

                repoEndpoint.PoolSize = poolsize;

                IRepositoryEndpoint globalEndpoint = RepositoryManager.AddEndpoint(repoEndpoint);

                AddDbContextConfiguration(globalEndpoint.Context);

                registry.AddObject(iRepoType, globalEndpoint);
                registry.AddObject(repoType, globalEndpoint);
                registry.AddObject(repoOptionsType, globalEndpoint.Options);

                registry.AddObject(istoreRepoType, storeEndpoint);
                registry.AddObject(ipoolRepoType, storeEndpoint);
                registry.AddObject(ifactoryRepoType, storeEndpoint);
                registry.AddObject(idataRepoType, storeEndpoint);
                registry.AddObject(storeRepoType, storeEndpoint);
                registry.AddObject(storeOptionsType, storeEndpoint.Options);

                RepositoryManager.AddEndpointPool(globalEndpoint.ContextType, poolsize);
            }

            return this;
        }

        public IServiceSetup ConfigureServices(Assembly[] assemblies = null)
        {
            Assemblies ??= AppDomain.CurrentDomain.GetAssemblies();

            AddMapper(new DataMapper());

            AddCaching();

            AddIdentityServer();

            AddPolicies();

            ConfigureEndpoints(Assemblies);

            ConfigureClients(Assemblies);

            AddImplementations(Assemblies);

            AddSwagger();

            registry.MergeServices();

            return this;
        }
    }
}
