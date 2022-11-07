
// <copyright file="IFigure.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The Instant namespace.
/// </summary>
namespace System.Instant
{
    using Uniques;

    /// <summary>
    /// Interface IFigure
    /// Implements the <see cref="System.IUnique" />
    /// </summary>
    /// <seealso cref="System.IUnique" />
    public interface IFigure : IUnique
    {
        /// <summary>
        /// Gets or sets the <see cref="System.Object" /> with the specified property name.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>System.Object.</returns>
        object this[string propertyName] { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="System.Object" /> with the specified field identifier.
        /// </summary>
        /// <param name="fieldId">The field identifier.</param>
        /// <returns>System.Object.</returns>
        object this[int fieldId] { get; set; }

        /// <summary>
        /// Gets or sets the value array.
        /// </summary>
        /// <value>The value array.</value>
        object[] ValueArray { get; set; }

        /// <summary>
        /// Gets or sets the serial code.
        /// </summary>
        /// <value>The serial code.</value>
        Ussn SerialCode { get; set; }
    }
}
