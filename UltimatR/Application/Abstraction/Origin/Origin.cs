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
// Last Modified On : 01-20-2022
// ***********************************************************************
// <copyright file="Origin.cs" company="UltimatR.Framework">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;

/// <summary>
/// The UltimatR namespace.
/// </summary>
namespace UltimatR
{
    /// <summary>
    /// Class Origin.
    /// Implements the <see cref="UltimatR.IOrigin" />
    /// </summary>
    /// <seealso cref="UltimatR.IOrigin" />
    [StructLayout(LayoutKind.Sequential, Pack = 2, CharSet = CharSet.Ansi)]
    public abstract class Origin : IOrigin 
    {
        /// <summary>
        /// Gets or sets the origin identifier.
        /// </summary>
        /// <value>The origin identifier.</value>
        [JsonIgnore]      
        public virtual short OriginId { get; set; }

        /// <summary>
        /// Gets or sets the name of the origin.
        /// </summary>
        /// <value>The name of the origin.</value>
        [JsonIgnore]
        [StringLength(32)]
        public virtual string OriginName { get; set; }

        /// <summary>
        /// Gets or sets the modification time.
        /// </summary>
        /// <value>The modification time.</value>
        [Column(TypeName = "timestamp")]
        public virtual DateTime ModificationTime { get; set; } = DateTime.Now;

        /// <summary>
        /// Gets or sets the modifier.
        /// </summary>
        /// <value>The modifier.</value>
        [StringLength(32)]
        public virtual string Modifier { get; set; }

        /// <summary>
        /// Gets or sets the creation time.
        /// </summary>
        /// <value>The creation time.</value>
        [Column(TypeName = "timestamp")]
        public virtual DateTime CreationTime { get; set; }

        /// <summary>
        /// Gets or sets the creator.
        /// </summary>
        /// <value>The creator.</value>
        [StringLength(32)]
        public virtual string Creator { get; set; }
    }
}
