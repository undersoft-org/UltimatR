
// <copyright file="MathRubric.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The Mathset namespace.
/// </summary>
namespace System.Instant.Mathset
{
    using System;
    using System.Uniques;




    /// <summary>
    /// Class MathRubric.
    /// Implements the <see cref="System.IUnique" />
    /// </summary>
    /// <seealso cref="System.IUnique" />
    public class MathRubric : IUnique
    {
        #region Fields

        /// <summary>
        /// The formula
        /// </summary>
        [NonSerialized] private CombinedFormula formula;
        /// <summary>
        /// The formula rubrics
        /// </summary>
        [NonSerialized] private MathRubrics formulaRubrics;
        /// <summary>
        /// The formuler
        /// </summary>
        [NonSerialized] private Mathset formuler;
        /// <summary>
        /// The mathline rubrics
        /// </summary>
        [NonSerialized] private MathRubrics mathlineRubrics;
        /// <summary>
        /// The member rubric
        /// </summary>
        [NonSerialized] private MemberRubric memberRubric;
        /// <summary>
        /// The reckoner
        /// </summary>
        [NonSerialized] private CombinedMathset reckoner;
        /// <summary>
        /// The sub formuler
        /// </summary>
        [NonSerialized] private SubMathset subFormuler;

        #endregion

        #region Constructors






        /// <summary>
        /// Initializes a new instance of the <see cref="MathRubric" /> class.
        /// </summary>
        /// <param name="rubrics">The rubrics.</param>
        /// <param name="rubric">The rubric.</param>
        public MathRubric(MathRubrics rubrics, MemberRubric rubric)
        {
            memberRubric = rubric;
            mathlineRubrics = rubrics;
            SerialCode = rubric.SerialCode;
        }

        #endregion

        #region Properties




        /// <summary>
        /// Gets or sets the compute ordinal.
        /// </summary>
        /// <value>The compute ordinal.</value>
        public int ComputeOrdinal { get; set; }




        /// <summary>
        /// Gets the empty.
        /// </summary>
        /// <value>The empty.</value>
        public IUnique Empty => Ussn.Empty;




        /// <summary>
        /// Gets or sets the evaluator.
        /// </summary>
        /// <value>The evaluator.</value>
        public CombinedMathset Evaluator
        {
            get { return reckoner; }
            set { reckoner = value; }
        }




        /// <summary>
        /// Gets the figure field identifier.
        /// </summary>
        /// <value>The figure field identifier.</value>
        public int FigureFieldId { get => memberRubric.FieldId; }




        /// <summary>
        /// Gets or sets the formula.
        /// </summary>
        /// <value>The formula.</value>
        public Formula Formula
        {
            get
            {
                return (!ReferenceEquals(formula, null)) ? formula : null;
            }
            set
            {
                if (!ReferenceEquals(value, null))
                {
                    formula = value.Prepare(Formuler[this.memberRubric.RubricName]);
                }
            }
        }




        /// <summary>
        /// Gets the formula rubric.
        /// </summary>
        /// <value>The formula rubric.</value>
        public MathRubric FormulaRubric
        {
            get { return this; }
        }




        /// <summary>
        /// Gets or sets the formula rubrics.
        /// </summary>
        /// <value>The formula rubrics.</value>
        public MathRubrics FormulaRubrics
        {
            get { return formulaRubrics; }
            set { formulaRubrics = value; }
        }




        /// <summary>
        /// Gets or sets the formuler.
        /// </summary>
        /// <value>The formuler.</value>
        public Mathset Formuler
        {
            get { return formuler; }
            set { formuler = value; }
        }




        /// <summary>
        /// Gets or sets the mathset rubrics.
        /// </summary>
        /// <value>The mathset rubrics.</value>
        public MathRubrics MathsetRubrics
        {
            get { return mathlineRubrics; }
            set { mathlineRubrics = value; }
        }




