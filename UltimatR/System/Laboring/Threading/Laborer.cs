
// <copyright file="Laborer.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>

/// <summary>
/// The Laboring namespace.
/// </summary>
namespace System.Laboring
{
    using Collections.Generic;
    using Series;
using System.Diagnostics;
    using Uniques;

    /// <summary>
    /// Class Laborer.
    /// Implements the <see cref="System.IUnique" />
    /// Implements the <see cref="System.Laboring.ILaborer" />
    /// </summary>
    /// <seealso cref="System.IUnique" />
    /// <seealso cref="System.Laboring.ILaborer" />
    public class Laborer : IUnique, ILaborer
    {
        #region Fields

        /// <summary>
        /// The input
        /// </summary>
        private readonly Catalog<object> input = new Catalog<object>();
        /// <summary>
        /// The output
        /// </summary>
        private readonly Catalog<object> output = new Catalog<object>();
        /// <summary>
        /// The serial code
        /// </summary>
        private Ussn SerialCode;

        #endregion

        #region Constructors

        /// <summary>
        /// Prevents a default instance of the <see cref="Laborer" /> class from being created.
        /// </summary>
        private Laborer()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Laborer" /> class.
        /// </summary>
        /// <param name="Name">The name.</param>
        /// <param name="Method">The method.</param>
        public Laborer(string Name, IDeputy Method) : this()
        {
            Process = Method;
            this.Name = Name;
            ulong seed = Unique.New;
            SerialCode = new Ussn((Process.UniqueKey).UniqueKey(seed), seed);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the empty.
        /// </summary>
        /// <value>The empty.</value>
        public IUnique Empty => new Ussc();

        /// <summary>
        /// Gets or sets the evokers.
        /// </summary>
        /// <value>The evokers.</value>
        public NoteEvokers Evokers { get; set; } = new NoteEvokers();

        /// <summary>
        /// Gets the input.
        /// </summary>
        /// <returns>System.Object.</returns>
        public object GetInput()
        {
            object entry;
            input.TryDequeue(out entry);
            return entry;
        }
        /// <summary>
        /// Sets the input.
        /// </summary>
        /// <param name="value">The value.</param>
        public void   SetInput(object value)
        {
            input.Enqueue(value);
        }

        /// <summary>
        /// Gets the output.
        /// </summary>
        /// <returns>System.Object.</returns>
        public object GetOutput()
        {
            object entry;
            output.TryDequeue(out entry);
            return entry;
        }
        /// <summary>
        /// Sets the output.
        /// </summary>
        /// <param name="value">The value.</param>
        public void   SetOutput(object value)
        {
            output.Enqueue(value);
        }

        /// <summary>
        /// Gets or sets the labor.
        /// </summary>
        /// <value>The labor.</value>
        public Labor Labor { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the unique key.
        /// </summary>
        /// <value>The unique key.</value>
        public ulong UniqueKey { get => SerialCode.UniqueKey; set => SerialCode.UniqueKey = value; }

        /// <summary>
        /// Gets or sets the unique seed.
        /// </summary>
        /// <value>The unique seed.</value>
        public ulong UniqueSeed { get => SerialCode.UniqueSeed; set => SerialCode.UniqueSeed = value; }

        /// <summary>
        /// Gets or sets the process.
        /// </summary>
        /// <value>The process.</value>
        public IDeputy Process { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Results to.
        /// </summary>
        /// <param name="recipient">The recipient.</param>
        public void ResultTo(Labor recipient)
        {
            Evokers.Add(new NoteEvoker(Labor, recipient,  Labor));
        }

        /// <summary>
        /// Results to.
        /// </summary>
        /// <param name="Recipient">The recipient.</param>
        /// <param name="RelationLabors">The relation labors.</param>
        public void ResultTo(Labor Recipient, params Labor[] RelationLabors)
        {
            Evokers.Add(new NoteEvoker(Labor, Recipient, RelationLabors));
        }

        /// <summary>
        /// Results to.
        /// </summary>
        /// <param name="RecipientName">Name of the recipient.</param>
        public void ResultTo(string RecipientName)
        {
            Evokers.Add(new NoteEvoker(Labor, RecipientName, Name ));
        }

        /// <summary>
        /// Results to.
        /// </summary>
        /// <param name="RecipientName">Name of the recipient.</param>
        /// <param name="RelationNames">The relation names.</param>
        public void ResultTo(string RecipientName, params string[] RelationNames)
        {
            Evokers.Add(new NoteEvoker(Labor, RecipientName, RelationNames));
        }

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
            return SerialCode.GetBytes();
        }

        /// <summary>
        /// Gets the unique bytes.
        /// </summary>
        /// <returns>System.Byte[].</returns>
        public byte[] GetUniqueBytes()
        {
            return SerialCode.GetUniqueBytes();
        }

        #endregion
    }
}
