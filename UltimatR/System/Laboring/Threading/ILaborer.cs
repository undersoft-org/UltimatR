
// <copyright file="ILaborer.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>

using System.Collections.Generic;

/// <summary>
/// The Laboring namespace.
/// </summary>
namespace System.Laboring
{
    /// <summary>
    /// Interface ILaborer
    /// </summary>
    public interface ILaborer
    {



        /// <summary>
        /// Gets the input.
        /// </summary>
        /// <returns>System.Object.</returns>
        object GetInput();
        /// <summary>
        /// Sets the input.
        /// </summary>
        /// <param name="value">The value.</param>
        void SetInput(object value);




        /// <summary>
        /// Gets the output.
        /// </summary>
        /// <returns>System.Object.</returns>
        object GetOutput();
        /// <summary>
        /// Sets the output.
        /// </summary>
        /// <param name="value">The value.</param>
        void SetOutput(object value);




        /// <summary>
        /// Gets or sets the evokers.
        /// </summary>
        /// <value>The evokers.</value>
        NoteEvokers Evokers { get; set; }




        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        string Name { get; set; }




        /// <summary>
        /// Gets or sets the process.
        /// </summary>
        /// <value>The process.</value>
        IDeputy Process { get; set; }





        /// <summary>
        /// Results to.
        /// </summary>
        /// <param name="Recipient">The recipient.</param>
        void ResultTo(Labor Recipient);






        /// <summary>
        /// Results to.
        /// </summary>
        /// <param name="Recipient">The recipient.</param>
        /// <param name="RelationLabors">The relation labors.</param>
        void ResultTo(Labor Recipient, params Labor[] RelationLabors);





        /// <summary>
        /// Results to.
        /// </summary>
        /// <param name="RecipientName">Name of the recipient.</param>
        void ResultTo(string RecipientName);






        /// <summary>
        /// Results to.
        /// </summary>
        /// <param name="RecipientName">Name of the recipient.</param>
        /// <param name="RelationNames">The relation names.</param>
        void ResultTo(string RecipientName, params string[] RelationNames);
    }
}