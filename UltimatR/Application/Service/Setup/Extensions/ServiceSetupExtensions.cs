using UltimatR;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceSetupExtensions
    {
        public static IServiceSetup AddServiceSetup(this IServiceCollection services) 
        {
            return new ServiceSetup(services);
        }     
    }
}
