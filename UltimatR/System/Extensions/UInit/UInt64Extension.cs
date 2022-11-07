
// <copyright file="UInt64Extension.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The System namespace.
/// </summary>
namespace System
{



    /// <summary>
    /// Class UInt64Extension.
    /// </summary>
    public static class UInt64Extension
    {
        #region Methods






        /// <summary>
        /// Determines whether the specified i is even.
        /// </summary>
        /// <param name="i">The i.</param>
        /// <returns><c>true</c> if the specified i is even; otherwise, <c>false</c>.</returns>
        public static bool IsEven(this ulong i)
        {
            return !((i & 1UL) != 0);
        }






        /// <summary>
        /// Determines whether the specified i is odd.
        /// </summary>
        /// <param name="i">The i.</param>
        /// <returns><c>true</c> if the specified i is odd; otherwise, <c>false</c>.</returns>
        public static bool IsOdd(this ulong i)
        {
            return ((i & 1UL) != 0);
        }

        #endregion
    }
}
