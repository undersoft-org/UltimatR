
// <copyright file="ByteArrayExtractExtension.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The Extract namespace.
/// </summary>
namespace System.Extract
{



    /// <summary>
    /// Class ByteArrayExtractExtenstion.
    /// </summary>
    public static class ByteArrayExtractExtenstion
    {
        #region Methods







        /// <summary>
        /// Blocks the equal.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="dest">The dest.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public unsafe static bool BlockEqual(this byte[] source, byte[] dest)
        {
            long sl = source.LongLength;
            if (sl > dest.LongLength)
                return false;
            long sl64 = sl / 8;
            long sl8 = sl % 8;
            fixed (byte* psrc = source, pdst = dest)
            {
                long* lsrc = (long*)psrc, ldst = (long*)pdst;
                for (int i = 0; i < sl64; i++)
                    if (*(&lsrc[i]) != (*(&ldst[i])))
                        return false;
                byte* psrc8 = psrc + (sl64 * 8), pdst8 = pdst + (sl64 * 8);
                for (int i = 0; i < sl8; i++)
                    if (*(&psrc8[i]) != (*(&pdst8[i])))
                        return false;
                return true;
            }
        }










        /// <summary>
        /// Blocks the equal.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="srcOffset">The source offset.</param>
        /// <param name="dest">The dest.</param>
        /// <param name="destOffset">The dest offset.</param>
        /// <param name="count">The count.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public unsafe static bool BlockEqual(this IntPtr source, long srcOffset, IntPtr dest, long destOffset, long count)
        {
            long sl = count;
            long sl64 = sl / 8;
            long sl8 = sl % 8;
            long* lsrc = (long*)((byte*)source + srcOffset), ldst = (long*)((byte*)dest + destOffset);
            for (int i = 0; i < sl64; i++)
                if (*(&lsrc[i]) != (*(&ldst[i])))
                    return false;
            byte* psrc8 = (byte*)source + (sl64 * 8), pdst8 = (byte*)dest + (sl64 * 8);
            for (int i = 0; i < sl8; i++)
                if (*(&psrc8[i]) != (*(&pdst8[i])))
                    return false;
            return true;
        }








        /// <summary>
        /// Copies the block.
        /// </summary>
        /// <param name="src">The source.</param>
        /// <param name="dest">The dest.</param>
        /// <param name="count">The count.</param>
        public static unsafe void CopyBlock(this byte[] src, byte[] dest, uint count)
        {
            Extractor.CopyBlock(src, dest, 0, count);
        }








        /// <summary>
        /// Copies the block.
        /// </summary>
        /// <param name="src">The source.</param>
        /// <param name="dest">The dest.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="count">The count.</param>
        public static unsafe void CopyBlock(this byte[] src, byte[] dest, uint offset, uint count)
        {
            Extractor.CopyBlock(src, dest, offset, count);
        }







        /// <summary>
        /// Copies the block.
        /// </summary>
        /// <param name="src">The source.</param>
        /// <param name="dest">The dest.</param>
        /// <param name="count">The count.</param>
        public static unsafe void CopyBlock(this byte[] src, byte[] dest, ulong count)
        {
            Extractor.CopyBlock(src, dest, 0, count);
        }








        /// <summary>
        /// Copies the block.
        /// </summary>
        /// <param name="src">The source.</param>
        /// <param name="dest">The dest.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="count">The count.</param>
        public static unsafe void CopyBlock(this byte[] src, byte[] dest, ulong offset, ulong count)
        {
            Extractor.CopyBlock(src, dest, offset, count);
        }








        /// <summary>
        /// Creates new structure.
        /// </summary>
        /// <param name="binary">The binary.</param>
        /// <param name="structure">The structure.</param>
        /// <param name="offset">The offset.</param>
        /// <returns>System.Object.</returns>
        public static object NewStructure(this byte[] binary, Type structure, long offset = 0)
        {
            return Extractor.BytesToStructure(binary, structure, offset);
        }






        /// <summary>
        /// Converts to int32.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="offset">The offset.</param>
        /// <returns>System.Int32.</returns>
        public unsafe static int ToInt32(this Byte[] bytes, int offset = 0)
        {
            int v = 0;
            uint l = (uint)(bytes.Length  - offset);
            fixed (byte* pbyte = &bytes[offset])
            {
                if (l < 4)
                {
                    byte* a = stackalloc byte[4];
                    Extractor.CopyBlock(a, pbyte, l);
                    v = *((int*)a);

                }
                v = *((int*)pbyte);
            }
            return v;
        }






        /// <summary>
        /// Converts to int64.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="offset">The offset.</param>
        /// <returns>System.Int64.</returns>
        public unsafe static long ToInt64(this Byte[] bytes, int offset = 0)
        {
            long v = 0;
            uint l = (uint)(bytes.Length - offset);
            fixed (byte* pbyte = &bytes[offset])
            {
                if (l < 8)
                {
                    byte* a = stackalloc byte[8];
                    Extractor.CopyBlock(a, pbyte, l);
                    v = *((long*)a);

                }
                v = *((long*)pbyte);
            }
            return v;
        }








        /// <summary>
        /// Converts to structure.
        /// </summary>
        /// <param name="binary">The binary.</param>
        /// <param name="structure">The structure.</param>
        /// <param name="offset">The offset.</param>
        /// <returns>System.Object.</returns>
        public unsafe static object ToStructure(this byte[] binary, object structure, long offset = 0)
        {
            return Extractor.BytesToStructure(binary, structure, offset);
        }








        /// <summary>
        /// Converts to structure.
        /// </summary>
        /// <param name="binary">The binary.</param>
        /// <param name="structure">The structure.</param>
        /// <param name="offset">The offset.</param>
        /// <returns>ValueType.</returns>
        public unsafe static ValueType ToStructure(this byte[] binary, ValueType structure, long offset = 0)
        {
            return Extractor.BytesToStructure(binary, ref structure, offset);
        }






        /// <summary>
        /// Converts to uint32.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="offset">The offset.</param>
        /// <returns>System.UInt32.</returns>
        public unsafe static uint ToUInt32(this Byte[] bytes, int offset = 0)
        {
            uint v = 0;
            uint l = (uint)(bytes.Length - offset);
            fixed (byte* pbyte = &bytes[offset])
            {
                if (l < 4)
                {
                    byte* a = stackalloc byte[4];
                    Extractor.CopyBlock(a, pbyte, l);
                    v = *((uint*)a);

                }
                v = *((uint*)pbyte);
            }
            return v;
        }






        /// <summary>
        /// Converts to uint64.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="offset">The offset.</param>
        /// <returns>System.UInt64.</returns>
        public unsafe static ulong ToUInt64(this Byte[] bytes, int offset = 0)
        {
            ulong v = 0;
            uint l = (uint)(bytes.Length - offset);
            fixed (byte* pbyte = &bytes[offset])
            {
                if (l < 8)
                {
                    byte* a = stackalloc byte[8];
                    Extractor.CopyBlock(a, pbyte, l);
                    v = *((ulong*)a);

                }
                v = *((ulong*)pbyte);
            }
            return v;
        }

        #endregion
    }
}
