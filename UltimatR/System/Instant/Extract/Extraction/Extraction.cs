
// <copyright file="Extraction.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The Extract namespace.
/// </summary>
namespace System.Extract
{
    using System.Reflection;
    using System.Reflection.Emit;




    /// <summary>
    /// Class Extraction.
    /// </summary>
    public static partial class Extraction
    {
        #region Fields

        /// <summary>
        /// The extract
        /// </summary>
        internal static readonly IExtractor _extract;
        /// <summary>
        /// The asm builder
        /// </summary>
        private static AssemblyBuilder _asmBuilder;
        /// <summary>
        /// The asm name
        /// </summary>
        private static AssemblyName _asmName = new AssemblyName() { Name = "Extract" };
        /// <summary>
        /// The mod builder
        /// </summary>
        private static ModuleBuilder _modBuilder;

        #endregion

        #region Constructors




        /// <summary>
        /// Initializes static members of the <see cref="Extraction" /> class.
        /// </summary>
        static Extraction()
        {
            _asmBuilder = AssemblyBuilder.DefineDynamicAssembly(_asmName, AssemblyBuilderAccess.RunAndCollect);
            _modBuilder = _asmBuilder.DefineDynamicModule(_asmName.Name + ".dll");

            var typeBuilder = _modBuilder.DefineType("Extract",
                       TypeAttributes.Public
                       | TypeAttributes.AutoClass
                       | TypeAttributes.AnsiClass
                       | TypeAttributes.Class
                       | TypeAttributes.Serializable
                       | TypeAttributes.BeforeFieldInit);

            typeBuilder.AddInterfaceImplementation(typeof(IExtractor));

            CompileCopyByteArrayBlockUInt32(typeBuilder);
            CompileCopyPointerBlockUInt32(typeBuilder);

            CompileCopyByteArrayBlockUInt64(typeBuilder);
            CompileCopyPointerBlockUInt64(typeBuilder);

            CompileValueObjectToPointer(typeBuilder);
            CompileValueObjectToByteArrayRef(typeBuilder);

            CompileValueObjectToNewByteArray(typeBuilder);
            CompileValueTypeToNewByteArray(typeBuilder);
            CompileValueObjectToNewPointer(typeBuilder);

            CompilePointerToValueObject(typeBuilder);
            CompilePointerToValueType(typeBuilder);

            CompileByteArrayToValueObject(typeBuilder);
            CompileByteArrayToValueType(typeBuilder);

            TypeInfo _extractType = typeBuilder.CreateTypeInfo();
            _extract = (IExtractor)Activator.CreateInstance(_extractType);
        }

        #endregion

        #region Methods








        /// <summary>
        /// Byteses to value structure.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="structure">The structure.</param>
        /// <param name="offset">The offset.</param>
        /// <returns>ValueType.</returns>
        public unsafe static ValueType BytesToValueStructure(byte[] array, ValueType structure, ulong offset)
        {
            _extract.BytesToValueStructure(array, ref structure, offset);
            return structure;
        }








        /// <summary>
        /// Byteses to value structure.
        /// </summary>
        /// <param name="ptr">The PTR.</param>
        /// <param name="structure">The structure.</param>
        /// <param name="offset">The offset.</param>
        /// <returns>System.Object.</returns>
        public unsafe static object BytesToValueStructure(byte[] ptr, object structure, ulong offset)
        {
            _extract.BytesToValueStructure(ptr, ref structure, offset);
            return structure;
        }









        /// <summary>
        /// Copies the block.
        /// </summary>
        /// <param name="dest">The dest.</param>
        /// <param name="destOffset">The dest offset.</param>
        /// <param name="src">The source.</param>
        /// <param name="srcOffset">The source offset.</param>
        /// <param name="count">The count.</param>
        public static unsafe void CopyBlock(byte* dest, uint destOffset, byte* src, uint srcOffset, uint count)
        {
            _extract.CopyBlock(dest, destOffset, src, srcOffset, count);
        }









        /// <summary>
        /// Copies the block.
        /// </summary>
        /// <param name="dest">The dest.</param>
        /// <param name="destOffset">The dest offset.</param>
        /// <param name="src">The source.</param>
        /// <param name="srcOffset">The source offset.</param>
        /// <param name="count">The count.</param>
        public static unsafe void CopyBlock(byte* dest, ulong destOffset, byte* src, ulong srcOffset, ulong count)
        {
            _extract.CopyBlock(dest, destOffset, src, srcOffset, count);
        }









        /// <summary>
        /// Copies the block.
        /// </summary>
        /// <param name="dest">The dest.</param>
        /// <param name="destOffset">The dest offset.</param>
        /// <param name="src">The source.</param>
        /// <param name="srcOffset">The source offset.</param>
        /// <param name="count">The count.</param>
        public static unsafe void CopyBlock(byte[] dest, uint destOffset, byte[] src, uint srcOffset, uint count)
        {
            _extract.CopyBlock(dest, destOffset, src, srcOffset, count);
        }









