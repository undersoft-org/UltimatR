
// <copyright file="LeftFormula.cs" company="UltimatR.Core">
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
    /// Class LeftFormula.
    /// Implements the <see cref="System.Instant.Mathset.Formula" />
    /// </summary>
    /// <seealso cref="System.Instant.Mathset.Formula" />
    [Serializable]
    public abstract class LeftFormula : Formula
    {
        #region Methods












        /// <summary>
        /// Compiles the assign.
        /// </summary>
        /// <param name="g">The g.</param>
        /// <param name="cc">The cc.</param>
        /// <param name="post">if set to <c>true</c> [post].</param>
        /// <param name="partial">if set to <c>true</c> [partial].</param>
        public abstract void CompileAssign(ILGenerator g, CompilerContext cc, bool post, bool partial);

        #endregion
    }
}
