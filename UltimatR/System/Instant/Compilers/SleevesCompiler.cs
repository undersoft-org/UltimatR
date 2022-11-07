﻿
// <copyright file="SleevesCompiler.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The Instant namespace.
/// </summary>
namespace System.Instant
{
    using System.Collections.Generic;
    using System.Reflection;
    using System.Reflection.Emit;
    using System.Runtime.InteropServices;
    using System.Runtime.Serialization;

    /// <summary>
    /// Class SleevesCompiler.
    /// </summary>
    public class SleevesCompiler
    {
        #region Fields

        /// <summary>
        /// The marshal as ctor
        /// </summary>
        private readonly ConstructorInfo
            marshalAsCtor = typeof(MarshalAsAttribute)
            .GetConstructor(new Type[] { typeof(UnmanagedType) });
        /// <summary>
        /// The multemic field
        /// </summary>
        private FieldBuilder multemicField = null;
        /// <summary>
        /// The selective field
        /// </summary>
        private FieldBuilder selectiveField = null;
        /// <summary>
        /// The sleeve
        /// </summary>
        private Sleeves sleeve;
        /// <summary>
        /// The sleeve type
        /// </summary>
        private Type SleeveType = typeof(FigureSleeves);

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SleevesCompiler" /> class.
        /// </summary>
        /// <param name="instantSleeve">The instant sleeve.</param>
        public SleevesCompiler(Sleeves instantSleeve)
        {
            sleeve = instantSleeve;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Compiles the type of the figure.
        /// </summary>
        /// <param name="typeName">Name of the type.</param>
        /// <returns>Type.</returns>
        public Type CompileFigureType(string typeName)
        {
            TypeBuilder tb = GetTypeBuilder(typeName);

            CreateSleevesField(tb);

            CreateFiguresField(tb);

            CreateElementByIntProperty(tb);

            CreateItemByIntProperty(tb);

            CreateItemByStringProperty(tb);

            

            return tb.CreateTypeInfo();
        }

        /// <summary>
        /// Creates the array length field.
        /// </summary>
        /// <param name="tb">The tb.</param>
        private void CreateArrayLengthField(TypeBuilder tb)
        {
            PropertyInfo iprop = SleeveType.GetProperty("Length");

            MethodInfo accessor = iprop.GetGetMethod();

            ParameterInfo[] args = accessor.GetParameters();
            Type[] argTypes = Array.ConvertAll(args, a => a.ParameterType);

            MethodBuilder getter = tb.DefineMethod(accessor.Name, accessor.Attributes & ~MethodAttributes.Abstract,
                                                          accessor.CallingConvention, accessor.ReturnType, argTypes);
            tb.DefineMethodOverride(getter, accessor);

            
            ILGenerator il = getter.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0); 
            il.Emit(OpCodes.Ldfld, selectiveField); 
            il.Emit(OpCodes.Ldlen); 
            il.Emit(OpCodes.Ret); 
        }

        /// <summary>
        /// Creates the element by int property.
        /// </summary>
        /// <param name="tb">The tb.</param>
        private void CreateElementByIntProperty(TypeBuilder tb)
        {

            PropertyInfo prop = typeof(FigureSleeves).GetProperty("Item", new Type[] { typeof(int) });
            MethodInfo accessor = prop.GetGetMethod();
            if (accessor != null)
            {
                ParameterInfo[] args = accessor.GetParameters();
                Type[] argTypes = Array.ConvertAll(args, a => a.ParameterType);

                MethodBuilder method = tb.DefineMethod(accessor.Name, accessor.Attributes & ~MethodAttributes.Abstract,
                                                  accessor.CallingConvention, accessor.ReturnType, argTypes);
                tb.DefineMethodOverride(method, accessor);
                ILGenerator il = method.GetILGenerator();

                il.Emit(OpCodes.Ldarg_0); 
                il.Emit(OpCodes.Ldfld, selectiveField); 
                il.Emit(OpCodes.Ldarg_1); 
                il.EmitCall(OpCodes.Callvirt, typeof(IFigures).GetMethod("get_Item", new Type[] { typeof(int) }), null);
                il.Emit(OpCodes.Ret); 
            }

            MethodInfo mutator = prop.GetSetMethod();
            if (mutator != null)
            {
                ParameterInfo[] args = mutator.GetParameters();
                Type[] argTypes = Array.ConvertAll(args, a => a.ParameterType);

                MethodBuilder method = tb.DefineMethod(mutator.Name, mutator.Attributes & ~MethodAttributes.Abstract,
                                                   mutator.CallingConvention, mutator.ReturnType, argTypes);
                tb.DefineMethodOverride(method, mutator);
                ILGenerator il = method.GetILGenerator();

                il.Emit(OpCodes.Ldarg_0); 
                il.Emit(OpCodes.Ldfld, selectiveField); 
                il.Emit(OpCodes.Ldarg_1); 
                il.Emit(OpCodes.Ldarg_2); 
                il.EmitCall(OpCodes.Callvirt, typeof(IFigures).GetMethod("set_Item", new Type[] { typeof(int), typeof(IFigure) }), null);
                il.Emit(OpCodes.Ret); 
            }
        }

