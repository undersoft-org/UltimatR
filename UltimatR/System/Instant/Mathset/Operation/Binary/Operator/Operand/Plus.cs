
// <copyright file="Plus.cs" company="UltimatR.Core">
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
    /// Class Plus.
    /// Implements the <see cref="System.Instant.Mathset.BinaryOperator" />
    /// </summary>
    /// <seealso cref="System.Instant.Mathset.BinaryOperator" />
    [Serializable]
    public class Plus : BinaryOperator
    {
        #region Methods







        /// <summary>
        /// Applies the specified a.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns>System.Double.</returns>
        public override double Apply(double a, double b)
        {
            return a + b;
        }





        /// <summary>
        /// Compiles the specified g.
        /// </summary>
        /// <param name="g">The g.</param>
        public override void Compile(ILGenerator g)
        {
            g.Emit(OpCodes.Add);
        }

        #endregion
    }
}
