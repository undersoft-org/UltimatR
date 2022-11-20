﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace UltimatR
{
    public interface IServiceManager : IRepositoryManager, IServiceProvider
    {
        IServiceRegistry Registry { get; }
        IServiceConfiguration Configuration { get; set; }
        IServiceProvider Provider { get; }
        IServiceScope Scope { get; }

        T GetOrNewService<T>();
        object GetRequiredService(Type type);
        T GetRequiredService<T>();
        Lazy<T> GetRequiredServiceLazy<T>();
        T GetService<T>();
        object GetSingleton(Type type);
        T GetSingleton<T>() where T : class;
        Lazy<T> GetServiceLazy<T>();
        IEnumerable<object> GetServices(Type type);
        IEnumerable<T> GetServices<T>();
        Lazy<IEnumerable<T>> GetServicesLazy<T>();
        ObjectFactory NewFactory(Type instanceType, Type[] constrTypes);
        ObjectFactory NewFactory<T>(Type[] constrTypes);
        T NewService<T>(params object[] parameters);
    }
}