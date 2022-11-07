
// <copyright file="MathsetSize.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The Mathset namespace.
/// </summary>
namespace System.Instant.Mathset
{
    using System;




    /// <summary>
    /// Class MathsetSize.
    /// </summary>
    [Serializable]
    public class MathsetSize
    {
        #region Fields

        /// <summary>
        /// The scalar
        /// </summary>
        public static MathsetSize Scalar = new MathsetSize(1, 1);
        /// <summary>
        /// The cols
        /// </summary>
        public int cols;
        /// <summary>
        /// The rows
        /// </summary>
        public int rows;

        #endregion

        #region Constructors






        /// <summary>
        /// Initializes a new instance of the <see cref="MathsetSize" /> class.
        /// </summary>
        /// <param name="i">The i.</param>
        /// <param name="j">The j.</param>
        public MathsetSize(int i, int j)
        {
            rows = i;
            cols = j;
        }

        #endregion

        #region Methods






        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="o">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object o)
        {
            if (o is MathsetSize) return ((MathsetSize)o) == this;
            return false;
        }





        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            return rows * cols;
        }





        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return "" + rows + " " + cols;
        }

        #endregion


        /// <summary>
        /// Implements the != operator.
        /// </summary>
        /// <param name="o1">The o1.</param>
        /// <param name="o2">The o2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(MathsetSize o1, MathsetSize o2)
        {
            return o1.rows != o2.rows || o1.cols != o2.cols;
        }

        /// <summary>
        /// Implements the == operator.
        /// </summary>
        /// <param name="o1">The o1.</param>
        /// <param name="o2">The o2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(MathsetSize o1, MathsetSize o2)
        {
            return o1.rows == o2.rows && o1.cols == o2.cols;
        }
    }
}
