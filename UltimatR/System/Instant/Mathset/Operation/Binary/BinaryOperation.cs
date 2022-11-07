
// <copyright file="BinaryOperation.cs" company="UltimatR.Core">
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
    /// Class BinaryOperation.
    /// Implements the <see cref="System.Instant.Mathset.BinaryFormula" />
    /// </summary>
    /// <seealso cref="System.Instant.Mathset.BinaryFormula" />
    [Serializable]
    public class BinaryOperation : BinaryFormula
    {
        #region Fields

        /// <summary>
        /// The oper
        /// </summary>
        internal BinaryOperator oper;

        #endregion

        #region Constructors








        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryOperation" /> class.
        /// </summary>
        /// <param name="e1">The e1.</param>
        /// <param name="e2">The e2.</param>
        /// <param name="op">The op.</param>
        public BinaryOperation(Formula e1, Formula e2, BinaryOperator op) : base(e1, e2)
        {
            oper = op;
        }

        #endregion

        #region Properties








        /// <summary>
        /// Gets the size.
        /// </summary>
        /// <value>The size.</value>
        public override MathsetSize Size
        {
            get { return expr1.Size == MathsetSize.Scalar ? expr2.Size : expr1.Size; }
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
            expr1.Compile(g, cc);
            expr2.Compile(g, cc);
            if (cc.IsFirstPass())
                return;
            oper.Compile(g);
        }

        #endregion
    }
}
