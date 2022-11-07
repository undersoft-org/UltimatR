
// <copyright file="MethodRubric.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The Instant namespace.
/// </summary>
namespace System.Instant
{
    using System;
    using System.Globalization;
    using System.Reflection;




    /// <summary>
    /// Class MethodRubric.
    /// Implements the <see cref="System.Reflection.MethodInfo" />
    /// Implements the <see cref="System.Instant.IMemberRubric" />
    /// </summary>
    /// <seealso cref="System.Reflection.MethodInfo" />
    /// <seealso cref="System.Instant.IMemberRubric" />
    public class MethodRubric : MethodInfo, IMemberRubric
    {
        #region Constructors




        /// <summary>
        /// Initializes a new instance of the <see cref="MethodRubric" /> class.
        /// </summary>
        public MethodRubric()
        {
        }





        /// <summary>
        /// Initializes a new instance of the <see cref="MethodRubric" /> class.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="propertyId">The property identifier.</param>
        public MethodRubric(MethodInfo method, int propertyId = -1) :
            this(method.DeclaringType, method.Name, method.ReturnType, method.GetParameters(), method.Module, propertyId)
        {
            RubricInfo = method;
            RubricType = method.DeclaringType;
            RubricName = method.Name;
            RubricParameterInfo = method.GetParameters();
            RubricReturnType = method.ReturnType;
            RubricModule = method.Module;
        }









        /// <summary>
        /// Initializes a new instance of the <see cref="MethodRubric" /> class.
        /// </summary>
        /// <param name="declaringType">Type of the declaring.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="returnType">Type of the return.</param>
        /// <param name="parameterTypes">The parameter types.</param>
        /// <param name="module">The module.</param>
        /// <param name="propertyId">The property identifier.</param>
        public MethodRubric(Type declaringType, string propertyName, Type returnType, ParameterInfo[] parameterTypes, Module module, int propertyId = -1)
        {
            RubricType = declaringType;
            RubricName = propertyName;
            RubricId = propertyId;
            RubricParameterInfo = parameterTypes;
            RubricReturnType = returnType;
            RubricModule = module;
        }

        #endregion

        #region Properties




        /// <summary>
        /// Gets the attributes associated with this method.
        /// </summary>
        /// <value>The attributes.</value>
        public override MethodAttributes Attributes => RubricInfo.Attributes;




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
        /// Gets a handle to the internal metadata representation of a method.
        /// </summary>
        /// <value>The method handle.</value>
        public override RuntimeMethodHandle MethodHandle => RubricInfo.MethodHandle;




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
        /// Gets the custom attributes for the return type.
        /// </summary>
        /// <value>The return type custom attributes.</value>
        public override ICustomAttributeProvider ReturnTypeCustomAttributes => RubricInfo.ReturnTypeCustomAttributes;




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
        public MethodInfo RubricInfo { get; set; }




        /// <summary>
        /// Gets or sets the rubric module.
        /// </summary>
        /// <value>The rubric module.</value>
        public Module RubricModule { get; set; }




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
        public ParameterInfo[] RubricParameterInfo { get; set; }




        /// <summary>
        /// Gets or sets the type of the rubric return.
        /// </summary>
        /// <value>The type of the rubric return.</value>
        public Type RubricReturnType { get; set; }




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
        /// When overridden in a derived class, returns the <see cref="T:System.Reflection.MethodInfo" /> object for the method on the direct or indirect base class in which the method represented by this instance was first declared.
        /// </summary>
        /// <returns>A <see cref="T:System.Reflection.MethodInfo" /> object for the first implementation of this method.</returns>
        public override MethodInfo GetBaseDefinition()
        {
            return RubricInfo.GetBaseDefinition();
        }






        /// <summary>
        /// When overridden in a derived class, returns an array of all custom attributes applied to this member.
        /// </summary>
        /// <param name="inherit"><see langword="true" /> to search this member's inheritance chain to find the attributes; otherwise, <see langword="false" />. This parameter is ignored for properties and events.</param>
        /// <returns>An array that contains all the custom attributes applied to this member, or an array with zero elements if no attributes are defined.</returns>
        public override object[] GetCustomAttributes(bool inherit)
        {
            return RubricInfo.GetCustomAttributes(inherit);
        }







