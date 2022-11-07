
// <copyright file="BinaryFormula.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The Mathset namespace.
/// </summary>
namespace System.Instant.Mathset
{
    using System;




    /// <summary>
    /// Class BinaryFormula.
    /// Implements the <see cref="System.Instant.Mathset.Formula" />
    /// </summary>
    /// <seealso cref="System.Instant.Mathset.Formula" />
    [Serializable]
    public abstract class BinaryFormula : Formula
    {
        #region Fields

        /// <summary>
        /// The expr1
        /// </summary>
        protected Formula expr1, expr2;

        #endregion

        #region Constructors






        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryFormula" /> class.
        /// </summary>
        /// <param name="e1">The e1.</param>
        /// <param name="e2">The e2.</param>
        public BinaryFormula(Formula e1, Formula e2)
        {
            expr1 = e1;
            expr2 = e2;
        }

        #endregion
    }
}
