
// <copyright file="ISpectrum.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>




/// <summary>
/// The Series namespace.
/// </summary>
namespace System.Series
{
    using System.Collections.Generic;





    /// <summary>
    /// Interface ISpectrum
    /// Implements the <see cref="System.Collections.Generic.IEnumerable{System.Series.CardBase{V}}" />
    /// </summary>
    /// <typeparam name="V"></typeparam>
    /// <seealso cref="System.Collections.Generic.IEnumerable{System.Series.CardBase{V}}" />
    public interface ISpectrum<V> : IEnumerable<CardBase<V>>
    {
        #region Properties




        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>The count.</value>
        int Count { get; }




        /// <summary>
        /// Gets the size.
        /// </summary>
        /// <value>The size.</value>
        int Size { get; }

        #endregion

        #region Methods







        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool Add(int key, V value);






        /// <summary>
        /// Determines whether this instance contains the object.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns><c>true</c> if [contains] [the specified key]; otherwise, <c>false</c>.</returns>
        bool Contains(int key);






        /// <summary>
        /// Nexts the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>System.Int32.</returns>
        int Next(int key);






        /// <summary>
        /// Previouses the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>System.Int32.</returns>
        int Previous(int key);






        /// <summary>
        /// Removes the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool Remove(int key);







        /// <summary>
        /// Sets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool Set(int key, V value);

        #endregion
    }
}
