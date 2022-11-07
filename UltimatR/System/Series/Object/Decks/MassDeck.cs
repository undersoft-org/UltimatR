
// <copyright file="MassDeck.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>


/// <summary>
/// The Series namespace.
/// </summary>
namespace System.Series
{
    using System.Collections.Generic;

    /// <summary>
    /// Class MassDeck.
    /// Implements the <see cref="System.Series.MassDeckBase{V}" />
    /// </summary>
    /// <typeparam name="V"></typeparam>
    /// <seealso cref="System.Series.MassDeckBase{V}" />
    public class MassDeck<V> : MassDeckBase<V> where V : IUnique
    {
        #region Constructors






        /// <summary>
        /// Initializes a new instance of the <see cref="MassDeck{V}" /> class.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="capacity">The capacity.</param>
        public MassDeck(IEnumerable<ICard<V>> collection, int capacity = 9) : base(collection, capacity)
        {
        }





        /// <summary>
        /// Initializes a new instance of the <see cref="MassDeck{V}" /> class.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="capacity">The capacity.</param>
        public MassDeck(IEnumerable<IUnique<V>> collection, int capacity = 9) : base(collection, capacity)
        {
        }





        /// <summary>
        /// Initializes a new instance of the <see cref="MassDeck{V}" /> class.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="capacity">The capacity.</param>
        public MassDeck(IList<ICard<V>> collection, int capacity = 9) : base(collection, capacity)
        {
        }





        /// <summary>
        /// Initializes a new instance of the <see cref="MassDeck{V}" /> class.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="capacity">The capacity.</param>
        public MassDeck(IList<IUnique<V>> collection, int capacity = 9) : base(collection, capacity)
        {
        }




        /// <summary>
        /// Initializes a new instance of the <see cref="MassDeck{V}" /> class.
        /// </summary>
        /// <param name="capacity">The capacity.</param>
        public MassDeck(int capacity = 9) : base(capacity)
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
            return new MassCard<V>();
        }






        /// <summary>
        /// Empties the card table.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <returns>ICard&lt;V&gt;[].</returns>
        public override ICard<V>[] EmptyCardTable(int size)
        {
            return new MassCard<V>[size];
        }






        /// <summary>
        /// Creates new card.
        /// </summary>
        /// <param name="card">The card.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        public override ICard<V> NewCard(ICard<V> card)
        {
            return new MassCard<V>(card);
        }







        /// <summary>
        /// Creates new card.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        public override ICard<V> NewCard(object key, V value)
        {
            return new MassCard<V>(key, value);
        }







        /// <summary>
        /// Creates new card.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        public override ICard<V> NewCard(ulong key, V value)
        {
            return new MassCard<V>(key, value);
        }






        /// <summary>
        /// Creates new card.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        public override ICard<V> NewCard(V value)
        {
            return new MassCard<V>(value);
        }

        #endregion
    }
}
