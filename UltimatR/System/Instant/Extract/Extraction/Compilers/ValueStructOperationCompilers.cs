
// <copyright file="ValueStructOperationCompilers.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The Extract namespace.
/// </summary>
namespace System.Extract
{
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Emit;
    using System.Runtime.InteropServices;




    /// <summary>
    /// Class Extraction.
    /// </summary>
    public static partial class Extraction
    {
        #region Methods





        /// <summary>
        /// Compiles the byte array to value object.
        /// </summary>
        /// <param name="tb">The tb.</param>
        public static void CompileByteArrayToValueObject(TypeBuilder tb)
        {
            var ptrToValueStructureMethod = tb.DefineMethod("BytesToValueStructure",
                                             MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.Virtual,
                                             typeof(void),
                                             new Type[] { typeof(byte[]), typeof(object).MakeByRefType(), typeof(ulong) });
            var code = ptrToValueStructureMethod.GetILGenerator();

            
            
            
            

            code.DeclareLocal(typeof(int));             
            code.DeclareLocal(typeof(TypedReference));  
            code.DeclareLocal(typeof(byte*));           
            code.DeclareLocal(typeof(byte[]).MakePointerType(), pinned: true);

            
            code.Emit(OpCodes.Ldarg_2); 
            code.Emit(OpCodes.Ldind_Ref);
            code.EmitCall(OpCodes.Call, typeof(Marshal).GetMethod("SizeOf", new[] { typeof(object) }), null); 
            code.Emit(OpCodes.Stloc_0); 

            
            code.Emit(OpCodes.Ldarg, 2);   
                                           
            code.Emit(OpCodes.Mkrefany, typeof(object)); 
            code.Emit(OpCodes.Stloc_1); 
            code.Emit(OpCodes.Ldloca, 1); 
            code.Emit(OpCodes.Conv_U); 
            code.Emit(OpCodes.Ldind_I); 

            MethodInfo method_IntPtr_op_Explicit = typeof(IntPtr).GetMethods().Where(m => m.Name == "op_Explicit")
                                                                              .Where(m => m.ReturnType == typeof(void*)).FirstOrDefault();
            code.EmitCall(OpCodes.Call, method_IntPtr_op_Explicit, null); 
            code.Emit(OpCodes.Stloc, 2);    

            
            code.Emit(OpCodes.Ldloc_2); 
            code.Emit(OpCodes.Ldarg_1); 
            
            code.Emit(OpCodes.Ldarg_3);
            code.Emit(OpCodes.Ldelema, typeof(byte));
            code.Emit(OpCodes.Stloc_3);
            code.Emit(OpCodes.Ldloc_3);
            code.Emit(OpCodes.Ldloc_0);  
            code.Emit(OpCodes.Conv_U);
            code.Emit(OpCodes.Cpblk);

            code.Emit(OpCodes.Ret); 

            tb.DefineMethodOverride(ptrToValueStructureMethod, typeof(IExtractor).GetMethod("BytesToValueStructure",
                                                                                        new Type[]
                                                                                        {
                                                                                            typeof(byte[]),
                                                                                            typeof(object).MakeByRefType(),
                                                                                            typeof(ulong)
                                                                                        }));
        }





