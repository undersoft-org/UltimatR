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
// Last Modified On : 01-13-2022
// ***********************************************************************
// <copyright file="CatalogObject.cs" company="UltimatR.Framework">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>

using Microsoft.Extensions.DependencyInjection;
using System;

/// <summary>
/// The UltimatR namespace.
/// </summary>
namespace UltimatR
{
    /// <summary>
    /// Class ServiceCatalog.
    /// Implements the <see cref="System.Series.Catalog{Microsoft.Extensions.DependencyInjection.ServiceDescriptor}" />
    /// Implements the <see cref="UltimatR.IServiceCatalog" />
    /// </summary>
    /// <seealso cref="System.Series.Catalog{Microsoft.Extensions.DependencyInjection.ServiceDescriptor}" />
    /// <seealso cref="UltimatR.IServiceCatalog" />
    public partial class ServiceCatalog
    {
        /// <summary>
        /// Tries the add object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>ObjectAccessor&lt;T&gt;.</returns>
        public ObjectAccessor<T> TryAddObject<T>() where T : class
        {           
            if (ContainsKey(typeof(ObjectAccessor<T>)))
            {
                return (ObjectAccessor<T>)Get<ObjectAccessor<T>>()?.ImplementationInstance;
            }

            return AddObject<T>();
        }

        /// <summary>
        /// Tries the add object.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>ObjectAccessor.</returns>
        public ObjectAccessor TryAddObject(Type type)
        {
            Type accessorType = typeof(ObjectAccessor<>).MakeGenericType(type);
            if (ContainsKey(accessorType))
            {
                return (ObjectAccessor)Get(accessorType)?.ImplementationInstance;
            }

            return AddObject(type);
        }

        /// <summary>
        /// Adds the object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>ObjectAccessor&lt;T&gt;.</returns>
        public ObjectAccessor<T> AddObject<T>() where T : class
        {
            return AddObject(new ObjectAccessor<T>());
        }

        /// <summary>
        /// Adds the object.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>ObjectAccessor.</returns>
        public ObjectAccessor AddObject(Type type)
        {
            Type oaType = typeof(ObjectAccessor<>).MakeGenericType(type);
            Type iaType = typeof(IObjectAccessor<>).MakeGenericType(type);

            ObjectAccessor accessor = (ObjectAccessor)oaType.New();

            if (ContainsKey(oaType))
            {
                return accessor;
            }

            Put(ServiceDescriptor.Singleton(oaType), accessor);
            Put(ServiceDescriptor.Singleton(iaType), accessor);

            return accessor;
        }

        /// <summary>
        /// Adds the object.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="obj">The object.</param>
        /// <returns>ObjectAccessor.</returns>
        public ObjectAccessor AddObject(Type type, object obj)
        {
            Type oaType = typeof(ObjectAccessor<>).MakeGenericType(type);
            Type iaType = typeof(IObjectAccessor<>).MakeGenericType(type);

            ObjectAccessor accessor = (ObjectAccessor)oaType.New(obj);

            if (ContainsKey(oaType))
            {
                return accessor;
            }
            
            Put(ServiceDescriptor.Singleton(oaType), accessor);
            Put(ServiceDescriptor.Singleton(iaType), accessor);

            if (obj != null)
                this.AddSingleton(type, obj);

            return accessor;
        }

        /// <summary>
        /// Adds the object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object.</param>
        /// <returns>ObjectAccessor&lt;T&gt;.</returns>
        public ObjectAccessor<T> AddObject<T>(T obj) where T : class
        {
            return AddObject(new ObjectAccessor<T>(obj));
        }

        /// <summary>
        /// Adds the object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="accessor">The accessor.</param>
        /// <returns>ObjectAccessor&lt;T&gt;.</returns>
        public ObjectAccessor<T> AddObject<T>(ObjectAccessor<T> accessor) where T : class
        {
            if (ContainsKey(typeof(ObjectAccessor<T>)))
            {
                return accessor;
            }

            Put(ServiceDescriptor.Singleton(typeof(ObjectAccessor<T>), accessor));
            Put(ServiceDescriptor.Singleton(typeof(IObjectAccessor<T>), accessor));
            
            if(accessor.Value != null)
                this.AddSingleton<T>(accessor.Value);

            return accessor;
        }

        /// <summary>
        /// Gets the object.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>System.Object.</returns>
        public object GetObject(Type type)
        {
            Type accessorType = typeof(IObjectAccessor<>).MakeGenericType(type);
            return ((ObjectAccessor)GetSingleton(accessorType))?.Value;
        }

        /// <summary>
        /// Gets the object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>T.</returns>
        public T GetObject<T>()
            where T : class 
        {
            return GetSingleton<IObjectAccessor<T>>()?.Value;
        }

        /// <summary>
        /// Gets the required object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>T.</returns>
        /// <exception cref="System.Exception">Could not find an object of {typeof(T).AssemblyQualifiedName} in  Be sure that you have used AddObjectAccessor before!</exception>
        public T GetRequiredObject<T>()
            where T : class
        {
            return GetObject<T>() ?? throw new Exception($"Could not find an object of {typeof(T).AssemblyQualifiedName} in  Be sure that you have used AddObjectAccessor before!");
        }
    }
}