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
// <copyright file="Query.cs" company="UltimatR.Framework">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>

using MediatR;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Series;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// The UltimatR namespace.
/// </summary>
namespace UltimatR
{
    /// <summary>
    /// Class Query.
    /// Implements the <see cref="MediatR.IRequest{TResult}" />
    /// Implements the <see cref="UltimatR.IQuery{TEntity}" />
    /// </summary>
    /// <typeparam name="TStore">The type of the t store.</typeparam>
    /// <typeparam name="TEntity">The type of the t entity.</typeparam>
    /// <typeparam name="TResult">The type of the t result.</typeparam>
    /// <seealso cref="MediatR.IRequest{TResult}" />
    /// <seealso cref="UltimatR.IQuery{TEntity}" />
    public abstract class Query<TStore, TEntity, TResult> : IRequest<TResult>, IQuery<TEntity> where TEntity : Entity where TStore : IDataStore
    {
        /// <summary>
        /// Gets the keys.
        /// </summary>
        /// <value>The keys.</value>
        public object[] Keys { get; }
        /// <summary>
        /// Gets the sort.
        /// </summary>
        /// <value>The sort.</value>
        [JsonIgnore] public SortExpression<TEntity> Sort { get; }
        /// <summary>
        /// Gets the expanders.
        /// </summary>
        /// <value>The expanders.</value>
        [JsonIgnore] public Expression<Func<TEntity, object>>[] Expanders { get; }
        /// <summary>
        /// Gets the predicate.
        /// </summary>
        /// <value>The predicate.</value>
        [JsonIgnore] public Expression<Func<TEntity, bool>> Predicate { get; }

        /// <summary>
        /// Gets the input.
        /// </summary>
        /// <value>The input.</value>
        public object Input => new object[] { Keys, Predicate, Expanders };

        /// <summary>
        /// Gets the output.
        /// </summary>
        /// <value>The output.</value>
        public object Output => new object[] { Keys, Predicate, Expanders };

        /// <summary>
        /// Initializes a new instance of the <see cref="Query{TStore, TEntity, TResult}"/> class.
        /// </summary>
        /// <param name="keys">The keys.</param>
        public Query(params object[] keys)
        {
            Keys = keys;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Query{TStore, TEntity, TResult}"/> class.
        /// </summary>
        /// <param name="keys">The keys.</param>
        /// <param name="expanders">The expanders.</param>
        public Query(object[] keys, params Expression<Func<TEntity, object>>[] expanders)
        {
            Keys = keys;
            Expanders = expanders;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Query{TStore, TEntity, TResult}"/> class.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        public Query(Expression<Func<TEntity, bool>> predicate)
        {
            Predicate = predicate;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Query{TStore, TEntity, TResult}"/> class.
        /// </summary>
        /// <param name="expanders">The expanders.</param>
        public Query(params Expression<Func<TEntity, object>>[] expanders)
        {       
            Expanders = expanders;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Query{TStore, TEntity, TResult}"/> class.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="expanders">The expanders.</param>
        public Query(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] expanders)
        {
            Predicate = predicate;
            Expanders = expanders;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Query{TStore, TEntity, TResult}"/> class.
        /// </summary>
        /// <param name="sortTerms">The sort terms.</param>
        /// <param name="expanders">The expanders.</param>
        public Query(SortExpression<TEntity> sortTerms, params Expression<Func<TEntity, object>>[] expanders)
        {
            Sort = sortTerms;
            Expanders = expanders;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Query{TStore, TEntity, TResult}"/> class.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="sortTerms">The sort terms.</param>
        /// <param name="expanders">The expanders.</param>
        public Query(Expression<Func<TEntity, bool>> predicate, SortExpression<TEntity> sortTerms,  params Expression<Func<TEntity, object>>[] expanders)
        {
            Predicate = predicate;
            Sort = sortTerms;
            Expanders = expanders;
        }


    }
}
