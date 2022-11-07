
// <copyright file="InstantBuilder.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The Instant namespace.
/// </summary>
namespace System.Instant
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Series;
    using System.Uniques;

    /// <summary>
    /// Class RubricBuilder.
    /// Implements the <see cref="System.IUnique" />
    /// </summary>
    /// <seealso cref="System.IUnique" />
    public class RubricBuilder : IUnique
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RubricBuilder" /> class.
        /// </summary>
        /// <param name="member">The member.</param>
        public RubricBuilder(MemberRubric member)
        {
            SetMember(member);
        }
        /// <summary>
        /// Sets the member.
        /// </summary>
        /// <param name="member">The member.</param>
        /// <returns>MemberInfo.</returns>
        public MemberInfo SetMember(MemberRubric member)
        {
            if (member.MemberType == MemberTypes.Field)
            {
                SetField((FieldRubric)member.RubricInfo);
                Member ??= member;
                Member.FigureField = member.FigureField;

            }
            else if (member.MemberType == MemberTypes.Property)
            {
                SetProperty((PropertyRubric)member.RubricInfo);
                Member = member;
            }
            return null;
        }
        /// <summary>
        /// Sets the property.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns>PropertyRubric.</returns>
        public PropertyRubric SetProperty(PropertyRubric property)
        {
            Property = property;
            Name ??= property.RubricName;
            Type   = property.PropertyType;
            Getter = property.GetGetMethod();
            Setter = property.GetSetMethod();
            return property;
        }
        /// <summary>
        /// Sets the field.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <returns>FieldRubric.</returns>
        public FieldRubric SetField(FieldRubric field)
        {
            Field = field;
            Type ??= field.FieldType;
            FieldType = field.FieldType;
            FieldName = field.FieldName;
            Name ??= field.RubricName;
            return field;
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the name of the field.
        /// </summary>
        /// <value>The name of the field.</value>
        public string FieldName { get; set; }
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public Type Type { get; set; }
        /// <summary>
        /// Gets or sets the member.
        /// </summary>
        /// <value>The member.</value>
        public MemberRubric Member { get; set; }
        /// <summary>
        /// Gets or sets the property.
        /// </summary>
        /// <value>The property.</value>
        public PropertyRubric Property { get; set; }
        /// <summary>
        /// Gets or sets the field.
        /// </summary>
        /// <value>The field.</value>
        public FieldRubric Field { get; set; }
        /// <summary>
        /// Gets or sets the getter.
        /// </summary>
        /// <value>The getter.</value>
        public MethodInfo Getter { get; set; }
        /// <summary>
        /// Gets or sets the setter.
        /// </summary>
        /// <value>The setter.</value>
        public MethodInfo Setter { get; set; }
        /// <summary>
        /// Gets or sets the type of the field.
        /// </summary>
        /// <value>The type of the field.</value>
        public Type FieldType { get; set; }

        /// <summary>
        /// Gets the empty.
        /// </summary>
        /// <value>The empty.</value>
        /// <exception cref="System.NotImplementedException"></exception>
        public IUnique Empty => throw new NotImplementedException();
        /// <summary>
        /// Gets or sets the unique key.
        /// </summary>
        /// <value>The unique key.</value>
        /// <exception cref="System.NotImplementedException"></exception>
        public ulong UniqueKey { get => Name.UniqueKey64(); set => throw new NotImplementedException(); }
        /// <summary>
        /// Gets or sets the unique seed.
        /// </summary>
        /// <value>The unique seed.</value>
        /// <exception cref="System.NotImplementedException"></exception>
        public ulong UniqueSeed { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="other">An object to compare with this instance.</param>
        /// <returns>A value that indicates the relative order of the objects being compared. The return value has these meanings:
        /// <list type="table"><listheader><term> Value</term><description> Meaning</description></listheader><item><term> Less than zero</term><description> This instance precedes <paramref name="other" /> in the sort order.</description></item><item><term> Zero</term><description> This instance occurs in the same position in the sort order as <paramref name="other" />.</description></item><item><term> Greater than zero</term><description> This instance follows <paramref name="other" /> in the sort order.</description></item></list></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public int CompareTo(IUnique other)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><see langword="true" /> if the current object is equal to the <paramref name="other" /> parameter; otherwise, <see langword="false" />.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public bool Equals(IUnique other)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Gets the bytes.
        /// </summary>
        /// <returns>System.Byte[].</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public byte[] GetBytes()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Gets the unique bytes.
        /// </summary>
        /// <returns>System.Byte[].</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public byte[] GetUniqueBytes()
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Class InstantBuilder.
    /// </summary>
    public class InstantBuilder
    {
        /// <summary>
        /// Creates the builders.
        /// </summary>
        /// <param name="modelType">Type of the model.</param>
        /// <returns>IDeck&lt;RubricBuilder&gt;.</returns>
        public IDeck<RubricBuilder> CreateBuilders(Type modelType)
        {
            Catalog<RubricBuilder> rubricBuilders = new Catalog<RubricBuilder>();

            var memberRubrics = PrepareMembers(GetMembers(modelType));

            return CreateBuilders(memberRubrics);
        }

        /// <summary>
        /// Creates the builders.
        /// </summary>
        /// <param name="memberRubrics">The member rubrics.</param>
        /// <returns>IDeck&lt;RubricBuilder&gt;.</returns>
        public IDeck<RubricBuilder> CreateBuilders(MemberRubric[] memberRubrics)
        {
            Catalog<RubricBuilder> rubricBuilders = new Catalog<RubricBuilder>();

            memberRubrics.ForEach((member) =>
            {
                if (!rubricBuilders.TryGet(member.RubricName, out RubricBuilder fieldProperty))
                    rubricBuilders.Put(new RubricBuilder(member));
                else
                    fieldProperty.SetMember(member);
            });
            int order = 0;
            rubricBuilders.ForEach((fp) =>
            {
                order++;
                fp.Member.RubricId = order;
                fp.Member.FieldId = order;
            });

            return rubricBuilders;
        }

        /// <summary>
        /// Prepares the members.
        /// </summary>
        /// <param name="membersInfo">The members information.</param>
        /// <returns>MemberRubric[].</returns>
        public MemberRubric[] PrepareMembers(IEnumerable<MemberInfo> membersInfo)
        {
            return membersInfo.Select(m => !(m is MemberRubric rubric)
                ? m.MemberType == MemberTypes.Field ? new MemberRubric((FieldInfo)m)
                : m.MemberType == MemberTypes.Property ? new MemberRubric((PropertyInfo)m)
                : null
                : rubric).Where(p => p != null).ToArray();
        }

        /// <summary>
        /// Gets the members.
        /// </summary>
        /// <param name="modelType">Type of the model.</param>
        /// <returns>MemberInfo[].</returns>
        public MemberInfo[] GetMembers(Type modelType)
        {
            return modelType.GetMembers(BindingFlags.Instance |
                                        BindingFlags.NonPublic |
                                        BindingFlags.Public)
                            .Where(m => m.Name  != "Item" && m.Name != "ValueArray" &&
                                  (m.MemberType == MemberTypes.Field ||
                                  (m.MemberType == MemberTypes.Property &&
                                  ((PropertyInfo)m).CanRead && ((PropertyInfo)m).CanWrite)))
                                    .ToArray();
        }
    }
}
