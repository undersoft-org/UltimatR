
// <copyright file="IExtractor.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The Extract namespace.
/// </summary>
namespace System.Extract
{



    /// <summary>
    /// Interface IExtractor
    /// </summary>
    public interface IExtractor
    {
        #region Methods







        /// <summary>
        /// Byteses to value structure.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="structure">The structure.</param>
        /// <param name="offset">The offset.</param>
        unsafe void BytesToValueStructure(byte[] array, ref ValueType structure, ulong offset);







        /// <summary>
        /// Byteses to value structure.
        /// </summary>
        /// <param name="ptr">The PTR.</param>
        /// <param name="structure">The structure.</param>
        /// <param name="offset">The offset.</param>
        unsafe void BytesToValueStructure(byte[] ptr, ref object structure, ulong offset);









        /// <summary>
        /// Copies the block.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="srcOffset">The source offset.</param>
        /// <param name="dest">The dest.</param>
        /// <param name="destOffset">The dest offset.</param>
        /// <param name="count">The count.</param>
        unsafe void CopyBlock(byte* source, uint srcOffset, byte* dest, uint destOffset, uint count);









        /// <summary>
        /// Copies the block.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="srcOffset">The source offset.</param>
        /// <param name="dest">The dest.</param>
        /// <param name="destOffset">The dest offset.</param>
        /// <param name="count">The count.</param>
        unsafe void CopyBlock(byte* source, ulong srcOffset, byte* dest, ulong destOffset, ulong count);









        /// <summary>
        /// Copies the block.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="srcOffset">The source offset.</param>
        /// <param name="dest">The dest.</param>
        /// <param name="destOffset">The dest offset.</param>
        /// <param name="count">The count.</param>
        void CopyBlock(byte[] source, uint srcOffset, byte[] dest, uint destOffset, uint count);









        /// <summary>
        /// Copies the block.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="srcOffset">The source offset.</param>
        /// <param name="dest">The dest.</param>
        /// <param name="destOffset">The dest offset.</param>
        /// <param name="count">The count.</param>
        void CopyBlock(byte[] source, ulong srcOffset, byte[] dest, ulong destOffset, ulong count);







        /// <summary>
        /// Pointers to value structure.
        /// </summary>
        /// <param name="ptr">The PTR.</param>
        /// <param name="structure">The structure.</param>
        /// <param name="offset">The offset.</param>
        unsafe void PointerToValueStructure(byte* ptr, ref object structure, ulong offset);







        /// <summary>
        /// Pointers to value structure.
        /// </summary>
        /// <param name="ptr">The PTR.</param>
        /// <param name="structure">The structure.</param>
        /// <param name="offset">The offset.</param>
        unsafe void PointerToValueStructure(byte* ptr, ref ValueType structure, ulong offset);






        /// <summary>
        /// Values the structure to bytes.
        /// </summary>
        /// <param name="structure">The structure.</param>
        /// <returns>System.Byte[].</returns>
        byte[] ValueStructureToBytes(object structure);







        /// <summary>
        /// Values the structure to bytes.
        /// </summary>
        /// <param name="structure">The structure.</param>
        /// <param name="ptr">The PTR.</param>
        /// <param name="offset">The offset.</param>
        void ValueStructureToBytes(object structure, ref byte[] ptr, ulong offset);






        /// <summary>
        /// Values the structure to bytes.
        /// </summary>
        /// <param name="structure">The structure.</param>
        /// <returns>System.Byte[].</returns>
        byte[] ValueStructureToBytes(ValueType structure);






        /// <summary>
        /// Values the structure to pointer.
        /// </summary>
        /// <param name="structure">The structure.</param>
        /// <returns>System.Byte.</returns>
        unsafe byte* ValueStructureToPointer(object structure);







        /// <summary>
        /// Values the structure to pointer.
        /// </summary>
        /// <param name="structure">The structure.</param>
        /// <param name="ptr">The PTR.</param>
        /// <param name="offset">The offset.</param>
        unsafe void ValueStructureToPointer(object structure, byte* ptr, ulong offset);

        #endregion
    }
}
