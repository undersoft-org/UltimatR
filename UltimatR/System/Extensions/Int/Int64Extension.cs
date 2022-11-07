
// <copyright file="Int64Extension.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The System namespace.
/// </summary>
namespace System
{



    /// <summary>
    /// Class Int64Extension.
    /// </summary>
    public static class Int64Extension
    {
        #region Methods






        /// <summary>
        /// Determines whether the specified i is even.
        /// </summary>
        /// <param name="i">The i.</param>
        /// <returns><c>true</c> if the specified i is even; otherwise, <c>false</c>.</returns>
        public static bool IsEven(this long i)
        {
            return !((i & 1L) != 0);
        }






        /// <summary>
        /// Determines whether the specified i is odd.
        /// </summary>
        /// <param name="i">The i.</param>
        /// <returns><c>true</c> if the specified i is odd; otherwise, <c>false</c>.</returns>
        public static bool IsOdd(this long i)
        {
            return ((i & 1) != 0);
        }






        /// <summary>
        /// Removes the sign.
        /// </summary>
        /// <param name="i">The i.</param>
        /// <returns>System.Int64.</returns>
        public static long RemoveSign(this long i)
        {
            return (long)(((ulong)i << 1) >> 1);
        }

        #endregion
    }
}
