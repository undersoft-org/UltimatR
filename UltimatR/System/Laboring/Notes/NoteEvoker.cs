
// <copyright file="NoteEvoker.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The Laboring namespace.
/// </summary>
namespace System.Laboring
{
    using System.Collections.Generic;
    using System.Extract;
    using System.Linq;
    using System.Series;
    using System.Uniques;




    /// <summary>
    /// Class NoteEvoker.
    /// Implements the <see cref="System.Series.Catalog{System.Laboring.Labor}" />
    /// Implements the <see cref="System.IUnique" />
    /// </summary>
    /// <seealso cref="System.Series.Catalog{System.Laboring.Labor}" />
    /// <seealso cref="System.IUnique" />
    public class NoteEvoker : Catalog<Labor>, IUnique
    {
        #region Fields

        /// <summary>
        /// The related labors
        /// </summary>
        public IDeck<Labor> RelatedLabors = new Board<Labor>();
        /// <summary>
        /// The related labor names
        /// </summary>
        public IDeck<string> RelatedLaborNames = new Board<string>();
        /// <summary>
        /// The serial code
        /// </summary>
        private Ussn SerialCode;

        #endregion

        #region Constructors







        /// <summary>
        /// Initializes a new instance of the <see cref="NoteEvoker" /> class.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="recipient">The recipient.</param>
        /// <param name="relayLabors">The relay labors.</param>
        public NoteEvoker(Labor sender, Labor recipient, params Labor[] relayLabors)
        {
            Sender = sender;
            SenderName = sender.Laborer.Name;
            Recipient = recipient;
            RecipientName = recipient.Laborer.Name;
            SerialCode = new Ussn(SenderName.UniqueKey(RecipientName.UniqueKey()), RecipientName.UniqueKey());
            RelatedLabors.Add(relayLabors);
            RelatedLaborNames.Add(RelatedLabors.Select(rn => rn.Laborer.Name));
        }






        /// <summary>
        /// Initializes a new instance of the <see cref="NoteEvoker" /> class.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="recipient">The recipient.</param>
        /// <param name="relayNames">The relay names.</param>
        public NoteEvoker(Labor sender, Labor recipient, params string[] relayNames)
        {
            Sender = sender;
            SenderName = sender.Name;
            Recipient = recipient;
            RecipientName = recipient.Name;
            SerialCode = new Ussn(SenderName.UniqueKey(RecipientName.UniqueKey()), RecipientName.UniqueKey());
            RelatedLaborNames.Add(relayNames);
            var namekeys = relayNames.ForEach(s => s.UniqueKey());
            RelatedLabors.Add(Sender.Case.AsValues()
                .Where(m => m
                .Any(k => namekeys.Contains(k.UniqueKey)))
                .SelectMany(os => os.AsValues()).ToList());
        }






        /// <summary>
        /// Initializes a new instance of the <see cref="NoteEvoker" /> class.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="recipientName">Name of the recipient.</param>
        /// <param name="relayLabors">The relay labors.</param>
        public NoteEvoker(Labor sender, string recipientName, params Labor[] relayLabors)
        {
            Sender = sender;
            SenderName = sender.Name;
            RecipientName = recipientName;
            SerialCode = new Ussn(SenderName.UniqueKey(RecipientName.UniqueKey()), RecipientName.UniqueKey());
            var rcpts = Sender.Case.AsValues()
                                        .Where(m => m.ContainsKey(recipientName))
                                            .SelectMany(os => os.AsValues()).ToArray();
            Recipient = rcpts.FirstOrDefault();
            RelatedLabors.Add(relayLabors);
            RelatedLaborNames.Add(RelatedLabors.Select(rn => rn.Laborer.Name));
        }






        /// <summary>
        /// Initializes a new instance of the <see cref="NoteEvoker" /> class.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="recipientName">Name of the recipient.</param>
        /// <param name="relayNames">The relay names.</param>
        public NoteEvoker(Labor sender, string recipientName,  params string[] relayNames)
        {
            Sender = sender;
            SenderName = sender.Laborer.Name;
            var rcpts = Sender.Case.AsValues()
                .Where(m => m.ContainsKey(recipientName))
                .SelectMany(os => os.AsValues()).ToArray();
            Recipient = rcpts.FirstOrDefault();
            RecipientName = recipientName;
            SerialCode = new Ussn(SenderName.UniqueKey(RecipientName.UniqueKey()), RecipientName.UniqueKey());
            RelatedLaborNames.Add(relayNames);
            var namekeys = relayNames.ForEach(s => s.UniqueKey());
            RelatedLabors.Add(Sender.Case.AsValues()
                .Where(m => m
                    .Any(k => namekeys.Contains(k.UniqueKey)))
                .SelectMany(os => os.AsValues()).ToList());
        }

        #endregion

        #region Properties




        /// <summary>
        /// Gets the empty.
        /// </summary>
        /// <value>The empty.</value>
        public IUnique Empty => new Usid();




        /// <summary>
        /// Gets or sets the name of the evoker.
        /// </summary>
        /// <value>The name of the evoker.</value>
        public string EvokerName { get; set; }




        /// <summary>
        /// Gets or sets the type of the evoker.
        /// </summary>
        /// <value>The type of the evoker.</value>
        public EvokerType EvokerType { get; set; }




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
        public new ulong UniqueKey { get => SerialCode.UniqueKey; set => SerialCode.SetUniqueKey(value); }




        /// <summary>
        /// Gets or sets the unique seed.
        /// </summary>
        /// <value>The unique seed.</value>
        public ulong UniqueSeed { get => SerialCode.UniqueSeed; set => SerialCode.SetUniqueSeed(value); }

        #endregion

        #region Methods






        /// <summary>
        /// Compares to.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>System.Int32.</returns>
        public int CompareTo(IUnique other)
        {
            return SerialCode.CompareTo(other);
        }






        /// <summary>
        /// Equalses the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
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
            return ($"{SenderName}.{RecipientName}").GetBytes();
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
