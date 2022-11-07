
// <copyright file="UnsignedFormula.cs" company="UltimatR.Core">
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
    /// Class UnsignedFormula.
    /// Implements the <see cref="System.Instant.Mathset.Formula" />
    /// </summary>
    /// <seealso cref="System.Instant.Mathset.Formula" />
    [Serializable]
    public class UnsignedFormula : Formula
    {
        #region Fields

        /// <summary>
        /// The thevalue
        /// </summary>
        internal double thevalue;

        #endregion

        #region Constructors





        /// <summary>
        /// Initializes a new instance of the <see cref="UnsignedFormula" /> class.
        /// </summary>
        /// <param name="vv">The vv.</param>
        public UnsignedFormula(double vv)
        {
            thevalue = vv;
        }

        #endregion

        #region Properties








        /// <summary>
        /// Gets the size.
        /// </summary>
        /// <value>The size.</value>
        public override MathsetSize Size
        {
            get { return MathsetSize.Scalar; }
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
            if (cc.IsFirstPass()) return;
            g.Emit(OpCodes.Ldc_R8, thevalue);
        }

        #endregion
    }
}
