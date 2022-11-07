
// <copyright file="Deck.cs" company="UltimatR.Core">
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
    /// Class Deck.
    /// Implements the <see cref="System.Series.DeckBase{V}" />
    /// </summary>
    /// <typeparam name="V"></typeparam>
    /// <seealso cref="System.Series.DeckBase{V}" />
    public class Deck<V> : DeckBase<V>
    {
        #region Constructors




        /// <summary>
        /// Initializes a new instance of the <see cref="Deck{V}" /> class.
        /// </summary>
        public Deck() : base(17, HashBits.bit64)
        {
        }





        /// <summary>
        /// Initializes a new instance of the <see cref="Deck{V}" /> class.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="capacity">The capacity.</param>
        public Deck(IEnumerable<ICard<V>> collection, int capacity = 9) : base(collection, capacity)
        {
        }






        /// <summary>
        /// Initializes a new instance of the <see cref="Deck{V}" /> class.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="capacity">The capacity.</param>
        /// <param name="bits">The bits.</param>
        public Deck(IEnumerable<IUnique<V>> collection, int capacity = 17, HashBits bits = HashBits.bit64) : this(capacity, bits)
        {
            foreach (var c in collection)
                this.Add(c);
        }





        /// <summary>
        /// Initializes a new instance of the <see cref="Deck{V}" /> class.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="capacity">The capacity.</param>
        public Deck(IEnumerable<IUnique<V>> collection, int capacity = 9) : base(collection, capacity)
        {
        }






        /// <summary>
        /// Initializes a new instance of the <see cref="Deck{V}" /> class.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="capacity">The capacity.</param>
        /// <param name="bits">The bits.</param>
        public Deck(IEnumerable<V> collection, int capacity = 17, HashBits bits = HashBits.bit64) : this(capacity, bits)
        {
            foreach (var c in collection)
                this.Add(c);
        }





        /// <summary>
        /// Initializes a new instance of the <see cref="Deck{V}" /> class.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="capacity">The capacity.</param>
        public Deck(IList<ICard<V>> collection, int capacity = 9) : base(collection, capacity)
        {
        }






        /// <summary>
        /// Initializes a new instance of the <see cref="Deck{V}" /> class.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="capacity">The capacity.</param>
        /// <param name="bits">The bits.</param>
        public Deck(IList<IUnique<V>> collection, int capacity = 17, HashBits bits = HashBits.bit64) : this(capacity > collection.Count ? capacity : collection.Count, bits)
        {
            foreach (var c in collection)
                this.Add(c);
        }





        /// <summary>
        /// Initializes a new instance of the <see cref="Deck{V}" /> class.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="capacity">The capacity.</param>
        public Deck(IList<IUnique<V>> collection, int capacity = 9) : base(collection, capacity)
        {
        }






        /// <summary>
        /// Initializes a new instance of the <see cref="Deck{V}" /> class.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="capacity">The capacity.</param>
        /// <param name="bits">The bits.</param>
        public Deck(IList<V> collection, int capacity = 17, HashBits bits = HashBits.bit64) : this(capacity > collection.Count ? capacity : collection.Count, bits)
        {
            foreach (var c in collection)
                this.Add(c);
        }





        /// <summary>
        /// Initializes a new instance of the <see cref="Deck{V}" /> class.
        /// </summary>
        /// <param name="capacity">The capacity.</param>
        /// <param name="bits">The bits.</param>
        public Deck(int capacity = 17, HashBits bits = HashBits.bit64) : base(capacity, bits)
        {
        }




        /// <summary>
        /// Initializes a new instance of the <see cref="Deck{V}" /> class.
        /// </summary>
        /// <param name="capacity">The capacity.</param>
        public Deck(int capacity = 9) : base(capacity)
        {
        }

        #endregion

        #region Methods





        /// <summary>
        /// Empties the card.
        /// </summary>
        /// <returns>ICard&lt;V&gt;.</returns>
        public override ICard<V> EmptyCard()
        {
            return new Card<V>();
        }






        /// <summary>
        /// Empties the card table.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <returns>ICard&lt;V&gt;[].</returns>
        public override ICard<V>[] EmptyCardTable(int size)
        {
            return new Card<V>[size];
        }






        /// <summary>
        /// Creates new card.
        /// </summary>
        /// <param name="card">The card.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        public override ICard<V> NewCard(ICard<V> card)
        {
            return new Card<V>(card);
        }







        /// <summary>
        /// Creates new card.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        public override ICard<V> NewCard(object key, V value)
        {
            return new Card<V>(key, value);
        }







        /// <summary>
        /// Creates new card.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        public override ICard<V> NewCard(ulong key, V value)
        {
            return new Card<V>(key, value);
        }






        /// <summary>
        /// Creates new card.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        public override ICard<V> NewCard(V value)
        {
            return new Card<V>(value);
        }

        #endregion
    }
}
