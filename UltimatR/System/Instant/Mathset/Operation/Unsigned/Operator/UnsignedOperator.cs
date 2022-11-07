
// <copyright file="UnsignedOperator.cs" company="UltimatR.Core">
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
    /// Class UnsignedOperator.
    /// Implements the <see cref="System.Instant.Mathset.Formula" />
    /// </summary>
    /// <seealso cref="System.Instant.Mathset.Formula" />
    [Serializable]
    public class UnsignedOperator : Formula
    {
        #region Fields

        /// <summary>
        /// The e
        /// </summary>
        protected Formula e;

        #endregion

        #region Constructors





        /// <summary>
        /// Initializes a new instance of the <see cref="UnsignedOperator" /> class.
        /// </summary>
        /// <param name="ee">The ee.</param>
        public UnsignedOperator(Formula ee)
        {
            e = ee;
        }

        #endregion

        #region Properties








        /// <summary>
        /// Gets the size.
        /// </summary>
        /// <value>The size.</value>
        public override MathsetSize Size
        {
            get { return e.Size; }
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
            e.Compile(g, cc);
        }

        #endregion
    }
}
