
// <copyright file="ICard.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



using System.Collections.Generic;

/// <summary>
/// The Series namespace.
/// </summary>
namespace System.Series
{
    #region Interfaces

    /// <summary>
    /// Interface ICard
    /// Implements the <see cref="System.IComparable{System.UInt64}" />
    /// Implements the <see cref="System.IComparable{System.Series.ICard{V}}" />
    /// Implements the <see cref="System.IEquatable{System.Series.ICard{V}}" />
    /// Implements the <see cref="System.IEquatable{System.Object}" />
    /// Implements the <see cref="System.IEquatable{System.UInt64}" />
    /// Implements the <see cref="System.IComparable{System.Object}" />
    /// Implements the <see cref="System.IUnique{V}" />
    /// Implements the <see cref="System.IDisposable" />
    /// Implements the <see cref="System.Collections.Generic.IEnumerable{V}" />
    /// </summary>
    /// <typeparam name="V"></typeparam>
    /// <seealso cref="System.IComparable{System.UInt64}" />
    /// <seealso cref="System.IComparable{System.Series.ICard{V}}" />
    /// <seealso cref="System.IEquatable{System.Series.ICard{V}}" />
    /// <seealso cref="System.IEquatable{System.Object}" />
    /// <seealso cref="System.IEquatable{System.UInt64}" />
    /// <seealso cref="System.IComparable{System.Object}" />
    /// <seealso cref="System.IUnique{V}" />
    /// <seealso cref="System.IDisposable" />
    /// <seealso cref="System.Collections.Generic.IEnumerable{V}" />
    public interface ICard<V> : IComparable<ulong>, IComparable<ICard<V>>, IEquatable<ICard<V>>, IEquatable<object>, 
                                IEquatable<ulong>, IComparable<object>, IUnique<V>, IDisposable, IEnumerable<V>
    {
        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ICard{V}" /> is repeated.
        /// </summary>
        /// <value><c>true</c> if repeated; otherwise, <c>false</c>.</value>
        bool Repeated { get; set; }

        /// <summary>
        /// Gets or sets the extended.
        /// </summary>
        /// <value>The extended.</value>
        ICard<V> Extended { get; set; }

        /// <summary>
        /// Gets or sets the index.
        /// </summary>
        /// <value>The index.</value>
        int Index { get; set; }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        ulong Key { get; set; }

        /// <summary>
        /// Gets or sets the next.
        /// </summary>
        /// <value>The next.</value>
        ICard<V> Next { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ICard{V}" /> is removed.
        /// </summary>
        /// <value><c>true</c> if removed; otherwise, <c>false</c>.</value>
        bool Removed { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        V Value { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        int GetHashCode();

        /// <summary>
        /// Gets the type of the unique.
        /// </summary>
        /// <returns>Type.</returns>
        Type GetUniqueType();

        /// <summary>
        /// Sets the specified card.
        /// </summary>
        /// <param name="card">The card.</param>
        void Set(ICard<V> card);

        /// <summary>
        /// Sets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        void Set(object key, V value);

        /// <summary>
        /// Sets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        void Set(ulong key, V value);

        /// <summary>
        /// Sets the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        void Set(V value);

        /// <summary>
        /// Moves the next.
        /// </summary>
        /// <param name="card">The card.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        ICard<V> MoveNext(ICard<V> card);

        /// <summary>
        /// Ases the cards.
        /// </summary>
        /// <returns>IEnumerable&lt;ICard&lt;V&gt;&gt;.</returns>
        IEnumerable<ICard<V>> AsCards();

        /// <summary>
        /// Ases the values.
        /// </summary>
        /// <returns>IEnumerable&lt;V&gt;.</returns>
        IEnumerable<V> AsValues();

        #endregion
    }

    #endregion
}
