
// <copyright file="Mathset.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The Mathset namespace.
/// </summary>
namespace System.Instant.Mathset
{
    using System.Reflection.Emit;


    /// <summary>
    /// Class Mathset.
    /// Implements the <see cref="System.Instant.Mathset.LeftFormula" />
    /// </summary>
    /// <seealso cref="System.Instant.Mathset.LeftFormula" />
    [Serializable]
    public class Mathset : LeftFormula
    {
        /// <summary>
        /// The context
        /// </summary>
        [NonSerialized]
        private CompilerContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="Mathset" /> class.
        /// </summary>
        /// <param name="rubric">The rubric.</param>
        public Mathset(MathRubric rubric)
        {
            Rubric = rubric;
            Formuler = rubric.Formuler;
        }

        /// <summary>
        /// Gets or sets the formuler.
        /// </summary>
        /// <value>The formuler.</value>
        public Mathset Formuler
        { get; set; }
        /// <summary>
        /// Gets or sets the sub formuler.
        /// </summary>
        /// <value>The sub formuler.</value>
        public SubMathset SubFormuler
        { get; set; }
        /// <summary>
        /// Gets or sets the formula.
        /// </summary>
        /// <value>The formula.</value>
        public Formula Formula
        {
            get => Rubric.Formula;
            set => Rubric.Formula = value;
        }
        /// <summary>
        /// The partial formula
        /// </summary>
        public Formula PartialFormula;
        /// <summary>
        /// The sub formula
        /// </summary>
        public Mathstage SubFormula;

        /// <summary>
        /// The partial mathset
        /// </summary>
        public bool PartialMathset = false;

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <value>The data.</value>
        public IFigures Data
        { get => Rubric.MathsetRubrics.Data; }

        /// <summary>
        /// Gets or sets the rubrics.
        /// </summary>
        /// <value>The rubrics.</value>
        public MathRubrics Rubrics
        {
            get => Rubric.FormulaRubrics;
            set => Rubric.FormulaRubrics = value;
        }
        /// <summary>
        /// Gets or sets the rubric.
        /// </summary>
        /// <value>The rubric.</value>
        public MathRubric Rubric
        { get; set; }

        /// <summary>
        /// Gets the name of the rubric.
        /// </summary>
        /// <value>The name of the rubric.</value>
        public string RubricName
        { get => Rubric.RubricName; }
        /// <summary>
        /// Gets the type of the rubric.
        /// </summary>
        /// <value>The type of the rubric.</value>
        public Type RubricType
        { get => Rubric.RubricType; }
        /// <summary>
        /// Gets the field identifier.
        /// </summary>
        /// <value>The field identifier.</value>
        public int FieldId
        { get => Rubric.FigureFieldId; }

        /// <summary>
        /// Gets or sets the row count.
        /// </summary>
        /// <value>The row count.</value>
        public int rowCount
        { get; set; }
        /// <summary>
        /// Gets the col count.
        /// </summary>
        /// <value>The col count.</value>
        public int colCount
        { get => Rubrics.Count; }

        /// <summary>
        /// The start identifier
        /// </summary>
        public int startId = 0;

        /// <summary>
        /// Assigns the rubric.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>MathRubric.</returns>
        public MathRubric AssignRubric(string name)
        {
            return Rubric.AssignRubric(name);
        }
        /// <summary>
        /// Assigns the rubric.
        /// </summary>
        /// <param name="ordinal">The ordinal.</param>
        /// <returns>MathRubric.</returns>
        public MathRubric AssignRubric(int ordinal)
        {
            return Rubric.AssignRubric(ordinal);
        }
        /// <summary>
        /// Removes the rubric.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>MathRubric.</returns>
        public MathRubric RemoveRubric(string name)
        {
            return Rubric.AssignRubric(name);
        }

