
// <copyright file="MemberRubrics.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The Instant namespace.
/// </summary>
namespace System.Instant
{
    using System.Collections.Generic;
    using System.Extract;
    using System.Linq;
    using System.Series;
    using System.Uniques;

    /// <summary>
    /// Class MemberRubrics.
    /// Implements the <see cref="System.Series.AlbumBase{System.Instant.MemberRubric}" />
    /// Implements the <see cref="System.Instant.IRubrics" />
    /// </summary>
    /// <seealso cref="System.Series.AlbumBase{System.Instant.MemberRubric}" />
    /// <seealso cref="System.Instant.IRubrics" />
    public partial class MemberRubrics : AlbumBase<MemberRubric>, IRubrics
    {
        #region Fields

        /// <summary>
        /// The binary size
        /// </summary>
        private int binarySize;
        /// <summary>
        /// The binary sizes
        /// </summary>
        private int[] binarySizes;
        /// <summary>
        /// The ordinals
        /// </summary>
        private int[] ordinals;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberRubrics" /> class.
        /// </summary>
        public MemberRubrics()
            : base()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="MemberRubrics" /> class.
        /// </summary>
        /// <param name="collection">The collection.</param>
        public MemberRubrics(IEnumerable<MemberRubric> collection)
            : base(collection)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="MemberRubrics" /> class.
        /// </summary>
        /// <param name="collection">The collection.</param>
        public MemberRubrics(IList<MemberRubric> collection)
            : base(collection)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the size of the binary.
        /// </summary>
        /// <value>The size of the binary.</value>
        public int BinarySize { get => binarySize; }

        /// <summary>
        /// Gets the binary sizes.
        /// </summary>
        /// <value>The binary sizes.</value>
        public int[] BinarySizes { get => binarySizes; }

        /// <summary>
        /// Gets or sets the figures.
        /// </summary>
        /// <value>The figures.</value>
        public IFigures Figures { get; set; }

        /// <summary>
        /// Gets or sets the key rubrics.
        /// </summary>
        /// <value>The key rubrics.</value>
        public IRubrics KeyRubrics { get; set; }

        /// <summary>
        /// Gets or sets the mappings.
        /// </summary>
        /// <value>The mappings.</value>
        public FieldMappings Mappings { get; set; }

        /// <summary>
        /// Gets the ordinals.
        /// </summary>
        /// <value>The ordinals.</value>
        public int[] Ordinals { get => ordinals; }

        /// <summary>
        /// Gets or sets the serial code.
        /// </summary>
        /// <value>The serial code.</value>
        public Ussn SerialCode { get => Figures.SerialCode; set => Figures.SerialCode = value; }

        /// <summary>
        /// Gets or sets the unique key.
        /// </summary>
        /// <value>The unique key.</value>
        public override ulong UniqueKey { get => Figures.UniqueKey; set => Figures.UniqueKey = value; }

        /// <summary>
        /// Gets or sets the unique seed.
        /// </summary>
        /// <value>The unique seed.</value>
        public override ulong UniqueSeed { get => Figures.UniqueSeed; set => Figures.UniqueSeed = value; }

        /// <summary>
        /// Gets or sets the value array.
        /// </summary>
        /// <value>The value array.</value>
        public object[] ValueArray { get => Figures.ValueArray; set => Figures.ValueArray = value; }

        #endregion

        #region Methods

        /// <summary>
        /// Compares to.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>System.Int32.</returns>
        public new int CompareTo(IUnique other)
        {
            return Figures.CompareTo(other);
        }

        /// <summary>
        /// Empties the card.
        /// </summary>
        /// <returns>ICard&lt;MemberRubric&gt;.</returns>
        public override ICard<MemberRubric> EmptyCard()
        {
            return new RubricCard();
        }

        /// <summary>
        /// Empties the card table.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <returns>ICard&lt;MemberRubric&gt;[].</returns>
        public override ICard<MemberRubric>[] EmptyCardTable(int size)
        {
            return new RubricCard[size];
        }

        /// <summary>
        /// Empties the deck.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <returns>ICard&lt;MemberRubric&gt;[].</returns>
        public override ICard<MemberRubric>[] EmptyDeck(int size)
        {
            return new RubricCard[size];
        }

        /// <summary>
        /// Equalses the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public override bool Equals(IUnique other)
        {
            return Figures.Equals(other);
        }

        /// <summary>
        /// Gets the bytes.
        /// </summary>
        /// <returns>System.Byte[].</returns>
        public override byte[] GetBytes()
        {
            return Figures.GetBytes();
        }

