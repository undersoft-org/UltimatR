using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection.Extensions;
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
using Microsoft.OpenApi.Models;
using System;
using System.Linq;
using System.Reflection;

namespace UltimatR
{
    public partial class ServiceSetup : IServiceSetup
    {
        private IServiceManager manager { get; }
        private IServiceCatalog catalog { get; set; }
        private IServiceConfiguration configuration => manager.Configuration;
        private IServiceCollection services => catalog.Services;
        private Assembly[] Assemblies;
        private string[] apiVersions=new string[1] { "1"};

        public ServiceSetup(IServiceCollection services)
        {
            manager = new ServiceManager(services);
            catalog = manager.Catalog;          
        }
        public ServiceSetup(IServiceCollection services, IConfiguration configuration) : this(services)
        {
            manager.Configuration = new ServiceConfiguration(configuration);
        }
        public IServiceSetup ConfigureServices(Assembly[] assemblies = null)
        {
            Assemblies ??= AppDomain.CurrentDomain.GetAssemblies();           

            AddMapper(new RepositoryMapper());

            AddCaching();

            AddIdentityServer();

            AddPolicies();

            ConfigureEndpoints(Assemblies);

            ConfigureClients(Assemblies);

            AddImplementations(Assemblies);

            AddSwagger();                

            catalog.MergeServices();

            return this;
        }

        public IServiceSetup ConfigureEndpoints(Assembly[] assemblies = null)
        {        
            var       config = configuration;
                assemblies ??= AppDomain.CurrentDomain.GetAssemblies();
            var definedTypes = assemblies.SelectMany(a => a.DefinedTypes).ToArray();
            var    endpoints = config.Endpoints();

            var repoEndpoints = new RepositoryEndpoints();
            services.AddSingleton(catalog.AddObject<IRepositoryEndpoints>(repoEndpoints).Value);      

            foreach (var endpoint in endpoints)
            {
                var connectionString = config.EndpointConnectionString(endpoint);
                var         provider = config.EndpointProvider(endpoint);              
                var         poolsize = config.EndpointPoolSize(endpoint);
                var      contextType = definedTypes.Where(t => t.FullName == endpoint.Key)
                                              .Select(t => t.UnderlyingSystemType)
                                              .FirstOrDefault();             

                        if (provider == EndpointProvider.None ||
                    connectionString == null ||
                         contextType == null)
                                    continue;

                Type        iRepoType = typeof(IRepositoryEndpoint<>).MakeGenericType(contextType);
                Type         repoType = typeof(RepositoryEndpoint<>).MakeGenericType(contextType);
                Type  repoOptionsType = typeof(DbContextOptions<>).MakeGenericType(contextType);

                var      repoEndpoint = (IRepositoryEndpoint)repoType.New(provider, connectionString);
               
                Type      storeDbType = typeof(DbContext<>).MakeGenericType(DbCatalog.GetDbStore(contextType));
                Type storeOptionsType = typeof(DbContextOptions<>).MakeGenericType(storeDbType);
                Type    storeRepoType = typeof(RepositoryEndpoint<>).MakeGenericType(storeDbType);

                var     storeEndpoint = (IRepositoryEndpoint)storeRepoType.New(repoEndpoint);

                Type   istoreRepoType = typeof(IRepositoryEndpoint<>).MakeGenericType(storeDbType);
                Type    ipoolRepoType = typeof(IDataContextPool<>).MakeGenericType(storeDbType);
                Type ifactoryRepoType = typeof(IDataContextFactory<>).MakeGenericType(storeDbType);
                Type    idataRepoType = typeof(IDataContext<>).MakeGenericType(storeDbType);                               

                repoEndpoint.PoolSize = poolsize;
                
                var    globalEndpoint = RepositoryManager.AddEndpoint(repoEndpoint);                

                AddDbContextConfiguration(globalEndpoint.Context);

                catalog.AddObject(iRepoType,        globalEndpoint);
                catalog.AddObject(repoType,         globalEndpoint);
                catalog.AddObject(repoOptionsType,  globalEndpoint.Options);

                catalog.AddObject(istoreRepoType,   storeEndpoint);                
                catalog.AddObject(ipoolRepoType,    storeEndpoint);
                catalog.AddObject(ifactoryRepoType, storeEndpoint);
                catalog.AddObject(idataRepoType,    storeEndpoint);
                catalog.AddObject(storeRepoType,    storeEndpoint);                              
                catalog.AddObject(storeOptionsType, storeEndpoint.Options);                                      

                RepositoryManager.AddEndpointPool(globalEndpoint.ContextType, poolsize);
            }
            
            return this;
        }

