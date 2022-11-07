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
// Last Modified On : 12-29-2021
// ***********************************************************************
// <copyright file="EventDto.cs" company="UltimatR.Framework">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>

using System;

/// <summary>
/// The UltimatR namespace.
/// </summary>
namespace UltimatR
{
    /// <summary>
    /// Class EventDto.
    /// Implements the <see cref="UltimatR.Dto" />
    /// </summary>
    /// <seealso cref="UltimatR.Dto" />
    public class EventDto : Dto
    {
        /// <summary>
        /// Gets or sets the event identifier.
        /// </summary>
        /// <value>The event identifier.</value>
        public long     EventId 
        { 
            get => base.Id; 
            set => base.Id = value; 
        }
        /// <summary>
        /// Gets or sets the type of the event.
        /// </summary>
        /// <value>The type of the event.</value>
        public string   EventType { get; set; }
        /// <summary>
        /// Gets or sets the aggregate identifier.
        /// </summary>
        /// <value>The aggregate identifier.</value>
        public ulong    AggregateId { get; set; }
        /// <summary>
        /// Gets or sets the type of the aggregate.
        /// </summary>
        /// <value>The type of the aggregate.</value>
        public string   AggregateType { get; set; }
        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>The data.</value>
        public string   Data { get; set; }
        /// <summary>
        /// Gets or sets the publish time.
        /// </summary>
        /// <value>The publish time.</value>
        public DateTime PublishTime { get; set; }
        /// <summary>
        /// Gets or sets the publish status.
        /// </summary>
        /// <value>The publish status.</value>
        public PublishStatus PublishStatus { get; set; }
    }

}