        /// <summary>
        /// Assigns the context.
        /// </summary>
        /// <param name="Context">The context.</param>
        public void AssignContext(CompilerContext Context)
        {
            if (context == null || !ReferenceEquals(context, Context))
                context = Context;
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns>Mathset.</returns>
        public Mathset Clone()
        {
            Mathset mx = (Mathset)this.MemberwiseClone();
            return mx;
        }

        /// <summary>
        /// Gets or sets the <see cref="System.Double" /> at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>System.Double.</returns>
        public double this[long index]
        {
            get
            {
                int length = Data.GetType().GetFields().Length - 1;
                return Convert.ToDouble((Data[(int)index / length])[(int)index % length]);
            }
            set
            {
                int length = Data.GetType().GetFields().Length - 1;
                (Data[(int)index / length])[(int)index % length] = value;
            }
        }
        /// <summary>
        /// Gets or sets the <see cref="System.Double" /> at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="field">The field.</param>
        /// <returns>System.Double.</returns>
        public double this[long index, long field]
        {
            get { return Convert.ToDouble(Data[(int)index, (int)field]); }
            set { Data[(int)index, (int)field] = value; }
        }
        /// <summary>
        /// Gets the <see cref="SubMathset" /> with the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>SubMathset.</returns>
        public SubMathset this[string name]
        {
            get
            {
                if (SubFormula == null)
                    SubFormula = new Mathstage(this);
                return SubFormula[name];
            }
        }
        /// <summary>
        /// Gets the <see cref="SubMathset" /> with the specified r.
        /// </summary>
        /// <param name="r">The r.</param>
        /// <param name="name">The name.</param>
        /// <returns>SubMathset.</returns>
        public SubMathset this[int r, string name]
        {
            get
            {
                if (SubFormula == null)
                    SubFormula = new Mathstage(this, r, r);
                return SubFormula[name];
            }
        }
        /// <summary>
        /// Gets the <see cref="Mathstage" /> with the specified r.
        /// </summary>
        /// <param name="r">The r.</param>
        /// <returns>Mathstage.</returns>
        public Mathstage this[int r]
        {
            get
            {
                return new Mathstage(this, r, r);
            }
        }
        /// <summary>
        /// Gets the <see cref="Mathstage" /> with the specified q.
        /// </summary>
        /// <param name="q">The q.</param>
        /// <returns>Mathstage.</returns>
        public Mathstage this[IndexRange q]
        {
            get { return new Mathstage(this, q.first, q.last); }
        }

        /// <summary>
        /// Ranges the specified i1.
        /// </summary>
        /// <param name="i1">The i1.</param>
        /// <param name="i2">The i2.</param>
        /// <returns>IndexRange.</returns>
        public static IndexRange Range(int i1, int i2)
        {
            return new IndexRange(i1, i2);
        }

        /// <summary>
        /// Compiles the assign.
        /// </summary>
        /// <param name="g">The g.</param>
        /// <param name="cc">The cc.</param>
        /// <param name="post">if set to <c>true</c> [post].</param>
        /// <param name="partial">if set to <c>true</c> [partial].</param>
        public override void CompileAssign(ILGenerator g, CompilerContext cc, bool post, bool partial)
        {
            if (cc.IsFirstPass())
            {
                cc.Add(Data);
                PartialFormula = Formula.RightFormula.Prepare(this[RubricName], false);
                PartialFormula.Compile(g, cc);
                Rubric.PartialMathset = true;
            }
            else
            {
                PartialFormula.Compile(g, cc);
            }

        }
        /// <summary>
        /// Compiles the specified g.
        /// </summary>
        /// <param name="g">The g.</param>
        /// <param name="cc">The cc.</param>
        public override void Compile(ILGenerator g, CompilerContext cc)
        {
            if (cc.IsFirstPass())
            {
                cc.Add(Data);
                PartialFormula = Formula.RightFormula.Prepare(this[RubricName], true);
                PartialFormula.Compile(g, cc);
                Rubric.PartialMathset = true;
            }
            else
            {
                PartialFormula.Compile(g, cc);
            }
        }

        /// <summary>
        /// Gets the size.
        /// </summary>
        /// <value>The size.</value>
        public override MathsetSize Size
        {
            get { return new MathsetSize(Data.Count, Rubrics.Count); }
        }

        /// <summary>
        /// Sets the dimensions.
        /// </summary>
        /// <param name="sm">The sm.</param>
        /// <param name="mx">The mx.</param>
        /// <param name="offset">The offset.</param>
        public void SetDimensions(SubMathset sm, Mathset mx = null, int offset = 0)
        {
            sm.startId = offset;
            sm.SetDimensions(mx);
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <param name="rubric">The rubric.</param>
        /// <returns>SubMathset.</returns>
        public SubMathset GetAll(MathRubric rubric)
        {
            SubMathset smx = new SubMathset(rubric, this);
            return smx;
        }
        /// <summary>
        /// Gets the range.
        /// </summary>
        /// <param name="startRowId">The start row identifier.</param>
        /// <param name="endRowId">The end row identifier.</param>
        /// <param name="rubric">The rubric.</param>
        /// <returns>SubMathset.</returns>
        public SubMathset GetRange(int startRowId, int endRowId, MathRubric rubric)
        {
            startId = startRowId;
            rowCount = (endRowId - startRowId) + 1;
            SubMathset smx = new SubMathset(rubric, this);
            return smx;
        }
        /// <summary>
        /// Gets the column.
        /// </summary>
        /// <param name="j">The j.</param>
        /// <returns>SubMathset.</returns>
        public SubMathset GetColumn(int j)
        {
            return GetRange(0, j, null);
        }
        /// <summary>
        /// Gets the column count.
        /// </summary>
        /// <param name="j1">The j1.</param>
        /// <param name="j2">The j2.</param>
        /// <returns>SubMathset.</returns>
        public SubMathset GetColumnCount(int j1, int j2)
        {
            return GetRange(0, 1, null);
        }
        /// <summary>
        /// Gets the row.
        /// </summary>
        /// <param name="i">The i.</param>
        /// <returns>SubMathset.</returns>
        public SubMathset GetRow(int i)
        {
            return GetRange(i, 1, null);
        }
        /// <summary>
        /// Gets the row count.
        /// </summary>
        /// <param name="i1">The i1.</param>
        /// <param name="i2">The i2.</param>
        /// <returns>SubMathset.</returns>
        public SubMathset GetRowCount(int i1, int i2)
        {
            return GetRange(i1, i2, null);
        }
        /// <summary>
        /// Gets the elements.
        /// </summary>
        /// <param name="e1">The e1.</param>
        /// <param name="e2">The e2.</param>
        /// <returns>SubMathset.</returns>
        public SubMathset GetElements(int e1, int e2)
        {
            return new SubMathset(null, this);
        }

        /// <summary>
        /// Class Mathstage.
        /// </summary>
        [Serializable]
        public class Mathstage
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Mathstage" /> class.
            /// </summary>
            /// <param name="m">The m.</param>
            internal Mathstage(Mathset m)
            {
                formuler = m;
                firstRow = 0;
                rowCount = (m.rowCount - firstRow) - 1;
            }
            /// <summary>
            /// Initializes a new instance of the <see cref="Mathstage" /> class.
            /// </summary>
            /// <param name="m">The m.</param>
            /// <param name="startRowId">The start row identifier.</param>
            /// <param name="endRowId">The end row identifier.</param>
            internal Mathstage(Mathset m, int startRowId, int endRowId)
            {
                firstRow = startRowId;
                rowCount = (endRowId - startRowId);
                formuler = m;
            }

            /// <summary>
            /// Gets the <see cref="SubMathset" /> with the specified ordinal.
            /// </summary>
            /// <param name="ordinal">The ordinal.</param>
            /// <returns>SubMathset.</returns>
            public SubMathset this[int ordinal]
            {
                get
                {
                    MathRubric rubric = formuler.Rubric.AssignRubric(ordinal);
                    return formuler.GetAll(rubric);
                }
            }
            /// <summary>
            /// Gets the <see cref="SubMathset" /> with the specified name.
            /// </summary>
            /// <param name="name">The name.</param>
            /// <returns>SubMathset.</returns>
            public SubMathset this[string name]
            {
                get
                {
                    try
                    {
                        MathRubric rubric = formuler.Rubric.AssignRubric(name);
                        return formuler.GetAll(rubric);
                    }
                    catch (Exception ex)
                    {
                        return null;
                    }
                }
            }

            /// <summary>
            /// Performs an explicit conversion from <see cref="Mathstage" /> to <see cref="LeftFormula" />.
            /// </summary>
            /// <param name="r">The r.</param>
            /// <returns>The result of the conversion.</returns>
            public static explicit operator LeftFormula(Mathstage r)
            {
                return r.formuler.GetElements(r.firstRow, r.lastRow);
            }

            /// <summary>
            /// The formuler
            /// </summary>
            private Mathset formuler;

            /// <summary>
            /// The first row
            /// </summary>
            public int firstRow;
            /// <summary>
            /// The row count
            /// </summary>
            public int rowCount = -1;
            /// <summary>
            /// Gets the last row.
            /// </summary>
            /// <value>The last row.</value>
            public int lastRow
            {
                get { return (formuler.rowCount > (firstRow + rowCount + 1) && rowCount > -1) ? firstRow + rowCount : formuler.rowCount - 1; }
            }
        }

        /// <summary>
        /// Struct IndexRange
        /// </summary>
        [Serializable]
        public struct IndexRange
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="IndexRange" /> struct.
            /// </summary>
            /// <param name="i1">The i1.</param>
            /// <param name="i2">The i2.</param>
            internal IndexRange(int i1, int i2)
            {
                first = i1;
                last = i2;
            }
            /// <summary>
            /// The first
            /// </summary>
            internal int first, last;
        }
    }
}
