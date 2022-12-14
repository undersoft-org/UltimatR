
// <copyright file="Catalog32.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>


/// <summary>
/// The Series namespace.
/// </summary>
namespace System.Series
{
    using System.Collections.Generic;
    using System.Uniques;

    /// <summary>
    /// Class Catalog32.
    /// Implements the <see cref="System.Series.CatalogBase{V}" />
    /// </summary>
    /// <typeparam name="V"></typeparam>
    /// <seealso cref="System.Series.CatalogBase{V}" />
    public class Catalog32<V> : CatalogBase<V>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Catalog32{V}" /> class.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="capacity">The capacity.</param>
        /// <param name="repeatable">if set to <c>true</c> [repeatable].</param>
        public Catalog32(IEnumerable<IUnique<V>> collection, int capacity = 17, bool repeatable = false) : this(repeatable, capacity)
        {
            foreach (var c in collection)
                this.Add(c);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Catalog32{V}" /> class.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="capacity">The capacity.</param>
        /// <param name="repeatable">if set to <c>true</c> [repeatable].</param>
        public Catalog32(IEnumerable<V> collection, int capacity = 17, bool repeatable = false) : this(repeatable, capacity)
        {
            foreach (var c in collection)
                this.Add(c);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Catalog32{V}" /> class.
        /// </summary>
        /// <param name="repeatable">if set to <c>true</c> [repeatable].</param>
        /// <param name="capacity">The capacity.</param>
        public Catalog32(bool repeatable = false, int capacity = 17) : base(repeatable, capacity, HashBits.bit32)
        {
        }

        #endregion

        #region Methods






        /// <summary>
        /// Empties the deck.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <returns>ICard&lt;V&gt;[].</returns>
        public override ICard<V>[] EmptyDeck(int size)
        {
            return new Card32<V>[size];
        }





        /// <summary>
        /// Empties the card.
        /// </summary>
        /// <returns>ICard&lt;V&gt;.</returns>
        public override ICard<V> EmptyCard()
        {
            return new Card32<V>();
        }






        /// <summary>
        /// Empties the card table.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <returns>ICard&lt;V&gt;[].</returns>
        public override ICard<V>[] EmptyCardTable(int size)
        {
            return new Card32<V>[size];
        }






        /// <summary>
        /// Creates new card.
        /// </summary>
        /// <param name="card">The card.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        public override ICard<V> NewCard(ICard<V> card)
        {
            return new Card32<V>(card);
        }







        /// <summary>
        /// Creates new card.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        public override ICard<V> NewCard(object key, V value)
        {
            return new Card32<V>(key, value);
        }







        /// <summary>
        /// Creates new card.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        public override ICard<V> NewCard(ulong key, V value)
        {
            return new Card32<V>(key, value);
        }






        /// <summary>
        /// Creates new card.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        public override ICard<V> NewCard(V value)
        {
            return new Card32<V>(value);
        }

        #endregion
    }
}