        /// <summary>
        /// Gets or sets a value indicating whether [partial mathset].
        /// </summary>
        /// <value><c>true</c> if [partial mathset]; otherwise, <c>false</c>.</value>
        public bool PartialMathset { get; set; }




        /// <summary>
        /// Gets or sets the right formula.
        /// </summary>
        /// <value>The right formula.</value>
        public Formula RightFormula { get; set; }




        /// <summary>
        /// Gets the name of the rubric.
        /// </summary>
        /// <value>The name of the rubric.</value>
        public string RubricName { get => memberRubric.RubricName; }




        /// <summary>
        /// Gets the type of the rubric.
        /// </summary>
        /// <value>The type of the rubric.</value>
        public Type RubricType { get => memberRubric.RubricType; }













        /// <summary>
        /// Gets or sets the serial code.
        /// </summary>
        /// <value>The serial code.</value>
        public Ussn SerialCode { get; set; }




        /// <summary>
        /// Gets or sets the sub formuler.
        /// </summary>
        /// <value>The sub formuler.</value>
        public SubMathset SubFormuler { get; set; }




        /// <summary>
        /// Gets or sets the unique key.
        /// </summary>
        /// <value>The unique key.</value>
        public ulong UniqueKey { get => SerialCode.UniqueKey; set => SerialCode.SetUniqueKey(value); }




        /// <summary>
        /// Gets or sets the unique seed.
        /// </summary>
        /// <value>The unique seed.</value>
        public ulong UniqueSeed { get => SerialCode.UniqueSeed; set => SerialCode.SetUniqueSeed(value); }

        #endregion

        #region Methods






        /// <summary>
        /// Assigns the rubric.
        /// </summary>
        /// <param name="ordinal">The ordinal.</param>
        /// <returns>MathRubric.</returns>
        public MathRubric AssignRubric(int ordinal)
        {
            if (FormulaRubrics == null)
                FormulaRubrics = new MathRubrics(mathlineRubrics);

            MathRubric erubric = null;
            MemberRubric rubric = MathsetRubrics.Rubrics[ordinal];
            if (rubric != null)
            {
                erubric = new MathRubric(MathsetRubrics, rubric);
                assignRubric(erubric);
            }
            return erubric;
        }






        /// <summary>
        /// Assigns the rubric.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>MathRubric.</returns>
        public MathRubric AssignRubric(string name)
        {
            if (FormulaRubrics == null)
                FormulaRubrics = new MathRubrics(mathlineRubrics);

            MathRubric erubric = null;
            MemberRubric rubric = MathsetRubrics.Rubrics[name];
            if (rubric != null)
            {
                erubric = new MathRubric(MathsetRubrics, rubric);
                assignRubric(erubric);
            }
            return erubric;
        }





        /// <summary>
        /// Clones the mathset.
        /// </summary>
        /// <returns>Mathset.</returns>
        public Mathset CloneMathset()
        {
            return formuler.Clone();
        }





        /// <summary>
        /// Combines the evaluator.
        /// </summary>
        /// <returns>CombinedMathset.</returns>
        public CombinedMathset CombineEvaluator()
        {
            if (reckoner == null)
                reckoner = formula.CombineMathset(formula);

            return reckoner;
        }






        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="other">An object to compare with this instance.</param>
        /// <returns>A value that indicates the relative order of the objects being compared. The return value has these meanings:
        /// <list type="table"><listheader><term> Value</term><description> Meaning</description></listheader><item><term> Less than zero</term><description> This instance precedes <paramref name="other" /> in the sort order.</description></item><item><term> Zero</term><description> This instance occurs in the same position in the sort order as <paramref name="other" />.</description></item><item><term> Greater than zero</term><description> This instance follows <paramref name="other" /> in the sort order.</description></item></list></returns>
        public int CompareTo(IUnique other)
        {
            return (int)(UniqueKey - other.UniqueKey);
        }