        /// <summary>
        /// Gets the bytes.
        /// </summary>
        /// <param name="figure">The figure.</param>
        /// <returns>System.Byte[].</returns>
        public unsafe byte[] GetBytes(IFigure figure)
        {
            int size = Figures.FigureSize;
            byte* figurePtr = stackalloc byte[size * 2];
            byte* bufferPtr = figurePtr + size;
            figure.StructureTo(figurePtr);
            int destOffset = 0;
            foreach (var rubric in AsValues())
            {
                int l = rubric.RubricSize;
                Extractor.CopyBlock(bufferPtr, destOffset, figurePtr, rubric.RubricOffset, l);
                destOffset += l;
            }
            byte[] b = new byte[destOffset];
            fixed (byte* bp = b)
                Extractor.CopyBlock(bp, bufferPtr, destOffset);
            return b;
        }

        /// <summary>
        /// Gets the unique bytes.
        /// </summary>
        /// <returns>System.Byte[].</returns>
        public override byte[] GetUniqueBytes()
        {
            return Figures.GetUniqueBytes();
        }

        /// <summary>
        /// Gets the unique bytes.
        /// </summary>
        /// <param name="figure">The figure.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>System.Byte[].</returns>
        public unsafe byte[] GetUniqueBytes(IFigure figure, uint seed = 0)
        {
            int size = Figures.FigureSize;
            byte* figurePtr = stackalloc byte[size * 2];
            byte* bufferPtr = figurePtr + size;
            figure.StructureTo(figurePtr);
            int destOffset = 0;
            foreach (var rubric in AsValues())
            {
                int l = rubric.RubricSize;
                Extractor.CopyBlock(bufferPtr, destOffset, figurePtr, rubric.RubricOffset, l);
                destOffset += l;
            }
            ulong hash = Hasher64.ComputeKey(bufferPtr, destOffset, seed);
            byte[] b = new byte[8];
            fixed (byte* bp = b)
                *((ulong*)bp) = hash;
            return b;
        }

        /// <summary>
        /// Gets the unique key.
        /// </summary>
        /// <param name="figure">The figure.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>System.UInt64.</returns>
        public unsafe ulong GetUniqueKey(IFigure figure, uint seed = 0)
        {
            int size = Figures.FigureSize;
            byte* figurePtr = stackalloc byte[size * 2];
            byte* bufferPtr = figurePtr + size;
            figure.StructureTo(figurePtr);
            int destOffset = 0;
            foreach (var rubric in AsValues())
            {
                int l = rubric.RubricSize;
                Extractor.CopyBlock(bufferPtr, destOffset, figurePtr, rubric.RubricOffset, l);
                destOffset += l;
            }
            return Hasher64.ComputeKey(bufferPtr, destOffset, seed);
        }

        /// <summary>
        /// Creates new card.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;MemberRubric&gt;.</returns>
        public override ICard<MemberRubric> NewCard(ICard<MemberRubric> value)
        {
            return new RubricCard(value);
        }

        /// <summary>
        /// Creates new card.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;MemberRubric&gt;.</returns>
        public override ICard<MemberRubric> NewCard(MemberRubric value)
        {
            return new RubricCard(value);
        }

        /// <summary>
        /// Creates new card.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;MemberRubric&gt;.</returns>
        public override ICard<MemberRubric> NewCard(object key, MemberRubric value)
        {
            return new RubricCard(key, value);
        }

        /// <summary>
        /// Creates new card.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;MemberRubric&gt;.</returns>
        public override ICard<MemberRubric> NewCard(ulong key, MemberRubric value)
        {
            return new RubricCard(key, value);
        }

        /// <summary>
        /// Sets the unique key.
        /// </summary>
        /// <param name="figure">The figure.</param>
        /// <param name="seed">The seed.</param>
        public void SetUniqueKey(IFigure figure, uint seed = 0)
        {
            figure.UniqueKey = GetUniqueKey(figure, seed);
        }

        /// <summary>
        /// Updates this instance.
        /// </summary>
        public void Update()
        {
            ordinals = AsValues().Select(o => o.RubricId).ToArray();
            
            binarySizes = AsValues().Select(o => o.RubricSize).ToArray();
            
            binarySize = AsValues().Sum(b => b.RubricSize);

            AsValues().Where(r => r.IsKey || r.RubricType is IUnique)
                      .ForEach(r => r.IsUnique = true).ToArray();
            
            if (KeyRubrics != null)
                KeyRubrics.Update();
        }

        #endregion
    }
}
