using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace UltimatR
{
    public class EventBusRabbitMqModule 
    {
        public void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();

            context.Services.Configure<RabbitMqEventBusOptions>(configuration.GetSection("RabbitMQ:EventBus"));
        }

        public void OnApplicationInitialization()
        {
            ServiceManager.GetManager()
                .GetRequiredService<RabbitMqEventBus>()
                .Initialize();
        }
    }
}