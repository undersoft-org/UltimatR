
// <copyright file="ValueTypeExtractExtension.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The Extract namespace.
/// </summary>
namespace System.Extract
{
    using System.Runtime.InteropServices;




    /// <summary>
    /// Class ValueTypeExtractExtenstion.
    /// </summary>
    public static class ValueTypeExtractExtenstion
    {
        #region Methods






        /// <summary>
        /// Gets the bytes.
        /// </summary>
        /// <param name="hash">The hash.</param>
        /// <returns>System.Byte[].</returns>
        public unsafe static byte[] GetBytes(this int hash)
        {
            byte[] q = new byte[4];
            fixed (byte* pbyte = q)
                *((int*)pbyte) = hash;
            return q;
        }






        /// <summary>
        /// Gets the bytes.
        /// </summary>
        /// <param name="hash">The hash.</param>
        /// <returns>System.Byte[].</returns>
        public unsafe static byte[] GetBytes(this long hash)
        {
            byte[] q = new byte[8];
            fixed (byte* pbyte = q)
                *((long*)pbyte) = hash;
            return q;
        }






        /// <summary>
        /// Gets the bytes.
        /// </summary>
        /// <param name="hash">The hash.</param>
        /// <returns>System.Byte[].</returns>
        public unsafe static byte[] GetBytes(this uint hash)
        {
            byte[] q = new byte[4];
            fixed (byte* pbyte = q)
                *((uint*)pbyte) = hash;
            return q;
        }






        /// <summary>
        /// Gets the bytes.
        /// </summary>
        /// <param name="hash">The hash.</param>
        /// <returns>System.Byte[].</returns>
        public unsafe static byte[] GetBytes(this ulong hash)
        {
            byte[] q = new byte[8];
            fixed (byte* pbyte = q)
                *((ulong*)pbyte) = hash;
            return q;
        }






        /// <summary>
        /// Gets the bytes.
        /// </summary>
        /// <param name="objvalue">The objvalue.</param>
        /// <returns>Byte[].</returns>
        public unsafe static Byte[] GetBytes(this ValueType objvalue)
        {
            return Extractor.GetStructureBytes(objvalue);
        }






        /// <summary>
        /// Gets the primitive bytes.
        /// </summary>
        /// <param name="objvalue">The objvalue.</param>
        /// <returns>Byte[].</returns>
        public unsafe static Byte[] GetPrimitiveBytes(this object objvalue)
        {
            byte[] b = new byte[Marshal.SizeOf(objvalue)];
            fixed (byte* pb = b)
                objvalue.StructureTo(pb);
            return b;
        }






        /// <summary>
        /// Gets the primitive long.
        /// </summary>
        /// <param name="objvalue">The objvalue.</param>
        /// <returns>System.Int64.</returns>
        public unsafe static long GetPrimitiveLong(this object objvalue)
        {
            byte* ps = stackalloc byte[8];
            Marshal.StructureToPtr(objvalue, (IntPtr)ps, false);
            return *(long*)ps;
        }






        /// <summary>
        /// Structures from.
        /// </summary>
        /// <param name="structure">The structure.</param>
        /// <param name="binary">The binary.</param>
        public unsafe static void StructureFrom(this ValueType structure, byte* binary)
        {
            structure = Extractor.PointerToStructure(binary, structure);
        }







        /// <summary>
        /// Structures from.
        /// </summary>
        /// <param name="structure">The structure.</param>
        /// <param name="binary">The binary.</param>
        /// <param name="offset">The offset.</param>
        public unsafe static void StructureFrom(this ValueType structure, byte[] binary, long offset = 0)
        {
            structure = Extractor.BytesToStructure(binary, ref structure, offset);
        }






        /// <summary>
        /// Structures from.
        /// </summary>
        /// <param name="structure">The structure.</param>
        /// <param name="binary">The binary.</param>
        public unsafe static void StructureFrom(this ValueType structure, IntPtr binary)
        {
            structure = Extractor.PointerToStructure(binary, structure);
        }

        #endregion
    }
}
