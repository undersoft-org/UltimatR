
// <copyright file="IMassDeck.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>




/// <summary>
/// The Series namespace.
/// </summary>
namespace System.Series
{
    using System.Collections.Generic;

    /// <summary>
    /// Interface IMassDeck
    /// Implements the <see cref="System.Series.IDeck{V}" />
    /// </summary>
    /// <typeparam name="V"></typeparam>
    /// <seealso cref="System.Series.IDeck{V}" />
    public interface IMassDeck<V> : IDeck<V> where V : IUnique
    {
        /// <summary>
        /// Gets or sets the <see cref="V" /> with the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>V.</returns>
        V this[object key, ulong seed] { get; set; }
        /// <summary>
        /// Gets or sets the <see cref="V" /> with the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>V.</returns>
        V this[IUnique key, ulong seed] { get; set; }

        /// <summary>
        /// Determines whether the specified key contains key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="seed">The seed.</param>
        /// <returns><c>true</c> if the specified key contains key; otherwise, <c>false</c>.</returns>
        bool ContainsKey(object key, ulong seed);

        /// <summary>
        /// Determines whether this instance contains the object.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="seed">The seed.</param>
        /// <returns><c>true</c> if [contains] [the specified item]; otherwise, <c>false</c>.</returns>
        bool Contains(V item, ulong seed);

        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>V.</returns>
        V Get(object key, ulong seed);

        /// <summary>
        /// Tries the get.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="seed">The seed.</param>
        /// <param name="output">The output.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool TryGet(object key, ulong seed, out ICard<V> output);
        /// <summary>
        /// Tries the get.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="seed">The seed.</param>
        /// <param name="output">The output.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool TryGet(object key, ulong seed, out V output);

        /// <summary>
        /// Gets the card.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        ICard<V> GetCard(object key, ulong seed);

        /// <summary>
        /// News the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        ICard<V> New(object key, ulong seed);

        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="seed">The seed.</param>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool Add(object key, ulong seed, V value);
        /// <summary>
        /// Adds the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="seed">The seed.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool Add(V value, ulong seed);
        /// <summary>
        /// Adds the specified cards.
        /// </summary>
        /// <param name="cards">The cards.</param>
        /// <param name="seed">The seed.</param>
        void Add(IList<V> cards, ulong seed);
        /// <summary>
        /// Adds the specified cards.
        /// </summary>
        /// <param name="cards">The cards.</param>
        /// <param name="seed">The seed.</param>
        void Add(IEnumerable<V> cards, ulong seed);

        /// <summary>
        /// Enqueues the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="seed">The seed.</param>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool Enqueue(object key, ulong seed, V value);
        /// <summary>
        /// Enqueues the specified card.
        /// </summary>
        /// <param name="card">The card.</param>
        /// <param name="seed">The seed.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool Enqueue(V card, ulong seed);

        /// <summary>
        /// Puts the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="seed">The seed.</param>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        ICard<V> Put(object key, ulong seed, V value);
        /// <summary>
        /// Puts the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="seed">The seed.</param>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        ICard<V> Put(object key, ulong seed, object value);
        /// <summary>
        /// Puts the specified cards.
        /// </summary>
        /// <param name="cards">The cards.</param>
        /// <param name="seed">The seed.</param>
        void Put(IList<V> cards, ulong seed);
        /// <summary>
        /// Puts the specified cards.
        /// </summary>
        /// <param name="cards">The cards.</param>
        /// <param name="seed">The seed.</param>
        void Put(IEnumerable<V> cards, ulong seed);
        /// <summary>
        /// Puts the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        ICard<V> Put(V value, ulong seed);

        /// <summary>
        /// Removes the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>V.</returns>
        V Remove(object key, ulong seed);
        /// <summary>
        /// Tries the remove.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="seed">The seed.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool TryRemove(object key, ulong seed);

        /// <summary>
        /// Creates new card.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        ICard<V> NewCard(V value, ulong seed);
        /// <summary>
        /// Creates new card.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="seed">The seed.</param>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        ICard<V> NewCard(object key, ulong seed, V value);
        /// <summary>
        /// Creates new card.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="seed">The seed.</param>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        ICard<V> NewCard(ulong key, ulong seed, V value);
    }
}