        /// <summary>
        /// Compiles the type of the byte array to value.
        /// </summary>
        /// <param name="tb">The tb.</param>
        public static void CompileByteArrayToValueType(TypeBuilder tb)
        {
            var ptrToValueStructureMethod = tb.DefineMethod("BytesToValueStructure",
                                             MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.Virtual,
                                             typeof(void),
                                             new Type[] { typeof(byte[]), typeof(ValueType).MakeByRefType(), typeof(ulong) });
            var code = ptrToValueStructureMethod.GetILGenerator();

            
            
            
            
            
            

            code.DeclareLocal(typeof(int));             
            code.DeclareLocal(typeof(TypedReference));  
            code.DeclareLocal(typeof(byte*));           
            
            code.DeclareLocal(typeof(byte[]).MakePointerType(), pinned: true);  

            
            code.Emit(OpCodes.Ldarg_2); 
            code.Emit(OpCodes.Ldind_Ref);
            code.EmitCall(OpCodes.Call, typeof(Marshal).GetMethod("SizeOf", new[] { typeof(ValueType) }), null); 
            code.Emit(OpCodes.Stloc_0); 

            
            code.Emit(OpCodes.Ldarg, 2);   
            code.Emit(OpCodes.Mkrefany, typeof(ValueType)); 
            code.Emit(OpCodes.Stloc_1); 
            code.Emit(OpCodes.Ldloca, 1); 
            code.Emit(OpCodes.Conv_U); 
            code.Emit(OpCodes.Ldind_I); 

            MethodInfo method_IntPtr_op_Explicit = typeof(IntPtr).GetMethods().Where(m => m.Name == "op_Explicit").Where(m => m.ReturnType == typeof(void*)).FirstOrDefault();
            code.EmitCall(OpCodes.Call, method_IntPtr_op_Explicit, null); 
            code.Emit(OpCodes.Stloc, 2);    


            

            code.Emit(OpCodes.Ldarg_1); 
            code.Emit(OpCodes.Ldarg_3);  
            code.Emit(OpCodes.Ldelema, typeof(byte)); 
            code.Emit(OpCodes.Stloc, 3); 

            
            
            
            

            
            code.Emit(OpCodes.Ldloc_2); 
            code.Emit(OpCodes.Ldloc_3); 
            code.Emit(OpCodes.Ldloc_0); 
            code.Emit(OpCodes.Conv_U);
            code.Emit(OpCodes.Cpblk);

            
            
            
            
            code.Emit(OpCodes.Ret);

            tb.DefineMethodOverride(ptrToValueStructureMethod, typeof(IExtractor).GetMethod("BytesToValueStructure", new Type[] { typeof(byte[]), typeof(ValueType).MakeByRefType(), typeof(ulong) }));
        }





        /// <summary>
        /// Compiles the pointer to value object.
        /// </summary>
        /// <param name="tb">The tb.</param>
        public static void CompilePointerToValueObject(TypeBuilder tb)
        {
            var ptrToValueStructureMethod = tb.DefineMethod("PointerToValueStructure",
                                             MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.Virtual,
                                             typeof(void),
                                             new Type[] { typeof(byte*), typeof(object).MakeByRefType(), typeof(ulong) });
            var code = ptrToValueStructureMethod.GetILGenerator();

            
            
            
            

            code.DeclareLocal(typeof(int));             
            code.DeclareLocal(typeof(TypedReference));  
            code.DeclareLocal(typeof(byte*));           

            
            code.Emit(OpCodes.Ldarg_2); 
            code.Emit(OpCodes.Ldind_Ref);
            code.EmitCall(OpCodes.Call, typeof(Marshal).GetMethod("SizeOf", new[] { typeof(object) }), null); 
            code.Emit(OpCodes.Stloc_0); 

            
            code.Emit(OpCodes.Ldarg, 2);   
            
            code.Emit(OpCodes.Mkrefany, typeof(object)); 
            code.Emit(OpCodes.Stloc_1); 
            code.Emit(OpCodes.Ldloca, 1); 
            code.Emit(OpCodes.Conv_U); 
            code.Emit(OpCodes.Ldind_I); 

            MethodInfo method_IntPtr_op_Explicit = typeof(IntPtr).GetMethods().Where(m => m.Name == "op_Explicit").Where(m => m.ReturnType == typeof(void*)).FirstOrDefault();
            code.EmitCall(OpCodes.Call, method_IntPtr_op_Explicit, null); 
            code.Emit(OpCodes.Stloc, 2);    

            
            code.Emit(OpCodes.Ldloc_2); 
            code.Emit(OpCodes.Ldarg_1); 
            code.Emit(OpCodes.Ldarg_3);
            code.Emit(OpCodes.Add);
            
            code.Emit(OpCodes.Ldloc_0);  
            code.Emit(OpCodes.Conv_U);
            code.Emit(OpCodes.Cpblk);

            code.Emit(OpCodes.Ret); 

            tb.DefineMethodOverride(ptrToValueStructureMethod, typeof(IExtractor).GetMethod("PointerToValueStructure",
                                                                                          new Type[]
                                                                                          {
                                                                                              typeof(byte*),
                                                                                              typeof(object).MakeByRefType(),
                                                                                              typeof(ulong)
                                                                                          }));
        }





