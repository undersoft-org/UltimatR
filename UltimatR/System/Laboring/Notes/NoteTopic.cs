
// <copyright file="NoteTopic.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The Laboring namespace.
/// </summary>
namespace System.Laboring
{
    using System.Collections.Generic;
    using System.Series;




    /// <summary>
    /// Class NoteTopic.
    /// Implements the <see cref="System.Series.Catalog{System.Laboring.Note}" />
    /// </summary>
    /// <seealso cref="System.Series.Catalog{System.Laboring.Note}" />
    public class NoteTopic : Catalog<Note>
    {
        #region Fields




        /// <summary>
        /// The recipient box
        /// </summary>
        public NoteBox RecipientBox;

        #endregion

        #region Constructors







        /// <summary>
        /// Initializes a new instance of the <see cref="NoteTopic" /> class.
        /// </summary>
        /// <param name="senderName">Name of the sender.</param>
        /// <param name="notelist">The notelist.</param>
        /// <param name="recipient">The recipient.</param>
        public NoteTopic(string senderName, IList<Note> notelist, NoteBox recipient = null)
        {
            if (recipient != null)
            {
                RecipientBox = recipient;
            }
            if (notelist != null && notelist.Count > 0)
            {
                foreach (Note evocation in notelist)
                {
                    evocation.SenderName = SenderName;
                    Notes = evocation;
                }
            }
        }






        /// <summary>
        /// Initializes a new instance of the <see cref="NoteTopic" /> class.
        /// </summary>
        /// <param name="senderName">Name of the sender.</param>
        /// <param name="note">The note.</param>
        /// <param name="recipient">The recipient.</param>
        public NoteTopic(string senderName, Note note, NoteBox recipient = null)
        {
            if (recipient != null)
            {
                RecipientBox = recipient;
            }
            SenderName = senderName;
            Notes = note;
        }





        /// <summary>
        /// Initializes a new instance of the <see cref="NoteTopic" /> class.
        /// </summary>
        /// <param name="senderName">Name of the sender.</param>
        /// <param name="recipient">The recipient.</param>
        public NoteTopic(string senderName, NoteBox recipient = null)
        {
            if (recipient != null)
                RecipientBox = recipient;
            SenderName = senderName;
        }






        /// <summary>
        /// Initializes a new instance of the <see cref="NoteTopic" /> class.
        /// </summary>
        /// <param name="senderName">Name of the sender.</param>
        /// <param name="recipient">The recipient.</param>
        /// <param name="parameters">The parameters.</param>
        public NoteTopic(string senderName, NoteBox recipient = null, params object[] parameters)
        {
            if (recipient != null)
                RecipientBox = recipient;
            SenderName = senderName;
            if (parameters != null)
            {
                if (parameters[0].GetType() == typeof(Dictionary<string, object>))
                {
                    Note result = new Note(senderName, parameters);
                    Notes = result;
                }
            }
        }

        #endregion

        #region Properties




        /// <summary>
        /// Gets or sets the notes.
        /// </summary>
        /// <value>The notes.</value>
        public Note Notes
        {
            get
            {
                Note _result = null;
                TryDequeue(out _result);
                return _result;
            }
            set
            {
                value.SenderName = SenderName;
                Enqueue(DateTime.Now.ToBinary(), value);
                if (RecipientBox != null)
                    RecipientBox.QualifyToEvoke();
            }
        }




        /// <summary>
        /// Gets or sets the name of the sender.
        /// </summary>
        /// <value>The name of the sender.</value>
        public string SenderName { get; set; }

        #endregion

        #region Methods





        /// <summary>
        /// Notifies the specified note list.
        /// </summary>
        /// <param name="noteList">The note list.</param>
        public void Notify(IList<Note> noteList)
        {
            foreach (Note result in noteList)
                Notes = result;
        }





        /// <summary>
        /// Notifies the specified note.
        /// </summary>
        /// <param name="note">The note.</param>
        public void Notify(Note note)
        {
            Notes = note;
        }





        /// <summary>
        /// Notifies the specified parameters.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        public void Notify(params object[] parameters)
        {
            if (parameters != null)
            {
                Note result = new Note(SenderName);
                result.Parameters = parameters;
                Notes = result;
            }
        }






        /// <summary>
        /// Notifies the specified sender name.
        /// </summary>
        /// <param name="senderName">Name of the sender.</param>
        /// <param name="parameters">The parameters.</param>
        public void Notify(string senderName, params object[] parameters)
        {
            SenderName = senderName;
            if (parameters != null)
            {
                Note result = new Note(senderName);
                result.Parameters = parameters;
                Notes = result;
            }
        }

        #endregion
    }
}
