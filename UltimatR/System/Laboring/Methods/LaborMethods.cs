/// <summary>
/// The Laboring namespace.
/// </summary>
namespace System.Laboring
{
    using System.Series;




    /// <summary>
    /// Class LaborMethods.
    /// Implements the <see cref="System.Series.Catalog{System.IDeputy}" />
    /// </summary>
    /// <seealso cref="System.Series.Catalog{System.IDeputy}" />
    public class LaborMethods : Catalog<IDeputy>
    {
        #region Methods






        /// <summary>
        /// Empties the deck.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <returns>ICard&lt;IDeputy&gt;[].</returns>
        public override ICard<IDeputy>[] EmptyDeck(int size)
        {
            return new LaborMethod[size];
        }





        /// <summary>
        /// Empties the card.
        /// </summary>
        /// <returns>ICard&lt;IDeputy&gt;.</returns>
        public override ICard<IDeputy> EmptyCard()
        {
            return new LaborMethod();
        }






        /// <summary>
        /// Empties the card table.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <returns>ICard&lt;IDeputy&gt;[].</returns>
        public override ICard<IDeputy>[] EmptyCardTable(int size)
        {
            return new LaborMethod[size];
        }






        /// <summary>
        /// Creates new card.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;IDeputy&gt;.</returns>
        public override ICard<IDeputy> NewCard(IDeputy value)
        {
            return new LaborMethod(value);
        }







        /// <summary>
        /// Creates new card.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;IDeputy&gt;.</returns>
        public override ICard<IDeputy> NewCard(object key, IDeputy value)
        {
            return new LaborMethod(key, value);
        }







        /// <summary>
        /// Creates new card.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;IDeputy&gt;.</returns>
        public override ICard<IDeputy> NewCard(ulong key, IDeputy value)
        {
            return new LaborMethod(key, value);
        }

        #endregion
    }
}