        /// <summary>
        /// Compiles the type of the pointer to value.
        /// </summary>
        /// <param name="tb">The tb.</param>
        public static void CompilePointerToValueType(TypeBuilder tb)
        {
            var ptrToValueStructureMethod = tb.DefineMethod("PointerToValueStructure",
                                             MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.Virtual,
                                             typeof(void),
                                             new Type[] { typeof(byte*), typeof(ValueType).MakeByRefType(), typeof(ulong) });
            var code = ptrToValueStructureMethod.GetILGenerator();

            
            
            
            

            code.DeclareLocal(typeof(int));             
            code.DeclareLocal(typeof(TypedReference));  
            code.DeclareLocal(typeof(byte*));           

            
            code.Emit(OpCodes.Ldarg_2); 
            code.Emit(OpCodes.Ldind_Ref);
            code.EmitCall(OpCodes.Call, typeof(Marshal).GetMethod("SizeOf", new[] { typeof(object) }), null); 
            code.Emit(OpCodes.Stloc_0); 

            
            code.Emit(OpCodes.Ldarg, 2);   
            
            code.Emit(OpCodes.Mkrefany, typeof(object)); 
            code.Emit(OpCodes.Stloc_1); 
            code.Emit(OpCodes.Ldloca, 1); 
            code.Emit(OpCodes.Conv_U); 
            code.Emit(OpCodes.Ldind_I); 

            MethodInfo method_IntPtr_op_Explicit = typeof(IntPtr).GetMethods().Where(m => m.Name == "op_Explicit").Where(m => m.ReturnType == typeof(void*)).FirstOrDefault();
            code.EmitCall(OpCodes.Call, method_IntPtr_op_Explicit, null); 
            code.Emit(OpCodes.Stloc, 2);    

            
            code.Emit(OpCodes.Ldloc_2); 
            code.Emit(OpCodes.Ldarg_1); 
            code.Emit(OpCodes.Ldarg_3);
            code.Emit(OpCodes.Add);
            
            code.Emit(OpCodes.Ldloc_0);  
            code.Emit(OpCodes.Conv_U);
            code.Emit(OpCodes.Cpblk);

            code.Emit(OpCodes.Ret); 

            tb.DefineMethodOverride(ptrToValueStructureMethod, typeof(IExtractor).GetMethod("PointerToValueStructure",
                                                                                          new Type[]
                                                                                          {
                                                                                              typeof(byte*),
                                                                                              typeof(ValueType).MakeByRefType(),
                                                                                              typeof(ulong)
                                                                                          }));
        }





