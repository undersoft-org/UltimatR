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
// Last Modified On : 01-11-2022
// ***********************************************************************
// <copyright file="EventBase.cs" company="UltimatR.Framework">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

/// <summary>
/// The UltimatR namespace.
/// </summary>
namespace UltimatR
{
    /// <summary>
    /// Class Event.
    /// Implements the <see cref="UltimatR.Entity" />
    /// Implements the <see cref="UltimatR.IEvent" />
    /// </summary>
    /// <seealso cref="UltimatR.Entity" />
    /// <seealso cref="UltimatR.IEvent" />
    public class Event : Entity, IEvent
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [Key]
        [DatabaseGenerated
        (DatabaseGeneratedOption.Identity)]
        public override long Id { get; set;}
        /// <summary>
        /// Gets or sets the event identifier.
        /// </summary>
        /// <value>The event identifier.</value>
        public virtual long     EventId 
        { 
            get => base.Id; 
            set => base.Id = value; 
        }
        /// <summary>
        /// Gets or sets the event version.
        /// </summary>
        /// <value>The event version.</value>
        public virtual uint     EventVersion { get; set; }
        /// <summary>
        /// Gets or sets the type of the event.
        /// </summary>
        /// <value>The type of the event.</value>
        public virtual string   EventType { get; set; }
        /// <summary>
        /// Gets or sets the event CLS identifier.
        /// </summary>
        /// <value>The event CLS identifier.</value>
        public virtual Guid     EventClsId { get; set; }
        /// <summary>
        /// Gets or sets the aggregate identifier.
        /// </summary>
        /// <value>The aggregate identifier.</value>
        public virtual long     AggregateId { get; set; }
        /// <summary>
        /// Gets or sets the type of the aggregate.
        /// </summary>
        /// <value>The type of the aggregate.</value>
        public virtual string   AggregateType { get; set; }
        /// <summary>
        /// Gets or sets the aggregate CLS identifier.
        /// </summary>
        /// <value>The aggregate CLS identifier.</value>
        public virtual Guid     AggregateClsId { get; set; }
        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>The data.</value>
        public virtual string   Data { get; set; }
        /// <summary>
        /// Gets or sets the publish time.
        /// </summary>
        /// <value>The publish time.</value>
        public virtual DateTime PublishTime { get; set; }
        /// <summary>
        /// Gets or sets the publish status.
        /// </summary>
        /// <value>The publish status.</value>
        public virtual PublishStatus PublishStatus { get; set; }

        /// <summary>
        /// Gets or sets the unique key.
        /// </summary>
        /// <value>The unique key.</value>
        public override ulong UniqueKey { get => (ulong)Id; set => Id=(long)value; }
    }

}