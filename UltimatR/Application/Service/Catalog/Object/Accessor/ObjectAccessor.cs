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
// Last Modified On : 12-10-2021
// ***********************************************************************
// <copyright file="ObjectAccessor.cs" company="UltimatR.Framework">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>

/// <summary>
/// The UltimatR namespace.
/// </summary>
namespace UltimatR
{
    /// <summary>
    /// Class ObjectAccessor.
    /// Implements the <see cref="UltimatR.ObjectAccessor" />
    /// Implements the <see cref="UltimatR.IObjectAccessor{T}" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="UltimatR.ObjectAccessor" />
    /// <seealso cref="UltimatR.IObjectAccessor{T}" />
    public class ObjectAccessor<T> : ObjectAccessor, IObjectAccessor<T>
    {
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public new T Value { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectAccessor{T}"/> class.
        /// </summary>
        public ObjectAccessor()
        {            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectAccessor{T}"/> class.
        /// </summary>
        /// <param name="obj">The object.</param>
        public ObjectAccessor(T obj)
        {
            Value = obj;
        }
    }

    /// <summary>
    /// Class ObjectAccessor.
    /// Implements the <see cref="UltimatR.ObjectAccessor" />
    /// Implements the <see cref="UltimatR.IObjectAccessor{T}" />
    /// </summary>
    /// <seealso cref="UltimatR.ObjectAccessor" />
    /// <seealso cref="UltimatR.IObjectAccessor{T}" />
    public class ObjectAccessor : IObjectAccessor
    {
        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>The value.</value>
        public object Value { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectAccessor"/> class.
        /// </summary>
        public ObjectAccessor()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectAccessor"/> class.
        /// </summary>
        /// <param name="obj">The object.</param>
        public ObjectAccessor(object obj)
        {
            Value = obj;
        }
    }
}