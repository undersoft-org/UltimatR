
// <copyright file="IFindable.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The Series namespace.
/// </summary>
namespace System.Series
{
    /// <summary>
    /// Interface IFindable
    /// Implements the <see cref="System.Series.IFindable" />
    /// </summary>
    /// <typeparam name="V"></typeparam>
    /// <seealso cref="System.Series.IFindable" />
    public interface IFindable<V> : IFindable
    {
        /// <summary>
        /// Gets or sets the <see cref="V" /> with the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>V.</returns>
        new V this[object key] { get; set; }
    }

    /// <summary>
    /// Interface IFindable
    /// Implements the <see cref="System.Series.IFindable" />
    /// </summary>
    /// <seealso cref="System.Series.IFindable" />
    public interface IFindable
    {
        /// <summary>
        /// Gets or sets the <see cref="System.Object" /> with the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>System.Object.</returns>
        object this[object key] { get; set; }
    }
}