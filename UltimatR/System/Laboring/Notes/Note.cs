
// <copyright file="Note.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The Laboring namespace.
/// </summary>
namespace System.Laboring
{
    using System.Uniques;

    /// <summary>
    /// Class Note.
    /// Implements the <see cref="System.IUnique" />
    /// </summary>
    /// <seealso cref="System.IUnique" />
    public class Note : IUnique
    {
        #region Fields

        /// <summary>
        /// The parameters
        /// </summary>
        public object[] Parameters;
        /// <summary>
        /// The sender box
        /// </summary>
        public NoteBox SenderBox;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Note" /> class.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="recipient">The recipient.</param>
        /// <param name="Out">The out.</param>
        /// <param name="In">The in.</param>
        /// <param name="Params">The parameters.</param>
        public Note(Labor sender, Labor recipient, NoteEvoker Out, NoteEvokers In, params object[] Params)
        {
            Parameters = Params;

            if (recipient != null)
            {
                Recipient = recipient;
                RecipientName = Recipient.Laborer.Name;
            }

            Sender = sender;
            SenderName = Sender.Laborer.Name;

            if (Out != null)
                EvokerOut = Out;

            if (In != null)
                EvokersIn = In;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Note" /> class.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="Params">The parameters.</param>
        public Note(string sender, params object[] Params) : this(sender, null, null, null, Params)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Note" /> class.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="recipient">The recipient.</param>
        /// <param name="Out">The out.</param>
        /// <param name="In">The in.</param>
        /// <param name="Params">The parameters.</param>
        public Note(string sender, string recipient, NoteEvoker Out, NoteEvokers In, params object[] Params)
        {
            SenderName = sender;
            Parameters = Params;

            if (recipient != null)
                RecipientName = recipient;

            if (Out != null)
                EvokerOut = Out;

            if (In != null)
                EvokersIn = In;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Note" /> class.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="recipient">The recipient.</param>
        /// <param name="Out">The out.</param>
        /// <param name="Params">The parameters.</param>
        public Note(string sender, string recipient, NoteEvoker Out, params object[] Params) : this(sender, recipient, Out, null, Params)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Note" /> class.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="recipient">The recipient.</param>
        /// <param name="Params">The parameters.</param>
        public Note(string sender, string recipient, params object[] Params) : this(sender, recipient, null, null, Params)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the empty.
        /// </summary>
        /// <value>The empty.</value>
        public IUnique Empty => new Ussc();

        /// <summary>
        /// Gets or sets the evoker out.
        /// </summary>
        /// <value>The evoker out.</value>
        public NoteEvoker EvokerOut { get; set; }

        /// <summary>
        /// Gets or sets the evokers in.
        /// </summary>
        /// <value>The evokers in.</value>
        public NoteEvokers EvokersIn { get; set; }

        /// <summary>
        /// Gets or sets the recipient.
        /// </summary>
        /// <value>The recipient.</value>
        public Labor Recipient { get; set; }

        /// <summary>
        /// Gets or sets the name of the recipient.
        /// </summary>
        /// <value>The name of the recipient.</value>
        public string RecipientName { get; set; }

        /// <summary>
        /// Gets or sets the sender.
        /// </summary>
        /// <value>The sender.</value>
        public Labor Sender { get; set; }

        /// <summary>
        /// Gets or sets the name of the sender.
        /// </summary>
        /// <value>The name of the sender.</value>
        public string SenderName { get; set; }

        /// <summary>
        /// Gets or sets the unique key.
        /// </summary>
        /// <value>The unique key.</value>
        public ulong UniqueKey { get => Sender.UniqueKey; set => Sender.UniqueKey = value; }

        /// <summary>
        /// Gets or sets the unique seed.
        /// </summary>
        /// <value>The unique seed.</value>
        public ulong UniqueSeed { get => ((IUnique)Sender).UniqueSeed; set => ((IUnique)Sender).UniqueSeed = value; }

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
            return Sender.CompareTo(other);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><see langword="true" /> if the current object is equal to the <paramref name="other" /> parameter; otherwise, <see langword="false" />.</returns>
        public bool Equals(IUnique other)
        {
            return Sender.Equals(other);
        }

        /// <summary>
        /// Gets the bytes.
        /// </summary>
        /// <returns>System.Byte[].</returns>
        public byte[] GetBytes()
        {
            return Sender.GetBytes();
        }

        /// <summary>
        /// Gets the unique bytes.
        /// </summary>
        /// <returns>System.Byte[].</returns>
        public byte[] GetUniqueBytes()
        {
            return Sender.GetUniqueBytes();
        }

        #endregion
    }
}