        public IServiceSetup ConfigureClients(Assembly[] assemblies = null)
        {
            var       config = configuration;
                assemblies ??= AppDomain.CurrentDomain.GetAssemblies();
            var definedTypes = assemblies.SelectMany(a => a.DefinedTypes).ToArray();           
            var      clients = config.Clients();
            var  repoClients = new RepositoryClients();

            services.AddSingleton(catalog.AddObject<IRepositoryClients>(repoClients).Value);                        

            foreach (var client in clients)
            {
                var         provider = config.ClientProvider(client);
                var connectionString = config.ClientConnectionString(client).Trim();
                var         poolsize = config.ClientPoolSize(client);
                var   preContextType = definedTypes.Where(t => t.FullName.Contains(client.Key))
                                                   .Select(t => t.UnderlyingSystemType)
                                                   .FirstOrDefault();                
                        if (provider == ClientProvider.None ||
                    connectionString == null ||
                      preContextType == null)
                                    continue;              

                var contextTypes = new Type[] { preContextType.MakeGenericType(typeof(IEntryStore)), 
                                                preContextType.MakeGenericType(typeof(IReportStore)) };

                foreach (var contextType in contextTypes)
                {                    
                    var routePrefix = AddDsClientPrefix(contextType).Trim();
                    if (!connectionString.EndsWith('/')) connectionString += "/";
                    if (routePrefix.StartsWith('/')) routePrefix = routePrefix.Substring(1);
                    var _connectionString = $"{connectionString}{routePrefix}";                   

                    Type        iRepoType = typeof(IRepositoryClient<>).MakeGenericType(contextType);
                    Type         repoType = typeof(RepositoryClient<>).MakeGenericType(contextType);                    

                    var        repoClient = (IRepositoryClient)repoType.New(provider, _connectionString);

                    Type      storeDbType = typeof(DsContext<>).MakeGenericType(DsCatalog.GetDsStore(contextType));                    
                    Type    storeRepoType = typeof(RepositoryClient<>).MakeGenericType(storeDbType);

                    var storeClient = (IRepositoryClient)storeRepoType.New(repoClient);

                    Type   istoreRepoType = typeof(IRepositoryClient<>).MakeGenericType(storeDbType);
                    Type    ipoolRepoType = typeof(IDataContextPool<>).MakeGenericType(storeDbType);
                    Type ifactoryRepoType = typeof(IDataContextFactory<>).MakeGenericType(storeDbType);
                    Type    idataRepoType = typeof(IDataContext<>).MakeGenericType(storeDbType);

                      repoClient.PoolSize = poolsize;

                    var      globalClient = RepositoryManager.AddClient(repoClient);

                    catalog.AddObject(iRepoType, globalClient);
                    catalog.AddObject(repoType,  globalClient);                    

                    catalog.AddObject(istoreRepoType,   storeClient);
                    catalog.AddObject(ipoolRepoType,    storeClient);
                    catalog.AddObject(ifactoryRepoType, storeClient);
                    catalog.AddObject(idataRepoType,    storeClient);
                    catalog.AddObject(storeRepoType,    storeClient);                    

                    RepositoryManager.AddClientPool(globalClient.ContextType, poolsize);
                }
            }

            return this;
        }

        public IServiceSetup AddMapper(params Profile[] profiles)
        {            
            AddMapper(new RepositoryMapper(profiles));

            return this;
        }
        public IServiceSetup AddMapper(IRepositoryMapper mapper)
        {
            catalog.AddObject(mapper);
            catalog.AddObject<IDataMapper>(mapper);
            catalog.AddScoped<IMapper, RepositoryMapper>();

            return this;
        }
        public IServiceSetup AddMapper<TProfile>()
            where TProfile : Profile
        {
            AddMapper(new RepositoryMapper(typeof(TProfile).New<TProfile>()));            

            return this;
        }