        /// <summary>
        /// Copies the block.
        /// </summary>
        /// <param name="dest">The dest.</param>
        /// <param name="destOffset">The dest offset.</param>
        /// <param name="src">The source.</param>
        /// <param name="srcOffset">The source offset.</param>
        /// <param name="count">The count.</param>
        public static unsafe void CopyBlock(byte[] dest, ulong destOffset, byte[] src, ulong srcOffset, ulong count)
        {
            _extract.CopyBlock(dest, destOffset, src, srcOffset, count);
        }





        /// <summary>
        /// Gets the extractor.
        /// </summary>
        /// <returns>IExtractor.</returns>
        public static IExtractor GetExtractor()
        {
            return _extract;
        }








        /// <summary>
        /// Pointers to value structure.
        /// </summary>
        /// <param name="ptr">The PTR.</param>
        /// <param name="structure">The structure.</param>
        /// <param name="offset">The offset.</param>
        /// <returns>System.Object.</returns>
        public unsafe static object PointerToValueStructure(byte* ptr, object structure, ulong offset)
        {
            _extract.PointerToValueStructure(ptr, ref structure, offset);
            return structure;
        }








        /// <summary>
        /// Pointers to value structure.
        /// </summary>
        /// <param name="ptr">The PTR.</param>
        /// <param name="structure">The structure.</param>
        /// <param name="offset">The offset.</param>
        /// <returns>ValueType.</returns>
        public unsafe static ValueType PointerToValueStructure(byte* ptr, ValueType structure, ulong offset)
        {
            _extract.PointerToValueStructure(ptr, ref structure, offset);
            return structure;
        }








        /// <summary>
        /// Pointers to value structure.
        /// </summary>
        /// <param name="ptr">The PTR.</param>
        /// <param name="structure">The structure.</param>
        /// <param name="offset">The offset.</param>
        /// <returns>System.Object.</returns>
        public unsafe static object PointerToValueStructure(IntPtr ptr, object structure, ulong offset)
        {
            _extract.PointerToValueStructure((byte*)ptr.ToPointer(), ref structure, offset);
            return structure;
        }








        /// <summary>
        /// Pointers to value structure.
        /// </summary>
        /// <param name="ptr">The PTR.</param>
        /// <param name="structure">The structure.</param>
        /// <param name="offset">The offset.</param>
        /// <returns>ValueType.</returns>
        public unsafe static ValueType PointerToValueStructure(IntPtr ptr, ValueType structure, ulong offset)
        {
            _extract.PointerToValueStructure((byte*)ptr.ToPointer(), ref structure, offset);
            return structure;
        }






        /// <summary>
        /// Values the structure to bytes.
        /// </summary>
        /// <param name="structure">The structure.</param>
        /// <returns>System.Byte[].</returns>
        public static byte[] ValueStructureToBytes(object structure)
        {
            return _extract.ValueStructureToBytes(structure);
        }







        /// <summary>
        /// Values the structure to bytes.
        /// </summary>
        /// <param name="structure">The structure.</param>
        /// <param name="ptr">The PTR.</param>
        /// <param name="offset">The offset.</param>
        public unsafe static void ValueStructureToBytes(object structure, ref byte[] ptr, ulong offset)
        {
            _extract.ValueStructureToBytes(structure, ref ptr, offset);
        }






        /// <summary>
        /// Values the structure to bytes.
        /// </summary>
        /// <param name="structure">The structure.</param>
        /// <returns>System.Byte[].</returns>
        public static byte[] ValueStructureToBytes(ValueType structure)
        {
            return _extract.ValueStructureToBytes(structure);
        }






        /// <summary>
        /// Values the structure to int PTR.
        /// </summary>
        /// <param name="structure">The structure.</param>
        /// <returns>IntPtr.</returns>
        public static unsafe IntPtr ValueStructureToIntPtr(object structure)
        {
            return new IntPtr(_extract.ValueStructureToPointer(structure));
        }






        /// <summary>
        /// Values the structure to pointer.
        /// </summary>
        /// <param name="structure">The structure.</param>
        /// <returns>System.Byte.</returns>
        public static unsafe byte* ValueStructureToPointer(object structure)
        {
            return _extract.ValueStructureToPointer(structure);
        }







        /// <summary>
        /// Values the structure to pointer.
        /// </summary>
        /// <param name="structure">The structure.</param>
        /// <param name="ptr">The PTR.</param>
        /// <param name="offset">The offset.</param>
        public unsafe static void ValueStructureToPointer(object structure, byte* ptr, ulong offset)
        {
            _extract.ValueStructureToPointer(structure, ptr, offset);
        }

        #endregion
    }
}