        /// <summary>
        /// Compiles the value object to byte array reference.
        /// </summary>
        /// <param name="tb">The tb.</param>
        public static void CompileValueObjectToByteArrayRef(TypeBuilder tb)
        {
            var structToPtrMethod = tb.DefineMethod("ValueStructureToBytes",
                                             MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.Virtual,
                                             typeof(void),
                                             new Type[] { typeof(object), typeof(byte[]).MakeByRefType(), typeof(ulong) });
            var code = structToPtrMethod.GetILGenerator();


            
            
            
            
            

            code.DeclareLocal(typeof(int));             
            code.DeclareLocal(typeof(TypedReference));  
            code.DeclareLocal(typeof(byte*));           
            
            code.DeclareLocal(typeof(byte[]).MakePointerType(), pinned: true);           


            
            code.Emit(OpCodes.Ldarg_1); 
            
            code.EmitCall(OpCodes.Call, typeof(Marshal).GetMethod("SizeOf", new[] { typeof(object) }), null); 
            code.Emit(OpCodes.Stloc_0); 

            
            code.Emit(OpCodes.Ldarga, 1);   
            code.Emit(OpCodes.Ldind_Ref);
            code.Emit(OpCodes.Mkrefany, typeof(object)); 
            code.Emit(OpCodes.Stloc_1); 
            code.Emit(OpCodes.Ldloca, 1); 
            code.Emit(OpCodes.Conv_U); 
            code.Emit(OpCodes.Ldind_I); 
            

            MethodInfo method_IntPtr_op_Explicit = typeof(IntPtr).GetMethods().Where(m => m.Name == "op_Explicit").Where(m => m.ReturnType == typeof(void*)).FirstOrDefault();
            code.EmitCall(OpCodes.Call, method_IntPtr_op_Explicit, null); 
            code.Emit(OpCodes.Stloc, 2);    

            
            
            
            

            code.Emit(OpCodes.Ldarg_2);
            code.Emit(OpCodes.Ldind_I);
            code.Emit(OpCodes.Ldarg_3);
            code.Emit(OpCodes.Ldelema, typeof(byte));
            code.Emit(OpCodes.Stloc_3);
            code.Emit(OpCodes.Ldloc_3); 
            code.Emit(OpCodes.Ldloc_2); 
            code.Emit(OpCodes.Ldloc_0);  
            code.Emit(OpCodes.Conv_U);
            code.Emit(OpCodes.Cpblk);

            
            
            
            
            
            
            

            code.Emit(OpCodes.Ret); 
            MethodInfo t = typeof(IExtractor).GetMethod("ValueStructureToBytes", new Type[] { typeof(object), typeof(byte[]).MakeByRefType(), typeof(ulong) });
            tb.DefineMethodOverride(structToPtrMethod, typeof(IExtractor).GetMethod("ValueStructureToBytes", new Type[] { typeof(object), typeof(byte[]).MakeByRefType(), typeof(ulong) }));
        }





        /// <summary>
        /// Compiles the value object to new byte array.
        /// </summary>
        /// <param name="tb">The tb.</param>
        public static void CompileValueObjectToNewByteArray(TypeBuilder tb)
        {
            var structToPtrMethod = tb.DefineMethod("ValueStructureToBytes",
                                             MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.Virtual,
                                             typeof(byte[]),
                                             new Type[] { typeof(object) });
            var code = structToPtrMethod.GetILGenerator();


            
            
            
            
            

            code.DeclareLocal(typeof(int));             
            code.DeclareLocal(typeof(TypedReference));  
            code.DeclareLocal(typeof(byte*));           
            code.DeclareLocal(typeof(byte[]));           
            code.DeclareLocal(typeof(byte[]).MakePointerType(), true);           


            
            code.Emit(OpCodes.Ldarg_1); 
                                        
            code.EmitCall(OpCodes.Call, typeof(Marshal).GetMethod("SizeOf", new[] { typeof(object) }), null); 
            code.Emit(OpCodes.Stloc_0); 

            
            code.Emit(OpCodes.Ldarga, 1);   
            code.Emit(OpCodes.Mkrefany, typeof(object)); 
            code.Emit(OpCodes.Stloc_1); 
            code.Emit(OpCodes.Ldloca, 1); 
            code.Emit(OpCodes.Conv_U); 
            code.Emit(OpCodes.Ldind_I); 
            

            MethodInfo method_IntPtr_op_Explicit = typeof(IntPtr).GetMethods().Where(m => m.Name == "op_Explicit").Where(m => m.ReturnType == typeof(void*)).FirstOrDefault();
            code.EmitCall(OpCodes.Call, method_IntPtr_op_Explicit, null); 
            code.Emit(OpCodes.Stloc, 2);    

            
            code.Emit(OpCodes.Ldloc_0); 
            code.Emit(OpCodes.Newarr, typeof(byte));
            code.Emit(OpCodes.Stloc_3);

            
            code.Emit(OpCodes.Ldloc_3); 
            code.Emit(OpCodes.Ldc_I4_0);
            code.Emit(OpCodes.Ldelema, typeof(byte));
            code.Emit(OpCodes.Stloc, 4);
            code.Emit(OpCodes.Ldloc, 4);
            code.Emit(OpCodes.Ldloc_2); 
            code.Emit(OpCodes.Ldloc_0);  
            code.Emit(OpCodes.Conv_U);
            code.Emit(OpCodes.Cpblk);

            
            
            
            
            
            
            

            code.Emit(OpCodes.Ldloc_3); 
            code.Emit(OpCodes.Ret); 
            MethodInfo t = typeof(IExtractor).GetMethod("ValueStructureToBytes", new Type[] { typeof(object) });
            tb.DefineMethodOverride(structToPtrMethod, typeof(IExtractor).GetMethod("ValueStructureToBytes", new Type[] { typeof(object) }));
        }





