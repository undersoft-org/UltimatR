
// <copyright file="PropertyRubric.cs" company="UltimatR.Core">
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
    using System.Runtime.InteropServices;

    /// <summary>
    /// Class PropertyRubric.
    /// Implements the <see cref="System.Reflection.PropertyInfo" />
    /// Implements the <see cref="System.Instant.IMemberRubric" />
    /// </summary>
    /// <seealso cref="System.Reflection.PropertyInfo" />
    /// <seealso cref="System.Instant.IMemberRubric" />
    public class PropertyRubric : PropertyInfo, IMemberRubric
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyRubric" /> class.
        /// </summary>
        public PropertyRubric()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyRubric" /> class.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="size">The size.</param>
        /// <param name="propertyId">The property identifier.</param>
        public PropertyRubric(PropertyInfo property, int size = -1, int propertyId = -1) : this(property.PropertyType, property.Name, propertyId)
        {
            RubricInfo = property;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyRubric" /> class.
        /// </summary>
        /// <param name="propertyType">Type of the property.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="size">The size.</param>
        /// <param name="propertyId">The property identifier.</param>
        public PropertyRubric(Type propertyType, string propertyName, int size = -1, int propertyId = -1)
        {
            RubricType = propertyType;
            RubricName = propertyName;
            RubricId = propertyId;
            if (!propertyType.IsGenericType)
            {
                if (propertyType.IsValueType)
                {
                    if (propertyType == typeof(DateTime))
                        RubricSize = 8;
                    else if (propertyType.IsEnum)
                        RubricSize = 4;
                    else
                        RubricSize = Marshal.SizeOf(propertyType);
                }

                if (size > 0)
                    RubricSize = size;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the attributes for this property.
        /// </summary>
        /// <value>The attributes.</value>
        public override PropertyAttributes Attributes => RubricInfo != null ? RubricInfo.Attributes : PropertyAttributes.HasDefault;

        /// <summary>
        /// Gets a value indicating whether the property can be read.
        /// </summary>
        /// <value><c>true</c> if this instance can read; otherwise, <c>false</c>.</value>
        public override bool CanRead => Visible;

        /// <summary>
        /// Gets a value indicating whether the property can be written to.
        /// </summary>
        /// <value><c>true</c> if this instance can write; otherwise, <c>false</c>.</value>
        public override bool CanWrite => Editable;

        /// <summary>
        /// Gets the class that declares this member.
        /// </summary>
        /// <value>The type of the declaring.</value>
        public override Type DeclaringType => RubricInfo != null ? RubricInfo.DeclaringType : null;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="IMemberRubric" /> is editable.
        /// </summary>
        /// <value><c>true</c> if editable; otherwise, <c>false</c>.</value>
        public bool Editable { get; set; } = true;

        /// <summary>
        /// Gets the name of the current member.
        /// </summary>
        /// <value>The name.</value>
        public override string Name => RubricName;

        /// <summary>
        /// Gets the type of this property.
        /// </summary>
        /// <value>The type of the property.</value>
        public override Type PropertyType => RubricType;

        /// <summary>
        /// Gets the class object that was used to obtain this instance of <see langword="MemberInfo" />.
        /// </summary>
        /// <value>The type of the reflected.</value>
        public override Type ReflectedType => RubricInfo != null ? RubricInfo.ReflectedType : null;

        /// <summary>
        /// Gets or sets the rubric attributes.
        /// </summary>
        /// <value>The rubric attributes.</value>
        public object[] RubricAttributes { get; set; } = null;

        /// <summary>
        /// Gets or sets the rubric identifier.
        /// </summary>
        /// <value>The rubric identifier.</value>
        public int RubricId { get; set; } = -1;

        /// <summary>
        /// Gets the member information.
        /// </summary>
        /// <value>The member information.</value>
        public MemberInfo MemberInfo => RubricInfo;

        /// <summary>
        /// Gets or sets the rubric information.
        /// </summary>
        /// <value>The rubric information.</value>
        public PropertyInfo RubricInfo { get; set; }

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
        public int RubricOffset { get; set; } = -1;

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
        public int RubricSize { get; set; } = -1;

        /// <summary>
        /// Gets or sets the type of the rubric.
        /// </summary>
        /// <value>The type of the rubric.</value>
        public Type RubricType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="IMemberRubric" /> is visible.
        /// </summary>
        /// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
        public bool Visible { get; set; } = true;

        #endregion

        #region Methods

        /// <summary>
        /// Returns an array whose elements reflect the public and, if specified, non-public <see langword="get" /> and <see langword="set" /> accessors of the property reflected by the current instance.
        /// </summary>
        /// <param name="nonPublic">Indicates whether non-public methods should be returned in the returned array. <see langword="true" /> if non-public methods are to be included; otherwise, <see langword="false" />.</param>
        /// <returns>An array whose elements reflect the <see langword="get" /> and <see langword="set" /> accessors of the property reflected by the current instance. If <paramref name="nonPublic" /> is <see langword="true" />, this array contains public and non-public <see langword="get" /> and <see langword="set" /> accessors. If <paramref name="nonPublic" /> is <see langword="false" />, this array contains only public <see langword="get" /> and <see langword="set" /> accessors. If no accessors with the specified visibility are found, this method returns an array with zero (0) elements.</returns>
        public override MethodInfo[] GetAccessors(bool nonPublic)
        {
            if (RubricInfo != null)
                return RubricInfo.GetAccessors(nonPublic);
            return null;
        }

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
                        var _attrib = attrib.Where(r => r is FigureAsAttribute).ToArray();
                        if (_attrib.Any())
                        {
                            _attrib.Cast<FigureAsAttribute>().Select(a => RubricSize = a.SizeConst).ToArray();
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
                var _attrib = RubricAttributes.Where(r => r is MarshalAsAttribute).ToArray();
                if (_attrib.Any())
                {
                    _attrib.Cast<MarshalAsAttribute>().Select(a => RubricSize = a.SizeConst).ToArray();
                    return RubricAttributes;
                }

                return new[] { new MarshalAsAttribute(UnmanagedType.ByValTStr) { SizeConst = RubricSize } };
            }
            else if (RubricType.IsArray)
            {
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
            {
                attribs = attribs.Where(r => r.GetType() == attributeType).ToArray();
                if (!attribs.Any())
                    return null;
            }
            return attribs;
        }

        /// <summary>
        /// When overridden in a derived class, returns the public or non-public <see langword="get" /> accessor for this property.
        /// </summary>
        /// <param name="nonPublic">Indicates whether a non-public <see langword="get" /> accessor should be returned. <see langword="true" /> if a non-public accessor is to be returned; otherwise, <see langword="false" />.</param>
        /// <returns>A <see langword="MethodInfo" /> object representing the <see langword="get" /> accessor for this property, if <paramref name="nonPublic" /> is <see langword="true" />. Returns <see langword="null" /> if <paramref name="nonPublic" /> is <see langword="false" /> and the <see langword="get" /> accessor is non-public, or if <paramref name="nonPublic" /> is <see langword="true" /> but no <see langword="get" /> accessors exist.</returns>
        public override MethodInfo GetGetMethod(bool nonPublic)
        {
            if (RubricInfo != null)
                return RubricInfo.GetGetMethod(nonPublic);
            return null;
        }

        /// <summary>
        /// When overridden in a derived class, returns an array of all the index parameters for the property.
        /// </summary>
        /// <returns>An array of type <see langword="ParameterInfo" /> containing the parameters for the indexes. If the property is not indexed, the array has 0 (zero) elements.</returns>
        public override ParameterInfo[] GetIndexParameters()
        {
            if (RubricInfo != null)
                return RubricInfo.GetIndexParameters();
            return null;
        }

        /// <summary>
        /// When overridden in a derived class, returns the <see langword="set" /> accessor for this property.
        /// </summary>
        /// <param name="nonPublic">Indicates whether the accessor should be returned if it is non-public. <see langword="true" /> if a non-public accessor is to be returned; otherwise, <see langword="false" />.</param>
        /// <returns>This property's <see langword="Set" /> method, or <see langword="null" />, as shown in the following table.
        /// <list type="table"><listheader><term> Value</term><description> Condition</description></listheader><item><term> The <see langword="Set" /> method for this property.</term><description> The <see langword="set" /> accessor is public, OR <paramref name="nonPublic" /> is <see langword="true" /> and the <see langword="set" /> accessor is non-public.</description></item><item><term><see langword="null" /></term><description><paramref name="nonPublic" /> is <see langword="true" />, but the property is read-only, OR <paramref name="nonPublic" /> is <see langword="false" /> and the <see langword="set" /> accessor is non-public, OR there is no <see langword="set" /> accessor.</description></item></list></returns>
        public override MethodInfo GetSetMethod(bool nonPublic)
        {
            if (RubricInfo != null)
                return RubricInfo.GetSetMethod(nonPublic);
            return null;
        }

        /// <summary>
        /// When overridden in a derived class, returns the property value of a specified object that has the specified binding, index, and culture-specific information.
        /// </summary>
        /// <param name="obj">The object whose property value will be returned.</param>
        /// <param name="invokeAttr">A bitwise combination of the following enumeration members that specify the invocation attribute: <see langword="InvokeMethod" />, <see langword="CreateInstance" />, <see langword="Static" />, <see langword="GetField" />, <see langword="SetField" />, <see langword="GetProperty" />, and <see langword="SetProperty" />. You must specify a suitable invocation attribute. For example, to invoke a static member, set the <see langword="Static" /> flag.</param>
        /// <param name="binder">An object that enables the binding, coercion of argument types, invocation of members, and retrieval of <see cref="T:System.Reflection.MemberInfo" /> objects through reflection. If <paramref name="binder" /> is <see langword="null" />, the default binder is used.</param>
        /// <param name="index">Optional index values for indexed properties. This value should be <see langword="null" /> for non-indexed properties.</param>
        /// <param name="culture">The culture for which the resource is to be localized. If the resource is not localized for this culture, the <see cref="P:System.Globalization.CultureInfo.Parent" /> property will be called successively in search of a match. If this value is <see langword="null" />, the culture-specific information is obtained from the <see cref="P:System.Globalization.CultureInfo.CurrentUICulture" /> property.</param>
        /// <returns>The property value of the specified object.</returns>
        public override object GetValue(object obj, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture)
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
        /// <param name="index">The index.</param>
        /// <param name="culture">The culture.</param>
        public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture)
        {
            if (RubricId < 0)
                ((IFigure)obj)[RubricName] = value;
            ((IFigure)obj)[RubricId] = value;
        }

        #endregion
    }
}