        /// <summary>
        /// Computes this instance.
        /// </summary>
        /// <returns>LeftFormula.</returns>
        public LeftFormula Compute()
        {
            if (reckoner != null)
            {
                Evaluator reckon = new Evaluator(reckoner.Compute);
                reckon();
            }
            return formula.lexpr;
        }















        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><see langword="true" /> if the current object is equal to the <paramref name="other" /> parameter; otherwise, <see langword="false" />.</returns>
        public bool Equals(IUnique other)
        {
            return UniqueKey == other.UniqueKey;
        }





        /// <summary>
        /// Gets the bytes.
        /// </summary>
        /// <returns>System.Byte[].</returns>
        public byte[] GetBytes()
        {
            return SerialCode.GetBytes();
        }





        /// <summary>
        /// Gets the mathset.
        /// </summary>
        /// <returns>Mathset.</returns>
        public Mathset GetMathset()
        {
            if (!ReferenceEquals(Formuler, null))
                return Formuler;
            else
            {
                formulaRubrics = new MathRubrics(mathlineRubrics);
                return Formuler = new Mathset(this);
            }
        }





        /// <summary>
        /// Gets the unique bytes.
        /// </summary>
        /// <returns>System.Byte[].</returns>
        public byte[] GetUniqueBytes()
        {
            return SerialCode.GetUniqueBytes();
        }





        /// <summary>
        /// Creates new mathset.
        /// </summary>
        /// <returns>Mathset.</returns>
        public Mathset NewMathset()
        {
            return Formuler = new Mathset(this);
        }






        /// <summary>
        /// Removes the rubric.
        /// </summary>
        /// <param name="ordinal">The ordinal.</param>
        /// <returns>MathRubric.</returns>
        public MathRubric RemoveRubric(int ordinal)
        {
            MathRubric erubric = null;
            MemberRubric rubric = MathsetRubrics.Rubrics[ordinal];
            if (rubric != null)
            {
                erubric = MathsetRubrics[rubric];
                removeRubric(erubric);
            }
            return erubric;
        }






        /// <summary>
        /// Removes the rubric.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>MathRubric.</returns>
        public MathRubric RemoveRubric(string name)
        {
            MathRubric erubric = null;
            MemberRubric rubric = MathsetRubrics.Rubrics[name];
            if (rubric != null)
            {
                erubric = MathsetRubrics[rubric];
                removeRubric(erubric);
            }
            return erubric;
        }






        /// <summary>
        /// Assigns the rubric.
        /// </summary>
        /// <param name="erubric">The erubric.</param>
        /// <returns>MathRubric.</returns>
        private MathRubric assignRubric(MathRubric erubric)
        {
            if (!FormulaRubrics.Contains(erubric))
            {
                if (!MathsetRubrics.MathsetRubrics.Contains(erubric))
                {
                    MathsetRubrics.MathsetRubrics.Add(erubric);
                }

                if (erubric.FigureFieldId == FormulaRubric.FigureFieldId &&
                    !MathsetRubrics.FormulaRubrics.Contains(erubric))
                    MathsetRubrics.FormulaRubrics.Add(erubric);

                FormulaRubrics.Add(erubric);
            }
            return erubric;
        }






        /// <summary>
        /// Removes the rubric.
        /// </summary>
        /// <param name="erubric">The erubric.</param>
        /// <returns>MathRubric.</returns>
        private MathRubric removeRubric(MathRubric erubric)
        {
            if (!FormulaRubrics.Contains(erubric))
            {
                FormulaRubrics.Remove(erubric);

                if (!MathsetRubrics.MathsetRubrics.Contains(erubric))
                    MathsetRubrics.MathsetRubrics.Remove(erubric);

                if (!ReferenceEquals(Formuler, null) &&
                    !MathsetRubrics.FormulaRubrics.Contains(erubric))
                {
                    MathsetRubrics.Rubrics.Remove(erubric);
                    Formuler = null;
                    Formula = null;
                }
            }
            return erubric;
        }

        #endregion
    }
}
