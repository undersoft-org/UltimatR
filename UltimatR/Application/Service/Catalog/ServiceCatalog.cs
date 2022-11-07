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
// Last Modified On : 01-30-2022
// ***********************************************************************
// <copyright file="ServiceCatalog.cs" company="UltimatR.Framework">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Instant;
using System.Linq;
using System.Series;
using System.Uniques;

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
    public partial class ServiceCatalog : Catalog<ServiceDescriptor>, IServiceCatalog
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceCatalog"/> class.
        /// </summary>
        public ServiceCatalog() : base(true)
        {
            AddObject<IServiceProvider>();
            AddObject<IServiceCatalog>(this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceCatalog"/> class.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="manager">The manager.</param>
        public ServiceCatalog(IServiceCollection services, IServiceManager manager) : this()
        {
            Services = services;
            Manager = manager;                          
        }

        /// <summary>
        /// Gets the services.
        /// </summary>
        /// <value>The services.</value>
        public IServiceCollection Services { get; }
        /// <summary>
        /// Gets the manager.
        /// </summary>
        /// <value>The manager.</value>
        public IServiceManager Manager { get; }

        /// <summary>
        /// Gets or sets the <see cref="ServiceDescriptor" /> with the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>ServiceDescriptor.</returns>
        public ServiceDescriptor this[string name]
        {
            get => base[GetKey(name)];
            set => base.Set(GetKey(name), value);
        }
        /// <summary>
        /// Gets or sets the <see cref="ServiceDescriptor" /> with the specified service type.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns>ServiceDescriptor.</returns>
        public ServiceDescriptor this[Type serviceType]
        {
            get => base[GetKey(serviceType)];
            set => base.Set(GetKey(serviceType), value);
        }

        /// <summary>
        /// Gets the specified context type.
        /// </summary>
        /// <param name="contextType">Type of the context.</param>
        /// <returns>ServiceDescriptor.</returns>
        public ServiceDescriptor Get(Type contextType)
        {
            return base.Get(GetKey(contextType));
        }
        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <typeparam name="TService">The type of the t service.</typeparam>
        /// <returns>ServiceDescriptor.</returns>
        public ServiceDescriptor Get<TService>() where TService : class
        {
            return base.Get(GetKey<TService>());
        }

        /// <summary>
        /// Tries the get.
        /// </summary>
        /// <typeparam name="TService">The type of the t service.</typeparam>
        /// <param name="profile">The profile.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool TryGet<TService>(out ServiceDescriptor profile) where TService : class
        {
            if (base.TryGet(GetKey<TService>(), out profile))
                return true;
            return false;
        }

        /// <summary>
        /// Tries the add.
        /// </summary>
        /// <param name="profile">The profile.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public override bool TryAdd(ServiceDescriptor profile)
        {
            ulong key = GetKey(profile.ServiceType);
            if (ContainsKey(key)) return false;
            base.Add(key, profile);
            return true;
        }

        /// <summary>
        /// Removes this instance.
        /// </summary>
        /// <typeparam name="TContext">The type of the t context.</typeparam>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool Remove<TContext>() where TContext : class
        {
            return TryRemove(typeof(TContext));
        }

        /// <summary>
        /// Inners the add.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        protected override bool InnerAdd(ServiceDescriptor value)
        {
            var temp = base.InnerAdd(GetKey(value.ServiceType), value);
            return temp;
        }

        /// <summary>
        /// Inners the put.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        protected override ICard<ServiceDescriptor> InnerPut(ServiceDescriptor value)
        {
            var temp = base.InnerPut(GetKey(value.ServiceType), value);
            return temp;
        }

        /// <summary>
        /// Sets the specified descriptor.
        /// </summary>
        /// <param name="descriptor">The descriptor.</param>
        /// <returns>ICard&lt;ServiceDescriptor&gt;.</returns>
        public override ICard<ServiceDescriptor> Set(ServiceDescriptor descriptor)
        {
            return base.Set(GetKey(descriptor.ServiceType), descriptor);
        }

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        public override void Add(ServiceDescriptor item)
        {
            base.Add(item);
        }

        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>System.UInt64.</returns>
        public ulong GetKey(ServiceDescriptor item)
        {
            return item.ServiceType.FullName
                                   .UniqueKey64();
        }
        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>System.UInt64.</returns>
        public ulong GetKey(Type item)
        {
            return item.FullName
                       .UniqueKey64();
        }
        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>System.UInt64.</returns>
        public ulong GetKey(string item)
        {
            return item.UniqueKey64();
        }
        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>System.UInt64.</returns>
        public ulong GetKey<T>()
        {
            return typeof(T).FullName
                            .UniqueKey64();
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    foreach (var service in this)
                        service.Dispose();
                    base.Dispose(true);
                }
                disposedValue = true;
            }
        }

        /// <summary>
        /// Indexes the of.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>System.Int32.</returns>
        public override int IndexOf(ServiceDescriptor item)
        {
            return base.IndexOf(GetKey(item), item);
        }

        /// <summary>
        /// Inserts an item to the <see cref="T:System.Collections.Generic.IList`1" /> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="item" /> should be inserted.</param>
        /// <param name="item">The object to insert into the <see cref="T:System.Collections.Generic.IList`1" />.</param>
        public override void Insert(int index, ServiceDescriptor item)
        {
            base.Insert(index, GetCard(GetKey(item), item));
        }

        /// <summary>
        /// Determines whether this instance contains the object.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns><c>true</c> if [contains] [the specified item]; otherwise, <c>false</c>.</returns>
        public override bool Contains(ServiceDescriptor item)
        {
            return base.Contains(GetKey(item), item);
        }

        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1" /> to an <see cref="T:System.Array" />, starting at a particular <see cref="T:System.Array" /> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins.</param>
        public override void CopyTo(ServiceDescriptor[] array, int arrayIndex)
        {
            base.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        /// <returns><see langword="true" /> if <paramref name="item" /> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, <see langword="false" />. This method also returns <see langword="false" /> if <paramref name="item" /> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1" />.</returns>
        public override bool Remove(ServiceDescriptor item)
        {
            return base.TryRemove(GetKey(item.ServiceType), item);
        }

        /// <summary>
        /// Determines whether this instance contains key.
        /// </summary>
        /// <typeparam name="TService">The type of the t service.</typeparam>
        /// <returns><c>true</c> if this instance contains key; otherwise, <c>false</c>.</returns>
        public bool ContainsKey<TService>()
        {
            return base.ContainsKey(GetKey<TService>());
        }
        /// <summary>
        /// Determines whether the specified type contains key.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if the specified type contains key; otherwise, <c>false</c>.</returns>
        public bool ContainsKey(Type type)
        {
            return base.ContainsKey(GetKey(type));
        }

        /// <summary>
        /// Merges the services.
        /// </summary>
        public void MergeServices()
        {
            if (Services.Count == Count) return;

            var sdeck = new Album<ServiceDescriptor>(true);

            Services.ForEach(s =>
            {
                sdeck.Add(GetKey(s.ServiceType), s);
                if (!Contains(s))
                {
                    Add(s);
                }
            });

            this.ForEach(c =>
            {
                if (!sdeck.Contains(GetKey(c.ServiceType), c))
                {
                    Services.Add(c);
                }
            });
        }
    }
}
