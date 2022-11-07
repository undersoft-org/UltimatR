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
// Last Modified On : 10-29-2021
// ***********************************************************************
// <copyright file="EventStoreMapping.cs" company="UltimatR.Framework">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
/// The UltimatR namespace.
/// </summary>
namespace UltimatR
{
    /// <summary>
    /// Class EventStoreMapping.
    /// Implements the <see cref="UltimatR.EntityTypeMapping{UltimatR.Event}" />
    /// </summary>
    /// <seealso cref="UltimatR.EntityTypeMapping{UltimatR.Event}" />
    public class EventStoreMapping : EntityTypeMapping<Event>
    {
        /// <summary>
        /// The table name
        /// </summary>
        private const string TABLE_NAME = "EventStore";

        /// <summary>
        /// Configures the specified builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        public override void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.ToTable(TABLE_NAME, "Event");

            builder.Property(p => p.PublishTime)
                .HasColumnType("timestamp");
        }
    }
}