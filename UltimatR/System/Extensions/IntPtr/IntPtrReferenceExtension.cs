
// <copyright file="IntPtrReferenceExtension.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The System namespace.
/// </summary>
namespace System
{
    using System.Runtime.InteropServices;




    /// <summary>
    /// Class IntPtrReferenceExtension.
    /// </summary>
    public static class IntPtrReferenceExtension
    {
        #region Methods






        /// <summary>
        /// Addresses the of.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>System.IntPtr.</returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.PreserveSig)]
        public static unsafe System.IntPtr AddressOf(object obj)
        {
            if (obj == null) return System.IntPtr.Zero;

            System.TypedReference reference = __makeref(obj);

            System.TypedReference* pRef = &reference;

            return (IntPtr)pRef; 
        }







        /// <summary>
        /// Addresses the of.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t">The t.</param>
        /// <returns>System.IntPtr.</returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.PreserveSig)]
        public unsafe static System.IntPtr AddressOf<T>(T t)
        {
            System.TypedReference reference = __makeref(t);

            return *(IntPtr*)(&reference);
        }







        /// <summary>
        /// Addresses the of reference.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t">The t.</param>
        /// <returns>System.IntPtr.</returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.PreserveSig)]
        public static unsafe System.IntPtr AddressOfRef<T>(ref T t)
        {
            TypedReference reference = __makeref(t);

            TypedReference* pRef = &reference;

            return (IntPtr)pRef; 
        }








        /// <summary>
        /// Elements at.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ptr">The PTR.</param>
        /// <param name="index">The index.</param>
        /// <returns>T.</returns>
        public static T ElementAt<T>(this IntPtr ptr, int index)
        {
            var offset = Marshal.SizeOf(typeof(T)) * index;
            var offsetPtr = ptr.Increment(offset);
            return (T)Marshal.PtrToStructure(offsetPtr, typeof(T));
        }






        /// <summary>
        /// Converts to intptr.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>IntPtr.</returns>
        public static IntPtr ToIntPtr(this int value)
        {
            return new IntPtr(value);
        }






        /// <summary>
        /// Converts to intptr.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>IntPtr.</returns>
        public static IntPtr ToIntPtr(this long value)
        {
            unchecked
            {
                if (value > 0 && value <= 0xffffffff)
                    return new IntPtr((int)value);
            }

            return new IntPtr(value);
        }






        /// <summary>
        /// Converts to intptr.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>IntPtr.</returns>
        public static IntPtr ToIntPtr(this uint value)
        {
            unchecked
            {
                return new IntPtr((int)value);
            }
        }






        /// <summary>
        /// Converts to intptr.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>IntPtr.</returns>
        public static IntPtr ToIntPtr(this ulong value)
        {
            unchecked
            {
                return new IntPtr((long)value);
            }
        }






        /// <summary>
        /// Converts to uint32.
        /// </summary>
        /// <param name="pointer">The pointer.</param>
        /// <returns>UInt32.</returns>
        public static unsafe UInt32 ToUInt32(this IntPtr pointer)
        {
            return (UInt32)((void*)pointer);
        }






        /// <summary>
        /// Converts to uint64.
        /// </summary>
        /// <param name="pointer">The pointer.</param>
        /// <returns>UInt64.</returns>
        public static unsafe UInt64 ToUInt64(this IntPtr pointer)
        {
            return (UInt64)((void*)pointer);
        }

        #endregion
    }
}
