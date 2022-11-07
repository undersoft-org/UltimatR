
// <copyright file="IntPtrExtractExtension.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The Extract namespace.
/// </summary>
namespace System.Extract
{



    /// <summary>
    /// Class IntPtrExtractExtenstion.
    /// </summary>
    public static class IntPtrExtractExtenstion
    {
        #region Methods







        /// <summary>
        /// Copies the block.
        /// </summary>
        /// <param name="src">The source.</param>
        /// <param name="dest">The dest.</param>
        /// <param name="count">The count.</param>
        public static unsafe void CopyBlock(this IntPtr src, IntPtr dest, uint count)
        {
            Extractor.CopyBlock(dest, 0, src, 0, count);
        }








        /// <summary>
        /// Copies the block.
        /// </summary>
        /// <param name="src">The source.</param>
        /// <param name="dest">The dest.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="count">The count.</param>
        public static unsafe void CopyBlock(this IntPtr src, IntPtr dest, uint offset, uint count)
        {
            Extractor.CopyBlock(dest, offset, src, 0, count);
        }







        /// <summary>
        /// Copies the block.
        /// </summary>
        /// <param name="src">The source.</param>
        /// <param name="dest">The dest.</param>
        /// <param name="count">The count.</param>
        public static unsafe void CopyBlock(this IntPtr src, IntPtr dest, ulong count)
        {
            Extractor.CopyBlock(dest, 0, src, 0, count);
        }








        /// <summary>
        /// Copies the block.
        /// </summary>
        /// <param name="src">The source.</param>
        /// <param name="dest">The dest.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="count">The count.</param>
        public static unsafe void CopyBlock(this IntPtr src, IntPtr dest, ulong offset, ulong count)
        {
            Extractor.CopyBlock(dest, offset, src, 0, count);
        }









        /// <summary>
        /// Copies the block.
        /// </summary>
        /// <param name="src">The source.</param>
        /// <param name="srcOffset">The source offset.</param>
        /// <param name="dest">The dest.</param>
        /// <param name="destOffset">The dest offset.</param>
        /// <param name="count">The count.</param>
        public static unsafe void CopyBlock(this IntPtr src, uint srcOffset, IntPtr dest, uint destOffset, uint count)
        {
            Extractor.CopyBlock(dest, destOffset, src, srcOffset, count);
        }









        /// <summary>
        /// Copies the block.
        /// </summary>
        /// <param name="src">The source.</param>
        /// <param name="srcOffset">The source offset.</param>
        /// <param name="dest">The dest.</param>
        /// <param name="destOffset">The dest offset.</param>
        /// <param name="count">The count.</param>
        public static unsafe void CopyBlock(this IntPtr src, ulong srcOffset, IntPtr dest, ulong destOffset, ulong count)
        {
            Extractor.CopyBlock(dest, destOffset, src, srcOffset, count);
        }






        /// <summary>
        /// Froms the structure.
        /// </summary>
        /// <param name="binary">The binary.</param>
        /// <param name="structure">The structure.</param>
        public unsafe static void FromStructure(this IntPtr binary, object structure)
        {
            Extractor.StructureToPointer(structure, binary);
        }








        /// <summary>
        /// Creates new structure.
        /// </summary>
        /// <param name="binary">The binary.</param>
        /// <param name="structure">The structure.</param>
        /// <param name="offset">The offset.</param>
        /// <returns>System.Object.</returns>
        public unsafe static object NewStructure(this IntPtr binary, Type structure, int offset)
        {
            return Extractor.PointerToStructure(binary, structure, offset);
        }







        /// <summary>
        /// Converts to structure.
        /// </summary>
        /// <param name="binary">The binary.</param>
        /// <param name="structure">The structure.</param>
        /// <returns>System.Object.</returns>
        public unsafe static object ToStructure(this IntPtr binary, object structure)
        {
            return Extractor.PointerToStructure(binary, structure);
        }

        #endregion
    }
}
