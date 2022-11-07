
// <copyright file="LaborNotes.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The Laboring namespace.
/// </summary>
namespace System.Laboring
{
    using System.Linq;
    using System.Series;

    #region Enums

    /// <summary>
    /// Enum EvokerType
    /// </summary>
    public enum EvokerType
    {



        /// <summary>
        /// The always
        /// </summary>
        Always,



        /// <summary>
        /// The single
        /// </summary>
        Single,



        /// <summary>
        /// The schedule
        /// </summary>
        Schedule,



        /// <summary>
        /// The nome
        /// </summary>
        Nome
    }

    #endregion

    /// <summary>
    /// Class LaborNotes.
    /// Implements the <see cref="System.Series.Catalog{System.Laboring.NoteBox}" />
    /// </summary>
    /// <seealso cref="System.Series.Catalog{System.Laboring.NoteBox}" />
    public class LaborNotes : Catalog<NoteBox>
    {
        #region Properties

        /// <summary>
        /// Gets or sets the case.
        /// </summary>
        /// <value>The case.</value>
        private Case Case { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Sends the specified parameters.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        private void send(Note parameters)
        {
            if (parameters.RecipientName != null && parameters.SenderName != null)
            {
                if (ContainsKey(parameters.RecipientName))
                {
                    NoteBox iobox = Get(parameters.RecipientName);
                    if (iobox != null)
                        iobox.Notify(parameters);
                }
                else if (parameters.Recipient != null)
                {
                    Labor labor = parameters.Recipient;
                    NoteBox iobox = new NoteBox(labor.Laborer.Name);
                    iobox.Labor = labor;
                    iobox.Notify(parameters);
                    SetOutbox(iobox);
                }
                else if (Case != null)
                {
                    var labors = Case.AsValues()
                        .Where(m => m.ContainsKey(parameters.RecipientName))
                        .SelectMany(os => os.AsValues());

                    if (labors.Any())
                    {
                        Labor labor = labors.FirstOrDefault();
                        NoteBox iobox = new NoteBox(labor.Laborer.Name);
                        iobox.Labor = labor;
                        iobox.Notify(parameters);
                        SetOutbox(iobox);
                    }
                }
            }
        }

        /// <summary>
        /// Sends the specified parameters list.
        /// </summary>
        /// <param name="parametersList">The parameters list.</param>
        public void Send(params Note[] parametersList)
        {
            foreach (Note parameters in parametersList)
            {
                send(parameters);
            }
        }

        /// <summary>
        /// Sets the outbox.
        /// </summary>
        /// <param name="value">The value.</param>
        public void SetOutbox(NoteBox value)
        {
            if (value != null)
            {
                if (value.Labor != null)
                {           
                    Put(value.RecipientName, value);
                }
                else
                {
                    var labors = Case.AsValues()
                        .Where(m => m.ContainsKey(value.RecipientName))
                        .SelectMany(os => os.AsValues());

                    if (labors.Any())
                    {
                        Labor labor = labors.First();
                        value.Labor = labor;
                        Put(value.RecipientName, value);
                    }
                }
            }
        }

        /// <summary>
        /// Creates the outbox.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="noteBox">The note box.</param>
        public void CreateOutbox(string key, NoteBox noteBox)
        {
            if (noteBox != null)
            {
                if (noteBox.Labor != null)
                {
                    Labor labor = noteBox.Labor;
                    Put(noteBox.RecipientName, noteBox);
                }
                else
                {
                    var labors = Case.AsValues()
                        .Where(m => m.ContainsKey(key))
                        .SelectMany(os => os.AsValues());

                    if (labors.Any())
                    {
                        Labor labor = labors.FirstOrDefault();
                        noteBox.Labor = labor;
                        Put(key, noteBox);
                    }
                }
            }
            else
            {
                var labors = Case.AsValues()
                    .Where(m => m.ContainsKey(key))
                    .SelectMany(os => os.AsValues());

                if (labors.Any())
                {
                    Labor labor = labors.FirstOrDefault();
                    NoteBox iobox = new NoteBox(labor.Laborer.Name);
                    iobox.Labor = labor;
                    Put(key, iobox);
                }
            }
        }

        #endregion
    }
}
