
// <copyright file="Link.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The Linking namespace.
/// </summary>
namespace System.Instant.Linking
{
    using System.Uniques;

    /// <summary>
    /// Class Link.
    /// Implements the <see cref="System.IUnique" />
    /// </summary>
    /// <seealso cref="System.IUnique" />
    [Serializable]
    public class Link : IUnique
    {
        #region Fields

        /// <summary>
        /// The serialcode
        /// </summary>
        private Ussn serialcode;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Link" /> class.
        /// </summary>
        public Link()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Link" /> class.
        /// </summary>
        /// <param name="origin">The origin.</param>
        /// <param name="target">The target.</param>
        public Link(ISleeve origin, ISleeve target)
        {
            LinkPair(origin, target);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Link" /> class.
        /// </summary>
        /// <param name="origin">The origin.</param>
        /// <param name="target">The target.</param>
        /// <param name="parentKeys">The parent keys.</param>
        /// <param name="childKeys">The child keys.</param>
        public Link(ISleeve origin, ISleeve target, IRubrics parentKeys, IRubrics childKeys) : this(origin, target)
        {
            LinkParentKeys(parentKeys);
            LinkChildKeys(childKeys);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Link" /> class.
        /// </summary>
        /// <param name="origin">The origin.</param>
        /// <param name="target">The target.</param>
        /// <param name="parentKeys">The parent keys.</param>
        /// <param name="childKeys">The child keys.</param>
        public Link(ISleeve origin, ISleeve target, string[] parentKeys, string[] childKeys) : this(origin, target)
        {
            LinkParentKeys(parentKeys);
            LinkChildKeys(childKeys );
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Link" /> class.
        /// </summary>
        /// <param name="origin">The origin.</param>
        /// <param name="node">The node.</param>
        /// <param name="target">The target.</param>
        public Link(ISleeve origin, ISleeve node, ISleeve target)
        {
            LinkTrio(origin, node, target);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Link" /> class.
        /// </summary>
        /// <param name="origin">The origin.</param>
        /// <param name="node">The node.</param>
        /// <param name="target">The target.</param>
        /// <param name="parentKeys">The parent keys.</param>
        /// <param name="nodeParentKeys">The node parent keys.</param>
        /// <param name="nodeChildKeys">The node child keys.</param>
        /// <param name="childKeys">The child keys.</param>
        public Link(ISleeve origin, ISleeve node, ISleeve target, IRubrics parentKeys, IRubrics nodeParentKeys, IRubrics nodeChildKeys, IRubrics childKeys) : this(origin, node, target)
        {
            LinkParentKeys(parentKeys);
            LinkNodeKeys(nodeParentKeys, nodeChildKeys);
            LinkChildKeys(childKeys);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Link" /> class.
        /// </summary>
        /// <param name="origin">The origin.</param>
        /// <param name="node">The node.</param>
        /// <param name="target">The target.</param>
        /// <param name="parentKeys">The parent keys.</param>
        /// <param name="nodeParentKeys">The node parent keys.</param>
        /// <param name="nodeChildKeys">The node child keys.</param>
        /// <param name="childKeys">The child keys.</param>
        public Link(ISleeve origin, ISleeve node, ISleeve target, string[] parentKeys, string[] nodeParentKeys, string[] nodeChildKeys, string[] childKeys) : this(origin, node, target)
        {
            LinkParentKeys(parentKeys);
            LinkNodeKeys(nodeParentKeys, nodeParentKeys);
            LinkChildKeys(childKeys);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the empty.
        /// </summary>
        /// <value>The empty.</value>
        public IUnique Empty => Ussn.Empty;

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the origin.
        /// </summary>
        /// <value>The origin.</value>
        public LinkMember Origin { get; set; }

        /// <summary>
        /// Gets or sets the origin keys.
        /// </summary>
        /// <value>The origin keys.</value>
        public IRubrics OriginKeys
        {
            get
            {
                return Origin.KeyRubrics;
            }
            set
            {
                Origin.KeyRubrics.Renew(value);
                Origin.KeyRubrics.Update();
            }
        }

        /// <summary>
        /// Gets or sets the name of the origin.
        /// </summary>
        /// <value>The name of the origin.</value>
        public string OriginName
        {
            get { return Origin.Name; }
            set
            {
                Origin.Name = value;
            }
        }

        /// <summary>
        /// Gets or sets the origin rubrics.
        /// </summary>
        /// <value>The origin rubrics.</value>
        public IRubrics OriginRubrics
        {
            get
            {
                return Origin.Rubrics;
            }
            set
            {
                Origin.Rubrics = value;
            }
        }

        /// <summary>
        /// Gets or sets the serial code.
        /// </summary>
        /// <value>The serial code.</value>
        public Ussn SerialCode { get => serialcode; set => serialcode = value; }

        /// <summary>
        /// Gets or sets the node.
        /// </summary>
        /// <value>The node.</value>
        public LinkNode Node { get; set; }

        /// <summary>
        /// Gets or sets the node origin keys.
        /// </summary>
        /// <value>The node origin keys.</value>
        public IRubrics NodeOriginKeys
        {
            get
            {
                return Node.OriginKeyRubrics;
            }
            set
            {
                Node.OriginKeyRubrics.Renew(value);
                Node.OriginKeyRubrics.Update();
            }
        }

        /// <summary>
        /// Gets or sets the node target keys.
        /// </summary>
        /// <value>The node target keys.</value>
        public IRubrics NodeTargetKeys
        {
            get
            {
                return Node.TargetKeyRubrics;
            }
            set
            {
                Node.TargetKeyRubrics.Renew(value);
                Node.TargetKeyRubrics.Update();
            }
        }

        /// <summary>
        /// Gets or sets the name of the node.
        /// </summary>
        /// <value>The name of the node.</value>
        public string NodeName
        {
            get { return Node.Name; }
            set
            {
                Node.Name = value;
            }
        }

        /// <summary>
        /// Gets or sets the node rubrics.
        /// </summary>
        /// <value>The node rubrics.</value>
        public IRubrics NodeRubrics
        {
            get
            {
                return Node.Rubrics;
            }
            set
            {
                Node.Rubrics = value;
            }
        }

        /// <summary>
        /// Gets or sets the target.
        /// </summary>
        /// <value>The target.</value>
        public LinkMember Target { get; set; }

        /// <summary>
        /// Gets or sets the target keys.
        /// </summary>
        /// <value>The target keys.</value>
        public IRubrics TargetKeys
        {
            get
            {
                return Target.KeyRubrics;
            }
            set
            {
                Target.KeyRubrics.Renew(value);
                Target.KeyRubrics.Update();
            }
        }

        /// <summary>
        /// Gets or sets the name of the target.
        /// </summary>
        /// <value>The name of the target.</value>
        public string TargetName
        {
            get { return Target.Name; }
            set
            {
                Target.Name = value;
            }
        }

        /// <summary>
        /// Gets or sets the target rubrics.
        /// </summary>
        /// <value>The target rubrics.</value>
        public IRubrics TargetRubrics
        {
            get
            {
                return Target.KeyRubrics;
            }
            set
            {
                Target.KeyRubrics = value;
            }
        }

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
            return serialcode.CompareTo(other);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><see langword="true" /> if the current object is equal to the <paramref name="other" /> parameter; otherwise, <see langword="false" />.</returns>
        public bool Equals(IUnique other)
        {
            return serialcode.Equals(other);
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
        /// Sets the link.
        /// </summary>
        /// <param name="origin">The origin.</param>
        /// <param name="target">The target.</param>
        /// <param name="parentKeys">The parent keys.</param>
        /// <param name="childKeys">The child keys.</param>
        /// <returns>Link.</returns>
        public Link SetLink(ISleeve origin, ISleeve target, IRubrics parentKeys, IRubrics childKeys)
        {
            LinkPair(origin, target);
            LinkParentKeys(parentKeys);
            LinkChildKeys(childKeys);
            return this;
        }

        /// <summary>
        /// Sets the link.
        /// </summary>
        /// <param name="origin">The origin.</param>
        /// <param name="target">The target.</param>
        /// <param name="parentKeynames">The parent keynames.</param>
        /// <param name="childKeynames">The child keynames.</param>
        /// <returns>Link.</returns>
        public Link SetLink(ISleeve origin, ISleeve target, string[] parentKeynames, string[] childKeynames)
        {
            LinkPair(origin, target);
            LinkParentKeys(parentKeynames);
            LinkChildKeys(childKeynames);
            return this;
        }

        /// <summary>
        /// Links the pair.
        /// </summary>
        /// <param name="origin">The origin.</param>
        /// <param name="target">The target.</param>
        /// <returns>Link.</returns>
        public Link LinkPair(ISleeve origin, ISleeve target)
        {
            Name = origin.GetType().Name + "To" + target.GetType().Name;

            UniqueKey = Name.UniqueKey64();
            UniqueSeed = Name.UniqueKey32();

            Origin = new LinkMember(origin, this, LinkSite.Origin);
            Target = new LinkMember(target, this, LinkSite.Target);

            return Linker.Map.Put(this).Value;
        }

        /// <summary>
        /// Links the trio.
        /// </summary>
        /// <param name="origin">The origin.</param>
        /// <param name="node">The node.</param>
        /// <param name="target">The target.</param>
        /// <returns>Link.</returns>
        public Link LinkTrio(ISleeve origin, ISleeve node, ISleeve target)
        {
            Name = origin.GetType().Name + "To" + target.GetType().Name;

            UniqueKey = Name.UniqueKey64();
            UniqueSeed = Name.UniqueKey32();

            Origin = new LinkMember(origin, this, LinkSite.Origin);
            Node = new LinkNode(node, this);
            Target = new LinkMember(target, this, LinkSite.Target);

            return Linker.Map.Put(this).Value;
        }

        /// <summary>
        /// Links the parent keys.
        /// </summary>
        /// <param name="keyRubrics">The key rubrics.</param>
        /// <exception cref="System.IndexOutOfRangeException">Rubric not found</exception>
        public void LinkParentKeys(IRubrics keyRubrics)
        {
            foreach (IUnique rubric in keyRubrics)
{
                var originRubric = Origin.Rubrics[rubric];           
                if (originRubric != null)
                {
                    OriginKeys.Add(originRubric);
                }
                else
                    throw new IndexOutOfRangeException("Rubric not found");
                OriginKeys.Update();
            }
        }

        /// <summary>
        /// Links the node keys.
        /// </summary>
        /// <param name="originKeyRubric">The origin key rubric.</param>
        /// <param name="targetKeyRubric">The target key rubric.</param>
        /// <exception cref="System.IndexOutOfRangeException">Rubric not found</exception>
        public void LinkNodeKeys(IRubrics originKeyRubric, IRubrics targetKeyRubric)
        {
            foreach (var rubric in originKeyRubric)
            {
                var nodeRubric = Node.Rubrics[rubric];
                if (nodeRubric != null)
                {
                    NodeOriginKeys.Add(nodeRubric);
                }
                else
                    throw new IndexOutOfRangeException("Rubric not found");
            }
            foreach (var rubric in targetKeyRubric)
            {
                var nodeRubric = Node.Rubrics[rubric];
                if (nodeRubric != null)
                {
                    NodeTargetKeys.Add(nodeRubric);
                }
                else
                    throw new IndexOutOfRangeException("Rubric not found");
            }

            OriginKeys.Update();
            NodeOriginKeys.Update();
            NodeTargetKeys.Update();
            TargetKeys.Update();
        }

        /// <summary>
        /// Links the child keys.
        /// </summary>
        /// <param name="keyRubrics">The key rubrics.</param>
        /// <exception cref="System.IndexOutOfRangeException">Rubric not found</exception>
        public void LinkChildKeys(IRubrics keyRubrics)
        {
            foreach (IUnique rubric in keyRubrics)
            {
                var targetRubric = Target.Rubrics[rubric];
                if (targetRubric != null)
                {
                    TargetKeys.Add(targetRubric);
                }
                else
                    throw new IndexOutOfRangeException("Rubric not found");
                TargetKeys.Update();
            }
        }

        /// <summary>
        /// Links the child keys.
        /// </summary>
        /// <param name="keyRubricNames">The key rubric names.</param>
        /// <exception cref="System.IndexOutOfRangeException">Rubric not found</exception>
        public void LinkChildKeys(string[] keyRubricNames)
        {
            foreach (var name in keyRubricNames)
            {
                var targetRubric = Target.Rubrics[name];
                if (targetRubric != null )
                {
                    TargetKeys.Add(targetRubric);
                }
                else
                    throw new IndexOutOfRangeException("Rubric not found");
            }
            OriginKeys.Update();
            TargetKeys.Update();
        }

        /// <summary>
        /// Links the node keys.
        /// </summary>
        /// <param name="originKeyRubricNames">The origin key rubric names.</param>
        /// <param name="targetKeyRubricNames">The target key rubric names.</param>
        /// <exception cref="System.IndexOutOfRangeException">Rubric not found</exception>
        public void LinkNodeKeys(string[] originKeyRubricNames, string[] targetKeyRubricNames)
        {
            foreach (var name in originKeyRubricNames)
            {
                var nodeRubric = Node.Rubrics[name];
                if (nodeRubric != null)
                {
                    NodeOriginKeys.Add(nodeRubric);
                }
                else
                    throw new IndexOutOfRangeException("Rubric not found");
            }
            foreach (var name in targetKeyRubricNames)
            {
                var nodeRubric = Node.Rubrics[name];
                if (nodeRubric != null)
                {
                    NodeTargetKeys.Add(nodeRubric);
                }
                else
                    throw new IndexOutOfRangeException("Rubric not found");
            }

            OriginKeys.Update();
            NodeOriginKeys.Update();
            NodeTargetKeys.Update();
            TargetKeys.Update();
        }

        /// <summary>
        /// Links the parent keys.
        /// </summary>
        /// <param name="keyRubricNames">The key rubric names.</param>
        /// <exception cref="System.IndexOutOfRangeException">Rubric not found</exception>
        public void LinkParentKeys(string[] keyRubricNames)
        {
            foreach (var name in keyRubricNames)
            {
                var originRubric = Origin.Rubrics[name];
                if (originRubric != null )
                {
                    OriginKeys.Add(originRubric);
                }
                else
                    throw new IndexOutOfRangeException("Rubric not found");
            }
            OriginKeys.Update();
            TargetKeys.Update();
        }

        #endregion
    }
}
