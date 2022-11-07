
// <copyright file="SubMathset.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The Mathset namespace.
/// </summary>
namespace System.Instant.Mathset
{
    using System.Instant;
    using System.Reflection.Emit;




    /// <summary>
    /// Class SubMathset.
    /// Implements the <see cref="System.Instant.Mathset.LeftFormula" />
    /// </summary>
    /// <seealso cref="System.Instant.Mathset.LeftFormula" />
    [Serializable]
    public class SubMathset : LeftFormula
    {
        #region Fields

        /// <summary>
        /// The start identifier
        /// </summary>
        public int startId = 0;

        #endregion

        #region Constructors






        /// <summary>
        /// Initializes a new instance of the <see cref="SubMathset" /> class.
        /// </summary>
        /// <param name="evalRubric">The eval rubric.</param>
        /// <param name="formuler">The formuler.</param>
        public SubMathset(MathRubric evalRubric, Mathset formuler)
        {
            if (evalRubric != null) Rubric = evalRubric;

            SetDimensions(formuler);
        }

        #endregion

        #region Properties




        /// <summary>
        /// Gets the col count.
        /// </summary>
        /// <value>The col count.</value>
        public int colCount
        {
            get { return Formuler.Rubrics.Count; }
        }




        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <value>The data.</value>
        public IFigures Data
        {
            get { return Formuler.Data; }
        }




        /// <summary>
        /// Gets the field identifier.
        /// </summary>
        /// <value>The field identifier.</value>
        public int FieldId { get => Rubric.FigureFieldId; }




        /// <summary>
        /// Gets or sets the formuler.
        /// </summary>
        /// <value>The formuler.</value>
        public Mathset Formuler { get; set; }




        /// <summary>
        /// Gets the row count.
        /// </summary>
        /// <value>The row count.</value>
        public int rowCount
        {
            get { return Data.Count; }
        }




        /// <summary>
        /// Gets or sets the rubric.
        /// </summary>
        /// <value>The rubric.</value>
        public MathRubric Rubric { get; set; }




        /// <summary>
        /// Gets the name of the rubric.
        /// </summary>
        /// <value>The name of the rubric.</value>
        public string RubricName { get => Rubric.RubricName; }




        /// <summary>
        /// Gets the type of the rubric.
        /// </summary>
        /// <value>The type of the rubric.</value>
        public Type RubricType { get => Rubric.RubricType; }




        /// <summary>
        /// Gets the size.
        /// </summary>
        /// <value>The size.</value>
        public override MathsetSize Size
        {
            get { return new MathsetSize(rowCount, colCount); }
        }




        /// <summary>
        /// Gets or sets the sub formuler.
        /// </summary>
        /// <value>The sub formuler.</value>
        public SubMathset SubFormuler { get; set; }

        #endregion

        #region Methods








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
            }
            else
            {
                CompilerContext.GenLocalLoad(g, cc.GetSubIndexOf(Data));           

                g.Emit(OpCodes.Ldc_I4, FieldId);
                g.EmitCall(OpCodes.Callvirt, typeof(IFigure).GetMethod("get_Item", new Type[] { typeof(int) }), null);
                g.Emit(OpCodes.Unbox_Any, RubricType);
                g.Emit(OpCodes.Conv_R8);
            }
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
                return;
            }

            int i1 = cc.GetIndexVariable(0);

            if (!post)
            {
                if (!partial)
                {
                    CompilerContext.GenLocalLoad(g, cc.GetIndexOf(Data));

                    if (startId != 0)
                        g.Emit(OpCodes.Ldc_I4, startId);

                    g.Emit(OpCodes.Ldloc, i1);

                    if (startId != 0)
                        g.Emit(OpCodes.Add);

                    g.EmitCall(OpCodes.Callvirt, typeof(IFigures).GetMethod("get_Item", new Type[] { typeof(int) }), null);
                    CompilerContext.GenLocalStore(g, cc.GetSubIndexOf(Data));
                    CompilerContext.GenLocalLoad(g, cc.GetSubIndexOf(Data));
                }
                else
                {
                    CompilerContext.GenLocalLoad(g, cc.GetSubIndexOf(Data));
                }
                g.Emit(OpCodes.Ldc_I4, FieldId);

            }
            else
            {
                if (partial)
                {
                    g.Emit(OpCodes.Dup);
                    CompilerContext.GenLocalStore(g, cc.GetBufforIndexOf(Data));           
                }

                g.Emit(OpCodes.Box, typeof(double));
                g.EmitCall(OpCodes.Callvirt, typeof(IFigure).GetMethod("set_Item", new Type[] { typeof(int), typeof(object) }), null);


                if (partial)
                    CompilerContext.GenLocalLoad(g, cc.GetBufforIndexOf(Data));           
            }
        }





        /// <summary>
        /// Sets the dimensions.
        /// </summary>
        /// <param name="formuler">The formuler.</param>
        public void SetDimensions(Mathset formuler = null)
        {
            if (!ReferenceEquals(formuler, null))
                Formuler = formuler;
            Rubric.SubFormuler = this;
        }

        #endregion
    }
}
