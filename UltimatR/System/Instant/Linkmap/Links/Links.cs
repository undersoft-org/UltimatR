
// <copyright file="Links.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>

/// <summary>
/// The Linking namespace.
/// </summary>
namespace System.Instant.Linking
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Series;
    using System.Uniques;

    #region Enums

    /// <summary>
    /// Enum LinkSite
    /// </summary>
    public enum LinkSite
    {
        /// <summary>
        /// The none
        /// </summary>
        None,
        /// <summary>
        /// The origin
        /// </summary>
        Origin,
        /// <summary>
        /// The target
        /// </summary>
        Target,
        /// <summary>
        /// The node
        /// </summary>
        Node
    }

    #endregion

    /// <summary>
    /// Class Links.
    /// Implements the <see cref="System.Series.CatalogBase{System.Instant.Linking.Link}" />
    /// Implements the <see cref="System.IUnique" />
    /// </summary>
    /// <seealso cref="System.Series.CatalogBase{System.Instant.Linking.Link}" />
    /// <seealso cref="System.IUnique" />
    public class Links : CatalogBase<Link>, IUnique
    {
        /// <summary>
        /// The serialcode
        /// </summary>
        private new Ussn serialcode;

        /// <summary>
        /// Initializes a new instance of the <see cref="Links" /> class.
        /// </summary>
        public Links()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Links" /> class.
        /// </summary>
        /// <param name="links">The links.</param>
        public Links(IList<Link> links)
        {
            Add(links);
        }

        /// <summary>
        /// Gets or sets the <see cref="Link" /> with the specified link name.
        /// </summary>
        /// <param name="linkName">Name of the link.</param>
        /// <returns>Link.</returns>
        public Link this[string linkName]
        {
            get
            {
                return base[linkName];
            }
            set
            {
                base[linkName] = value;
            }
        }
        /// <summary>
        /// Gets or sets the <see cref="Link" /> with the specified linkid.
        /// </summary>
        /// <param name="linkid">The linkid.</param>
        /// <returns>Link.</returns>
        public new Link this[int linkid]
        {
            get
            {
                return base[linkid];
            }
            set
            {
                base[linkid] = value;
            }
        }

        /// <summary>
        /// Targets the link.
        /// </summary>
        /// <param name="TargetName">Name of the target.</param>
        /// <returns>Link.</returns>
        public Link TargetLink(string TargetName)
        {
            return AsValues().Where(o => o.TargetName.Equals(TargetName)).FirstOrDefault();
        }
        /// <summary>
        /// Origins the link.
        /// </summary>
        /// <param name="OriginName">Name of the origin.</param>
        /// <returns>Link.</returns>
        public Link OriginLink(string OriginName)
        {
            return AsValues().Where(o => o.OriginName.Equals(OriginName)).FirstOrDefault();
        }

        /// <summary>
        /// Targets the member.
        /// </summary>
        /// <param name="TargetName">Name of the target.</param>
        /// <returns>LinkMember.</returns>
        public LinkMember TargetMember(string TargetName)
        {
            Link link = TargetLink(TargetName);
            if (link != null)
                return link.Target;
            return null;
        }
        /// <summary>
        /// Origins the member.
        /// </summary>
        /// <param name="OriginName">Name of the origin.</param>
        /// <returns>LinkMember.</returns>
        public LinkMember OriginMember(string OriginName)
        {
            Link link = OriginLink(OriginName);
            if (link != null)
                return link.Origin;
            return null;
        }

        /// <summary>
        /// Empties the card.
        /// </summary>
        /// <returns>ICard&lt;Link&gt;.</returns>
        public override ICard<Link> EmptyCard()
        {
            return new Card<Link>();
        }

        /// <summary>
        /// Creates new card.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;Link&gt;.</returns>
        public override ICard<Link> NewCard(ulong key, Link value)
        {
            return new Card<Link>(key, value);
        }
        /// <summary>
        /// Creates new card.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;Link&gt;.</returns>
        public override ICard<Link> NewCard(object key, Link value)
        {
            return new Card<Link>(key, value);
        }
        /// <summary>
        /// Creates new card.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;Link&gt;.</returns>
        public override ICard<Link> NewCard(ICard<Link> value)
        {
            return new Card<Link>(value);
        }
        /// <summary>
        /// Creates new card.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;Link&gt;.</returns>
        public override ICard<Link> NewCard(Link value)
        {
            return new Card<Link>(value);
        }

        /// <summary>
        /// Empties the card table.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <returns>ICard&lt;Link&gt;[].</returns>
        public override ICard<Link>[] EmptyCardTable(int size)
        {
            return new Card<Link>[size];
        }

        /// <summary>
        /// Empties the deck.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <returns>ICard&lt;Link&gt;[].</returns>
        public override ICard<Link>[] EmptyDeck(int size)
        {
            return new Card<Link>[size];
        }

        /// <summary>
        /// Inners the add.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        internal override bool InnerAdd(Link value)
        {
            return InnerAdd(NewCard(value));
        }

        /// <summary>
        /// Inners the put.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;Link&gt;.</returns>
        protected override ICard<Link> InnerPut(Link value)
        {
            return InnerPut(NewCard(value));
        }

        /// <summary>
        /// Gets or sets the serial code.
        /// </summary>
        /// <value>The serial code.</value>
        public Ussn SerialCode
        {
            get => serialcode;
            set => serialcode = value;
        }

        /// <summary>
        /// Gets the empty.
        /// </summary>
        /// <value>The empty.</value>
        public new IUnique Empty => Ussn.Empty;

        /// <summary>
        /// Gets or sets the unique key.
        /// </summary>
        /// <value>The unique key.</value>
        public override ulong UniqueKey
        {
            get => serialcode.UniqueKey;
            set => serialcode.UniqueKey = value;
        }

        /// <summary>
        /// Gets or sets the unique seed.
        /// </summary>
        /// <value>The unique seed.</value>
        public override ulong UniqueSeed
        {
            get => serialcode.UniqueSeed;
            set => serialcode.UniqueSeed = value;
        }

        /// <summary>
        /// Compares to.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>System.Int32.</returns>
        public override int CompareTo(IUnique other)
        {
            return serialcode.CompareTo(other);
        }

        /// <summary>
        /// Equalses the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public override bool Equals(IUnique other)
        {
            return serialcode.Equals(other);
        }

        /// <summary>
        /// Gets the bytes.
        /// </summary>
        /// <returns>System.Byte[].</returns>
        public override byte[] GetBytes()
        {
            return serialcode.GetBytes();
        }

        /// <summary>
        /// Gets the unique bytes.
        /// </summary>
        /// <returns>System.Byte[].</returns>
        public override byte[] GetUniqueBytes()
        {
            return serialcode.GetUniqueBytes();
        }
    }
}
