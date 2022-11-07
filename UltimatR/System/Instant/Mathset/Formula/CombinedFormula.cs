
// <copyright file="CombinedFormula.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The Mathset namespace.
/// </summary>
namespace System.Instant.Mathset
{
    using System;
    using System.Reflection.Emit;





    /// <summary>
    /// Class CombinedFormula.
    /// Implements the <see cref="System.Instant.Mathset.Formula" />
    /// </summary>
    /// <seealso cref="System.Instant.Mathset.Formula" />
    [Serializable]
    public class CombinedFormula : Formula
    {
        #region Fields

        /// <summary>
        /// The expr
        /// </summary>
        public Formula expr;
        /// <summary>
        /// The lexpr
        /// </summary>
        public LeftFormula lexpr;
        /// <summary>
        /// The partial
        /// </summary>
        public bool partial = false;
        /// <summary>
        /// The i i
        /// </summary>
        internal int iI, lL;

        #endregion

        #region Constructors







        /// <summary>
        /// Initializes a new instance of the <see cref="CombinedFormula" /> class.
        /// </summary>
        /// <param name="m">The m.</param>
        /// <param name="e">The e.</param>
        /// <param name="partial">if set to <c>true</c> [partial].</param>
        public CombinedFormula(LeftFormula m, Formula e, bool partial = false)
        {
            lexpr = m;
            expr = e;
            this.partial = partial;
        }

        #endregion

        #region Properties




        /// <summary>
        /// Gets the size.
        /// </summary>
        /// <value>The size.</value>
        internal MathsetSize size
        {
            get { return expr.Size; }
        }

        #endregion

        #region Methods






        /// <summary>
        /// Compiles the specified g.
        /// </summary>
        /// <param name="g">The g.</param>
        /// <param name="cc">The cc.</param>
        public override void Compile(ILGenerator g, CompilerContext cc)
        {
            bool biloop = size.rows > 1 && size.cols > 1;

            if (cc.IsFirstPass())
            {
                if (!partial)
                {
                    iI = cc.AllocIndexVariable();   
                    lL = cc.AllocIndexVariable();   
                }
                expr.Compile(g, cc);
                lexpr.CompileAssign(g, cc, true, partial);
            }
            else
            {
                if (!partial)
                {
                    int i, l, svi;
                    Label topLabel;
                    Label topLabelE;

                    topLabel = g.DefineLabel();
                    topLabelE = g.DefineLabel();

                    i = cc.GetIndexVariable(iI);
                    l = cc.GetIndexVariable(lL);
                    
                    svi = cc.GetIndexVariable(0);

                    cc.SetIndexVariable(0, i);
                    cc.SetIndexVariable(1, size.rows);

                    g.Emit(OpCodes.Ldc_I4_0);   
                    g.Emit(OpCodes.Stloc, i);
                    g.Emit(OpCodes.Ldarg_0);
                    g.Emit(OpCodes.Ldc_I4_0);
                    g.EmitCall(OpCodes.Callvirt, typeof(CombinedMathset).GetMethod("GetRowCount", new Type[] { typeof(int) }), null);
                    g.Emit(OpCodes.Stloc, l);

                    if (size.rows > 1 || size.cols > 1)
                    {
                        
                        int index;
                        int count;

                        index = i;
                        count = size.rows;

                        
                        g.MarkLabel(topLabel);          

                        lexpr.CompileAssign(g, cc, false, false);
                        expr.Compile(g, cc);                        
                        lexpr.CompileAssign(g, cc, true, false);

                        
                        g.Emit(OpCodes.Ldloc, index);           
                        g.Emit(OpCodes.Ldc_I4_1);               
                        g.Emit(OpCodes.Add);                    
                        g.Emit(OpCodes.Dup);
                        g.Emit(OpCodes.Stloc, index);           

                        
                        

                        g.Emit(OpCodes.Ldloc, l);
                        g.Emit(OpCodes.Blt, topLabel);
                    }
                    else
                    {
                        lexpr.CompileAssign(g, cc, false, false);
                        expr.Compile(g, cc);                        
                        lexpr.CompileAssign(g, cc, true, false);
                    }
                    cc.SetIndexVariable(0, svi);
                }
                else
                {
                    lexpr.CompileAssign(g, cc, false, true);
                    expr.Compile(g, cc);                        
                    lexpr.CompileAssign(g, cc, true, true);
                }
            }
        }

        #endregion
    }
}
