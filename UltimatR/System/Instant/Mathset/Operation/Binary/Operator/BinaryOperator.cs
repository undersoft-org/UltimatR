
// <copyright file="BinaryOperator.cs" company="UltimatR.Core">
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
    /// Class BinaryOperator.
    /// </summary>
    [Serializable]
    public abstract class BinaryOperator
    {
        #region Methods







        /// <summary>
        /// Applies the specified a.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns>System.Double.</returns>
        public abstract double Apply(double a, double b);





        /// <summary>
        /// Compiles the specified g.
        /// </summary>
        /// <param name="g">The g.</param>
        public abstract void Compile(ILGenerator g);

        #endregion
    }
}
