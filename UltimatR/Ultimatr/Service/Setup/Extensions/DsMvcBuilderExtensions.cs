using System;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OData.Edm;

namespace UltimatR
{
    public static class DsMvcBuilderExtensions
    {
        public static IMvcBuilder AddDataService<TContext>(this IMvcBuilder mvcBuilder, string routePrefix, int pageLimit = 1000) where TContext : DbContext
        {
            var endpoint = addDsEntitySets<TContext>();

            routePrefix = assignDsoControllerRoute<TContext>(routePrefix);

            mvcBuilder.AddOData((opt) =>
            {
                opt.RouteOptions.EnableQualifiedOperationCall = true;                
                opt.RouteOptions.EnableUnqualifiedOperationCall = true;
                opt.RouteOptions.EnableKeyInParenthesis = true;
                opt.RouteOptions.EnableKeyAsSegment = false;
                opt.RouteOptions.EnableControllerNameCaseInsensitive = true;
                opt.EnableQueryFeatures(pageLimit);
                opt.AddRouteComponents(routePrefix, endpoint.GetServiceModel<IEdmModel>());
            });

            return mvcBuilder;
        }
        public static IMvcBuilder AddDataService<TContext>(this IMvcBuilder mvcBuilder, int pageLimit = 1000) where TContext : DbContext
        {
            return AddDataService<TContext>(mvcBuilder, null, pageLimit);
        }

        private static IRepositoryEndpoint<TContext> addDsEntitySets<TContext>() where TContext : DbContext
        {
            RepositoryManager.TryGetEndpoint<TContext>(out var endpoint);

            Assembly[] asm = AppDomain.CurrentDomain.GetAssemblies();
            var otypes = asm.SelectMany(a => a.GetTypes()
                .Where(type => typeof(ODataController).IsAssignableFrom(type)).ToArray())
                .Where(b => !b.IsAbstract && b.BaseType.IsGenericType && b.BaseType.GenericTypeArguments.Length == 3)
                .Select(a => a.BaseType).ToArray();

            foreach(var types in otypes)
            {
                var genTypes = types.GenericTypeArguments;
                if (DsRegistry.GetDsStore(typeof(TContext)) == genTypes[1])
                    endpoint.ServiceEntitySet(genTypes[2]);
            }

            return endpoint;
        }

        private static string AddDsEndpointPrefix(Type contextType, string routePrefix = null)
        {
            var iface = DbRegistry.GetDbStore(contextType);
            return GetDsoControllerRoute(iface, routePrefix);
        }

        private static string GetDsoControllerRoute(Type iface, string routePrefix = null)
        {
            if (iface == typeof(IEntryStore))
                return (routePrefix != null) ? DsoControllerRoute.EntryStore = routePrefix : DsoControllerRoute.EntryStore;
            else if (iface == typeof(IEventStore))
                return (routePrefix != null) ? DsoControllerRoute.EventStore = routePrefix : DsoControllerRoute.EventStore;
            else if (iface == typeof(IReportStore))
                return (routePrefix != null) ? DsoControllerRoute.ReportStore = routePrefix : DsoControllerRoute.ReportStore;
            else if (iface == typeof(IConfigStore))
                return (routePrefix != null) ? DsoControllerRoute.ConfigStore = routePrefix : DsoControllerRoute.ConfigStore;
            else
                return (routePrefix != null) ? DsoControllerRoute.StateStore = routePrefix : DsoControllerRoute.StateStore;
        }

        private static string assignDsoControllerRoute<TContext>(string routePrefix = null) where TContext : DbContext
        {
            return AddDsEndpointPrefix(typeof(TContext), routePrefix);
        }
    }   
}