        /// <summary>
        /// Compiles the value object to new pointer.
        /// </summary>
        /// <param name="tb">The tb.</param>
        public static void CompileValueObjectToNewPointer(TypeBuilder tb)
        {
            var structToPtrMethod = tb.DefineMethod("ValueStructureToPointer",
                                             MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.Virtual,
                                             typeof(byte*),
                                             new Type[] { typeof(object) });
            var code = structToPtrMethod.GetILGenerator();


            
            
            
            
            

            code.DeclareLocal(typeof(int));             
            code.DeclareLocal(typeof(TypedReference));  
            code.DeclareLocal(typeof(byte*));           
            code.DeclareLocal(typeof(byte*));           


            
            code.Emit(OpCodes.Ldarg_1); 
                                        
            code.EmitCall(OpCodes.Call, typeof(Marshal).GetMethod("SizeOf", new[] { typeof(object) }), null); 
            code.Emit(OpCodes.Stloc_0); 

            
            code.Emit(OpCodes.Ldarga, 1);   
            code.Emit(OpCodes.Mkrefany, typeof(object)); 
            code.Emit(OpCodes.Stloc_1); 
            code.Emit(OpCodes.Ldloca, 1); 
            code.Emit(OpCodes.Conv_U); 
            code.Emit(OpCodes.Ldind_I); 
            

            MethodInfo method_IntPtr_op_Explicit = typeof(IntPtr).GetMethods().Where(m => m.Name == "op_Explicit").Where(m => m.ReturnType == typeof(void*)).FirstOrDefault();
            code.EmitCall(OpCodes.Call, method_IntPtr_op_Explicit, null); 
            code.Emit(OpCodes.Stloc, 2);    

            
            code.Emit(OpCodes.Ldloc_0); 
            code.EmitCall(OpCodes.Call, typeof(Marshal).GetMethod("AllocHGlobal", new Type[] { typeof(int) }), null); 
            code.EmitCall(OpCodes.Call, method_IntPtr_op_Explicit, null);   
            code.Emit(OpCodes.Stloc_3);

            
            code.Emit(OpCodes.Ldloc_3); 
            code.Emit(OpCodes.Ldloc_2); 
            code.Emit(OpCodes.Ldloc_0);  
            code.Emit(OpCodes.Conv_U);
            code.Emit(OpCodes.Cpblk);

            
            
            
            
            
            
            

            code.Emit(OpCodes.Ldloc_3);
            code.Emit(OpCodes.Ret); 

            tb.DefineMethodOverride(structToPtrMethod, typeof(IExtractor).GetMethod("ValueStructureToPointer", new Type[] { typeof(object) }));
        }





        /// <summary>
        /// Compiles the value object to pointer.
        /// </summary>
        /// <param name="tb">The tb.</param>
        public static void CompileValueObjectToPointer(TypeBuilder tb)
        {
            var structToPtrMethod = tb.DefineMethod("ValueStructureToPointer",
                                             MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.Virtual,
                                             typeof(void),
                                             new Type[] { typeof(object), typeof(byte*), typeof(ulong) });
            var code = structToPtrMethod.GetILGenerator();


            
            
            
            
            

            code.DeclareLocal(typeof(int));             
            code.DeclareLocal(typeof(TypedReference));  
            code.DeclareLocal(typeof(byte*));           
            


            
            code.Emit(OpCodes.Ldarg_1); 
                                        
            code.EmitCall(OpCodes.Call, typeof(Marshal).GetMethod("SizeOf", new[] { typeof(object) }), null); 
            code.Emit(OpCodes.Stloc_0); 

            
            code.Emit(OpCodes.Ldarga, 1);   
                                            
            code.Emit(OpCodes.Mkrefany, typeof(object)); 
            code.Emit(OpCodes.Stloc_1); 
            code.Emit(OpCodes.Ldloca, 1); 
            code.Emit(OpCodes.Conv_U); 
            code.Emit(OpCodes.Ldind_I); 
            

            MethodInfo method_IntPtr_op_Explicit = typeof(IntPtr).GetMethods().Where(m => m.Name == "op_Explicit").Where(m => m.ReturnType == typeof(void*)).FirstOrDefault();
            code.EmitCall(OpCodes.Call, method_IntPtr_op_Explicit, null); 
            code.Emit(OpCodes.Stloc, 2);    

            
            
            
            
            

            
            code.Emit(OpCodes.Ldarg_2); 
            code.Emit(OpCodes.Ldarg_3);
            code.Emit(OpCodes.Add);
            code.Emit(OpCodes.Ldloc_2); 
            code.Emit(OpCodes.Ldloc_0);  
            code.Emit(OpCodes.Conv_U);
            code.Emit(OpCodes.Cpblk);

            
            
            
            
            
            
            

            code.Emit(OpCodes.Ret); 

            tb.DefineMethodOverride(structToPtrMethod, typeof(IExtractor).GetMethod("ValueStructureToPointer", new Type[] { typeof(object), typeof(byte*), typeof(ulong) }));
        }





