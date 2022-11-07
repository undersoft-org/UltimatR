
// <copyright file="TypeExtractExtensions.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The Extract namespace.
/// </summary>
namespace System.Extract
{



    /// <summary>
    /// Class TypeExtractExtenstion.
    /// </summary>
    public static class TypeExtractExtenstion
    {
        #region Methods








        /// <summary>
        /// Creates new structure.
        /// </summary>
        /// <param name="structure">The structure.</param>
        /// <param name="binary">The binary.</param>
        /// <param name="offset">The offset.</param>
        /// <returns>System.Object.</returns>
        public unsafe static object NewStructure(this Type structure, byte* binary, long offset = 0)
        {
            
            
            return Extractor.PointerToStructure(binary, structure, offset);
        }








        /// <summary>
        /// Creates new structure.
        /// </summary>
        /// <param name="structure">The structure.</param>
        /// <param name="binary">The binary.</param>
        /// <param name="offset">The offset.</param>
        /// <returns>System.Object.</returns>
        public static object NewStructure(this Type structure, byte[] binary, long offset = 0)
        {
            

            
            return Extractor.BytesToStructure(binary, structure, offset);
        }

        #endregion
    }
}
