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
// Last Modified On : 01-16-2022
// ***********************************************************************
// <copyright file="IServiceCatalog.cs" company="UltimatR.Framework">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>

using Microsoft.Extensions.DependencyInjection;
using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Series;

/// <summary>
/// The UltimatR namespace.
/// </summary>
namespace UltimatR
{
    /// <summary>
    /// Interface IServiceCatalog
    /// Implements the <see cref="Microsoft.Extensions.DependencyInjection.IServiceCollection" />
    /// </summary>
    /// <seealso cref="Microsoft.Extensions.DependencyInjection.IServiceCollection" />
    public interface IServiceCatalog : IServiceCollection
    {
        /// <summary>
        /// Gets or sets the <see cref="ServiceDescriptor"/> with the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>ServiceDescriptor.</returns>
        ServiceDescriptor this[string name] { get; set; }
        /// <summary>
        /// Gets or sets the <see cref="ServiceDescriptor"/> with the specified service type.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns>ServiceDescriptor.</returns>
        ServiceDescriptor this[Type serviceType] { get; set; }

        /// <summary>
        /// Gets the manager.
        /// </summary>
        /// <value>The manager.</value>
        IServiceManager Manager { get; }
        /// <summary>
        /// Gets the services.
        /// </summary>
        /// <value>The services.</value>
        IServiceCollection Services { get; }

        /// <summary>
        /// Adds the object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>ObjectAccessor&lt;T&gt;.</returns>
        ObjectAccessor<T> AddObject<T>() where T : class;
        /// <summary>
        /// Adds the object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="accessor">The accessor.</param>
        /// <returns>ObjectAccessor&lt;T&gt;.</returns>
        ObjectAccessor<T> AddObject<T>(ObjectAccessor<T> accessor) where T : class;
        /// <summary>
        /// Adds the object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object.</param>
        /// <returns>ObjectAccessor&lt;T&gt;.</returns>
        ObjectAccessor<T> AddObject<T>(T obj) where T : class;
        /// <summary>
        /// Adds the object.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="obj">The object.</param>
        /// <returns>ObjectAccessor.</returns>
        ObjectAccessor AddObject(Type type , object obj);
        /// <summary>
        /// Adds the object.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>ObjectAccessor.</returns>
        ObjectAccessor AddObject(Type type);
        /// <summary>
        /// Builds the service provider from factory.
        /// </summary>
        /// <returns>IServiceProvider.</returns>
        IServiceProvider BuildServiceProviderFromFactory();
        /// <summary>
        /// Builds the service provider from factory.
        /// </summary>
        /// <typeparam name="TContainerBuilder">The type of the t container builder.</typeparam>
        /// <param name="builderAction">The builder action.</param>
        /// <returns>IServiceProvider.</returns>
        IServiceProvider BuildServiceProviderFromFactory<TContainerBuilder>([NotNull] Action<TContainerBuilder> builderAction = null);
        /// <summary>
        /// Determines whether the specified type contains key.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if the specified type contains key; otherwise, <c>false</c>.</returns>
        bool ContainsKey(Type type);
        /// <summary>
        /// Determines whether this instance contains key.
        /// </summary>
        /// <typeparam name="TService">The type of the t service.</typeparam>
        /// <returns><c>true</c> if this instance contains key; otherwise, <c>false</c>.</returns>
        bool ContainsKey<TService>();
        /// <summary>
        /// Gets the specified context type.
        /// </summary>
        /// <param name="contextType">Type of the context.</param>
        /// <returns>ServiceDescriptor.</returns>
        ServiceDescriptor Get(Type contextType);
        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <typeparam name="TService">The type of the t service.</typeparam>
        /// <returns>ServiceDescriptor.</returns>
        ServiceDescriptor Get<TService>() where TService : class;
        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>System.UInt64.</returns>
        ulong GetKey(ServiceDescriptor item);
        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>System.UInt64.</returns>
        ulong GetKey(string item);
        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>System.UInt64.</returns>
        ulong GetKey(Type item);
        /// <summary>
        /// Gets the object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>T.</returns>
        T GetObject<T>() where T : class;
        /// <summary>
        /// Gets the object.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>System.Object.</returns>
        object GetObject(Type type);
        /// <summary>
        /// Gets the provider.
        /// </summary>
        /// <returns>IServiceProvider.</returns>
        IServiceProvider GetProvider();
        /// <summary>
        /// Gets the required object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>T.</returns>
        T GetRequiredObject<T>() where T : class;
        /// <summary>
        /// Gets the required service.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>System.Object.</returns>
        object GetRequiredService(Type type);
        /// <summary>
        /// Gets the required service.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>T.</returns>
        T GetRequiredService<T>() where T : class;
        /// <summary>
        /// Gets the required service lazy.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>Lazy&lt;System.Object&gt;.</returns>
        Lazy<object> GetRequiredServiceLazy(Type type);
        /// <summary>
        /// Gets the required service lazy.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>Lazy&lt;T&gt;.</returns>
        Lazy<T> GetRequiredServiceLazy<T>() where T : class;
        /// <summary>
        /// Gets the required singleton.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>T.</returns>
        T GetRequiredSingleton<T>() where T : class;
        /// <summary>
        /// Gets the service lazy.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>Lazy&lt;System.Object&gt;.</returns>
        Lazy<object> GetServiceLazy(Type type);
        /// <summary>
        /// Gets the service lazy.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>Lazy&lt;T&gt;.</returns>
        Lazy<T> GetServiceLazy<T>() where T : class;
        /// <summary>
        /// Gets the singleton.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>T.</returns>
        T GetSingleton<T>() where T : class;
        /// <summary>
        /// Determines whether the specified type is added.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if the specified type is added; otherwise, <c>false</c>.</returns>
        bool IsAdded(Type type);
        /// <summary>
        /// Determines whether this instance is added.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns><c>true</c> if this instance is added; otherwise, <c>false</c>.</returns>
        bool IsAdded<T>() where T : class;
        /// <summary>
        /// Merges the services.
        /// </summary>
        void MergeServices();
        /// <summary>
        /// Removes this instance.
        /// </summary>
        /// <typeparam name="TContext">The type of the t context.</typeparam>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool Remove<TContext>() where TContext : class;
        /// <summary>
        /// Sets the specified descriptor.
        /// </summary>
        /// <param name="descriptor">The descriptor.</param>
        /// <returns>ICard&lt;ServiceDescriptor&gt;.</returns>
        ICard<ServiceDescriptor> Set(ServiceDescriptor descriptor);
        /// <summary>
        /// Tries the add.
        /// </summary>
        /// <param name="profile">The profile.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool TryAdd(ServiceDescriptor profile);
        /// <summary>
        /// Tries the add object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>ObjectAccessor&lt;T&gt;.</returns>
        ObjectAccessor<T> TryAddObject<T>() where T : class;
        /// <summary>
        /// Tries the add object.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>ObjectAccessor.</returns>
        ObjectAccessor TryAddObject(Type type);
        /// <summary>
        /// Tries the get.
        /// </summary>
        /// <typeparam name="TService">The type of the t service.</typeparam>
        /// <param name="profile">The profile.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool TryGet<TService>(out ServiceDescriptor profile) where TService : class;
    }
}