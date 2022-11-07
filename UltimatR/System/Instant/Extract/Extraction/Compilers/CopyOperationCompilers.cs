
// <copyright file="CopyOperationCompilers.cs" company="UltimatR.Core">
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
        #region Methods





        /// <summary>
        /// Compiles the copy byte array block u int32.
        /// </summary>
        /// <param name="tb">The tb.</param>
        public static void CompileCopyByteArrayBlockUInt32(TypeBuilder tb)
        {
            var copyMethod = tb.DefineMethod("CopyBlock",
                                             MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.Virtual,
                                             typeof(void),
                                             new Type[] { typeof(byte[]), typeof(uint), typeof(byte[]), typeof(uint), typeof(uint) });
            var code = copyMethod.GetILGenerator();

            code.DeclareLocal(typeof(byte[]).MakePointerType(), pinned: true);
            code.DeclareLocal(typeof(byte[]).MakePointerType(), pinned: true);

            
            code.Emit(OpCodes.Ldarg_1);
            code.Emit(OpCodes.Ldarg_2);
            code.Emit(OpCodes.Ldelema, typeof(byte));
            code.Emit(OpCodes.Stloc_0);
            code.Emit(OpCodes.Ldloc_0);
            code.Emit(OpCodes.Ldarg_3);
            code.Emit(OpCodes.Ldarg, 4);
            code.Emit(OpCodes.Ldelema, typeof(byte));
            code.Emit(OpCodes.Stloc_1);
            code.Emit(OpCodes.Ldloc_1);
            code.Emit(OpCodes.Ldarg, 5);
            code.Emit(OpCodes.Conv_U);
            code.Emit(OpCodes.Cpblk);
            code.Emit(OpCodes.Ret);

            tb.DefineMethodOverride(copyMethod, typeof(IExtractor).GetMethod("CopyBlock", new Type[] { typeof(byte[]), typeof(uint), typeof(byte[]), typeof(uint), typeof(uint) }));
        }





        /// <summary>
        /// Compiles the copy byte array block u int64.
        /// </summary>
        /// <param name="tb">The tb.</param>
        public static void CompileCopyByteArrayBlockUInt64(TypeBuilder tb)
        {
            var copyMethod = tb.DefineMethod("CopyBlock",
                                             MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.Virtual,
                                             typeof(void),
                                             new Type[] { typeof(byte[]), typeof(ulong), typeof(byte[]), typeof(ulong), typeof(ulong) });
            var code = copyMethod.GetILGenerator();

            code.DeclareLocal(typeof(byte[]).MakePointerType(), pinned: true);
            code.DeclareLocal(typeof(byte[]).MakePointerType(), pinned: true);

            
            code.Emit(OpCodes.Ldarg_1);
            code.Emit(OpCodes.Ldarg_2);
            code.Emit(OpCodes.Ldelema, typeof(byte));
            code.Emit(OpCodes.Stloc_0);
            code.Emit(OpCodes.Ldloc_0);
            code.Emit(OpCodes.Ldarg_3);
            code.Emit(OpCodes.Ldarg, 4);
            code.Emit(OpCodes.Ldelema, typeof(byte));
            code.Emit(OpCodes.Stloc_1);
            code.Emit(OpCodes.Ldloc_1);
            code.Emit(OpCodes.Ldarg, 5);
            code.Emit(OpCodes.Conv_U);
            code.Emit(OpCodes.Cpblk);
            code.Emit(OpCodes.Ret);

            tb.DefineMethodOverride(copyMethod, typeof(IExtractor).GetMethod("CopyBlock", new Type[] { typeof(byte[]), typeof(ulong), typeof(byte[]), typeof(ulong), typeof(ulong) }));
        }





        /// <summary>
        /// Compiles the copy pointer block u int32.
        /// </summary>
        /// <param name="tb">The tb.</param>
        public static void CompileCopyPointerBlockUInt32(TypeBuilder tb)
        {
            var copyMethod = tb.DefineMethod("CopyBlock",
                                             MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.Virtual,
                                             typeof(void),
                                             new Type[] { typeof(byte*), typeof(uint), typeof(byte*), typeof(uint), typeof(uint) });
            var code = copyMethod.GetILGenerator();

            
            code.Emit(OpCodes.Ldarg_1);
            code.Emit(OpCodes.Ldarg_2);
            code.Emit(OpCodes.Add);
            code.Emit(OpCodes.Ldarg_3);
            code.Emit(OpCodes.Ldarg, 4);
            code.Emit(OpCodes.Add);
            code.Emit(OpCodes.Ldarg, 5);
            code.Emit(OpCodes.Conv_U);
            code.Emit(OpCodes.Cpblk);
            code.Emit(OpCodes.Ret);

            tb.DefineMethodOverride(copyMethod, typeof(IExtractor).GetMethod("CopyBlock", new Type[] { typeof(byte*), typeof(uint), typeof(byte*), typeof(uint), typeof(uint) }));
        }





        /// <summary>
        /// Compiles the copy pointer block u int64.
        /// </summary>
        /// <param name="tb">The tb.</param>
        public static void CompileCopyPointerBlockUInt64(TypeBuilder tb)
        {
            var copyMethod = tb.DefineMethod("CopyBlock",
                                             MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.Virtual,
                                             typeof(void),
                                             new Type[] { typeof(byte*), typeof(ulong), typeof(byte*), typeof(ulong), typeof(ulong) });
            var code = copyMethod.GetILGenerator();

            
            code.Emit(OpCodes.Ldarg_2);
            code.Emit(OpCodes.Ldarg_1);
            code.Emit(OpCodes.Ldarg_3);
            code.Emit(OpCodes.Add);
            code.Emit(OpCodes.Ldarg, 4);
            code.Emit(OpCodes.Conv_U);
            code.Emit(OpCodes.Cpblk);
            code.Emit(OpCodes.Ret);

            tb.DefineMethodOverride(copyMethod, typeof(IExtractor).GetMethod("CopyBlock", new Type[] { typeof(byte*), typeof(ulong), typeof(byte*), typeof(ulong), typeof(ulong) }));
        }

        #endregion
    }
}
