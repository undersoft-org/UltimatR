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
// Last Modified On : 01-12-2022
// ***********************************************************************
// <copyright file="IQuery.cs" company="UltimatR.Framework">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>

using System;
using System.Linq.Expressions;

/// <summary>
/// The UltimatR namespace.
/// </summary>
namespace UltimatR
{
    /// <summary>
    /// Interface IQuery
    /// Implements the <see cref="UltimatR.IDataIO" />
    /// </summary>
    /// <typeparam name="TEntity">The type of the t entity.</typeparam>
    /// <seealso cref="UltimatR.IDataIO" />
    public interface IQuery<TEntity> : IDataIO where TEntity : Entity
    {
        /// <summary>
        /// Gets the expanders.
        /// </summary>
        /// <value>The expanders.</value>
        Expression<Func<TEntity, object>>[] Expanders { get; }
        /// <summary>
        /// Gets the predicate.
        /// </summary>
        /// <value>The predicate.</value>
        Expression<Func<TEntity, bool>> Predicate { get; }
        /// <summary>
        /// Gets the sort.
        /// </summary>
        /// <value>The sort.</value>
        SortExpression<TEntity> Sort { get; }
    }
}