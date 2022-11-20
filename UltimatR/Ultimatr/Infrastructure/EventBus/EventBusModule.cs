using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UltimatR;

namespace UltimatR
{
    public class EventBusModule
    {
        private ServiceConfigurationContext context { get; set; }

        public void PreConfigureServices(ServiceConfigurationContext context)
        {
            AddEventHandlers(context.Services);
        }

        private static void AddEventHandlers(IServiceRegistry services)
        {
            var localHandlers = new List<Type>();

            services.OnRegistred(context =>
            {
                if (context.ImplementationType.IsAssignableTo(typeof(IEventHandler<>)))
                {
                    localHandlers.Add(context.ImplementationType);
                }
            });

                services.Configure<EventBusOptions>(options =>
            {
                localHandlers.Select(h => !options.Handlers.Any(o => o.GetType().Equals(h)));
            });
        }
    }
}