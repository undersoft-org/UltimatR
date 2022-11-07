
// <copyright file="IntPtrEqualityExtension.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The System namespace.
/// </summary>
namespace System
{



    /// <summary>
    /// Class IntPtrEqualityExtension.
    /// </summary>
    public static class IntPtrEqualityExtension
    {
        #region Methods







        /// <summary>
        /// Equalses the specified PTR2.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="ptr2">The PTR2.</param>
        /// <returns>Boolean.</returns>
        public static Boolean Equals(this IntPtr left, IntPtr ptr2)
        {
            return (left == ptr2);
        }







        /// <summary>
        /// Equalses the specified value.
        /// </summary>
        /// <param name="pointer">The pointer.</param>
        /// <param name="value">The value.</param>
        /// <returns>Boolean.</returns>
        public static Boolean Equals(this IntPtr pointer, Int32 value)
        {
            return (pointer.ToInt32() == value);
        }







        /// <summary>
        /// Equalses the specified value.
        /// </summary>
        /// <param name="pointer">The pointer.</param>
        /// <param name="value">The value.</param>
        /// <returns>Boolean.</returns>
        public static Boolean Equals(this IntPtr pointer, Int64 value)
        {
            return (pointer.ToInt64() == value);
        }







        /// <summary>
        /// Equalses the specified value.
        /// </summary>
        /// <param name="pointer">The pointer.</param>
        /// <param name="value">The value.</param>
        /// <returns>Boolean.</returns>
        public static Boolean Equals(this IntPtr pointer, UInt32 value)
        {
            return (pointer.ToUInt32() == value);
        }







        /// <summary>
        /// Equalses the specified value.
        /// </summary>
        /// <param name="pointer">The pointer.</param>
        /// <param name="value">The value.</param>
        /// <returns>Boolean.</returns>
        public static Boolean Equals(this IntPtr pointer, UInt64 value)
        {
            return (pointer.ToUInt64() == value);
        }







        /// <summary>
        /// Equalses the specified right.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>Boolean.</returns>
        public static Boolean Equals(this IntPtr[] left, IntPtr[] right)
        {
            int length = left.Length;
            for (int i = 0; i < length; i++)
                if (!left[i].Equals(right[i]))
                    return false;
            return true;
        }







        /// <summary>
        /// Determines whether [is greater than or equal to] [the specified right].
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>Boolean.</returns>
        public static Boolean isGreaterThanOrEqualTo(this IntPtr left, IntPtr right)
        {
            return (left.CompareTo(right) >= 0);
        }







        /// <summary>
        /// Determines whether [is less than or equal to] [the specified right].
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>Boolean.</returns>
        public static Boolean IsLessThanOrEqualTo(this IntPtr left, IntPtr right)
        {
            return (left.CompareTo(right) <= 0);
        }

        #endregion
    }
}
