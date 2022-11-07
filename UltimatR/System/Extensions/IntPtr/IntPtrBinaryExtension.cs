
// <copyright file="IntPtrBinaryExtension.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The System namespace.
/// </summary>
namespace System
{



    /// <summary>
    /// Class IntPtrBinaryExtension.
    /// </summary>
    public static class IntPtrBinaryExtension
    {
        #region Methods







        /// <summary>
        /// Ands the specified value.
        /// </summary>
        /// <param name="pointer">The pointer.</param>
        /// <param name="value">The value.</param>
        /// <returns>IntPtr.</returns>
        public static IntPtr And(this IntPtr pointer, IntPtr value)
        {
            switch (IntPtr.Size)
            {
                case sizeof(Int32):
                    return (new IntPtr(pointer.ToInt32() & value.ToInt32()));

                default:
                    return (new IntPtr(pointer.ToInt64() & value.ToInt64()));
            }
        }






        /// <summary>
        /// Nots the specified pointer.
        /// </summary>
        /// <param name="pointer">The pointer.</param>
        /// <returns>IntPtr.</returns>
        public static IntPtr Not(this IntPtr pointer)
        {
            switch (IntPtr.Size)
            {
                case sizeof(Int32):
                    return (new IntPtr(~pointer.ToInt32()));

                default:
                    return (new IntPtr(~pointer.ToInt64()));
            }
        }







        /// <summary>
        /// Ors the specified value.
        /// </summary>
        /// <param name="pointer">The pointer.</param>
        /// <param name="value">The value.</param>
        /// <returns>IntPtr.</returns>
        public static IntPtr Or(this IntPtr pointer, IntPtr value)
        {
            switch (IntPtr.Size)
            {
                case sizeof(Int32):
                    return (new IntPtr(pointer.ToInt32() | value.ToInt32()));

                default:
                    return (new IntPtr(pointer.ToInt64() | value.ToInt64()));
            }
        }







        /// <summary>
        /// Xors the specified value.
        /// </summary>
        /// <param name="pointer">The pointer.</param>
        /// <param name="value">The value.</param>
        /// <returns>IntPtr.</returns>
        public static IntPtr Xor(this IntPtr pointer, IntPtr value)
        {
            switch (IntPtr.Size)
            {
                case sizeof(Int32):
                    return (new IntPtr(pointer.ToInt32() ^ value.ToInt32()));

                default:
                    return (new IntPtr(pointer.ToInt64() ^ value.ToInt64()));
            }
        }

        #endregion
    }
}
