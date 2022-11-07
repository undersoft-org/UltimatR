
// <copyright file="ISleeves.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The Instant namespace.
/// </summary>
namespace System.Instant
{



    /// <summary>
    /// Interface ISleeves
    /// Implements the <see cref="System.Instant.IFigures" />
    /// </summary>
    /// <seealso cref="System.Instant.IFigures" />
    public interface ISleeves : IFigures
    {
        #region Properties




        /// <summary>
        /// Gets or sets the figures.
        /// </summary>
        /// <value>The figures.</value>
        IFigures Figures { get; set; }




        /// <summary>
        /// Gets or sets the instant.
        /// </summary>
        /// <value>The instant.</value>
        new IInstant Instant { get; set; }




        /// <summary>
        /// Gets or sets the sleeves.
        /// </summary>
        /// <value>The sleeves.</value>
        IFigures Sleeves { get; set; }

        #endregion
    }
}
