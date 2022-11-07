
// <copyright file="UInt32Extension.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The System namespace.
/// </summary>
namespace System
{



    /// <summary>
    /// Class UInt32Extension.
    /// </summary>
    public static class UInt32Extension
    {
        #region Methods      






        /// <summary>
        /// Determines whether the specified i is even.
        /// </summary>
        /// <param name="i">The i.</param>
        /// <returns><c>true</c> if the specified i is even; otherwise, <c>false</c>.</returns>
        public static bool IsEven(this uint i)
        {
            return !((i & 1) != 0);
        }






        /// <summary>
        /// Determines whether the specified i is odd.
        /// </summary>
        /// <param name="i">The i.</param>
        /// <returns><c>true</c> if the specified i is odd; otherwise, <c>false</c>.</returns>
        public static bool IsOdd(this uint i)
        {
            return ((i & 1) != 0);
        }

        #endregion
    }
}
