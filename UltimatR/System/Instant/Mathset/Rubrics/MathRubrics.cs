
// <copyright file="MathRubrics.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The Mathset namespace.
/// </summary>
namespace System.Instant.Mathset
{
    using System.Linq;
    using System.Series;




    /// <summary>
    /// Class MathRubrics.
    /// Implements the <see cref="System.Series.AlbumBase{System.Instant.Mathset.MathRubric}" />
    /// </summary>
    /// <seealso cref="System.Series.AlbumBase{System.Instant.Mathset.MathRubric}" />
    public class MathRubrics : AlbumBase<MathRubric>
    {
        #region Constructors





        /// <summary>
        /// Initializes a new instance of the <see cref="MathRubrics" /> class.
        /// </summary>
        /// <param name="data">The data.</param>
        public MathRubrics(IFigures data)
        {
            Rubrics = data.Rubrics;
            FormulaRubrics = new MathRubrics(Rubrics);
            MathsetRubrics = new MathRubrics(Rubrics);
            Data = data;
        }




        /// <summary>
        /// Initializes a new instance of the <see cref="MathRubrics" /> class.
        /// </summary>
        /// <param name="rubrics">The rubrics.</param>
        public MathRubrics(IRubrics rubrics)
        {
            Rubrics = rubrics;
            Data = rubrics.Figures;
        }




        /// <summary>
        /// Initializes a new instance of the <see cref="MathRubrics" /> class.
        /// </summary>
        /// <param name="rubrics">The rubrics.</param>
        public MathRubrics(MathRubrics rubrics)
        {
            Rubrics = rubrics.Rubrics;
            Data = rubrics.Data;
        }

        #endregion

        #region Properties




        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>The data.</value>
        public IFigures Data { get; set; }




        /// <summary>
        /// Gets or sets the formula rubrics.
        /// </summary>
        /// <value>The formula rubrics.</value>
        public MathRubrics FormulaRubrics { get; set; }




        /// <summary>
        /// Gets or sets the mathset rubrics.
        /// </summary>
        /// <value>The mathset rubrics.</value>
        public MathRubrics MathsetRubrics { get; set; }




        /// <summary>
        /// Gets the rows count.
        /// </summary>
        /// <value>The rows count.</value>
        public int RowsCount
        {
            get
            {
                return Data.Count;
            }
        }




        /// <summary>
        /// Gets or sets the rubrics.
        /// </summary>
        /// <value>The rubrics.</value>
        public IRubrics Rubrics { get; set; }




        /// <summary>
        /// Gets the rubrics count.
        /// </summary>
        /// <value>The rubrics count.</value>
        public int RubricsCount
        {
            get
            {
                return Rubrics.Count;
            }
        }

        #endregion

        #region Methods





        /// <summary>
        /// Combines this instance.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool Combine()
        {
            if (!ReferenceEquals(Data, null))
            {
                CombinedMathset[] evs = CombineEvaluators();
                bool[] b = evs.Select(e => e.SetParams(Data, 0)).ToArray();
                return true;
            }
            CombineEvaluators();
            return false;
        }






        /// <summary>
        /// Combines the specified table.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool Combine(IFigures table)
        {
            if (!ReferenceEquals(Data, table))
            {
                Data = table;
                CombinedMathset[] evs = CombineEvaluators();
                bool[] b = evs.Select(e => e.SetParams(Data, 0)).ToArray();
                return true;
            }
            CombineEvaluators();
            return false;
        }





        /// <summary>
        /// Combines the evaluators.
        /// </summary>
        /// <returns>CombinedMathset[].</returns>
        public CombinedMathset[] CombineEvaluators()
        {
            return this.AsValues().Select(m => m.CombineEvaluator()).ToArray();
        }






        /// <summary>
        /// Empties the deck.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <returns>ICard&lt;MathRubric&gt;[].</returns>
        public override ICard<MathRubric>[] EmptyDeck(int size)
        {
            return new MathRubricCard[size];
        }





        /// <summary>
        /// Empties the card.
        /// </summary>
        /// <returns>ICard&lt;MathRubric&gt;.</returns>
        public override ICard<MathRubric> EmptyCard()
        {
            return new MathRubricCard();
        }






        /// <summary>
        /// Empties the card table.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <returns>ICard&lt;MathRubric&gt;[].</returns>
        public override ICard<MathRubric>[] EmptyCardTable(int size)
        {
            return new MathRubricCard[size];
        }






        /// <summary>
        /// Creates new card.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;MathRubric&gt;.</returns>
        public override ICard<MathRubric> NewCard(ICard<MathRubric> value)
        {
            return new MathRubricCard(value);
        }






        /// <summary>
        /// Creates new card.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;MathRubric&gt;.</returns>
        public override ICard<MathRubric> NewCard(MathRubric value)
        {
            return new MathRubricCard(value);
        }







        /// <summary>
        /// Creates new card.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;MathRubric&gt;.</returns>
        public override ICard<MathRubric> NewCard(object key, MathRubric value)
        {
            return new MathRubricCard(key, value);
        }







        /// <summary>
        /// Creates new card.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;MathRubric&gt;.</returns>
        public override ICard<MathRubric> NewCard(ulong key, MathRubric value)
        {
            return new MathRubricCard(key, value);
        }

        #endregion
    }
}
