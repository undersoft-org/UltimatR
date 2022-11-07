
// <copyright file="MemberRubric.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The Instant namespace.
/// </summary>
namespace System.Instant
{
    using System.Instant.Linking;
    using System.Instant.Treatments;
    using System.Linq;
    using System.Reflection;
    using System.Uniques;

    /// <summary>
    /// Class MemberRubric.
    /// Implements the <see cref="System.Reflection.MemberInfo" />
    /// Implements the <see cref="System.Instant.IRubric" />
    /// </summary>
    /// <seealso cref="System.Reflection.MemberInfo" />
    /// <seealso cref="System.Instant.IRubric" />
    public class MemberRubric : MemberInfo, IRubric
    {
        #region Fields

        /// <summary>
        /// The serialcode
        /// </summary>
        private Ussn serialcode;

        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="MemberRubric" /> class.
        /// </summary>
        public MemberRubric() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="MemberRubric" /> class.
        /// </summary>
        /// <param name="field">The field.</param>
        public MemberRubric(FieldInfo field) : this((IMemberRubric)new FieldRubric(field))
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="MemberRubric" /> class.
        /// </summary>
        /// <param name="field">The field.</param>
        public MemberRubric(FieldRubric field) : this((IMemberRubric)field)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="MemberRubric" /> class.
        /// </summary>
        /// <param name="member">The member.</param>
        public MemberRubric(IMemberRubric member)
        {
            RubricInfo = ((MemberInfo)member);
            RubricName = member.RubricName;
            RubricId = member.RubricId;
            Visible = member.Visible;
            Editable = member.Editable;
            if (RubricInfo.MemberType == MemberTypes.Method)
                serialcode.UniqueKey = (RubricName + "_" + new String(RubricParameterInfo
                                                    .SelectMany(p => p.ParameterType.Name)
                                                        .ToArray())).UniqueKey64();
            else
                serialcode.UniqueKey = RubricName.UniqueKey64();
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="MemberRubric" /> class.
        /// </summary>
        /// <param name="member">The member.</param>
        /// <param name="copyMode">if set to <c>true</c> [copy mode].</param>
        public MemberRubric(MemberRubric member, bool copyMode = false) : this(!copyMode && member.RubricInfo != null ? (IMemberRubric)member.RubricInfo : member)
        {
            
            FigureType = member.FigureType;
            FigureField = member.FigureField;
            FieldId = member.FieldId;
            RubricOffset = member.RubricOffset;
            IsKey = member.IsKey;
            IsIdentity = member.IsIdentity;
            IsAutoincrement = member.IsAutoincrement;
            IdentityOrder = member.IdentityOrder;
            Required = member.Required;
            DisplayName = member.DisplayName;
            RubricSize = member.RubricSize;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="MemberRubric" /> class.
        /// </summary>
        /// <param name="method">The method.</param>
        public MemberRubric(MethodInfo method) : this((IMemberRubric)new MethodRubric(method))
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="MemberRubric" /> class.
        /// </summary>
        /// <param name="method">The method.</param>
        public MemberRubric(MethodRubric method) : this((IMemberRubric)method)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="MemberRubric" /> class.
        /// </summary>
        /// <param name="property">The property.</param>
        public MemberRubric(PropertyInfo property) : this((IMemberRubric)new PropertyRubric(property))
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="MemberRubric" /> class.
        /// </summary>
        /// <param name="property">The property.</param>
        public MemberRubric(PropertyRubric property) : this((IMemberRubric)property)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Shalows the copy.
        /// </summary>
        /// <param name="dst">The DST.</param>
        /// <returns>MemberRubric.</returns>
        public MemberRubric ShalowCopy(MemberRubric dst)
        {
            if (dst == null)
                dst = new MemberRubric();

                dst.RubricInfo = this;
                dst.RubricName = RubricName;
                dst.RubricId = RubricId;
                dst.Visible = Visible;
                dst.Editable = Editable;
                dst.FigureType = FigureType;
                dst.FigureField = FigureField;
                dst.FieldId = FieldId;
                dst.RubricOffset = RubricOffset;
                dst.IsKey = IsKey;
                dst.IsIdentity = IsIdentity;
                dst.IsAutoincrement = IsAutoincrement;
                dst.IdentityOrder = IdentityOrder;
                dst.Required = Required;
                dst.DisplayName = DisplayName;
                dst.serialcode.UniqueKey = RubricName.UniqueKey64();
            
            return dst;
        }

        /// <summary>
        /// Gets or sets the aggregate link identifier.
        /// </summary>
        /// <value>The aggregate link identifier.</value>
        public int AggregateLinkId { get; set; }

        /// <summary>
        /// Gets or sets the aggregate links.
        /// </summary>
        /// <value>The aggregate links.</value>
        public Links AggregateLinks { get; set; }

        /// <summary>
        /// Gets or sets the aggregate operand.
        /// </summary>
        /// <value>The aggregate operand.</value>
        public AggregateOperand AggregateOperand { get; set; }

        /// <summary>
        /// Gets or sets the aggregate ordinal.
        /// </summary>
        /// <value>The aggregate ordinal.</value>
        public int AggregateOrdinal { get; set; }

        /// <summary>
        /// Gets or sets the aggregate rubric.
        /// </summary>
        /// <value>The aggregate rubric.</value>
        public IRubric AggregateRubric { get; set; }

        /// <summary>
        /// Gets the class that declares this member.
        /// </summary>
        /// <value>The type of the declaring.</value>
        public override Type DeclaringType => FigureType != null ? FigureType : RubricInfo.DeclaringType;

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        /// <value>The display name.</value>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="IMemberRubric" /> is editable.
        /// </summary>
        /// <value><c>true</c> if editable; otherwise, <c>false</c>.</value>
        public bool Editable { get; set; }

        /// <summary>
        /// Gets the empty.
        /// </summary>
        /// <value>The empty.</value>
        public IUnique Empty => Ussn.Empty;

        /// <summary>
        /// Gets or sets the field identifier.
        /// </summary>
        /// <value>The field identifier.</value>
        public int FieldId { get; set; }

        /// <summary>
        /// Gets or sets the figure field.
        /// </summary>
        /// <value>The figure field.</value>
        public FieldInfo FigureField { get; set; }

        /// <summary>
        /// Gets or sets the type of the figure.
        /// </summary>
        /// <value>The type of the figure.</value>
        public Type FigureType { get; set; }

        /// <summary>
        /// Gets or sets the identity order.
        /// </summary>
        /// <value>The identity order.</value>
        public short IdentityOrder { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is autoincrement.
        /// </summary>
        /// <value><c>true</c> if this instance is autoincrement; otherwise, <c>false</c>.</value>
        public bool IsAutoincrement { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is expandable.
        /// </summary>
        /// <value><c>true</c> if this instance is expandable; otherwise, <c>false</c>.</value>
        public bool IsExpandable { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is database null.
        /// </summary>
        /// <value><c>true</c> if this instance is database null; otherwise, <c>false</c>.</value>
        public bool IsDBNull { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is identity.
        /// </summary>
        /// <value><c>true</c> if this instance is identity; otherwise, <c>false</c>.</value>
        public bool IsIdentity { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is unique.
        /// </summary>
        /// <value><c>true</c> if this instance is unique; otherwise, <c>false</c>.</value>
        public bool IsUnique { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is key.
        /// </summary>
        /// <value><c>true</c> if this instance is key; otherwise, <c>false</c>.</value>
        public bool IsKey { get; set; }

        /// <summary>
        /// When overridden in a derived class, gets a <see cref="T:System.Reflection.MemberTypes" /> value indicating the type of the member - method, constructor, event, and so on.
        /// </summary>
        /// <value>The type of the member.</value>
        public override MemberTypes MemberType => RubricInfo.MemberType;

        /// <summary>
        /// Gets the name of the current member.
        /// </summary>
        /// <value>The name.</value>
        public override string Name => RubricInfo.Name;

        /// <summary>
        /// Gets the class object that was used to obtain this instance of <see langword="MemberInfo" />.
        /// </summary>
        /// <value>The type of the reflected.</value>
        public override Type ReflectedType => RubricInfo.ReflectedType;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="IRubric" /> is required.
        /// </summary>
        /// <value><c>true</c> if required; otherwise, <c>false</c>.</value>
        public bool Required { get; set; }

        /// <summary>
        /// Gets or sets the rubric attributes.
        /// </summary>
        /// <value>The rubric attributes.</value>
        public object[] RubricAttributes
        {
            get { return VirtualInfo.RubricAttributes; }
            set { VirtualInfo.RubricAttributes = value; }
        }

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
        public MemberInfo RubricInfo { get; set; }

        /// <summary>
        /// Gets the rubric module.
        /// </summary>
        /// <value>The rubric module.</value>
        public Module RubricModule { get => MemberType == MemberTypes.Method ? ((MethodRubric)RubricInfo).RubricModule : null; }

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
        /// Gets the getter.
        /// </summary>
        /// <value>The getter.</value>
        public MethodInfo Getter => (MemberType == MemberTypes.Property) ? ((PropertyRubric)RubricInfo).GetMethod : null;

        /// <summary>
        /// Gets the setter.
        /// </summary>
        /// <value>The setter.</value>
        public MethodInfo Setter => (MemberType == MemberTypes.Property) ? ((PropertyRubric) RubricInfo).SetMethod : null;

        /// <summary>
        /// Gets the rubric parameter information.
        /// </summary>
        /// <value>The rubric parameter information.</value>
        public ParameterInfo[] RubricParameterInfo { get => MemberType == MemberTypes.Method ? ((MethodRubric)RubricInfo).RubricParameterInfo : null; }

        /// <summary>
        /// Gets the type of the rubric return.
        /// </summary>
        /// <value>The type of the rubric return.</value>
        public Type RubricReturnType { get => MemberType == MemberTypes.Method ? ((MethodRubric)RubricInfo).RubricReturnType : null; }

        /// <summary>
        /// Gets or sets the rubrics.
        /// </summary>
        /// <value>The rubrics.</value>
        public MemberRubrics Rubrics { get; set; }

        /// <summary>
        /// Gets or sets the size of the rubric.
        /// </summary>
        /// <value>The size of the rubric.</value>
        public int RubricSize
        {
            get { return VirtualInfo.RubricSize; }
            set { VirtualInfo.RubricSize = value; }
        }

        /// <summary>
        /// Gets or sets the type of the rubric.
        /// </summary>
        /// <value>The type of the rubric.</value>
        public Type RubricType
        {
            get { return VirtualInfo.RubricType; }
            set { VirtualInfo.RubricType = value; }
        }

        /// <summary>
        /// The deltamarkset
        /// </summary>
        public bool deltamarkset = false;
        /// <summary>
        /// The deltamark
        /// </summary>
        public object deltamark;

        /// <summary>
        /// Gets the delta mark.
        /// </summary>
        /// <value>The delta mark.</value>
        public object DeltaMark
        {
            get
            {
                if (!deltamarkset)
                {
                    deltamark = RubricType.Default();
                    deltamarkset = true;
                }

                return deltamark;
            }
        }

        /// <summary>
        /// Gets or sets the serial code.
        /// </summary>
        /// <value>The serial code.</value>
        public Ussn SerialCode 
        { get => serialcode; set => serialcode = value; }

        /// <summary>
        /// Gets or sets the summary operand.
        /// </summary>
        /// <value>The summary operand.</value>
        public AggregateOperand SummaryOperand { get; set; }

        /// <summary>
        /// Gets or sets the summary ordinal.
        /// </summary>
        /// <value>The summary ordinal.</value>
        public int SummaryOrdinal { get; set; }

        /// <summary>
        /// Gets or sets the summary rubric.
        /// </summary>
        /// <value>The summary rubric.</value>
        public IRubric SummaryRubric { get; set; }

        /// <summary>
        /// Gets or sets the unique key.
        /// </summary>
        /// <value>The unique key.</value>
        public ulong UniqueKey
        { get => serialcode.UniqueKey; set => serialcode.SetUniqueKey(value); }

        /// <summary>
        /// Gets or sets the unique seed.
        /// </summary>
        /// <value>The unique seed.</value>
        public ulong UniqueSeed 
        { get => serialcode.UniqueSeed; set => serialcode.SetUniqueSeed(value); }

        /// <summary>
        /// Gets the virtual information.
        /// </summary>
        /// <value>The virtual information.</value>
        public IMemberRubric VirtualInfo => (IMemberRubric)RubricInfo;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="IMemberRubric" /> is visible.
        /// </summary>
        /// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
        public bool Visible { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="other">An object to compare with this instance.</param>
        /// <returns>A value that indicates the relative order of the objects being compared. The return value has these meanings:
        /// <list type="table"><listheader><term> Value</term><description> Meaning</description></listheader><item><term> Less than zero</term><description> This instance precedes <paramref name="other" /> in the sort order.</description></item><item><term> Zero</term><description> This instance occurs in the same position in the sort order as <paramref name="other" />.</description></item><item><term> Greater than zero</term><description> This instance follows <paramref name="other" /> in the sort order.</description></item></list></returns>
        public int CompareTo(IUnique other)
        {
            return (int)(UniqueKey - other.UniqueKey);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><see langword="true" /> if the current object is equal to the <paramref name="other" /> parameter; otherwise, <see langword="false" />.</returns>
        public bool Equals(IUnique other)
        {
            return UniqueKey == other.UniqueKey;
        }

        /// <summary>
        /// Gets the bytes.
        /// </summary>
        /// <returns>System.Byte[].</returns>
        public byte[] GetBytes()
        {
            return serialcode.GetBytes();
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
        /// Gets the unique bytes.
        /// </summary>
        /// <returns>System.Byte[].</returns>
        public byte[] GetUniqueBytes()
        {
            return serialcode.GetUniqueBytes();
        }

        /// <summary>
        /// Determines whether the specified attribute type is defined.
        /// </summary>
        /// <param name="attributeType">Type of the attribute.</param>
        /// <param name="inherit">if set to <c>true</c> [inherit].</param>
        /// <returns><c>true</c> if the specified attribute type is defined; otherwise, <c>false</c>.</returns>
        public override bool IsDefined(Type attributeType, bool inherit)
        {
            return RubricInfo.IsDefined(attributeType, inherit);
        }

        #endregion
    }
}