        public IServiceSetup AddCaching()
        {           
            catalog.AddObject(GlobalCache.Catalog);
            
            Type[] stores = new Type[] { typeof(IEntryStore), 
                                         typeof(IReportStore), 
                                         typeof(IEventStore) };
            foreach (var item in stores)
            {
                AddDataCache(item);
            }

            return this;
        }

        public IServiceSetup AddDataCache(Type tstore)
        {
            Type idatacache = typeof(IDataCache<>).MakeGenericType(tstore);
            Type datacache = typeof(DataCache<>).MakeGenericType(tstore);

            var cache = datacache.New(catalog.GetObject<IDataCache>());
            catalog.AddObject(idatacache, cache);
            catalog.AddObject(datacache, cache);

            return this;
        }

        public IServiceSetup AddDataService(IMvcBuilder mvcBuilder, Type contextType, string routePrefix, int? pageLimit = null)
        {          
            var endpoint = AddEntitySets(contextType);

            routePrefix = AddDsEndpointPrefix(contextType, routePrefix);

            mvcBuilder.AddOData((opt) =>
            {
                opt.RouteOptions.EnableQualifiedOperationCall = true;
                opt.RouteOptions.EnableUnqualifiedOperationCall = false;
                opt.RouteOptions.EnableKeyInParenthesis = true;
                opt.RouteOptions.EnableKeyAsSegment = false;
                opt.RouteOptions.EnableControllerNameCaseInsensitive = true;
                opt.EnableQueryFeatures(pageLimit);
                opt.AddRouteComponents(routePrefix, endpoint.GetEdmModel());
            });

            return this;
        }

        public IServiceSetup AddDataService<TContext>(IMvcBuilder mvcBuilder, string routePrefix, int? pageLimit = null) where TContext : DbContext
        {
            var endpoint = AddEntitySets<TContext>();

            routePrefix = AddDsEndpointPrefix(typeof(TContext), routePrefix);

            mvcBuilder.AddOData((opt) =>
            {
                opt.RouteOptions.EnableQualifiedOperationCall = true;
                opt.RouteOptions.EnableUnqualifiedOperationCall = false;
                opt.RouteOptions.EnableKeyInParenthesis = true;
                opt.RouteOptions.EnableKeyAsSegment = false;
                opt.RouteOptions.EnableControllerNameCaseInsensitive = true;
                opt.EnableQueryFeatures(pageLimit);
                opt.AddRouteComponents(routePrefix, endpoint.GetEdmModel());
            });

            return this;
        }

        public IServiceSetup ConfigureDataServices(IMvcBuilder mvcBuilder, int? pageLimit = null)
        {
            var endpoints = manager.GetEndpoints();

            foreach (var endpoint in endpoints)
            {
                AddDataService(mvcBuilder, endpoint.ContextType, null, pageLimit);
            }

            return this;
        }

        public IServiceSetup AddImplementations(Assembly[] assemblies = null)
        {
            AddServiceImplementations();
            
            AddDomainImplementations();

            AddAppImplementations(assemblies);

            return this;
        }

        public IServiceSetup AddSwagger()
        {
            var ver = configuration.Version;
            var scopes = configuration.IdentityServerScopes().ToDictionary(key => key.GetValue<string>("Name"), value=> value.GetValue<string>("Description"));          
            catalog.AddSwaggerGen(s =>
                    {
                        s.DocumentFilter<IgnoreApiDocument>();
                        foreach (var version in apiVersions)
                        {
                            s.SwaggerDoc($"{version}", new OpenApiInfo
                            {
                                Version = ver,
                                Title = configuration.Title,
                                Description = configuration.Description
                            });
                        }
                        s.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                        {
                            Type = SecuritySchemeType.OAuth2,
                            Flows = new OpenApiOAuthFlows
                            {
                                ClientCredentials = new OpenApiOAuthFlow
                                {
                                    AuthorizationUrl=new Uri(configuration.IdentityServerAddress()),
                                    TokenUrl = new Uri($"{configuration.IdentityServerAddress()}/connect/token"),
                                    Scopes = scopes
                                }

                            }
                        });
                        s.AddSecurityRequirement(new OpenApiSecurityRequirement
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

        public IServiceSetup AddIdentityServer()
        {
            catalog.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme).AddIdentityServerAuthentication(options =>
            {
                options.Authority     = configuration.IdentityServerAddress();
                options.ApiName       = configuration.IdentityServerApiName();
                options.EnableCaching = true;
                options.CacheDuration = TimeSpan.FromMinutes(10);
            });

            catalog.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                    });
            });

