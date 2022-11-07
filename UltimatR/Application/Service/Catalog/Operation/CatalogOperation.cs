// ***********************************************************************
// Assembly         : UltimatR.Framework
// Authors          : darisuz.hanc < undersoft.org >
// Participants
// Patronate        : m.krzetowski (architect), k.reszka (team-leader)
// Contribution     : d.hanc (r&d.soft.developer), p.grys (senior.soft.engineer)
// Development      : p.gasowski (jr.soft.developer)
// Business         : k.golos (po) m.rafalski (pm), m.korzeniewski (analyst) 
// QA               : a.urbanek
// DevOps           : k.manikowski        
// Created          : 02-05-2022
//
// Last Modified By : darisuz.hanc < undersoft.org >
// Last Modified On : 01-08-2022
// ***********************************************************************
// <copyright file="CatalogOperation.cs" company="UltimatR.Framework">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>

/// <summary>
/// The UltimatR namespace.
/// </summary>
namespace UltimatR
{
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Logs;
    using System.Reflection;

    /// <summary>
    /// Class ServiceCatalog.
    /// Implements the <see cref="System.Series.Catalog{Microsoft.Extensions.DependencyInjection.ServiceDescriptor}" />
    /// Implements the <see cref="UltimatR.IServiceCatalog" />
    /// </summary>
    /// <seealso cref="System.Series.Catalog{Microsoft.Extensions.DependencyInjection.ServiceDescriptor}" />
    /// <seealso cref="UltimatR.IServiceCatalog" />
    public partial class ServiceCatalog
    {
        #region Methods

        /// <summary>
        /// Builds the service provider from factory.
        /// </summary>
        /// <returns>IServiceProvider.</returns>
        public IServiceProvider BuildServiceProviderFromFactory()
        {
            foreach (var service in Services)
            {
                var factoryInterface = service.ImplementationInstance?.GetType()
                    .GetTypeInfo()
                    .GetInterfaces()
                    .FirstOrDefault(i => i.GetTypeInfo().IsGenericType &&
                                         i.GetGenericTypeDefinition() == typeof(IServiceProviderFactory<>));

                if (factoryInterface == null)
                {
                    continue;
                }

                var containerBuilderType = factoryInterface.GenericTypeArguments[0];
                return (IServiceProvider)typeof(ServiceCatalog)
                       .GetTypeInfo()
                       .GetMethods()
                       .Single(m => m.Name == nameof(BuildServiceProviderFromFactory) && m.IsGenericMethod)
                       .MakeGenericMethod(containerBuilderType)
                       .Invoke(null, new object[] {this , null });
            }

            return this.BuildServiceProvider();
        }

        /// <summary>
        /// Builds the service provider from factory.
        /// </summary>
        /// <typeparam name="TContainerBuilder">The type of the t container builder.</typeparam>
        /// <param name="builderAction">The builder action.</param>
        /// <returns>IServiceProvider.</returns>
        public IServiceProvider BuildServiceProviderFromFactory<TContainerBuilder>([NotNull] Action<TContainerBuilder> builderAction = null)
        {
            var serviceProviderFactory = GetSingleton<IServiceProviderFactory<TContainerBuilder>>();
            if (serviceProviderFactory == null)
            {
                Log.Failure<Datalog, Exception>(null, $"Could not find {typeof(IServiceProviderFactory<TContainerBuilder>).FullName} in {this}.");
            }

            var builder = serviceProviderFactory.CreateBuilder(this);
            builderAction?.Invoke(builder);
            return serviceProviderFactory.CreateServiceProvider(builder);
        }

        /// <summary>
        /// Gets the required service.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>T.</returns>
        public T GetRequiredService<T>() where T : class
        {
            return 
                GetSingleton<IServiceManager>()
                .Provider
                .GetRequiredService<T>();
        }

        /// <summary>
        /// Gets the required service.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>System.Object.</returns>
        public object GetRequiredService(Type type)
        {
            return 
                GetSingleton<IServiceManager>()
                .Provider
                .GetRequiredService(type);
        }

        /// <summary>
        /// Gets the required service lazy.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>Lazy&lt;T&gt;.</returns>
        public Lazy<T> GetRequiredServiceLazy<T>() where T : class
        {
            return new Lazy<T>(GetRequiredService<T>, true);
        }

        /// <summary>
        /// Gets the required service lazy.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>Lazy&lt;System.Object&gt;.</returns>
        public Lazy<object> GetRequiredServiceLazy(Type type)
        {
            return new Lazy<object>(() => GetRequiredService(type), true);
        }

        /// <summary>
        /// Gets the service lazy.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>Lazy&lt;T&gt;.</returns>
        public Lazy<T> GetServiceLazy<T>() where T : class
        {
            return new Lazy<T>(GetService<T>, true);
        }

        /// <summary>
        /// Gets the service lazy.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>Lazy&lt;System.Object&gt;.</returns>
        public Lazy<object> GetServiceLazy(Type type)
        {
            return new Lazy<object>(() => GetService(type), true);
        }

        /// <summary>
        /// Gets the provider.
        /// </summary>
        /// <returns>IServiceProvider.</returns>
        public IServiceProvider GetProvider()
        {
            return ((IObjectAccessor<IServiceProvider>)
                    Get<IObjectAccessor<IServiceProvider>>()?
                   .ImplementationInstance).Value;
        }

        /// <summary>
        /// Gets the required singleton.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>T.</returns>
        /// <exception cref="System.InvalidOperationException">Could not find singleton service: "
        ///                                                     + typeof(T).AssemblyQualifiedName</exception>
        public T GetRequiredSingleton<T>() where T : class
        {
            var service = GetSingleton<T>();
            if (service == null)
            {
                throw new InvalidOperationException("Could not find singleton service: "
                                                    + typeof(T).AssemblyQualifiedName);
            }

            return service;
        }

        /// <summary>
        /// Gets the singleton.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>T.</returns>
        public T GetSingleton<T>() where T : class
        {
            return (T)Get<T>()?.ImplementationInstance;
        }

        /// <summary>
        /// Gets the singleton.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>System.Object.</returns>
        public object GetSingleton(Type type)
        {
            return Get(type)?.ImplementationInstance;
        }

        /// <summary>
        /// Determines whether this instance is added.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns><c>true</c> if this instance is added; otherwise, <c>false</c>.</returns>
        public bool IsAdded<T>() where T : class
        {
            return ContainsKey<T>();
        }

        /// <summary>
        /// Determines whether the specified type is added.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if the specified type is added; otherwise, <c>false</c>.</returns>
        public bool IsAdded(Type type)
        {
            return ContainsKey(type);
        }

        /// <summary>
        /// Gets the service.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>T.</returns>
        internal T GetService<T>() where T : class
        {
            return GetSingleton<IServiceManager>()
                   .Provider
                   .GetService<T>();
        }

        /// <summary>
        /// Gets the service.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>System.Object.</returns>
        internal object GetService(Type type)
        {
            return GetSingleton<IServiceManager>()
                   .Provider
                   .GetService(type);
        }
     
        #endregion
    }
}
