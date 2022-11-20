//-----------------------------------------------------------------------
// <copyright file="SleeveCompilerBase.cs" company="Undersoft">
//     Author: Dariusz Hanc
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System.ComponentModel.DataAnnotations;

namespace System.Instant
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Emit;
    using System.Runtime.InteropServices;
    using System.Series;
    using System.Uniques;

    /// <summary>
    /// Class SleeveCompilerBase. Implements the <see cref="System.Instant.CompilerConstructors"/>
    /// </summary>
    /// <seealso cref="System.Instant.CompilerConstructors"/>
    public abstract class SleeveCompilerBase : CompilerConstructors
    {
        /// <summary>
        /// The length
        /// </summary>
        protected int length;
        /// <summary>
        /// The mode
        /// </summary>
        protected FigureMode mode;
        /// <summary>
        /// The scode
        /// </summary>
        protected int scode = 1;
        /// <summary>
        /// The sleeve
        /// </summary>
        protected Sleeve sleeve;
        /// <summary>
        /// The identities
        /// </summary>
        public SortedList<short, MemberRubric> Identities = new SortedList<short, MemberRubric>();


        /// <summary>
        /// The rubric builders
        /// </summary>
        public IDeck<RubricBuilder> rubricBuilders;


        /// <summary>
        /// Initializes a new instance of the <see cref="SleeveCompilerBase"/> class.
        /// </summary>
        /// <param name="instantSleeve">The instant sleeve.</param>
        /// <param name="rubricBuilders">The rubric builders.</param>
        public SleeveCompilerBase(Sleeve instantSleeve, IDeck<RubricBuilder> rubricBuilders)
        {
            sleeve = instantSleeve;
            this.rubricBuilders = rubricBuilders;
            length = this.rubricBuilders.Count;
        }

        /// <summary>
        /// Resolves the figure display attributes.
        /// </summary>
        /// <param name="fb">The fb.</param>
        /// <param name="mi">The mi.</param>
        /// <param name="mr">The mr.</param>
        void resolveFigureDisplayAttributes(FieldBuilder fb, MemberInfo mi, MemberRubric mr)
        {
            object[] o = mi.GetCustomAttributes(typeof(FigureDisplayAttribute), false);
            if((o != null) && o.Any())
            {
                FigureDisplayAttribute fda = (FigureDisplayAttribute)o.First();
                ;
                mr.DisplayName = fda.Name;

                if(fb != null)
                    CreateFigureDisplayAttribute(fb, fda);
            } else if(mr.DisplayName != null)
            {
                CreateFigureDisplayAttribute(fb, new FigureDisplayAttribute(mr.DisplayName));
            }
        }

        /// <summary>
        /// Resolves the figure identity attributes.
        /// </summary>
        /// <param name="fb">The fb.</param>
        /// <param name="mi">The mi.</param>
        /// <param name="mr">The mr.</param>
        void resolveFigureIdentityAttributes(FieldBuilder fb, MemberInfo mi, MemberRubric mr)
        {
            if(!mr.IsKey)
            {
                object[] o = mi.GetCustomAttributes(typeof(FigureIdentityAttribute), false);
                if((o != null) && o.Any())
                {
                    FigureIdentityAttribute fia = (FigureIdentityAttribute)o.First();
                    mr.IsIdentity = true;
                    mr.IsAutoincrement = fia.IsAutoincrement;

                    if(Identities.ContainsKey(fia.Order))
                        fia.Order = (short)(Identities.LastOrDefault().Key + 1);

                    mr.IdentityOrder = fia.Order;
                    Identities.Add(mr.IdentityOrder, mr);

                    if(fb != null)
                        CreateFigureIdentityAttribute(fb, fia);
                } else if(mr.IsIdentity)
                {
                    if(Identities.ContainsKey(mr.IdentityOrder))
                        mr.IdentityOrder += (short)(Identities.LastOrDefault().Key + 1);

                    Identities.Add(mr.IdentityOrder, mr);

                    if(fb != null)
                        CreateFigureIdentityAttribute(
                            fb,
                            new FigureIdentityAttribute
                            {
                                IsAutoincrement = mr.IsAutoincrement,
                                Order = mr.IdentityOrder
                            });
                }
            }
        }

        /// <summary>
        /// Resolves the figure key attributes.
        /// </summary>
        /// <param name="fb">The fb.</param>
        /// <param name="mi">The mi.</param>
        /// <param name="mr">The mr.</param>
        void resolveFigureKeyAttributes(FieldBuilder fb, MemberInfo mi, MemberRubric mr)
        {
            object[] o = mi.GetCustomAttributes(typeof(KeyAttribute), false);
            if((o == null) || !o.Any())
                o = mi.GetCustomAttributes(typeof(FigureKeyAttribute), false);
            else
                o[0] = new FigureKeyAttribute();

            if((o != null) && o.Any())
            {
                FigureKeyAttribute fka = (FigureKeyAttribute)o.First();
                mr.IsKey = true;
                mr.IsIdentity = true;
                mr.IsAutoincrement = fka.IsAutoincrement;

                if(Identities.ContainsKey(fka.Order))
                    fka.Order = (short)(Identities.LastOrDefault().Key + 1);

                mr.IdentityOrder = fka.Order;
                Identities.Add(mr.IdentityOrder, mr);
                mr.Required = true;

                if(fb != null)
                    CreateFigureKeyAttribute(fb, fka);
            } else if(mr.IsKey)
            {
                mr.IsIdentity = true;
                mr.Required = true;

                if(Identities.ContainsKey(mr.IdentityOrder))
                    mr.IdentityOrder += (short)(Identities.LastOrDefault().Key + 1);

                Identities.Add(mr.IdentityOrder, mr);

                if(fb != null)
                    CreateFigureKeyAttribute(
                        fb,
                        new FigureKeyAttribute { IsAutoincrement = mr.IsAutoincrement, Order = mr.IdentityOrder });
            }
        }

        /// <summary>
        /// Resolves the figure rquired attributes.
        /// </summary>
        /// <param name="fb">The fb.</param>
        /// <param name="mi">The mi.</param>
        /// <param name="mr">The mr.</param>
        void resolveFigureRquiredAttributes(FieldBuilder fb, MemberInfo mi, MemberRubric mr)
        {
            object[] o = mi.GetCustomAttributes(typeof(FigureRequiredAttribute), false);
            if((o != null) && o.Any())
            {
                mr.Required = true;

                if(fb != null)
                    CreateFigureRequiredAttribute(fb);
            } else if(mr.Required)
            {
                if(fb != null)
                    CreateFigureRequiredAttribute(fb);
            }
        }

        /// <summary>
        /// Resolves the figure treatment attributes.
        /// </summary>
        /// <param name="fb">The fb.</param>
        /// <param name="mi">The mi.</param>
        /// <param name="mr">The mr.</param>
        void resolveFigureTreatmentAttributes(FieldBuilder fb, MemberInfo mi, MemberRubric mr)
        {
            object[] o = mi.GetCustomAttributes(typeof(FigureTreatmentAttribute), false);
            if((o != null) && o.Any())
            {
                FigureTreatmentAttribute fta = (FigureTreatmentAttribute)o.First();
                ;

                mr.SummaryOperand = fta.SummaryOperand;

                if(fb != null)
                    CreateFigureTreatmentAttribute(fb, fta);
            } else if(mr.SummaryOperand != SummarizeOperand.None)
            {
                CreateFigureTreatmentAttribute(fb, new FigureTreatmentAttribute { SummaryOperand = mr.SummaryOperand });
            }
        }


        /// <summary>
        /// Compiles the type of the sleeve.
        /// </summary>
        /// <param name="typeName">Name of the type.</param>
        /// <returns>Type.</returns>
        public abstract Type CompileSleeveType(string typeName);

        /// <summary>
        /// Creates the compare to method.
        /// </summary>
        /// <param name="tb">The tb.</param>
        public abstract void CreateCompareToMethod(TypeBuilder tb);

        /// <summary>
        /// Creates the equals method.
        /// </summary>
        /// <param name="tb">The tb.</param>
        public abstract void CreateEqualsMethod(TypeBuilder tb);

        /// <summary>
        /// Creates the fields and properties.
        /// </summary>
        /// <param name="tb">The tb.</param>
        public abstract void CreateFieldsAndProperties(TypeBuilder tb);

        /// <summary>
        /// Creates the figure as attribute.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="attrib">The attribute.</param>
        public void CreateFigureAsAttribute(FieldBuilder field, FigureAsAttribute attrib)
        {
            field.SetCustomAttribute(
                new CustomAttributeBuilder(
                    marshalAsCtor,
                    new object[] { attrib.Value },
                    new FieldInfo[] { typeof(MarshalAsAttribute).GetField("SizeConst") },
                    new object[] { attrib.SizeConst }));
        }

        /// <summary>
        /// Creates the figure display attribute.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="attrib">The attribute.</param>
        public void CreateFigureDisplayAttribute(FieldBuilder field, FigureDisplayAttribute attrib)
        { field.SetCustomAttribute(new CustomAttributeBuilder(figureDisplayCtor, new object[] { attrib.Name })); }

        /// <summary>
        /// Creates the figure identity attribute.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="attrib">The attribute.</param>
        public void CreateFigureIdentityAttribute(FieldBuilder field, FigureIdentityAttribute attrib)
        {
            field.SetCustomAttribute(
                new CustomAttributeBuilder(
                    figureIdentityCtor,
                    Type.EmptyTypes,
                    new FieldInfo[]
                {
                    typeof(FigureIdentityAttribute).GetField("Order"),
                    typeof(FigureIdentityAttribute).GetField("IsAutoincrement")
                },
                    new object[] { attrib.Order, attrib.IsAutoincrement }));
        }

        /// <summary>
        /// Creates the figure key attribute.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="attrib">The attribute.</param>
        public void CreateFigureKeyAttribute(FieldBuilder field, FigureKeyAttribute attrib)
        {
            field.SetCustomAttribute(
                new CustomAttributeBuilder(
                    figureKeyCtor,
                    Type.EmptyTypes,
                    new FieldInfo[]
                {
                    typeof(FigureKeyAttribute).GetField("Order"),
                    typeof(FigureKeyAttribute).GetField("IsAutoincrement")
                },
                    new object[] { attrib.Order, attrib.IsAutoincrement }));
        }

        /// <summary>
        /// Creates the figure required attribute.
        /// </summary>
        /// <param name="field">The field.</param>
        public void CreateFigureRequiredAttribute(FieldBuilder field)
        { field.SetCustomAttribute(new CustomAttributeBuilder(figureRequiredCtor, Type.EmptyTypes)); }

        /// <summary>
        /// Creates the figure treatment attribute.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="attrib">The attribute.</param>
        public void CreateFigureTreatmentAttribute(FieldBuilder field, FigureTreatmentAttribute attrib)
        {
            field.SetCustomAttribute(
                new CustomAttributeBuilder(
                    figuresTreatmentCtor,
                    Type.EmptyTypes,
                    new FieldInfo[]
                {
                    typeof(FigureTreatmentAttribute).GetField("AggregateOperand"),
                    typeof(FigureTreatmentAttribute).GetField("SummaryOperand")
                },
                    new object[] { attrib.SummaryOperand }));
        }

        /// <summary>
        /// Creates the get bytes method.
        /// </summary>
        /// <param name="tb">The tb.</param>
        public abstract void CreateGetBytesMethod(TypeBuilder tb);

        /// <summary>
        /// Creates the get empty property.
        /// </summary>
        /// <param name="tb">The tb.</param>
        public virtual void CreateGetEmptyProperty(TypeBuilder tb)
        {
            PropertyBuilder prop = tb.DefineProperty(
                "Empty",
                PropertyAttributes.HasDefault,
                typeof(IUnique),
                Type.EmptyTypes);

            PropertyInfo iprop = typeof(IUnique).GetProperty("Empty");

            MethodInfo accessor = iprop.GetGetMethod();

            ParameterInfo[] args = accessor.GetParameters();
            Type[] argTypes = Array.ConvertAll(args, a => a.ParameterType);

            MethodBuilder getter = tb.DefineMethod(
                accessor.Name,
                accessor.Attributes & (~MethodAttributes.Abstract),
                accessor.CallingConvention,
                accessor.ReturnType,
                argTypes);
            tb.DefineMethodOverride(getter, accessor);

            prop.SetGetMethod(getter);
            ILGenerator il = getter.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldflda, rubricBuilders[0].Member.FigureField);
            il.EmitCall(OpCodes.Call, typeof(Ussn).GetMethod("get_Empty"), null);
            il.Emit(OpCodes.Ret);
        }

        /// <summary>
        /// Creates the get generic by int method.
        /// </summary>
        /// <param name="tb">The tb.</param>
        public virtual void CreateGetGenericByIntMethod(TypeBuilder tb)
        {
            string[] typeParameterNames = { "V" };
            GenericTypeParameterBuilder[] typeParameters =
                tb.DefineGenericParameters(typeParameterNames);

            MethodInfo mi = typeof(IFigure).GetMethod("Get", new Type[] { typeof(int) }).GetGenericMethodDefinition();

            ParameterInfo[] args = mi.GetParameters();
            Type[] argTypes = Array.ConvertAll(args, a => a.ParameterType);

            MethodBuilder method = tb.DefineMethod(
                mi.Name,
                mi.Attributes & (~MethodAttributes.Abstract),
                mi.CallingConvention,
                mi.ReturnType,
                argTypes);

            tb.DefineMethodOverride(method, mi);

            ILGenerator il = method.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldflda, rubricBuilders[0].Member.FigureField);
            il.Emit(OpCodes.Ldarg_1);
            il.EmitCall(OpCodes.Call, typeof(Ussn).GetMethod("CompareTo", new Type[] { typeof(IUnique) }), null);
            il.Emit(OpCodes.Ret);
        }

        /// <summary>
        /// Creates the get unique bytes method.
        /// </summary>
        /// <param name="tb">The tb.</param>
        public virtual void CreateGetUniqueBytesMethod(TypeBuilder tb)
        {
            MethodInfo createArray = typeof(IUnique).GetMethod("GetUniqueBytes");

            ParameterInfo[] args = createArray.GetParameters();
            Type[] argTypes = Array.ConvertAll(args, a => a.ParameterType);

            MethodBuilder method = tb.DefineMethod(
                createArray.Name,
                createArray.Attributes & (~MethodAttributes.Abstract),
                createArray.CallingConvention,
                createArray.ReturnType,
                argTypes);
            tb.DefineMethodOverride(method, createArray);

            ILGenerator il = method.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldflda, rubricBuilders[0].Member.FigureField);
            il.EmitCall(OpCodes.Call, typeof(Ussn).GetMethod("GetUniqueBytes"), null);
            il.Emit(OpCodes.Ret);
        }

        /// <summary>
        /// Creates the get unique key method.
        /// </summary>
        /// <param name="tb">The tb.</param>
        public virtual void CreateGetUniqueKeyMethod(TypeBuilder tb)
        {
            MethodInfo createArray = typeof(IUnique).GetMethod("GetUniqueKey");

            ParameterInfo[] args = createArray.GetParameters();
            Type[] argTypes = Array.ConvertAll(args, a => a.ParameterType);

            MethodBuilder method = tb.DefineMethod(
                createArray.Name,
                createArray.Attributes & (~MethodAttributes.Abstract),
                createArray.CallingConvention,
                createArray.ReturnType,
                argTypes);
            tb.DefineMethodOverride(method, createArray);

            ILGenerator il = method.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldflda, rubricBuilders[0].Member.FigureField);
            il.EmitCall(OpCodes.Call, typeof(Ussn).GetMethod("GetUniqueKey"), null);
            il.Emit(OpCodes.Ret);
        }

        /// <summary>
        /// Creates the get unique seed method.
        /// </summary>
        /// <param name="tb">The tb.</param>
        public virtual void CreateGetUniqueSeedMethod(TypeBuilder tb)
        {
            MethodInfo createArray = typeof(IUnique).GetMethod("GetUniqueSeed");

            ParameterInfo[] args = createArray.GetParameters();
            Type[] argTypes = Array.ConvertAll(args, a => a.ParameterType);

            MethodBuilder method = tb.DefineMethod(
                createArray.Name,
                createArray.Attributes & (~MethodAttributes.Abstract),
                createArray.CallingConvention,
                createArray.ReturnType,
                argTypes);
            tb.DefineMethodOverride(method, createArray);

            ILGenerator il = method.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldflda, rubricBuilders[0].Member.FigureField);
            il.EmitCall(OpCodes.Call, typeof(Ussn).GetMethod("GetUniqueSeed"), null);
            il.Emit(OpCodes.Ret);
        }

        /// <summary>
        /// Creates the instance property.
        /// </summary>
        /// <param name="tb">The tb.</param>
        /// <param name="type">The type.</param>
        /// <param name="name">The name.</param>
        public abstract void CreateInstanceProperty(TypeBuilder tb, Type type, string name);

        /// <summary>
        /// Creates the item by int property.
        /// </summary>
        /// <param name="tb">The tb.</param>
        public abstract void CreateItemByIntProperty(TypeBuilder tb);

        /// <summary>
        /// Creates the item by string property.
        /// </summary>
        /// <param name="tb">The tb.</param>
        public abstract void CreateItemByStringProperty(TypeBuilder tb);

        /// <summary>
        /// Creates the marsha asl attribute.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="attrib">The attribute.</param>
        public void CreateMarshaAslAttribute(FieldBuilder field, MarshalAsAttribute attrib)
        {
            field.SetCustomAttribute(
                new CustomAttributeBuilder(
                    marshalAsCtor,
                    new object[] { attrib.Value },
                    new FieldInfo[] { typeof(MarshalAsAttribute).GetField("SizeConst") },
                    new object[] { attrib.SizeConst }));
        }

        /// <summary>
        /// Creates the rubrics property.
        /// </summary>
        /// <param name="tb">The tb.</param>
        /// <param name="type">The type.</param>
        /// <param name="name">The name.</param>
        public abstract void CreateRubricsProperty(TypeBuilder tb, Type type, string name);

        /// <summary>
        /// Creates the serial code property.
        /// </summary>
        /// <param name="tb">The tb.</param>
        /// <param name="type">The type.</param>
        /// <param name="name">The name.</param>
        public abstract void CreateSerialCodeProperty(TypeBuilder tb, Type type, string name);

        /// <summary>
        /// Creates the set unique key method.
        /// </summary>
        /// <param name="tb">The tb.</param>
        public virtual void CreateSetUniqueKeyMethod(TypeBuilder tb)
        {
            MethodInfo createArray = typeof(IUnique).GetMethod("SetUniqueKey");

            ParameterInfo[] args = createArray.GetParameters();
            Type[] argTypes = Array.ConvertAll(args, a => a.ParameterType);

            MethodBuilder method = tb.DefineMethod(
                createArray.Name,
                createArray.Attributes & (~MethodAttributes.Abstract),
                createArray.CallingConvention,
                createArray.ReturnType,
                argTypes);
            tb.DefineMethodOverride(method, createArray);

            ILGenerator il = method.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Ldflda, rubricBuilders[0].Member.FigureField);
            il.EmitCall(OpCodes.Call, typeof(Ussn).GetMethod("SetUniqueKey"), null);
            il.Emit(OpCodes.Ret);
        }

        /// <summary>
        /// Creates the set unique seed method.
        /// </summary>
        /// <param name="tb">The tb.</param>
        public virtual void CreateSetUniqueSeedMethod(TypeBuilder tb)
        {
            MethodInfo createArray = typeof(IUnique).GetMethod("SetUniqueSeed");

            ParameterInfo[] args = createArray.GetParameters();
            Type[] argTypes = Array.ConvertAll(args, a => a.ParameterType);

            MethodBuilder method = tb.DefineMethod(
                createArray.Name,
                createArray.Attributes & (~MethodAttributes.Abstract),
                createArray.CallingConvention,
                createArray.ReturnType,
                argTypes);
            tb.DefineMethodOverride(method, createArray);

            ILGenerator il = method.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Ldflda, rubricBuilders[0].Member.FigureField);
            il.EmitCall(OpCodes.Call, typeof(Ussn).GetMethod("SetUniqueSeed"), null);
            il.Emit(OpCodes.Ret);
        }

        /// <summary>
        /// Creates the unique key property.
        /// </summary>
        /// <param name="tb">The tb.</param>
        public virtual void CreateUniqueKeyProperty(TypeBuilder tb)
        {
            PropertyBuilder prop = tb.DefineProperty(
                "UniqueKey",
                PropertyAttributes.HasDefault,
                typeof(ulong),
                new Type[] { typeof(ulong) });

            PropertyInfo iprop = typeof(IUnique).GetProperty("UniqueKey");

            MethodInfo accessor = iprop.GetGetMethod();

            ParameterInfo[] args = accessor.GetParameters();
            Type[] argTypes = Array.ConvertAll(args, a => a.ParameterType);

            MethodBuilder getter = tb.DefineMethod(
                accessor.Name,
                accessor.Attributes & (~MethodAttributes.Abstract),
                accessor.CallingConvention,
                accessor.ReturnType,
                argTypes);
            tb.DefineMethodOverride(getter, accessor);

            prop.SetGetMethod(getter);
            ILGenerator il = getter.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldflda, rubricBuilders[0].Member.FigureField);
            il.EmitCall(OpCodes.Call, typeof(Ussn).GetProperty("UniqueKey").GetGetMethod(), null);
            il.Emit(OpCodes.Ret);

            MethodInfo mutator = iprop.GetSetMethod();

            args = mutator.GetParameters();
            argTypes = Array.ConvertAll(args, a => a.ParameterType);

            MethodBuilder setter = tb.DefineMethod(
                mutator.Name,
                mutator.Attributes & (~MethodAttributes.Abstract),
                mutator.CallingConvention,
                mutator.ReturnType,
                argTypes);
            tb.DefineMethodOverride(setter, mutator);

            prop.SetSetMethod(setter);
            il = setter.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldflda, rubricBuilders[0].Member.FigureField);
            il.Emit(OpCodes.Ldarg_1);
            il.EmitCall(OpCodes.Call, typeof(Ussn).GetProperty("UniqueKey").GetSetMethod(), null);
            il.Emit(OpCodes.Ret);
        }

        /// <summary>
        /// Creates the unique seed property.
        /// </summary>
        /// <param name="tb">The tb.</param>
        public virtual void CreateUniqueSeedProperty(TypeBuilder tb)
        {
            PropertyBuilder prop = tb.DefineProperty(
                "UniqueSeed",
                PropertyAttributes.HasDefault,
                typeof(ulong),
                new Type[] { typeof(ulong) });

            PropertyInfo iprop = typeof(IUnique).GetProperty("UniqueSeed");

            MethodInfo accessor = iprop.GetGetMethod();

            ParameterInfo[] args = accessor.GetParameters();
            Type[] argTypes = Array.ConvertAll(args, a => a.ParameterType);

            MethodBuilder getter = tb.DefineMethod(
                accessor.Name,
                accessor.Attributes & (~MethodAttributes.Abstract),
                accessor.CallingConvention,
                accessor.ReturnType,
                argTypes);
            tb.DefineMethodOverride(getter, accessor);

            prop.SetGetMethod(getter);
            ILGenerator il = getter.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldflda, rubricBuilders[0].Member.FigureField);
            il.EmitCall(OpCodes.Call, typeof(Ussn).GetProperty("UniqueSeed").GetGetMethod(), null);
            il.Emit(OpCodes.Ret);

            MethodInfo mutator = iprop.GetSetMethod();

            args = mutator.GetParameters();
            argTypes = Array.ConvertAll(args, a => a.ParameterType);

            MethodBuilder setter = tb.DefineMethod(
                mutator.Name,
                mutator.Attributes & (~MethodAttributes.Abstract),
                mutator.CallingConvention,
                mutator.ReturnType,
                argTypes);
            tb.DefineMethodOverride(setter, mutator);

            prop.SetSetMethod(setter);
            il = setter.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldflda, rubricBuilders[0].Member.FigureField);
            il.Emit(OpCodes.Ldarg_1);
            il.EmitCall(OpCodes.Call, typeof(Ussn).GetProperty("UniqueSeed").GetSetMethod(), null);
            il.Emit(OpCodes.Ret);
        }

        /// <summary>
        /// Creates the value array property.
        /// </summary>
        /// <param name="tb">The tb.</param>
        public abstract void CreateValueArrayProperty(TypeBuilder tb);

        /// <summary>
        /// Gets the type builder.
        /// </summary>
        /// <param name="typeName">Name of the type.</param>
        /// <returns>TypeBuilder.</returns>
        public abstract TypeBuilder GetTypeBuilder(string typeName);

        /// <summary>
        /// Resolves the figure attributes.
        /// </summary>
        /// <param name="fb">The fb.</param>
        /// <param name="mr">The mr.</param>
        public void ResolveFigureAttributes(FieldBuilder fb, MemberRubric mr)
        {
            MemberInfo mi = mr.RubricInfo;
            if(!(((IMemberRubric)mi).MemberInfo is FieldBuilder) && !(((IMemberRubric)mi).MemberInfo is PropertyBuilder))
            {
                resolveFigureKeyAttributes(fb, mi, mr);

                resolveFigureIdentityAttributes(fb, mi, mr);

                resolveFigureRquiredAttributes(fb, mi, mr);

                resolveFigureDisplayAttributes(fb, mi, mr);

                resolveFigureTreatmentAttributes(fb, mi, mr);
            }
        }

        /// <summary>
        /// Resolves the marshal as attribute for array.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="member">The member.</param>
        /// <param name="type">The type.</param>
        public void ResolveMarshalAsAttributeForArray(FieldBuilder field, MemberRubric member, Type type)
        {
            MemberInfo _member = member.RubricInfo;
            if((member is MemberRubric) && (member.FigureField != null))
            {
                _member = member.FigureField;
            }

            object[] o = _member.GetCustomAttributes(typeof(MarshalAsAttribute), false);
            if((o == null) || !o.Any())
            {
                o = _member.GetCustomAttributes(typeof(FigureAsAttribute), false);
                if((o != null) && o.Any())
                {
                    FigureAsAttribute faa = (FigureAsAttribute)o.First();
                    CreateFigureAsAttribute(
                        field,
                        new FigureAsAttribute(UnmanagedType.ByValArray)
                        {
                            SizeConst = (faa.SizeConst < 1) ? 64 : faa.SizeConst
                        });
                } else
                {
                    int size = 64;
                    if(member.RubricSize > 0)
                        size = member.RubricSize;
                    CreateFigureAsAttribute(field, new FigureAsAttribute(UnmanagedType.ByValArray) { SizeConst = size });
                }
            } else
            {
                MarshalAsAttribute maa = (MarshalAsAttribute)o.First();
                CreateMarshaAslAttribute(
                    field,
                    new MarshalAsAttribute(UnmanagedType.ByValArray)
                    {
                        SizeConst = (maa.SizeConst < 1) ? 64 : maa.SizeConst
                    });
            }
        }

        /// <summary>
        /// Resolves the marshal as attribute for string.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="member">The member.</param>
        /// <param name="type">The type.</param>
        public void ResolveMarshalAsAttributeForString(FieldBuilder field, MemberRubric member, Type type)
        {
            MemberInfo _member = member.RubricInfo;
            if((member is MemberRubric) && (member.FigureField != null))
            {
                _member = member.FigureField;
            }

            object[] o = _member.GetCustomAttributes(typeof(MarshalAsAttribute), false);
            if((o == null) || !o.Any())
            {
                o = _member.GetCustomAttributes(typeof(FigureAsAttribute), false);
                if((o != null) && o.Any())
                {
                    FigureAsAttribute maa = (FigureAsAttribute)o.First();
                    CreateFigureAsAttribute(
                        field,
                        new FigureAsAttribute(UnmanagedType.ByValTStr)
                        {
                            SizeConst = (maa.SizeConst < 1) ? 64 : maa.SizeConst
                        });
                } else
                {
                    int size = 64;
                    if(member.RubricSize > 0)
                        size = member.RubricSize;
                    CreateFigureAsAttribute(field, new FigureAsAttribute(UnmanagedType.ByValTStr) { SizeConst = size });
                }
            } else
            {
                MarshalAsAttribute maa = (MarshalAsAttribute)o.First();
                CreateMarshaAslAttribute(
                    field,
                    new MarshalAsAttribute(UnmanagedType.ByValTStr)
                    {
                        SizeConst = (maa.SizeConst < 1) ? 64 : maa.SizeConst
                    });
            }
        }
    }
}