            return this;
        }

        public IServiceSetup AddPolicies()
        {
                 catalog.AddAuthorization(options =>
            configuration.IdentityServerScopes()
                         .Select(s => s.GetValue<string>("Name"))
                         .ForEach(s => options.AddPolicy(s, policy => policy.RequireScope(s))));
            return this;
        }
        public IServiceSetup ConfigureApiVersions(string[] apiVersions)
        {
            this.apiVersions = apiVersions;
            return this;
        }
            private IServiceSetup AddSwaggerDsSupport()
        {
            catalog.GetObject<IMvcBuilder>().AddMvcOptions(options =>
            {
                foreach (var outputFormatter in options.OutputFormatters.OfType<OutputFormatter>().Where(x => x.SupportedMediaTypes.Count == 0))                
                    outputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/prs.odatatestxx-odata"));                

                foreach (var inputFormatter in options.InputFormatters.OfType<InputFormatter>().Where(x => x.SupportedMediaTypes.Count == 0))             
                     inputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/prs.odatatestxx-odata"));                
            });
            return this;
        }

        private IRepositoryEndpoint<TContext> AddEntitySets<TContext>() where TContext : DbContext
        {           
            return (IRepositoryEndpoint<TContext>)AddEntitySets(typeof(TContext));
        }
        private IRepositoryEndpoint AddEntitySets(Type contextType)
        {
            if (!RepositoryManager.TryGetEndpoint
                   (contextType, out var endpoint))
                       return endpoint;

            Assembly[] asm = Assemblies;

            var otypes = asm
                .SelectMany(a => a.GetTypes()
                .Where(type => typeof(ODataController)
                .IsAssignableFrom(type)).ToArray())
                .Where(b =>(!b.IsAbstract &&
                             b.BaseType.IsGenericType &&
                             b.BaseType.GenericTypeArguments.Length == 3))
                .Select(a => a.BaseType).ToArray();

            foreach (var types in otypes)
            {
                var genTypes = types.GenericTypeArguments;
                if (DsCatalog.GetDsStore(contextType) == genTypes[1])
                     endpoint.EntitySet(genTypes[2]);                
            }

            return endpoint;
        }


        private string AddDsEndpointPrefix(Type contextType, string routePrefix = null)
        {
            var iface = DbCatalog.GetDbStore(contextType);
            return GetDsRoutePrefix(iface, routePrefix);
        }
        private string AddDsClientPrefix(Type contextType, string routePrefix = null)
        {
            var iface = DsCatalog.GetDsStore(contextType);
            return GetDsRoutePrefix(iface, routePrefix);
        }
        private string GetDsRoutePrefix(Type iface, string routePrefix = null)
        {            
             if (iface == typeof(IEntryStore))            
                return (routePrefix != null) ? DsRoutePrefix.EntryStore = routePrefix : DsRoutePrefix.EntryStore;            
            else if (iface == typeof(IEventStore))            
                return (routePrefix != null) ? DsRoutePrefix.EventStore = routePrefix : DsRoutePrefix.EventStore;            
            else if (iface == typeof(IReportStore))
                return (routePrefix != null) ? DsRoutePrefix.ReportStore = routePrefix : DsRoutePrefix.ReportStore;            
            else if (iface == typeof(IConfigStore))
                return (routePrefix != null) ? DsRoutePrefix.ConfigStore = routePrefix : DsRoutePrefix.ConfigStore;
            else
                return (routePrefix != null) ? DsRoutePrefix.StateStore = routePrefix : DsRoutePrefix.StateStore;
        }

        private void AddDbContextConfiguration(DbContext context)
        {
            context.ChangeTracker.AutoDetectChangesEnabled = true;
            context.ChangeTracker.LazyLoadingEnabled = true;
            context.Database.AutoTransactionsEnabled = false;                      
        }

    }
}
