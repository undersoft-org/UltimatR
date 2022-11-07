using Microsoft.Extensions.DependencyInjection;

namespace UltimatR
{
    public partial class ServiceSetup
    {
        public IServiceSetup AddServiceImplementations()
        {
            var service = catalog;
            service.AddScoped<DbContextStore>();
            service.AddScoped<IDbContextStore,  DbContextStore>();
            service.AddScoped<IUltimateService, UltimateService>();
            service.AddScoped<IUltimatr,       Ultimatr>();
            service.AddHttpContextAccessor();

            return this;
        }
    }
}