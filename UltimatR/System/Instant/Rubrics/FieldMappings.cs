
// <copyright file="FieldMappings.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The Instant namespace.
/// </summary>
namespace System.Instant
{
    using System.Series;
    using System.Uniques;




    /// <summary>
    /// Class FieldMapping.
    /// </summary>
    [Serializable]
    public class FieldMapping
    {
        #region Constructors





        /// <summary>
        /// Initializes a new instance of the <see cref="FieldMapping" /> class.
        /// </summary>
        /// <param name="dbDeckName">Name of the database deck.</param>
        public FieldMapping(string dbDeckName) : this(dbDeckName, new Deck<int>(), new Deck<int>())
        {
        }






        /// <summary>
        /// Initializes a new instance of the <see cref="FieldMapping" /> class.
        /// </summary>
        /// <param name="dbDeckName">Name of the database deck.</param>
        /// <param name="keyOrdinal">The key ordinal.</param>
        /// <param name="columnOrdinal">The column ordinal.</param>
        public FieldMapping(string dbDeckName, IDeck<int> keyOrdinal, IDeck<int> columnOrdinal)
        {
            KeyOrdinal = keyOrdinal;
            ColumnOrdinal = columnOrdinal;
            DbTableName = dbDeckName;
        }

        #endregion

        #region Properties




        /// <summary>
        /// Gets or sets the column ordinal.
        /// </summary>
        /// <value>The column ordinal.</value>
        public IDeck<int> ColumnOrdinal { get; set; }




        /// <summary>
        /// Gets or sets the name of the database table.
        /// </summary>
        /// <value>The name of the database table.</value>
        public string DbTableName { get; set; }




        /// <summary>
        /// Gets or sets the key ordinal.
        /// </summary>
        /// <value>The key ordinal.</value>
        public IDeck<int> KeyOrdinal { get; set; }

        #endregion
    }




    /// <summary>
    /// Class FieldMappings.
    /// Implements the <see cref="System.Series.AlbumBase{System.Instant.FieldMapping}" />
    /// </summary>
    /// <seealso cref="System.Series.AlbumBase{System.Instant.FieldMapping}" />
    [Serializable]
    public class FieldMappings : AlbumBase<FieldMapping>
    {
        #region Methods






        /// <summary>
        /// Empties the deck.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <returns>ICard&lt;FieldMapping&gt;[].</returns>
        public override ICard<FieldMapping>[] EmptyDeck(int size)
        {
            return new Card<FieldMapping>[size];
        }





        /// <summary>
        /// Empties the card.
        /// </summary>
        /// <returns>ICard&lt;FieldMapping&gt;.</returns>
        public override ICard<FieldMapping> EmptyCard()
        {
            return new Card<FieldMapping>();
        }






        /// <summary>
        /// Empties the card table.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <returns>ICard&lt;FieldMapping&gt;[].</returns>
        public override ICard<FieldMapping>[] EmptyCardTable(int size)
        {
            return new Card<FieldMapping>[size];
        }






        /// <summary>
        /// Creates new card.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;FieldMapping&gt;.</returns>
        public override ICard<FieldMapping> NewCard(FieldMapping value)
        {
            return new Card<FieldMapping>(value.DbTableName.UniqueKey(), value);
        }






        /// <summary>
        /// Creates new card.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;FieldMapping&gt;.</returns>
        public override ICard<FieldMapping> NewCard(ICard<FieldMapping> value)
        {
            return new Card<FieldMapping>(value);
        }







        /// <summary>
        /// Creates new card.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;FieldMapping&gt;.</returns>
        public override ICard<FieldMapping> NewCard(object key, FieldMapping value)
        {
            return new Card<FieldMapping>(key, value);
        }







        /// <summary>
        /// Creates new card.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;FieldMapping&gt;.</returns>
        public override ICard<FieldMapping> NewCard(ulong key, FieldMapping value)
        {
            return new Card<FieldMapping>(key, value);
        }

        #endregion
    }
}
