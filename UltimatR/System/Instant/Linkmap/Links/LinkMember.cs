
// <copyright file="LinkMember.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The Linking namespace.
/// </summary>
namespace System.Instant.Linking
{
    using System.Extract;
    using System.Linq;
    using System.Uniques;

    /// <summary>
    /// Class LinkMember.
    /// Implements the <see cref="System.IUnique" />
    /// </summary>
    /// <seealso cref="System.IUnique" />
    [Serializable]
    public class LinkMember : IUnique
    {
        #region Fields

        /// <summary>
        /// The serialcode
        /// </summary>
        private Ussc serialcode;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LinkMember" /> class.
        /// </summary>
        public LinkMember()
        {
            KeyRubrics = new MemberRubrics();
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="LinkMember" /> class.
        /// </summary>
        /// <param name="sleeve">The sleeve.</param>
        /// <param name="link">The link.</param>
        /// <param name="site">The site.</param>
        public LinkMember(Sleeve sleeve, Link link, LinkSite site) : this()
        {
            string[] names = link.Name.Split("To");
            LinkMember member;
            Site = site;
            Link = link;

            int siteId = 1;

            if (site == LinkSite.Origin)
            {
                siteId = 0;
                member = Link.Origin;
            }
            else
                member = Link.Target;

            Name = names[siteId];
            UniqueKey = names[siteId].UniqueKey64(link.UniqueKey);
            UniqueSeed = link.UniqueKey;
            Rubrics = sleeve.Rubrics;
            Sleeve = sleeve;
           
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="LinkMember" /> class.
        /// </summary>
        /// <param name="link">The link.</param>
        /// <param name="site">The site.</param>
        public LinkMember(Link link, LinkSite site) : this()
        {
            string[] names = link.Name.Split("_&_");
            Site = site;
            Link = link;
            LinkMember member;
            int siteId = 1;

            if (site == LinkSite.Origin)
            {
                siteId = 0;
                member = Link.Origin;
            }
            else
                member = Link.Target;

            Name = names[siteId];
            UniqueKey = names[siteId].UniqueKey64(link.UniqueKey);
            UniqueSeed = link.UniqueKey;
            Rubrics = member.Sleeve.Rubrics;
            Sleeve = member.Sleeve;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the empty.
        /// </summary>
        /// <value>The empty.</value>
        public IUnique Empty => Ussc.Empty;

        /// <summary>
        /// Gets or sets the sleeve.
        /// </summary>
        /// <value>The sleeve.</value>
        public Sleeve Sleeve { get; set; }

        /// <summary>
        /// Gets or sets the key rubrics.
        /// </summary>
        /// <value>The key rubrics.</value>
        public IRubrics KeyRubrics { get; set; }

        /// <summary>
        /// Gets or sets the link.
        /// </summary>
        /// <value>The link.</value>
        public Link Link { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the rubrics.
        /// </summary>
        /// <value>The rubrics.</value>
        public IRubrics Rubrics { get; set; }

        /// <summary>
        /// Gets or sets the serial code.
        /// </summary>
        /// <value>The serial code.</value>
        public Ussc SerialCode { get => serialcode; set => serialcode = value; }

        /// <summary>
        /// Gets or sets the site.
        /// </summary>
        /// <value>The site.</value>
        public LinkSite Site { get; set; }

        /// <summary>
        /// Gets or sets the unique key.
        /// </summary>
        /// <value>The unique key.</value>
        public ulong UniqueKey { get => serialcode.UniqueKey; set => serialcode.UniqueKey = value; }

        /// <summary>
        /// Gets or sets the unique seed.
        /// </summary>
        /// <value>The unique seed.</value>
        public ulong UniqueSeed { get => serialcode.UniqueSeed; set => serialcode.UniqueSeed = value; }

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
            return SerialCode.CompareTo(other);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><see langword="true" /> if the current object is equal to the <paramref name="other" /> parameter; otherwise, <see langword="false" />.</returns>
        public bool Equals(IUnique other)
        {
            return SerialCode.Equals(other);
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
        /// Gets the unique bytes.
        /// </summary>
        /// <returns>System.Byte[].</returns>
        public byte[] GetUniqueBytes()
        {
            return serialcode.GetUniqueBytes();
        }

        /// <summary>
        /// Links the key.
        /// </summary>
        /// <param name="figure">The figure.</param>
        /// <returns>System.UInt64.</returns>
        public unsafe ulong LinkKey(ISleeve figure)
        {
            byte[] b = KeyRubrics.Ordinals
                .SelectMany(x => figure[x].GetBytes()).ToArray();

            int l = b.Length;
            fixed(byte* pb = b)
            {
                return Hasher64.ComputeKey(pb, l);
            }
        }

        #endregion
    }

    /// <summary>
    /// Class LinkNode.
    /// Implements the <see cref="System.IUnique" />
    /// </summary>
    /// <seealso cref="System.IUnique" />
    [Serializable]
    public class LinkNode : IUnique
    {
        #region Fields

        /// <summary>
        /// The serialcode
        /// </summary>
        private Ussc serialcode;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LinkNode" /> class.
        /// </summary>
        public LinkNode()
        {
            OriginKeyRubrics = new MemberRubrics();
            TargetKeyRubrics = new MemberRubrics();
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="LinkNode" /> class.
        /// </summary>
        /// <param name="sleeve">The sleeve.</param>
        /// <param name="link">The link.</param>
        public LinkNode(Sleeve sleeve, Link link) : this()
        {
            Name = link.Name;
            LinkNode member;
            Site = LinkSite.Node;
            Link = link;

            member = Link.Node;

            UniqueKey = Name.UniqueKey64(link.UniqueKey);
            UniqueSeed = link.UniqueKey;
            Rubrics = sleeve.Rubrics;
            Sleeve = sleeve;
            
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the empty.
        /// </summary>
        /// <value>The empty.</value>
        public IUnique Empty => Ussc.Empty;

        /// <summary>
        /// Gets or sets the sleeve.
        /// </summary>
        /// <value>The sleeve.</value>
        public Sleeve Sleeve { get; set; }

        /// <summary>
        /// Gets or sets the origin key rubrics.
        /// </summary>
        /// <value>The origin key rubrics.</value>
        public IRubrics OriginKeyRubrics { get; set; }

        /// <summary>
        /// Gets or sets the target key rubrics.
        /// </summary>
        /// <value>The target key rubrics.</value>
        public IRubrics TargetKeyRubrics { get; set; }

        /// <summary>
        /// Gets or sets the link.
        /// </summary>
        /// <value>The link.</value>
        public Link Link { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the rubrics.
        /// </summary>
        /// <value>The rubrics.</value>
        public IRubrics Rubrics { get; set; }

        /// <summary>
        /// Gets or sets the serial code.
        /// </summary>
        /// <value>The serial code.</value>
        public Ussc SerialCode { get => serialcode; set => serialcode = value; }

        /// <summary>
        /// Gets or sets the site.
        /// </summary>
        /// <value>The site.</value>
        public LinkSite Site { get; set; }

        /// <summary>
        /// Gets or sets the unique key.
        /// </summary>
        /// <value>The unique key.</value>
        public ulong UniqueKey { get => serialcode.UniqueKey; set => serialcode.UniqueKey = value; }

        /// <summary>
        /// Gets or sets the unique seed.
        /// </summary>
        /// <value>The unique seed.</value>
        public ulong UniqueSeed { get => serialcode.UniqueSeed; set => serialcode.UniqueSeed = value; }

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
            return SerialCode.CompareTo(other);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><see langword="true" /> if the current object is equal to the <paramref name="other" /> parameter; otherwise, <see langword="false" />.</returns>
        public bool Equals(IUnique other)
        {
            return SerialCode.Equals(other);
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
        /// Gets the unique bytes.
        /// </summary>
        /// <returns>System.Byte[].</returns>
        public byte[] GetUniqueBytes()
        {
            return serialcode.GetUniqueBytes();
        }

        /// <summary>
        /// Links the origin key.
        /// </summary>
        /// <param name="figure">The figure.</param>
        /// <returns>System.UInt64.</returns>
        public unsafe ulong LinkOriginKey(ISleeve figure)
        {
            byte[] b = OriginKeyRubrics.Ordinals
                .SelectMany(x => figure[x].GetBytes()).ToArray();

            int l = b.Length;
            fixed (byte* pb = b)
            {
                return Hasher64.ComputeKey(pb, l);
            }
        }

        /// <summary>
        /// Links the target key.
        /// </summary>
        /// <param name="figure">The figure.</param>
        /// <returns>System.UInt64.</returns>
        public unsafe ulong LinkTargetKey(ISleeve figure)
        {
            byte[] b = OriginKeyRubrics.Ordinals
                .SelectMany(x => figure[x].GetBytes()).ToArray();

            int l = b.Length;
            fixed (byte* pb = b)
            {
                return Hasher64.ComputeKey(pb, l);
            }
        }

        #endregion
    }
}
