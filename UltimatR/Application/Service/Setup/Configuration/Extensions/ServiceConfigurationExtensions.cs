using UltimatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceConfigurationExtensions
    {
        public static IServiceCollection ReplaceConfiguration(this IServiceCollection services,
            IConfiguration configuration)
        {
            return services.Replace(ServiceDescriptor.Singleton(configuration));
        }

        public static IConfiguration GetConfiguration(this IServiceCatalog services)
        {
            var hostBuilderContext = services.GetSingleton<HostBuilderContext>();
            if (hostBuilderContext?.Configuration != null)
            {
                return hostBuilderContext.Configuration as IConfigurationRoot;
            }

            return services.GetSingleton<IConfiguration>();
        }
    }
}