        /// <summary>
        /// Creates the field.
        /// </summary>
        /// <param name="tb">The tb.</param>
        /// <param name="type">The type.</param>
        /// <param name="name">The name.</param>
        /// <returns>FieldBuilder.</returns>
        private FieldBuilder CreateField(TypeBuilder tb, Type type, string name)
        {
            return tb.DefineField("_" + name, type, FieldAttributes.Private);
        }

        /// <summary>
        /// Creates the figures field.
        /// </summary>
        /// <param name="tb">The tb.</param>
        private void CreateFiguresField(TypeBuilder tb)
        {
            FieldBuilder fb = CreateField(tb, typeof(object).MakeArrayType(), "Figures");
            multemicField = fb;

            PropertyInfo iprop = SleeveType.GetProperty("Figures");

            MethodInfo accessor = iprop.GetGetMethod();

            ParameterInfo[] args = accessor.GetParameters();
            Type[] argTypes = Array.ConvertAll(args, a => a.ParameterType);

            MethodBuilder getter = tb.DefineMethod(accessor.Name, accessor.Attributes & ~MethodAttributes.Abstract,
                                                          accessor.CallingConvention, accessor.ReturnType, argTypes);
            tb.DefineMethodOverride(getter, accessor);

            
            ILGenerator il = getter.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0); 
            il.Emit(OpCodes.Ldfld, fb); 
            il.Emit(OpCodes.Ret); 

            MethodInfo mutator = iprop.GetSetMethod();

            args = mutator.GetParameters();
            argTypes = Array.ConvertAll(args, a => a.ParameterType);

