
// <copyright file="FigureSharedAlbum.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>




/// <summary>
/// The Instant namespace.
/// </summary>
namespace System.Instant
{
    using System.Collections.Generic;
    using System.Series;




    /// <summary>
    /// Class FigureBaseCatalog.
    /// Implements the <see cref="System.Series.CatalogBase{System.Instant.IFigure}" />
    /// </summary>
    /// <seealso cref="System.Series.CatalogBase{System.Instant.IFigure}" />
    public abstract class FigureBaseCatalog : CatalogBase<IFigure>
    {
        #region Constructors




        /// <summary>
        /// Initializes a new instance of the <see cref="FigureBaseCatalog" /> class.
        /// </summary>
        public FigureBaseCatalog() : base()
        {
        }




        /// <summary>
        /// Initializes a new instance of the <see cref="FigureBaseCatalog" /> class.
        /// </summary>
        /// <param name="collections">The collections.</param>
        public FigureBaseCatalog(ICollection<IFigure> collections) : base(collections)
        {
        }




        /// <summary>
        /// Initializes a new instance of the <see cref="FigureBaseCatalog" /> class.
        /// </summary>
        /// <param name="collections">The collections.</param>
        public FigureBaseCatalog(IEnumerable<IFigure> collections) : base(collections)
        {
        }




        /// <summary>
        /// Initializes a new instance of the <see cref="FigureBaseCatalog" /> class.
        /// </summary>
        /// <param name="capacity">The capacity.</param>
        public FigureBaseCatalog(int capacity) : base(capacity)
        {
        }

        #endregion

        #region Methods






        /// <summary>
        /// Empties the deck.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <returns>ICard&lt;IFigure&gt;[].</returns>
        public override ICard<IFigure>[] EmptyDeck(int size)
        {
            return new Card<IFigure>[size];
        }





        /// <summary>
        /// Empties the card.
        /// </summary>
        /// <returns>ICard&lt;IFigure&gt;.</returns>
        public override ICard<IFigure> EmptyCard()
        {
            return new Card<IFigure>();
        }






        /// <summary>
        /// Empties the card table.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <returns>ICard&lt;IFigure&gt;[].</returns>
        public override ICard<IFigure>[] EmptyCardTable(int size)
        {
            return new Card<IFigure>[size];
        }






        /// <summary>
        /// Creates new card.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;IFigure&gt;.</returns>
        public override ICard<IFigure> NewCard(ICard<IFigure> value)
        {
            return new Card<IFigure>(value);
        }






        /// <summary>
        /// Creates new card.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;IFigure&gt;.</returns>
        public override ICard<IFigure> NewCard(IFigure value)
        {
            return new Card<IFigure>(value);
        }







        /// <summary>
        /// Creates new card.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;IFigure&gt;.</returns>
        public override ICard<IFigure> NewCard(object key, IFigure value)
        {
            return new Card<IFigure>(key, value);
        }







        /// <summary>
        /// Creates new card.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;IFigure&gt;.</returns>
        public override ICard<IFigure> NewCard(ulong key, IFigure value)
        {
            return new Card<IFigure>(key, value);
        }






        /// <summary>
        /// Inners the add.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        internal override bool InnerAdd(IFigure value)
        {
            return InnerAdd(NewCard(value));
        }






        /// <summary>
        /// Inners the put.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;IFigure&gt;.</returns>
        protected override ICard<IFigure> InnerPut(IFigure value)
        {
            return InnerPut(NewCard(value));
        }

        #endregion
    }
}
