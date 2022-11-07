
// <copyright file="FigureAlbum.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The Instant namespace.
/// </summary>
namespace System.Instant
{
    using System.Instant.Linking;
    using System.Instant.Treatments;
    using System.IO;
    using System.Linq;
    using System.Series;
    using System.Uniques;


    /// <summary>
    /// Class FigureAlbum.
    /// Implements the <see cref="System.Series.AlbumBase{System.Instant.IFigure}" />
    /// Implements the <see cref="System.Instant.IFigures" />
    /// </summary>
    /// <seealso cref="System.Series.AlbumBase{System.Instant.IFigure}" />
    /// <seealso cref="System.Instant.IFigures" />
    public abstract class FigureAlbum : AlbumBase<IFigure>, IFigures
    {
        /// <summary>
        /// Gets or sets the instant.
        /// </summary>
        /// <value>The instant.</value>
        public IInstant Instant { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="IFigures" /> is prime.
        /// </summary>
        /// <value><c>true</c> if prime; otherwise, <c>false</c>.</value>
        public abstract bool Prime { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="System.Object" /> at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>System.Object.</returns>
        public abstract object this[int index, string propertyName] { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="System.Object" /> at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="fieldId">The field identifier.</param>
        /// <returns>System.Object.</returns>
        public abstract object this[int index, int fieldId] { get; set; }

        /// <summary>
        /// Gets or sets the rubrics.
        /// </summary>
        /// <value>The rubrics.</value>
        public abstract IRubrics Rubrics { get; set; }

        /// <summary>
        /// Gets or sets the key rubrics.
        /// </summary>
        /// <value>The key rubrics.</value>
        public abstract IRubrics KeyRubrics { get; set; }

        /// <summary>
        /// Creates new figure.
        /// </summary>
        /// <returns>IFigure.</returns>
        public abstract IFigure NewFigure();

        /// <summary>
        /// Creates new sleeve.
        /// </summary>
        /// <returns>ISleeve.</returns>
        public abstract ISleeve NewSleeve();

        /// <summary>
        /// Gets or sets the type of the figure.
        /// </summary>
        /// <value>The type of the figure.</value>
        public abstract Type FigureType { get; set; }

        /// <summary>
        /// Gets or sets the size of the figure.
        /// </summary>
        /// <value>The size of the figure.</value>
        public abstract int FigureSize { get; set; }

        /// <summary>
        /// Gets or sets the serial code.
        /// </summary>
        /// <value>The serial code.</value>
        public abstract Ussn SerialCode { get; set; }

        /// <summary>
        /// Empties the card.
        /// </summary>
        /// <returns>ICard&lt;IFigure&gt;.</returns>
        public override ICard<IFigure> EmptyCard()
        {
            return new FigureCard(this);
        }

        /// <summary>
        /// Creates new card.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;IFigure&gt;.</returns>
        public override ICard<IFigure> NewCard(ulong key, IFigure value)
        {
            return new FigureCard(key, value, this);
        }
        /// <summary>
        /// Creates new card.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;IFigure&gt;.</returns>
        public override ICard<IFigure> NewCard(object key, IFigure value)
        {
            return new FigureCard(key, value, this);
        }
        /// <summary>
        /// Creates new card.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;IFigure&gt;.</returns>
        public override ICard<IFigure> NewCard(IFigure value)
        {
            return new FigureCard(value, this);
        }
        /// <summary>
        /// Creates new card.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;IFigure&gt;.</returns>
        public override ICard<IFigure> NewCard(ICard<IFigure> value)
        {
            return new FigureCard(value, this);
        }

        /// <summary>
        /// Empties the card table.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <returns>ICard&lt;IFigure&gt;[].</returns>
        public override ICard<IFigure>[] EmptyCardTable(int size)
        {
            return new FigureCard[size];
        }

        /// <summary>
        /// Empties the deck.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <returns>ICard&lt;IFigure&gt;[].</returns>
        public override ICard<IFigure>[] EmptyDeck(int size)
        {
            return new FigureCard[size];
        }

        /// <summary>
        /// Inners the add.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        protected override bool InnerAdd(IFigure value)
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

        /// <summary>
        /// News this instance.
        /// </summary>
        /// <returns>ICard&lt;IFigure&gt;.</returns>
        public override ICard<IFigure> New()
        {
            ICard<IFigure> newCard = NewCard(Unique.New, NewFigure());
            if (InnerAdd(newCard))
                return newCard;
            return null;
        }
        /// <summary>
        /// News the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>ICard&lt;IFigure&gt;.</returns>
        public override ICard<IFigure> New(ulong key)
        {
            ICard<IFigure> newCard = NewCard(key, NewFigure());
            if (InnerAdd(newCard))
                return newCard;
            return null;
        }
        /// <summary>
        /// News the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>ICard&lt;IFigure&gt;.</returns>
        public override ICard<IFigure> New(object key)
        {
            ICard<IFigure> newCard = NewCard(unique.Key(key), NewFigure());
            if (InnerAdd(newCard))
                return newCard;
            return null;
        }

        /// <summary>
        /// Gets or sets the value array.
        /// </summary>
        /// <value>The value array.</value>
        public object[] ValueArray { get => ToObjectArray(); set => Put(value); }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public Type Type { get; set; }

        /// <summary>
        /// Gets or sets the view.
        /// </summary>
        /// <value>The view.</value>
        public IQueryable<IFigure> View { get; set; }

        /// <summary>
        /// Gets or sets the summary.
        /// </summary>
        /// <value>The summary.</value>
        public IFigure Summary { get; set; }

        /// <summary>
        /// Gets or sets the filter.
        /// </summary>
        /// <value>The filter.</value>
        public FigureFilter Filter { get; set; }

        /// <summary>
        /// Gets or sets the sort.
        /// </summary>
        /// <value>The sort.</value>
        public FigureSort Sort { get; set; }

        /// <summary>
        /// Gets or sets the predicate.
        /// </summary>
        /// <value>The predicate.</value>
        public Func<IFigure, bool> Predicate { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="System.Object" /> with the specified field identifier.
        /// </summary>
        /// <param name="fieldId">The field identifier.</param>
        /// <returns>System.Object.</returns>
        object IFigure.this[int fieldId]
        {
            get => this[fieldId];
            set => this[fieldId] = (IFigure)value;
        }
        /// <summary>
        /// Gets or sets the <see cref="System.Object" /> with the specified property name.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>System.Object.</returns>
        public object this[string propertyName]
        {
            get => this[propertyName];
            set => this[propertyName] = (IFigure)value;
        }

        /// <summary>
        /// Gets or sets the serial count.
        /// </summary>
        /// <value>The serial count.</value>
        public int SerialCount { get; set; }
        /// <summary>
        /// Gets or sets the deserial count.
        /// </summary>
        /// <value>The deserial count.</value>
        public int DeserialCount { get; set; }
        /// <summary>
        /// Gets or sets the progress count.
        /// </summary>
        /// <value>The progress count.</value>
        public int ProgressCount { get; set; }

        /// <summary>
        /// Serializes the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="batchSize">Size of the batch.</param>
        /// <param name="serialFormat">The serial format.</param>
        /// <returns>System.Int32.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public int Serialize(Stream stream, int offset, int batchSize, SerialFormat serialFormat = SerialFormat.Binary)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Serializes the specified buffer.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="batchSize">Size of the batch.</param>
        /// <param name="serialFormat">The serial format.</param>
        /// <returns>System.Int32.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public int Serialize(ISerialBuffer buffer, int offset, int batchSize, SerialFormat serialFormat = SerialFormat.Binary)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Deserializes the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="serialFormat">The serial format.</param>
        /// <returns>System.Object.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public object Deserialize(Stream stream, SerialFormat serialFormat = SerialFormat.Binary)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Deserializes the specified buffer.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="serialFormat">The serial format.</param>
        /// <returns>System.Object.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public object Deserialize(ISerialBuffer buffer, SerialFormat serialFormat = SerialFormat.Binary)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <returns>System.Object[].</returns>
        public object[] GetMessage()
        {
            return new[] { (IFigures)this };
        }
        /// <summary>
        /// Gets the header.
        /// </summary>
        /// <returns>System.Object.</returns>
        public object   GetHeader()
        {
            return this;
        }

        /// <summary>
        /// Gets the items count.
        /// </summary>
        /// <value>The items count.</value>
        /// <exception cref="System.NotImplementedException"></exception>
        public int ItemsCount => throw new NotImplementedException();

        /// <summary>
        /// Gets or sets the linker.
        /// </summary>
        /// <value>The linker.</value>
        public Linker Linker { get; set; } = new Linker();

        /// <summary>
        /// The treatment
        /// </summary>
        private Treatment treatment;
        /// <summary>
        /// Gets or sets the treatment.
        /// </summary>
        /// <value>The treatment.</value>
        public Treatment Treatment
        {
            get => treatment == null ? treatment = new Treatment(this) : treatment;
            set => treatment = value;
        }

        /// <summary>
        /// Gets or sets the computations.
        /// </summary>
        /// <value>The computations.</value>
        public IDeck<IComputation> Computations { get; set; }

    }
}