        /// <summary>
        /// When overridden in a derived class, returns an array of custom attributes applied to this member and identified by <see cref="T:System.Type" />.
        /// </summary>
        /// <param name="attributeType">The type of attribute to search for. Only attributes that are assignable to this type are returned.</param>
        /// <param name="inherit"><see langword="true" /> to search this member's inheritance chain to find the attributes; otherwise, <see langword="false" />. This parameter is ignored for properties and events.</param>
        /// <returns>An array of custom attributes applied to this member, or an array with zero elements if no attributes assignable to <paramref name="attributeType" /> have been applied.</returns>
        public override object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            return RubricInfo.GetCustomAttributes(attributeType, inherit);
        }





        /// <summary>
        /// When overridden in a derived class, returns the <see cref="T:System.Reflection.MethodImplAttributes" /> flags.
        /// </summary>
        /// <returns>The <see langword="MethodImplAttributes" /> flags.</returns>
        public override MethodImplAttributes GetMethodImplementationFlags()
        {
            return RubricInfo.GetMethodImplementationFlags();
        }





        /// <summary>
        /// When overridden in a derived class, gets the parameters of the specified method or constructor.
        /// </summary>
        /// <returns>An array of type <see langword="ParameterInfo" /> containing information that matches the signature of the method (or constructor) reflected by this <see langword="MethodBase" /> instance.</returns>
        public override ParameterInfo[] GetParameters()
        {
            return RubricInfo.GetParameters();
        }










        /// <summary>
        /// When overridden in a derived class, invokes the reflected method or constructor with the given parameters.
        /// </summary>
        /// <param name="obj">The object on which to invoke the method or constructor. If a method is static, this argument is ignored. If a constructor is static, this argument must be <see langword="null" /> or an instance of the class that defines the constructor.</param>
        /// <param name="invokeAttr">A bitmask that is a combination of 0 or more bit flags from <see cref="T:System.Reflection.BindingFlags" />. If <paramref name="binder" /> is <see langword="null" />, this parameter is assigned the value <see cref="F:System.Reflection.BindingFlags.Default" />; thus, whatever you pass in is ignored.</param>
        /// <param name="binder">An object that enables the binding, coercion of argument types, invocation of members, and retrieval of <see langword="MemberInfo" /> objects via reflection. If <paramref name="binder" /> is <see langword="null" />, the default binder is used.</param>
        /// <param name="parameters">An argument list for the invoked method or constructor. This is an array of objects with the same number, order, and type as the parameters of the method or constructor to be invoked. If there are no parameters, this should be <see langword="null" />.
        /// If the method or constructor represented by this instance takes a ByRef parameter, there is no special attribute required for that parameter in order to invoke the method or constructor using this function. Any object in this array that is not explicitly initialized with a value will contain the default value for that object type. For reference-type elements, this value is <see langword="null" />. For value-type elements, this value is 0, 0.0, or <see langword="false" />, depending on the specific element type.</param>
        /// <param name="culture">An instance of <see langword="CultureInfo" /> used to govern the coercion of types. If this is <see langword="null" />, the <see cref="T:System.Globalization.CultureInfo" /> for the current thread is used. (This is necessary to convert a string that represents 1000 to a <see cref="T:System.Double" /> value, for example, since 1000 is represented differently by different cultures.)</param>
        /// <returns>An <see langword="Object" /> containing the return value of the invoked method, or <see langword="null" /> in the case of a constructor, or <see langword="null" /> if the method's return type is <see langword="void" />. Before calling the method or constructor, <see langword="Invoke" /> checks to see if the user has access permission and verifies that the parameters are valid.</returns>
        public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
        {
            return RubricInfo.Invoke(obj, invokeAttr, binder, parameters, culture);
        }







        /// <summary>
        /// Determines whether the specified attribute type is defined.
        /// </summary>
        /// <param name="attributeType">Type of the attribute.</param>
        /// <param name="inherit">if set to <c>true</c> [inherit].</param>
        /// <returns><c>true</c> if the specified attribute type is defined; otherwise, <c>false</c>.</returns>
        public override bool IsDefined(Type attributeType, bool inherit)
        {
            if (this.GetCustomAttributes(attributeType, inherit) != null)
                return true;
            return false;
        }

        #endregion
    }
}