            MethodBuilder setter = tb.DefineMethod(mutator.Name, mutator.Attributes & ~MethodAttributes.Abstract,
                                               mutator.CallingConvention, mutator.ReturnType, argTypes);
            tb.DefineMethodOverride(setter, mutator);

            
            il = setter.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0); 
            il.Emit(OpCodes.Ldarg_1); 
            il.Emit(OpCodes.Stfld, fb); 
            il.Emit(OpCodes.Ret);
        }

        /// <summary>
        /// Creates the item by int property.
        /// </summary>
        /// <param name="tb">The tb.</param>
        private void CreateItemByIntProperty(TypeBuilder tb)
        {
            PropertyInfo prop = typeof(FigureSleeves).GetProperty("Item", new Type[] { typeof(int), typeof(int) });
            MethodInfo accessor = prop.GetGetMethod();
            if (accessor != null)
            {
                ParameterInfo[] args = accessor.GetParameters();
                Type[] argTypes = Array.ConvertAll(args, a => a.ParameterType);

                MethodBuilder method = tb.DefineMethod(accessor.Name, accessor.Attributes & ~MethodAttributes.Abstract,
                                                  accessor.CallingConvention, accessor.ReturnType, argTypes);
                tb.DefineMethodOverride(method, accessor);
                ILGenerator il = method.GetILGenerator();

                il.Emit(OpCodes.Ldarg_0); 
                il.Emit(OpCodes.Ldfld, selectiveField); 
                il.Emit(OpCodes.Ldarg_1); 
                il.Emit(OpCodes.Ldarg_2); 
                il.EmitCall(OpCodes.Callvirt, typeof(IFigures).GetMethod("get_Item", new Type[] { typeof(int), typeof(int) }), null);
                il.Emit(OpCodes.Ret); 
            }

            MethodInfo mutator = prop.GetSetMethod();
            if (mutator != null)
            {
                ParameterInfo[] args = mutator.GetParameters();
                Type[] argTypes = Array.ConvertAll(args, a => a.ParameterType);

                MethodBuilder method = tb.DefineMethod(mutator.Name, mutator.Attributes & ~MethodAttributes.Abstract,
                                                   mutator.CallingConvention, mutator.ReturnType, argTypes);
                tb.DefineMethodOverride(method, mutator);
                ILGenerator il = method.GetILGenerator();

                il.Emit(OpCodes.Ldarg_0); 
                il.Emit(OpCodes.Ldfld, selectiveField); 
                il.Emit(OpCodes.Ldarg_1); 
                il.Emit(OpCodes.Ldarg_2); 
                il.Emit(OpCodes.Ldarg_3); 
                il.EmitCall(OpCodes.Callvirt, typeof(IFigures).GetMethod("set_Item", new Type[] { typeof(int), typeof(int), typeof(object) }), null);
                il.Emit(OpCodes.Ret); 
            }
        }

        /// <summary>
        /// Creates the item by string property.
        /// </summary>
        /// <param name="tb">The tb.</param>
        private void CreateItemByStringProperty(TypeBuilder tb)
        {
            PropertyInfo prop = typeof(FigureSleeves).GetProperty("Item", new Type[] { typeof(int), typeof(string) });
            MethodInfo accessor = prop.GetGetMethod();
            if (accessor != null)
            {
                ParameterInfo[] args = accessor.GetParameters();
                Type[] argTypes = Array.ConvertAll(args, a => a.ParameterType);

                MethodBuilder method = tb.DefineMethod(accessor.Name, accessor.Attributes & ~MethodAttributes.Abstract,
                                                   accessor.CallingConvention, accessor.ReturnType, argTypes);
                tb.DefineMethodOverride(method, accessor);
                ILGenerator il = method.GetILGenerator();

                il.Emit(OpCodes.Ldarg_0); 
                il.Emit(OpCodes.Ldfld, selectiveField); 
                il.Emit(OpCodes.Ldarg_1); 
                il.Emit(OpCodes.Ldarg_2); 
                il.EmitCall(OpCodes.Callvirt, typeof(IFigures).GetMethod("get_Item", new Type[] { typeof(int), typeof(string) }), null);
                il.Emit(OpCodes.Ret); 
            }

            MethodInfo mutator = prop.GetSetMethod();
            if (mutator != null)
            {
                ParameterInfo[] args = mutator.GetParameters();
                Type[] argTypes = Array.ConvertAll(args, a => a.ParameterType);
                MethodBuilder method = tb.DefineMethod(mutator.Name, mutator.Attributes & ~MethodAttributes.Abstract,
                                                   mutator.CallingConvention, mutator.ReturnType, argTypes);
                tb.DefineMethodOverride(method, mutator);
                ILGenerator il = method.GetILGenerator();

                il.Emit(OpCodes.Ldarg_0); 
                il.Emit(OpCodes.Ldfld, selectiveField); 
                il.Emit(OpCodes.Ldarg_1); 
                il.Emit(OpCodes.Ldarg_2); 
                il.Emit(OpCodes.Ldarg_3); 
                il.EmitCall(OpCodes.Callvirt, typeof(IFigures).GetMethod("set_Item", new Type[] { typeof(int), typeof(string), typeof(object) }), null);
                il.Emit(OpCodes.Ret); 
            }
        }

        /// <summary>
        /// Creates the marshal attribue.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="attrib">The attribute.</param>
        private void CreateMarshalAttribue(FieldBuilder field, MarshalAsAttribute attrib)
        {
            List<object> attribValues = new List<object>(1);
            List<FieldInfo> attribFields = new List<FieldInfo>(1);
            attribValues.Add(attrib.SizeConst);
            attribFields.Add(attrib.GetType().GetField("SizeConst"));
            field.SetCustomAttribute(new CustomAttributeBuilder(marshalAsCtor, new object[] { attrib.Value }, attribFields.ToArray(), attribValues.ToArray()));
        }

        /// <summary>
        /// Creates the new sleeves object.
        /// </summary>
        /// <param name="tb">The tb.</param>
        private void CreateNewSleevesObject(TypeBuilder tb)
        {
            MethodInfo createArray = SleeveType.GetMethod("NewSleeves");

            ParameterInfo[] args = createArray.GetParameters();
            Type[] argTypes = Array.ConvertAll(args, a => a.ParameterType);

            MethodBuilder method = tb.DefineMethod(createArray.Name, createArray.Attributes & ~MethodAttributes.Abstract,
                                                          createArray.CallingConvention, createArray.ReturnType, argTypes);
            tb.DefineMethodOverride(method, createArray);

            ILGenerator il = method.GetILGenerator();
            il.DeclareLocal(typeof(IFigure).MakeArrayType());

            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Newarr, typeof(IFigure));
            il.Emit(OpCodes.Stloc_0);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldloc_0);
            il.Emit(OpCodes.Stfld, selectiveField); 
            il.Emit(OpCodes.Ldloc_0);
            il.Emit(OpCodes.Ret);
        }

        /// <summary>
        /// Creates the property.
        /// </summary>
        /// <param name="tb">The tb.</param>
        /// <param name="field">The field.</param>
        /// <param name="type">The type.</param>
        /// <param name="name">The name.</param>
        /// <returns>PropertyBuilder.</returns>
        private PropertyBuilder CreateProperty(TypeBuilder tb, FieldBuilder field, Type type, string name)
        {

            PropertyBuilder prop = tb.DefineProperty(name, PropertyAttributes.HasDefault,
                                                     type, new Type[] { type });

            MethodBuilder getter = tb.DefineMethod("get_" + name, MethodAttributes.Public |
                                                            MethodAttributes.HideBySig, type,
                                                            Type.EmptyTypes);
            prop.SetGetMethod(getter);
            ILGenerator il = getter.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0); 
            il.Emit(OpCodes.Ldfld, field); 
            il.Emit(OpCodes.Ret); 

            MethodBuilder setter = tb.DefineMethod("set_" + name, MethodAttributes.Public |
                                                            MethodAttributes.HideBySig, typeof(void),
                                                            new Type[] { type });
            prop.SetSetMethod(setter);
            il = setter.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0); 
            il.Emit(OpCodes.Ldarg_1); 
            il.Emit(OpCodes.Stfld, field); 
            il.Emit(OpCodes.Ret);

            return prop;
        }

        /// <summary>
        /// Creates the sleeves field.
        /// </summary>
        /// <param name="tb">The tb.</param>
        private void CreateSleevesField(TypeBuilder tb)
        {
            FieldBuilder fb = CreateField(tb, typeof(object).MakeArrayType(), "Sleeves");
            selectiveField = fb;

            PropertyInfo iprop = SleeveType.GetProperty("Sleeves");

            MethodInfo accessor = iprop.GetGetMethod();

            ParameterInfo[] args = accessor.GetParameters();
            Type[] argTypes = Array.ConvertAll(args, a => a.ParameterType);

            MethodBuilder getter = tb.DefineMethod(accessor.Name, accessor.Attributes & ~MethodAttributes.Abstract,
                                                          accessor.CallingConvention, accessor.ReturnType, argTypes);
            tb.DefineMethodOverride(getter, accessor);

            
            ILGenerator il = getter.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0); 
            il.Emit(OpCodes.Ldfld, fb); 
            il.Emit(OpCodes.Ret); 

            MethodInfo mutator = iprop.GetSetMethod();

            args = mutator.GetParameters();
            argTypes = Array.ConvertAll(args, a => a.ParameterType);

            MethodBuilder setter = tb.DefineMethod(mutator.Name, mutator.Attributes & ~MethodAttributes.Abstract,
                                               mutator.CallingConvention, mutator.ReturnType, argTypes);
            tb.DefineMethodOverride(setter, mutator);

            
            il = setter.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0); 
            il.Emit(OpCodes.Ldarg_1); 
            il.Emit(OpCodes.Stfld, fb); 
            il.Emit(OpCodes.Ret);
        }

        /// <summary>
        /// Gets the type builder.
        /// </summary>
        /// <param name="typeName">Name of the type.</param>
        /// <returns>TypeBuilder.</returns>
        private TypeBuilder GetTypeBuilder(string typeName)
        {
            string typeSignature = typeName;
            AssemblyName an = new AssemblyName(typeSignature);

            AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(an, AssemblyBuilderAccess.RunAndCollect);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule(typeSignature + "Module");
            TypeBuilder tb = null;

            tb = moduleBuilder.DefineType(typeSignature, TypeAttributes.Class | TypeAttributes.Public | TypeAttributes.Serializable |
                                                         TypeAttributes.AnsiClass);

            tb.SetCustomAttribute(new CustomAttributeBuilder(typeof(DataContractAttribute).GetConstructor(Type.EmptyTypes), new object[0]));
            tb.SetParent(typeof(FigureSleeves));
            return tb;
        }

        #endregion
    }
}
