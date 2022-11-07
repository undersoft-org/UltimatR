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
// Last Modified On : 01-14-2022
// ***********************************************************************
// <copyright file="Event.cs" company="UltimatR.Framework">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>

using MediatR;
using System;
using System.Text.Json;
using System.Uniques;

/// <summary>
/// The UltimatR namespace.
/// </summary>
namespace UltimatR
{
    /// <summary>
    /// Class Event.
    /// Implements the <see cref="UltimatR.Event" />
    /// Implements the <see cref="MediatR.INotification" />
    /// </summary>
    /// <typeparam name="TCommand">The type of the t command.</typeparam>
    /// <seealso cref="UltimatR.Event" />
    /// <seealso cref="MediatR.INotification" />
    public abstract class Event<TCommand> : Event, INotification where TCommand : Command
    {
        /// <summary>
        /// Gets the command.
        /// </summary>
        /// <value>The command.</value>
        public TCommand Command { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Event{TCommand}"/> class.
        /// </summary>
        /// <param name="command">The command.</param>
        protected Event(TCommand command)
        {
            var aggregateName = command.Entity.ProxyRetypeName();
            var eventType = GetType();

            Command = command;            
            Id = (long)Unique.New;
            AggregateId = command.Id;
            AggregateType = aggregateName;       
            EventType = eventType.FullName;           
            var entity = (Entity)command.Entity;
            OriginId = entity.OriginId;
            OriginName = entity.OriginName;
            Modifier = entity.Modifier;
            ModificationTime = entity.ModificationTime;
            Creator = entity.Creator;
            CreationTime = entity.CreationTime;
            PublishStatus = PublishStatus.Ready;
            PublishTime = DateTime.Now;          

            Data = JsonSerializer.Serialize((Command)command);
        }
    }
}