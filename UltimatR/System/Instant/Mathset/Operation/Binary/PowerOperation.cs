
// <copyright file="PowerOperation.cs" company="UltimatR.Core">
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
    /// Class PowerOperation.
    /// Implements the <see cref="System.Instant.Mathset.BinaryFormula" />
    /// </summary>
    /// <seealso cref="System.Instant.Mathset.BinaryFormula" />
    [Serializable]
    public class PowerOperation : BinaryFormula
    {
        #region Constructors






        /// <summary>
        /// Initializes a new instance of the <see cref="PowerOperation" /> class.
        /// </summary>
        /// <param name="e1">The e1.</param>
        /// <param name="e2">The e2.</param>
        public PowerOperation(Formula e1, Formula e2) : base(e1, e2)
        {
        }

        #endregion

        #region Properties




        /// <summary>
        /// Gets the size.
        /// </summary>
        /// <value>The size.</value>
        public override MathsetSize Size
        {
            get { return expr1.Size; }
        }

        #endregion

        #region Methods






        /// <summary>
        /// Compiles the specified g.
        /// </summary>
        /// <param name="g">The g.</param>
        /// <param name="cc">The cc.</param>
        /// <exception cref="System.Instant.Mathset.SizeMismatchException">Pow Operator requires a scalar second operand</exception>
        public override void Compile(ILGenerator g, CompilerContext cc)
        {
            if (cc.IsFirstPass())
            {
                expr1.Compile(g, cc);
                expr2.Compile(g, cc);
                if (!(expr2.Size == MathsetSize.Scalar))
                    throw new SizeMismatchException("Pow Operator requires a scalar second operand");
                return;
            }
            expr1.Compile(g, cc);
            expr2.Compile(g, cc);
            g.EmitCall(OpCodes.Call, typeof(Math).GetMethod("Pow"), null);
        }

        #endregion
    }
}
