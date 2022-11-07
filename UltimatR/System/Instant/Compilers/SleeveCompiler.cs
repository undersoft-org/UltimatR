﻿
// <copyright file="SleeveCompiler.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



using System.Series;

/// <summary>
/// The Instant namespace.
/// </summary>
namespace System.Instant
{
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Emit;
    using System.Runtime.Serialization;
    using System.Uniques;

    /// <summary>
    /// Class SleeveCompiler.
    /// Implements the <see cref="System.Instant.SleeveCompilerBase" />
    /// </summary>
    /// <seealso cref="System.Instant.SleeveCompilerBase" />
    public class SleeveCompiler : SleeveCompilerBase
    {

        #region Fields

        /// <summary>
        /// The scode field
        /// </summary>
        protected FieldInfo       scodeField;
        /// <summary>
        /// The sobject field
        /// </summary>
        protected FieldBuilder  sobjectField;
        /// <summary>
        /// The rubrics field
        /// </summary>
        protected FieldBuilder  rubricsField;
        /// <summary>
        /// The has serial code
        /// </summary>
        public bool hasSerialCode;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SleeveCompiler" /> class.
        /// </summary>
        /// <param name="instantSleeve">The instant sleeve.</param>
        /// <param name="rubricBuilders">The rubric builders.</param>
        public SleeveCompiler(Sleeve instantSleeve, IDeck<RubricBuilder> rubricBuilders) 
            : base(instantSleeve, rubricBuilders)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Compiles the type of the sleeve.
        /// </summary>
        /// <param name="typeName">Name of the type.</param>
        /// <returns>Type.</returns>
        public override Type CompileSleeveType(string typeName)
        {
            TypeBuilder tb = GetTypeBuilder(typeName);

            CreateInstanceProperty(tb, typeof(object), "Devisor");

            CreateRubricsProperty(tb, typeof(MemberRubrics), "Rubrics");

            CreateFieldsAndProperties(tb);

            CreateSerialCodeProperty(tb, typeof(Ussn), "SerialCode");

            CreateValueArrayProperty(tb);

            CreateItemByIntProperty(tb);

            CreateItemByStringProperty(tb);

            CreateUniqueKeyProperty(tb);

            CreateUniqueSeedProperty(tb);

            CreateGetUniqueBytesMethod(tb);

            CreateGetBytesMethod(tb);

            

            CreateEqualsMethod(tb);

            CreateCompareToMethod(tb);

            return tb.CreateTypeInfo();
        }

        /// <summary>
        /// Creates the compare to method.
        /// </summary>
        /// <param name="tb">The tb.</param>
        public override void CreateCompareToMethod(TypeBuilder tb)
        {
            MethodInfo mi = typeof(IComparable<IUnique>).GetMethod("CompareTo");

            ParameterInfo[] args = mi.GetParameters();
            Type[] argTypes = Array.ConvertAll(args, a => a.ParameterType);

            MethodBuilder method = tb.DefineMethod(mi.Name, mi.Attributes & ~MethodAttributes.Abstract,
                                                          mi.CallingConvention, mi.ReturnType, argTypes);
            tb.DefineMethodOverride(method, mi);

            ILGenerator il = method.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0);
            if (hasSerialCode)
                il.Emit(OpCodes.Ldfld, sobjectField); 
            il.Emit(OpCodes.Ldflda, scodeField); 
            il.Emit(OpCodes.Ldarg_1);
            il.EmitCall(OpCodes.Call, typeof(Ussn).GetMethod("CompareTo", new Type[] { typeof(IUnique) }), null);
            il.Emit(OpCodes.Ret);
        }

