using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using ProtoBuf.Grpc.Configuration;

namespace UltimatR
{
    internal class ProcedureServiceBinder : ServiceBinder
    {
        private readonly IServiceRegistry registry;

        public ProcedureServiceBinder(IServiceRegistry registry)
        {
            this.registry = registry;
        }

        public override IList<object> GetMetadata(MethodInfo method, Type contractType, Type serviceType)
        {
            var resolvedServiceType = serviceType;
            if (serviceType.IsInterface)
                resolvedServiceType = registry[serviceType]?.ImplementationType ?? serviceType;

            return base.GetMetadata(method, contractType, resolvedServiceType);
        }
    }
}