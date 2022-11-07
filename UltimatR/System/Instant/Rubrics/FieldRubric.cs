
// <copyright file="FieldRubric.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The Instant namespace.
/// </summary>
namespace System.Instant
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Emit;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Class FieldRubric.
    /// Implements the <see cref="System.Reflection.FieldInfo" />
    /// Implements the <see cref="System.Instant.IMemberRubric" />
    /// </summary>
    /// <seealso cref="System.Reflection.FieldInfo" />
    /// <seealso cref="System.Instant.IMemberRubric" />
    public class FieldRubric : FieldInfo, IMemberRubric
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldRubric" /> class.
        /// </summary>
        public FieldRubric()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="FieldRubric" /> class.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="size">The size.</param>
        /// <param name="fieldId">The field identifier.</param>
        public FieldRubric(FieldInfo field, int size = -1, int fieldId = -1) : this(field.FieldType, field.Name, size, fieldId)
        {
            if (!(field is FieldBuilder))
            {
                if (field.GetCustomAttribute(typeof(CompilerGeneratedAttribute)) != null)
                {
                    string name = field.Name;
                    int end = name.LastIndexOf('>'), start = (name.IndexOf('<') + 1), length = end - start;
                    RubricName = new String(field.Name.ToCharArray(start, length));
                    IsBackingField = true;
                }
            }
            RubricInfo = field;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="FieldRubric" /> class.
        /// </summary>
        /// <param name="fieldType">Type of the field.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="size">The size.</param>
        /// <param name="fieldId">The field identifier.</param>
        public FieldRubric(Type fieldType, string fieldName, int size = -1, int fieldId = -1)
        {
            RubricType = fieldType;
            RubricName = fieldName;
            FieldName = fieldName;
            RubricId = fieldId;
            if (size > 0)
                RubricSize = size;
            else
            {
                if (!fieldType.IsGenericType)
                {
                    if (fieldType.IsValueType)
                    {
                        if (fieldType == typeof(DateTime))
                            RubricSize = 8;
                        else if (fieldType.IsEnum)
                            RubricSize = 4;
                        else
                            RubricSize = Marshal.SizeOf(fieldType);
                    }
                    else
                    {
                        RubricSize = Marshal.SizeOf(typeof(IntPtr));
                    }

                    if (size > 0)
                        RubricSize = size;
                }
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the attributes associated with this field.
        /// </summary>
        /// <value>The attributes.</value>
        public override FieldAttributes Attributes => RubricInfo != null ? RubricInfo.Attributes : FieldAttributes.HasDefault;

        /// <summary>
        /// Gets the class that declares this member.
        /// </summary>
        /// <value>The type of the declaring.</value>
        public override Type DeclaringType => RubricInfo != null ? RubricInfo.DeclaringType : null;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="FieldRubric" /> is editable.
        /// </summary>
        /// <value><c>true</c> if editable; otherwise, <c>false</c>.</value>
        public bool Editable { get; set; } = true;

        /// <summary>
        /// Gets a <see langword="RuntimeFieldHandle" />, which is a handle to the internal metadata representation of a field.
        /// </summary>
        /// <value>The field handle.</value>
        /// <exception cref="System.NotImplementedException"></exception>
        public override RuntimeFieldHandle FieldHandle => RubricInfo != null ? RubricInfo.FieldHandle : throw new NotImplementedException();

        /// <summary>
        /// Gets or sets the name of the field.
        /// </summary>
        /// <value>The name of the field.</value>
        public string FieldName { get; set; }

        /// <summary>
        /// Gets the type of this field object.
        /// </summary>
        /// <value>The type of the field.</value>
        public override Type FieldType => RubricType;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is backing field.
        /// </summary>
        /// <value><c>true</c> if this instance is backing field; otherwise, <c>false</c>.</value>
        public bool IsBackingField { get; set; }

        /// <summary>
        /// Gets the name of the current member.
        /// </summary>
        /// <value>The name.</value>
        public override string Name => RubricName;

        /// <summary>
        /// Gets the class object that was used to obtain this instance of <see langword="MemberInfo" />.
        /// </summary>
        /// <value>The type of the reflected.</value>
        public override Type ReflectedType => RubricInfo != null ? RubricInfo.ReflectedType : null;

        /// <summary>
        /// Gets or sets the rubric attributes.
        /// </summary>
        /// <value>The rubric attributes.</value>
        public object[] RubricAttributes { get; set; }

        /// <summary>
        /// Gets or sets the rubric identifier.
        /// </summary>
        /// <value>The rubric identifier.</value>
        public int RubricId { get; set; }

        /// <summary>
        /// Gets the member information.
        /// </summary>
        /// <value>The member information.</value>
        public MemberInfo MemberInfo => RubricInfo;

        /// <summary>
        /// Gets or sets the rubric information.
        /// </summary>
        /// <value>The rubric information.</value>
        public FieldInfo RubricInfo { get; set; }

        /// <summary>
        /// Gets or sets the rubric module.
        /// </summary>
        /// <value>The rubric module.</value>
        /// <exception cref="System.NotImplementedException"></exception>
        public Module RubricModule { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        /// <summary>
        /// Gets or sets the name of the rubric.
        /// </summary>
        /// <value>The name of the rubric.</value>
        public string RubricName { get; set; }

        /// <summary>
        /// Gets or sets the rubric offset.
        /// </summary>
        /// <value>The rubric offset.</value>
        public int RubricOffset { get; set; }

        /// <summary>
        /// Gets or sets the rubric parameter information.
        /// </summary>
        /// <value>The rubric parameter information.</value>
        /// <exception cref="System.NotImplementedException"></exception>
        public PropertyInfo[] RubricParameterInfo { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        /// <summary>
        /// Gets or sets the type of the rubric return.
        /// </summary>
        /// <value>The type of the rubric return.</value>
        /// <exception cref="System.NotImplementedException"></exception>
        public Type RubricReturnType { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        /// <summary>
        /// Gets or sets the size of the rubric.
        /// </summary>
        /// <value>The size of the rubric.</value>
        public int RubricSize { get; set; }

        /// <summary>
        /// Gets or sets the type of the rubric.
        /// </summary>
        /// <value>The type of the rubric.</value>
        public Type RubricType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="FieldRubric" /> is visible.
        /// </summary>
        /// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
        public bool Visible { get; set; } = true;

        #endregion

        #region Methods

        /// <summary>
        /// When overridden in a derived class, returns an array of all custom attributes applied to this member.
        /// </summary>
        /// <param name="inherit"><see langword="true" /> to search this member's inheritance chain to find the attributes; otherwise, <see langword="false" />. This parameter is ignored for properties and events.</param>
        /// <returns>An array that contains all the custom attributes applied to this member, or an array with zero elements if no attributes are defined.</returns>
        public override object[] GetCustomAttributes(bool inherit)
        {
            if (RubricAttributes != null)
                return RubricAttributes;

            RubricAttributes = new object[0];
            if (RubricInfo != null)
            {
                var attrib = RubricInfo.GetCustomAttributes(inherit);
                if (attrib != null)
                {
                    if (RubricType.IsArray || RubricType == typeof(string))
                    {
                        if (attrib.Where(r => r is MarshalAsAttribute).Any())
                        {
                            attrib.Where(r => r is MarshalAsAttribute).Cast<MarshalAsAttribute>().Select(a => RubricSize = a.SizeConst).ToArray();
                            return RubricAttributes = attrib;
                        }
                        else
                            RubricAttributes.Concat(attrib).ToArray();
                    }
                    else
                        return RubricAttributes.Concat(attrib).ToArray();
                }
            }

            if (RubricType == typeof(string))
            {
                if (RubricSize < 1)
                    RubricSize = 16;
                return new[] { new MarshalAsAttribute(UnmanagedType.ByValTStr) { SizeConst = RubricSize } };
            }
            else if (RubricType.IsArray)
            {
                if (RubricSize < 1)
                    RubricSize = 8;

                if (RubricType == typeof(byte[]))
                    return RubricAttributes.Concat(new[] { new MarshalAsAttribute(UnmanagedType.ByValArray) { SizeConst = RubricSize, ArraySubType = UnmanagedType.U1 } }).ToArray();
                if (RubricType == typeof(char[]))
                    return RubricAttributes.Concat(new[] { new MarshalAsAttribute(UnmanagedType.ByValArray) { SizeConst = RubricSize, ArraySubType = UnmanagedType.U1 } }).ToArray();
                if (RubricType == typeof(int[]))
                    return RubricAttributes.Concat(new[] { new MarshalAsAttribute(UnmanagedType.ByValArray) { SizeConst = RubricSize / 4, ArraySubType = UnmanagedType.I4 } }).ToArray();
                if (RubricType == typeof(long[]))
                    return RubricAttributes.Concat(new[] { new MarshalAsAttribute(UnmanagedType.ByValArray) { SizeConst = RubricSize / 8, ArraySubType = UnmanagedType.I8 } }).ToArray();
                if (RubricType == typeof(float[]))
                    return RubricAttributes.Concat(new[] { new MarshalAsAttribute(UnmanagedType.ByValArray) { SizeConst = RubricSize / 4, ArraySubType = UnmanagedType.R4 } }).ToArray();
                if (RubricType == typeof(double[]))
                    return RubricAttributes.Concat(new[] { new MarshalAsAttribute(UnmanagedType.ByValArray) { SizeConst = RubricSize / 8, ArraySubType = UnmanagedType.R8 } }).ToArray();
            }
            return null;
        }

        /// <summary>
        /// When overridden in a derived class, returns an array of custom attributes applied to this member and identified by <see cref="T:System.Type" />.
        /// </summary>
        /// <param name="attributeType">The type of attribute to search for. Only attributes that are assignable to this type are returned.</param>
        /// <param name="inherit"><see langword="true" /> to search this member's inheritance chain to find the attributes; otherwise, <see langword="false" />. This parameter is ignored for properties and events.</param>
        /// <returns>An array of custom attributes applied to this member, or an array with zero elements if no attributes assignable to <paramref name="attributeType" /> have been applied.</returns>
        public override object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            var attribs = this.GetCustomAttributes(inherit);
            if (attribs != null)
                attribs = attribs.Where(r => r.GetType() == attributeType).ToArray();
            return attribs;
        }

        /// <summary>
        /// When overridden in a derived class, returns the value of a field supported by a given object.
        /// </summary>
        /// <param name="obj">The object whose field value will be returned.</param>
        /// <returns>An object containing the value of the field reflected by this instance.</returns>
        public override object GetValue(object obj)
        {
            if (RubricId < 0)
                return ((IFigure)obj)[RubricName];
            return ((IFigure)obj)[RubricId];
        }

        /// <summary>
        /// When overridden in a derived class, indicates whether one or more attributes of the specified type or of its derived types is applied to this member.
        /// </summary>
        /// <param name="attributeType">The type of custom attribute to search for. The search includes derived types.</param>
        /// <param name="inherit"><see langword="true" /> to search this member's inheritance chain to find the attributes; otherwise, <see langword="false" />. This parameter is ignored for properties and events.</param>
        /// <returns><see langword="true" /> if one or more instances of <paramref name="attributeType" /> or any of its derived types is applied to this member; otherwise, <see langword="false" />.</returns>
        public override bool IsDefined(Type attributeType, bool inherit)
        {
            if (this.GetCustomAttributes(attributeType, inherit) != null)
                return true;
            return false;
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="value">The value.</param>
        /// <param name="invokeAttr">The invoke attribute.</param>
        /// <param name="binder">The binder.</param>
        /// <param name="culture">The culture.</param>
        public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, CultureInfo culture)
        {
            if (RubricId < 0)
                ((IFigure)obj)[RubricName] = value;
            ((IFigure)obj)[RubricId] = value;
        }

        #endregion
    }
}