        /// <summary>
        /// Compiles the value type to new byte array.
        /// </summary>
        /// <param name="tb">The tb.</param>
        public static void CompileValueTypeToNewByteArray(TypeBuilder tb)
        {
            var structToPtrMethod = tb.DefineMethod("ValueStructureToBytes",
                                             MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.Virtual,
                                             typeof(byte[]),
                                             new Type[] { typeof(ValueType) });
            var code = structToPtrMethod.GetILGenerator();


            
            
            
            
            

            code.DeclareLocal(typeof(int));             
            code.DeclareLocal(typeof(TypedReference));  
            code.DeclareLocal(typeof(byte*));           
            code.DeclareLocal(typeof(byte[]));           
            code.DeclareLocal(typeof(byte[]).MakePointerType(), true);           


            
            code.Emit(OpCodes.Ldarg_1); 
                                        
            code.EmitCall(OpCodes.Call, typeof(Marshal).GetMethod("SizeOf", new[] { typeof(object) }), null); 
            code.Emit(OpCodes.Stloc_0); 

            
            code.Emit(OpCodes.Ldarga, 1);   
            code.Emit(OpCodes.Mkrefany, typeof(object)); 
            code.Emit(OpCodes.Stloc_1); 
            code.Emit(OpCodes.Ldloca, 1); 
            code.Emit(OpCodes.Conv_U); 
            code.Emit(OpCodes.Ldind_I); 
            

            MethodInfo method_IntPtr_op_Explicit = typeof(IntPtr).GetMethods().Where(m => m.Name == "op_Explicit").Where(m => m.ReturnType == typeof(void*)).FirstOrDefault();
            code.EmitCall(OpCodes.Call, method_IntPtr_op_Explicit, null); 
            code.Emit(OpCodes.Stloc, 2);    

            
            code.Emit(OpCodes.Ldloc_0); 
            code.Emit(OpCodes.Newarr, typeof(byte));
            code.Emit(OpCodes.Stloc_3);

            
            code.Emit(OpCodes.Ldloc_3); 
            code.Emit(OpCodes.Ldc_I4_0);
            code.Emit(OpCodes.Ldelema, typeof(byte));
            code.Emit(OpCodes.Stloc, 4);
            code.Emit(OpCodes.Ldloc, 4);
            code.Emit(OpCodes.Ldloc_2); 
            code.Emit(OpCodes.Ldloc_0);  
            code.Emit(OpCodes.Conv_U);
            code.Emit(OpCodes.Cpblk);

            
            
            
            
            
            
            

            code.Emit(OpCodes.Ldc_I4_0);
            code.Emit(OpCodes.Conv_U);
            code.Emit(OpCodes.Stloc, 4);

            code.Emit(OpCodes.Ldloc_3); 
            code.Emit(OpCodes.Ret); 

            tb.DefineMethodOverride(structToPtrMethod, typeof(IExtractor).GetMethod("ValueStructureToBytes", new Type[] { typeof(ValueType) }));
        }

        #endregion
    }
}
