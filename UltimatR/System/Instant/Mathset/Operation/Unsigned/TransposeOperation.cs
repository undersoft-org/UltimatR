
// <copyright file="TransposeOperation.cs" company="UltimatR.Core">
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
    /// Class TransposeOperation.
    /// Implements the <see cref="System.Instant.Mathset.UnsignedOperator" />
    /// </summary>
    /// <seealso cref="System.Instant.Mathset.UnsignedOperator" />
    [Serializable]
    public class TransposeOperation : UnsignedOperator
    {
        #region Constructors





        /// <summary>
        /// Initializes a new instance of the <see cref="TransposeOperation" /> class.
        /// </summary>
        /// <param name="e">The e.</param>
        public TransposeOperation(Formula e) : base(e)
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
            get
            {
                MathsetSize o = e.Size;
                return new MathsetSize(o.cols, o.rows);
            }
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
            if (cc.IsFirstPass())
            {
                e.Compile(g, cc);
                return;
            }

            
            int i1 = cc.GetIndexVariable(0);
            int i2 = cc.GetIndexVariable(1);
            cc.SetIndexVariable(1, i1);
            cc.SetIndexVariable(0, i2);
            e.Compile(g, cc);
            cc.SetIndexVariable(0, i1);
            cc.SetIndexVariable(1, i2);
        }

        #endregion
    }
}
