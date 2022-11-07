
// <copyright file="Compiler.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The Mathset namespace.
/// </summary>
namespace System.Instant.Mathset
{
    using System;
    using System.Reflection;
    using System.Reflection.Emit;




    /// <summary>
    /// Class Compiler.
    /// </summary>
    public class Compiler
    {
        #region Fields

        /// <summary>
        /// The assembly
        /// </summary>
        internal static AssemblyBuilder ASSEMBLY;
        /// <summary>
        /// The class identifier
        /// </summary>
        internal static int CLASS_ID;
        /// <summary>
        /// The collect mode
        /// </summary>
        internal static bool COLLECT_MODE = false;
        /// <summary>
        /// The executable name
        /// </summary>
        internal static string EXE_NAME = "GENERATED_CODE";
        /// <summary>
        /// The module
        /// </summary>
        internal static ModuleBuilder MODULE;
        /// <summary>
        /// The type prefix
        /// </summary>
        internal static string TYPE_PREFIX = "MATHSET_";

        #endregion

        #region Properties




        /// <summary>
        /// Gets or sets a value indicating whether [collect mode].
        /// </summary>
        /// <value><c>true</c> if [collect mode]; otherwise, <c>false</c>.</value>
        /// <exception cref="System.Logs.ILogSate.Exception">SaveMode cannot be more Changed!</exception>
        public static bool CollectMode
        {
            get { return COLLECT_MODE; }
            set
            {
                if (MODULE == null)
                    COLLECT_MODE = value;
                else
                {
                    throw new Exception("SaveMode cannot be more Changed!");
                }
            }
        }

        #endregion

        #region Methods






        /// <summary>
        /// Compiles the specified formula.
        /// </summary>
        /// <param name="formula">The formula.</param>
        /// <returns>CombinedMathset.</returns>
        public static CombinedMathset Compile(Formula formula)
        {
            if (MODULE == null)
            {
                AssemblyName assemblyName = new AssemblyName();
                assemblyName.Name = "EmittedAssembly";

                ASSEMBLY = AssemblyBuilder.DefineDynamicAssembly(assemblyName, CollectMode ? AssemblyBuilderAccess.RunAndCollect : AssemblyBuilderAccess.Run);
                MODULE = ASSEMBLY.DefineDynamicModule("EmittedModule");
                CLASS_ID = 0;
            }
            CLASS_ID++;

            TypeBuilder MathsetFormula = MODULE.DefineType(TYPE_PREFIX + CLASS_ID, TypeAttributes.Public, typeof(CombinedMathset));
            Type[] constructorArgs = { };
            ConstructorBuilder constructor = MathsetFormula.DefineConstructor(
                MethodAttributes.Public, CallingConventions.Standard, null);

            ILGenerator constructorIL = constructor.GetILGenerator();
            constructorIL.Emit(OpCodes.Ldarg_0);
            ConstructorInfo superConstructor = typeof(Object).GetConstructor(new Type[0]);
            constructorIL.Emit(OpCodes.Call, superConstructor);
            constructorIL.Emit(OpCodes.Ret);

            Type[] args = { };
            MethodBuilder fxMethod = MathsetFormula.DefineMethod("Compute", MethodAttributes.Public | MethodAttributes.Virtual, typeof(void), args);
            ILGenerator methodIL = fxMethod.GetILGenerator();
            CompilerContext context = new CompilerContext();

            
            
            
            
            formula.Compile(methodIL, context);
            context.NextPass();
            context.GenerateLocalInit(methodIL);
            formula.Compile(methodIL, context);

            
            methodIL.Emit(OpCodes.Ret);

            
            Type mxt = MathsetFormula.CreateTypeInfo();
            CombinedMathset computation = (CombinedMathset)Activator.CreateInstance(mxt, new Object[] { });
            computation.SetParams(context.ParamCards, context.Count);

            return computation;
        }

        #endregion
    }
}