        /// <summary>
        /// Creates the equals method.
        /// </summary>
        /// <param name="tb">The tb.</param>
        public override void CreateEqualsMethod(TypeBuilder tb)
        {
            MethodInfo createArray = typeof(IEquatable<IUnique>).GetMethod("Equals");

            ParameterInfo[] args = createArray.GetParameters();
            Type[] argTypes = Array.ConvertAll(args, a => a.ParameterType);

            MethodBuilder method = tb.DefineMethod(createArray.Name, createArray.Attributes & ~MethodAttributes.Abstract,
                                                          createArray.CallingConvention, createArray.ReturnType, argTypes);
            tb.DefineMethodOverride(method, createArray);

            ILGenerator il = method.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0);
            if (hasSerialCode)
                il.Emit(OpCodes.Ldfld, sobjectField); 
            il.Emit(OpCodes.Ldflda, scodeField); 
            il.Emit(OpCodes.Ldarg_1);
            il.EmitCall(OpCodes.Call, typeof(Ussn).GetMethod("Equals", new Type[] { typeof(IUnique) }), null);
            il.Emit(OpCodes.Ret);
        }

        /// <summary>
        /// Creates the fields and properties.
        /// </summary>
        /// <param name="tb">The tb.</param>
        public override void CreateFieldsAndProperties(TypeBuilder tb)
        {
            rubricBuilders.AsValues().ForEach((m) =>
            {
                if (m.Field != null)
                {
                    if (!(m.Field.IsBackingField))
                        ResolveFigureAttributes(null, new MemberRubric(m.Field));
                    else if (m.Property != null)
                        ResolveFigureAttributes(null, new MemberRubric(m.Property));
                }
                else if (m.Property != null)
                {
                    ResolveFigureAttributes(null, new MemberRubric(m.Property));
                }
            });
        }

        /// <summary>
        /// Creates the get bytes method.
        /// </summary>
        /// <param name="tb">The tb.</param>
        public override void CreateGetBytesMethod(TypeBuilder tb)
        {
            MethodInfo createArray = typeof(IUnique).GetMethod("GetBytes");

            ParameterInfo[] args = createArray.GetParameters();
            Type[] argTypes = Array.ConvertAll(args, a => a.ParameterType);

            MethodBuilder method = tb.DefineMethod(createArray.Name, createArray.Attributes & ~MethodAttributes.Abstract,
                                                          createArray.CallingConvention, createArray.ReturnType, argTypes);
            tb.DefineMethodOverride(method, createArray);

            ILGenerator il = method.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            if (hasSerialCode)
                il.Emit(OpCodes.Ldfld, sobjectField); 
            il.Emit(OpCodes.Ldflda, scodeField); 
            il.EmitCall(OpCodes.Call, typeof(IUnique).GetMethod("GetBytes"), null);
            il.Emit(OpCodes.Ret);
        }

        /// <summary>
        /// Creates the get empty property.
        /// </summary>
        /// <param name="tb">The tb.</param>
        public override void CreateGetEmptyProperty(TypeBuilder tb)
        {
            PropertyBuilder prop = tb.DefineProperty("Empty", PropertyAttributes.HasDefault,
                                                     typeof(IUnique), Type.EmptyTypes);

            PropertyInfo iprop = typeof(IUnique).GetProperty("Empty");

            MethodInfo accessor = iprop.GetGetMethod();

            ParameterInfo[] args = accessor.GetParameters();
            Type[] argTypes = Array.ConvertAll(args, a => a.ParameterType);

            MethodBuilder getter = tb.DefineMethod(accessor.Name, accessor.Attributes & ~MethodAttributes.Abstract,
                                                          accessor.CallingConvention, accessor.ReturnType, argTypes);
            tb.DefineMethodOverride(getter, accessor);
           
            prop.SetGetMethod(getter);
            ILGenerator il = getter.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0); 
            if (hasSerialCode)
                il.Emit(OpCodes.Ldfld, sobjectField); 
            il.Emit(OpCodes.Ldflda, scodeField); 
            il.EmitCall(OpCodes.Call, typeof(Ussn).GetMethod("get_Empty"), null);
            il.Emit(OpCodes.Ret); 
        }

        /// <summary>
        /// Creates the get unique bytes method.
        /// </summary>
        /// <param name="tb">The tb.</param>
        public override void CreateGetUniqueBytesMethod(TypeBuilder tb)
        {
            MethodInfo createArray = typeof(IUnique).GetMethod("GetUniqueBytes");

            ParameterInfo[] args = createArray.GetParameters();
            Type[] argTypes = Array.ConvertAll(args, a => a.ParameterType);

            MethodBuilder method = tb.DefineMethod(createArray.Name, createArray.Attributes & ~MethodAttributes.Abstract,
                                                          createArray.CallingConvention, createArray.ReturnType, argTypes);
            tb.DefineMethodOverride(method, createArray);

            ILGenerator il = method.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            if (hasSerialCode)
                il.Emit(OpCodes.Ldfld, sobjectField); 
            il.Emit(OpCodes.Ldflda, scodeField); 
            il.EmitCall(OpCodes.Call, typeof(Ussn).GetMethod("GetUniqueBytes"), null);
            il.Emit(OpCodes.Ret);
        }

        /// <summary>
        /// Creates the instance property.
        /// </summary>
        /// <param name="tb">The tb.</param>
        /// <param name="type">The type.</param>
        /// <param name="name">The name.</param>
        public override void CreateInstanceProperty(TypeBuilder tb, Type type, string name)
        {
            sobjectField = tb.DefineField(name.ToLower(), type, FieldAttributes.Private);

            PropertyBuilder prop = tb.DefineProperty(name, PropertyAttributes.HasDefault,
                                                     type, new Type[] { type });

            PropertyInfo iprop = typeof(ISleeve).GetProperty(name);

            MethodInfo accessor = iprop.GetGetMethod();

            ParameterInfo[] args = accessor.GetParameters();
            Type[] argTypes = Array.ConvertAll(args, a => a.ParameterType);

            MethodBuilder getter = tb.DefineMethod(accessor.Name, accessor.Attributes & ~MethodAttributes.Abstract,
                                                          accessor.CallingConvention, accessor.ReturnType, argTypes);

            tb.DefineMethodOverride(getter, accessor);

            prop.SetGetMethod(getter);
            ILGenerator il = getter.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0); 
            il.Emit(OpCodes.Ldfld, sobjectField); 
            il.Emit(OpCodes.Ret); 

            MethodInfo mutator = iprop.GetSetMethod();

            args = mutator.GetParameters();
            argTypes = Array.ConvertAll(args, a => a.ParameterType);

            MethodBuilder setter = tb.DefineMethod(mutator.Name, mutator.Attributes & ~MethodAttributes.Abstract,
                                               mutator.CallingConvention, mutator.ReturnType, argTypes);
            tb.DefineMethodOverride(setter, mutator);

            prop.SetSetMethod(setter);
            il = setter.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0); 
            il.Emit(OpCodes.Ldarg_1); 
            il.Emit(OpCodes.Stfld, sobjectField); 
            il.Emit(OpCodes.Ret);

            prop.SetCustomAttribute(new CustomAttributeBuilder(
                                       dataMemberCtor, new object[0],
                                       dataMemberProps, new object[2] { -1, name.ToUpper() }));
        }

        /// <summary>
        /// Creates the item by int property.
        /// </summary>
        /// <param name="tb">The tb.</param>
        public override void CreateItemByIntProperty(TypeBuilder tb)
        {
            foreach (PropertyInfo prop in typeof(IFigure).GetProperties())
            {
                MethodInfo accessor = prop.GetGetMethod();
                if (accessor != null)
                {
                    ParameterInfo[] args = accessor.GetParameters();
                    Type[] argTypes = Array.ConvertAll(args, a => a.ParameterType);

                    if (args.Length == 1 && argTypes[0] == typeof(int))
                    {
                        MethodBuilder method = tb.DefineMethod(accessor.Name, accessor.Attributes & ~MethodAttributes.Abstract,
                                                               accessor.CallingConvention, accessor.ReturnType, argTypes);
                        tb.DefineMethodOverride(method, accessor);
                        ILGenerator il = method.GetILGenerator();

                        Label[] branches = new Label[length];
                        for (int i = 0; i < length; i++)
                        {
                            branches[i] = il.DefineLabel();
                        }

                        il.Emit(OpCodes.Ldarg_1); 

                        il.Emit(OpCodes.Switch, branches); 
                                                           
                        il.ThrowException(typeof(ArgumentOutOfRangeException));

                        for (int i = 0; i < length; i++)
                        {
                            il.MarkLabel(branches[i]);
                            il.Emit(OpCodes.Ldarg_0); 
                            il.Emit(OpCodes.Ldfld, sobjectField); 
                            if (rubricBuilders[i].Field == null ||
                            rubricBuilders[i].Field.IsBackingField)
                            {
                                il.EmitCall(OpCodes.Call, rubricBuilders[i].Getter, null);
                            }
                            else
                                il.Emit(OpCodes.Ldfld, rubricBuilders[i].Field.RubricInfo); 

                            if (rubricBuilders[i].Type.IsValueType)
                            {
                                il.Emit(OpCodes.Box, rubricBuilders[i].Type); 
                            }
                            il.Emit(OpCodes.Ret); 
                        }
                    }
                }

                MethodInfo mutator = prop.GetSetMethod();
                if (mutator != null)
                {
                    ParameterInfo[] args = mutator.GetParameters();
                    Type[] argTypes = Array.ConvertAll(args, a => a.ParameterType);

                    if (args.Length == 2 && argTypes[0] == typeof(int) && argTypes[1] == typeof(object))
                    {
                        MethodBuilder method = tb.DefineMethod(mutator.Name, mutator.Attributes & ~MethodAttributes.Abstract,
                                                           mutator.CallingConvention, mutator.ReturnType, argTypes);
                        tb.DefineMethodOverride(method, mutator);
                        ILGenerator il = method.GetILGenerator();

                        Label[] branches = new Label[length];
                        for (int i = 0; i < length; i++)
                        {
                            branches[i] = il.DefineLabel();
                        }

                        il.Emit(OpCodes.Ldarg_1); 

                        il.Emit(OpCodes.Switch, branches); 
                                                           
                        il.ThrowException(typeof(ArgumentOutOfRangeException));

                        for (int i = 0; i < length; i++)
                        {
                            il.MarkLabel(branches[i]);
                            il.Emit(OpCodes.Ldarg_0); 
                            il.Emit(OpCodes.Ldfld, sobjectField); 
                            il.Emit(OpCodes.Ldarg_2); 

                            il.Emit(rubricBuilders[i].Type.IsValueType ?
                                                     OpCodes.Unbox_Any :
                                                     OpCodes.Castclass,
                                                     rubricBuilders[i].Type); 

                            if (rubricBuilders[i].Field == null ||
                                rubricBuilders[i].Field.IsBackingField)
                            {
                                il.EmitCall(OpCodes.Call, rubricBuilders[i].Setter, null); 
                            }
                            else
                                il.Emit(OpCodes.Stfld, rubricBuilders[i].Field.RubricInfo); 

                            il.Emit(OpCodes.Ret); 
                        }
                    }
                }

            }
        }

        /// <summary>
        /// Creates the item by string property.
        /// </summary>
        /// <param name="tb">The tb.</param>
        public override void CreateItemByStringProperty(TypeBuilder tb)
        {
            foreach (PropertyInfo prop in typeof(IFigure).GetProperties())
            {
                MethodInfo accessor = prop.GetGetMethod();
                if (accessor != null)
                {
                    ParameterInfo[] args = accessor.GetParameters();
                    Type[] argTypes = Array.ConvertAll(args, a => a.ParameterType);

                    if (args.Length == 1 && argTypes[0] == typeof(string))
                    {
                        MethodBuilder method = tb.DefineMethod(accessor.Name, accessor.Attributes & ~MethodAttributes.Abstract,
                                                               accessor.CallingConvention, accessor.ReturnType, argTypes);
                        tb.DefineMethodOverride(method, accessor);
                        ILGenerator il = method.GetILGenerator();

                        il.DeclareLocal(typeof(string));

                        Label[] branches = new Label[length];

                        for (int i = 0; i < length; i++)
                        {
                            branches[i] = il.DefineLabel();
                        }

                        il.Emit(OpCodes.Ldarg_1); 
                        il.Emit(OpCodes.Stloc_0);

                        for (int i = 0; i < length; i++)
                        {
                            il.Emit(OpCodes.Ldloc_0);
                            il.Emit(OpCodes.Ldstr, rubricBuilders[i].Name);
                            il.EmitCall(OpCodes.Call, typeof(string).GetMethod("op_Equality", new Type[] { typeof(string), typeof(string) }), null);
                            il.Emit(OpCodes.Brtrue, branches[i]);
                        }

                        il.Emit(OpCodes.Ldnull); 
                        il.Emit(OpCodes.Ret);

                        for (int i = 0; i < length; i++)
                        {
                            il.MarkLabel(branches[i]);
                            il.Emit(OpCodes.Ldarg_0); 
                            il.Emit(OpCodes.Ldfld, sobjectField); 

                            if (rubricBuilders[i].Field == null || rubricBuilders[i].Field.IsBackingField)
                            {
                                il.EmitCall(OpCodes.Call, rubricBuilders[i].Getter, null);
                            }
                            else
                                il.Emit(OpCodes.Ldfld, rubricBuilders[i].Field.RubricInfo); 

                            if (rubricBuilders[i].Type.IsValueType)
                            {
                                il.Emit(OpCodes.Box, rubricBuilders[i].Type); 
                            }
                            il.Emit(OpCodes.Ret); 
                        }
                    }
                }


                MethodInfo mutator = prop.GetSetMethod();
                if (mutator != null)
                {
                    ParameterInfo[] args = mutator.GetParameters();
                    Type[] argTypes = Array.ConvertAll(args, a => a.ParameterType);

                    if (args.Length == 2 && argTypes[0] == typeof(string) && argTypes[1] == typeof(object))
                    {
                        MethodBuilder method = tb.DefineMethod(mutator.Name, mutator.Attributes & ~MethodAttributes.Abstract,
                                                           mutator.CallingConvention, mutator.ReturnType, argTypes);
                        tb.DefineMethodOverride(method, mutator);
                        ILGenerator il = method.GetILGenerator();

                        il.DeclareLocal(typeof(string));

                        Label[] branches = new Label[length];
                        for (int i = 0; i < length; i++)
                        {
                            branches[i] = il.DefineLabel();
                        }

                        il.Emit(OpCodes.Ldarg_1); 
                        il.Emit(OpCodes.Stloc_0);

                        for (int i = 0; i < length; i++)
                        {
                            il.Emit(OpCodes.Ldloc_0);
                            il.Emit(OpCodes.Ldstr, rubricBuilders[i].Name);
                            il.EmitCall(OpCodes.Call, typeof(string).GetMethod("op_Equality", new[] { typeof(string), typeof(string) }), null);
                            il.Emit(OpCodes.Brtrue, branches[i]);
                        }

                        il.Emit(OpCodes.Ret);

                        for (int i = 0; i < length; i++)
                        {
                            il.MarkLabel(branches[i]);
                            il.Emit(OpCodes.Ldarg_0); 
                            il.Emit(OpCodes.Ldfld, sobjectField); 
                            il.Emit(OpCodes.Ldarg_2); 

                            il.Emit(rubricBuilders[i].Type.IsValueType ?
                                                             OpCodes.Unbox_Any :
                                                             OpCodes.Castclass,
                                                             rubricBuilders[i].Type); 

                            if (rubricBuilders[i].Field == null || rubricBuilders[i].Field.IsBackingField)
                            {
                                il.EmitCall(OpCodes.Call, rubricBuilders[i].Setter, null); 
                            }
                            else
                                il.Emit(OpCodes.Stfld, rubricBuilders[i].Field.RubricInfo); 
                            il.Emit(OpCodes.Ret); 
                        }
                    }
                }

            }
        }

        /// <summary>
        /// Creates the serial code property.
        /// </summary>
        /// <param name="tb">The tb.</param>
        /// <param name="type">The type.</param>
        /// <param name="name">The name.</param>
        public override void CreateSerialCodeProperty(TypeBuilder tb, Type type, string name)
        {
            RubricBuilder fp = null;
            var field = rubricBuilders.AsValues().FirstOrDefault
                                        (p => p.Field != null &&  p.Field.FieldName
                                        .Contains(name, StringComparison.InvariantCultureIgnoreCase));
            if (field != null)
            {
                scodeField = field.Field.RubricInfo;
            }
            else
            {
                FieldBuilder fb = createField(tb, null, type, name.ToLower());
                scodeField = fb;
                fp = new RubricBuilder(new MemberRubric(fb));
            }

            PropertyBuilder prop = tb.DefineProperty(name, PropertyAttributes.HasDefault,
                                                     type, new Type[] { type });

            PropertyInfo iprop = typeof(IFigure).GetProperty(name);

            MethodInfo accessor = iprop.GetGetMethod();

            ParameterInfo[] args = accessor.GetParameters();
            Type[] argTypes = Array.ConvertAll(args, a => a.ParameterType);

            MethodBuilder getter = tb.DefineMethod(accessor.Name, accessor.Attributes & ~MethodAttributes.Abstract,
                                                          accessor.CallingConvention, accessor.ReturnType, argTypes);
            tb.DefineMethodOverride(getter, accessor);
          
            ILGenerator il = getter.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0); 
            if (hasSerialCode)
                il.Emit(OpCodes.Ldfld, sobjectField); 
            il.Emit(OpCodes.Ldfld, scodeField); 
            il.Emit(OpCodes.Ret); 

            MethodInfo mutator = iprop.GetSetMethod();

            args = mutator.GetParameters();
            argTypes = Array.ConvertAll(args, a => a.ParameterType);

            MethodBuilder setter = tb.DefineMethod(mutator.Name, mutator.Attributes & ~MethodAttributes.Abstract,
                                               mutator.CallingConvention, mutator.ReturnType, argTypes);
            tb.DefineMethodOverride(setter, mutator);

            il = setter.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0); 
            if (hasSerialCode)
                il.Emit(OpCodes.Ldfld, sobjectField); 
            il.Emit(OpCodes.Ldarg_1); 
            il.Emit(OpCodes.Stfld, scodeField); 
            il.Emit(OpCodes.Ret);

            prop.SetCustomAttribute(new CustomAttributeBuilder(
                                       dataMemberCtor, new object[0],
                                       dataMemberProps, new object[2] { length, name.ToUpper() }));
            if (fp != null)
            {
                fp.SetMember(new MemberRubric(prop));
                rubricBuilders.Add(fp);
            }
        }

        /// <summary>
        /// Creates the rubrics property.
        /// </summary>
        /// <param name="tb">The tb.</param>
        /// <param name="type">The type.</param>
        /// <param name="name">The name.</param>
        public override void CreateRubricsProperty(TypeBuilder tb, Type type, string name)
        {
            rubricsField = tb.DefineField(name.ToLower(), type, FieldAttributes.Private);

            PropertyBuilder prop = tb.DefineProperty(name, PropertyAttributes.HasDefault,
                                                     type, new Type[] { type });

            PropertyInfo iprop = typeof(ISleeve).GetProperty(name);

            MethodInfo accessor = iprop.GetGetMethod();

            ParameterInfo[] args = accessor.GetParameters();
            Type[] argTypes = Array.ConvertAll(args, a => a.ParameterType);

            MethodBuilder getter = tb.DefineMethod(accessor.Name, accessor.Attributes & ~MethodAttributes.Abstract,
                                                          accessor.CallingConvention, accessor.ReturnType, argTypes);
            tb.DefineMethodOverride(getter, accessor);

            prop.SetGetMethod(getter);
            ILGenerator il = getter.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0); 
            il.Emit(OpCodes.Ldfld, rubricsField); 
            il.Emit(OpCodes.Ret); 

            MethodInfo mutator = iprop.GetSetMethod();

            args = mutator.GetParameters();
            argTypes = Array.ConvertAll(args, a => a.ParameterType);

            MethodBuilder setter = tb.DefineMethod(mutator.Name, mutator.Attributes & ~MethodAttributes.Abstract,
                mutator.CallingConvention, mutator.ReturnType, argTypes);
            tb.DefineMethodOverride(setter, mutator);

            prop.SetSetMethod(setter);
            il = setter.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0); 
            il.Emit(OpCodes.Ldarg_1); 
            il.Emit(OpCodes.Stfld, rubricsField); 
            il.Emit(OpCodes.Ret);

            prop.SetCustomAttribute(new CustomAttributeBuilder(
                dataMemberCtor, new object[0],
                dataMemberProps, new object[2] { -3, name.ToUpper() }));
        }

        /// <summary>
        /// Creates the unique key property.
        /// </summary>
        /// <param name="tb">The tb.</param>
        public override void CreateUniqueKeyProperty(TypeBuilder tb)
        {
            PropertyBuilder prop = tb.DefineProperty("UniqueKey", PropertyAttributes.HasDefault,
                                                     typeof(ulong), new Type[] { typeof(ulong) });

            PropertyInfo iprop = typeof(IUnique).GetProperty("UniqueKey");

            MethodInfo accessor = iprop.GetGetMethod();

            ParameterInfo[] args = accessor.GetParameters();
            Type[] argTypes = Array.ConvertAll(args, a => a.ParameterType);

            MethodBuilder getter = tb.DefineMethod(accessor.Name, accessor.Attributes & ~MethodAttributes.Abstract,
                                                          accessor.CallingConvention, accessor.ReturnType, argTypes);
            tb.DefineMethodOverride(getter, accessor);

           
            ILGenerator il = getter.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0); 
            if (hasSerialCode)
                il.Emit(OpCodes.Ldfld, sobjectField); 
            il.Emit(OpCodes.Ldflda, scodeField); 
            il.EmitCall(OpCodes.Call, typeof(Ussn).GetMethod("GetUniqueKey"), null);
            il.Emit(OpCodes.Ret); 

            MethodInfo mutator = iprop.GetSetMethod();

            args = mutator.GetParameters();
            argTypes = Array.ConvertAll(args, a => a.ParameterType);

            MethodBuilder setter = tb.DefineMethod(mutator.Name, mutator.Attributes & ~MethodAttributes.Abstract,
                                                          mutator.CallingConvention, mutator.ReturnType, argTypes);
            tb.DefineMethodOverride(setter, mutator);

           
            il = setter.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0); 
            if (hasSerialCode)
                il.Emit(OpCodes.Ldfld, sobjectField); 
            il.Emit(OpCodes.Ldflda, scodeField); 
            il.Emit(OpCodes.Ldarg_1); 
            il.EmitCall(OpCodes.Call, typeof(Ussn).GetMethod("SetUniqueKey"), null);
            il.Emit(OpCodes.Ret); 
        }

        /// <summary>
        /// Creates the unique seed property.
        /// </summary>
        /// <param name="tb">The tb.</param>
        public override void CreateUniqueSeedProperty(TypeBuilder tb)
        {

            PropertyBuilder prop = tb.DefineProperty("UniqueSeed", PropertyAttributes.HasDefault,
                                                     typeof(ulong), new Type[] { typeof(ulong) });

            PropertyInfo iprop = typeof(IUnique).GetProperty("UniqueSeed");

            MethodInfo accessor = iprop.GetGetMethod();

            ParameterInfo[] args = accessor.GetParameters();
            Type[] argTypes = Array.ConvertAll(args, a => a.ParameterType);

            MethodBuilder getter = tb.DefineMethod(accessor.Name, accessor.Attributes & ~MethodAttributes.Abstract,
                                                          accessor.CallingConvention, accessor.ReturnType, argTypes);
            tb.DefineMethodOverride(getter, accessor);

           
            ILGenerator il = getter.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0); 
            if (hasSerialCode)
                il.Emit(OpCodes.Ldfld, sobjectField); 
            il.Emit(OpCodes.Ldflda, scodeField); 
            il.EmitCall(OpCodes.Call, typeof(Ussn).GetMethod("GetUniqueSeed"), null);
            il.Emit(OpCodes.Ret); 

            MethodInfo mutator = iprop.GetSetMethod();

            args = mutator.GetParameters();
            argTypes = Array.ConvertAll(args, a => a.ParameterType);

            MethodBuilder setter = tb.DefineMethod(mutator.Name, mutator.Attributes & ~MethodAttributes.Abstract,
                                                          mutator.CallingConvention, mutator.ReturnType, argTypes);
            tb.DefineMethodOverride(setter, mutator);

            il = setter.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0); 
            if (hasSerialCode)
                il.Emit(OpCodes.Ldfld, sobjectField); 
            il.Emit(OpCodes.Ldflda, scodeField); 
            il.Emit(OpCodes.Ldarg_1); 
            il.EmitCall(OpCodes.Call, typeof(Ussn).GetMethod("SetUniqueSeed"), null);
            il.Emit(OpCodes.Ret); 
        }

        /// <summary>
        /// Creates the value array property.
        /// </summary>
        /// <param name="tb">The tb.</param>
        public override void CreateValueArrayProperty(TypeBuilder tb)
        {
            PropertyInfo prop = typeof(IFigure).GetProperty("ValueArray");

            MethodInfo accessor = prop.GetGetMethod();

            ParameterInfo[] args = accessor.GetParameters();
            Type[] argTypes = Array.ConvertAll(args, a => a.ParameterType);

            MethodBuilder method = tb.DefineMethod(accessor.Name, accessor.Attributes & ~MethodAttributes.Abstract,
                                                   accessor.CallingConvention, accessor.ReturnType, argTypes);
            tb.DefineMethodOverride(method, accessor);

            ILGenerator il = method.GetILGenerator();
            il.DeclareLocal(typeof(object[]));

            il.Emit(OpCodes.Ldc_I4, length);
            il.Emit(OpCodes.Newarr, typeof(object));
            il.Emit(OpCodes.Stloc_0);

            for (int i = 0; i < length; i++)
            {
                il.Emit(OpCodes.Ldloc_0); 
                il.Emit(OpCodes.Ldc_I4, i);
                il.Emit(OpCodes.Ldarg_0); 
                il.Emit(OpCodes.Ldfld, sobjectField); 

                if (rubricBuilders[i].Field == null || rubricBuilders[i].Field.IsBackingField)
                {
                    il.EmitCall(OpCodes.Call, rubricBuilders[i].Getter, null); 
                }
                else
                    il.Emit(OpCodes.Ldfld, rubricBuilders[i].Field.RubricInfo); 

                if (rubricBuilders[i].Type.IsValueType)
                {
                    il.Emit(OpCodes.Box, rubricBuilders[i].Type); 
                }

                il.Emit(OpCodes.Stelem, typeof(object)); 
            }

            il.Emit(OpCodes.Ldloc_0);
            il.Emit(OpCodes.Ret); 

            MethodInfo mutator = prop.GetSetMethod();

            args = mutator.GetParameters();
            argTypes = Array.ConvertAll(args, a => a.ParameterType);

            method = tb.DefineMethod(mutator.Name, mutator.Attributes & ~MethodAttributes.Abstract,
                                               mutator.CallingConvention, mutator.ReturnType, argTypes);
            tb.DefineMethodOverride(method, mutator);
            il = method.GetILGenerator();
            il.DeclareLocal(typeof(object[]));

            il.Emit(OpCodes.Ldarg_1); 
            il.Emit(OpCodes.Stloc_0);

            for (int i = 0; i < length; i++)
            {
                il.Emit(OpCodes.Ldarg_0); 
                il.Emit(OpCodes.Ldfld, sobjectField); 
                il.Emit(OpCodes.Ldloc_0);
                il.Emit(OpCodes.Ldc_I4, i);
                il.Emit(OpCodes.Ldelem, typeof(object));
                il.Emit(rubricBuilders[i].Type.IsValueType ?
                                             OpCodes.Unbox_Any :
                                             OpCodes.Castclass,
                                             rubricBuilders[i].Type); 

                if (rubricBuilders[i].Field == null || rubricBuilders[i].Field.IsBackingField)
                {
                    il.EmitCall(OpCodes.Call, rubricBuilders[i].Setter, null); 
                }
                else
                    il.Emit(OpCodes.Stfld, rubricBuilders[i].Field.RubricInfo); 
            }

            il.Emit(OpCodes.Ret);
        }

        /// <summary>
        /// Gets the type builder.
        /// </summary>
        /// <param name="typeName">Name of the type.</param>
        /// <returns>TypeBuilder.</returns>
        public override TypeBuilder GetTypeBuilder(string typeName)
        {
            string typeSignature = (typeName != null && typeName != "") ? typeName : Unique.New.ToString();
            AssemblyName an = new AssemblyName(typeSignature);

            AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(an, AssemblyBuilderAccess.RunAndCollect);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule(typeSignature + "Module");
            TypeBuilder tb = null;

            tb = moduleBuilder.DefineType(typeSignature, TypeAttributes.AnsiClass | TypeAttributes.Public | TypeAttributes.Class |
                                                         TypeAttributes.Serializable);

            tb.SetCustomAttribute(new CustomAttributeBuilder(typeof(DataContractAttribute)
                                        .GetConstructor(Type.EmptyTypes), new object[0]));

            tb.AddInterfaceImplementation(typeof(ISleeve));

            tb.SetParent(sleeve.BaseType);

            return tb;
        }

        /// <summary>
        /// Creates the field.
        /// </summary>
        /// <param name="tb">The tb.</param>
        /// <param name="mr">The mr.</param>
        /// <param name="type">The type.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns>FieldBuilder.</returns>
        private FieldBuilder createField(TypeBuilder tb, MemberRubric mr, Type type, string fieldName)
        {
            if (type == typeof(string) || type.IsArray)
            {
                FieldBuilder fb = tb.DefineField(fieldName, type, FieldAttributes.Private | FieldAttributes.HasDefault | FieldAttributes.HasFieldMarshal);

                if (type == typeof(string))
                    ResolveMarshalAsAttributeForString(fb, mr, type);
                else
                    ResolveMarshalAsAttributeForArray(fb, mr, type);

                return fb;
            }
            else
            {
                return tb.DefineField(fieldName, type, FieldAttributes.Private);
            }
        }

        #endregion
    }
}
