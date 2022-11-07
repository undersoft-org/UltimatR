/// <summary>
/// The Series namespace.
/// </summary>
namespace System.Series
{
    using System.Collections.Generic;
    using System.Uniques;

    /// <summary>
    /// Class Album32.
    /// Implements the <see cref="System.Series.AlbumBase{V}" />
    /// </summary>
    /// <typeparam name="V"></typeparam>
    /// <seealso cref="System.Series.AlbumBase{V}" />
    public class Album32<V> : AlbumBase<V>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Album32{V}" /> class.
        /// </summary>
        public Album32() : base(17, HashBits.bit32)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Album32{V}" /> class.
        /// </summary>
        /// <param name="collections">The collections.</param>
        /// <param name="_deckSize">Size of the deck.</param>
        /// <param name="repeatable">if set to <c>true</c> [repeatable].</param>
        public Album32(IEnumerable<IUnique<V>> collections, int _deckSize = 17, bool repeatable = false) : base(collections, _deckSize, repeatable, HashBits.bit32)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Album32{V}" /> class.
        /// </summary>
        /// <param name="collections">The collections.</param>
        /// <param name="_deckSize">Size of the deck.</param>
        /// <param name="repeatable">if set to <c>true</c> [repeatable].</param>
        public Album32(IEnumerable<V> collections, int _deckSize = 17, bool repeatable = false) : base(collections, _deckSize, repeatable, HashBits.bit32)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Album32{V}" /> class.
        /// </summary>
        /// <param name="repeatable">if set to <c>true</c> [repeatable].</param>
        /// <param name="_deckSize">Size of the deck.</param>
        public Album32(bool repeatable = false, int _deckSize = 17) : base(repeatable, _deckSize, HashBits.bit32)
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
