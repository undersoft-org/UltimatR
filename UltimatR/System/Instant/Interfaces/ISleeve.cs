
// <copyright file="ISleeve.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



using System.Series;

/// <summary>
/// The Instant namespace.
/// </summary>
namespace System.Instant
{
    /// <summary>
    /// Interface ISleeve
    /// Implements the <see cref="System.Instant.IFigure" />
    /// </summary>
    /// <seealso cref="System.Instant.IFigure" />
    public interface ISleeve : IFigure
    {
        /// <summary>
        /// Gets or sets the rubrics.
        /// </summary>
        /// <value>The rubrics.</value>
        IRubrics Rubrics { get; set; }

        /// <summary>
        /// Gets or sets the devisor.
        /// </summary>
        /// <value>The devisor.</value>
        object Devisor { get; set; }
    }
}
