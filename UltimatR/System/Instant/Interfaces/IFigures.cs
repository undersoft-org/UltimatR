
// <copyright file="IFigures.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The Instant namespace.
/// </summary>
namespace System.Instant
{
    using System.Instant.Linking;
    using System.Instant.Treatments;
    using System.Linq;
    using System.Series;

    /// <summary>
    /// Interface IFigures
    /// Implements the <see cref="System.Series.IDeck{System.Instant.IFigure}" />
    /// Implements the <see cref="System.Instant.IFigure" />
    /// Implements the <see cref="System.ISerialFormatter" />
    /// </summary>
    /// <seealso cref="System.Series.IDeck{System.Instant.IFigure}" />
    /// <seealso cref="System.Instant.IFigure" />
    /// <seealso cref="System.ISerialFormatter" />
    public interface IFigures : IDeck<IFigure>, IFigure, ISerialFormatter
    {
        /// <summary>
        /// Gets or sets the instant.
        /// </summary>
        /// <value>The instant.</value>
        IInstant Instant { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="IFigures" /> is prime.
        /// </summary>
        /// <value><c>true</c> if prime; otherwise, <c>false</c>.</value>
        bool Prime { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="IFigure" /> at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>IFigure.</returns>
        new IFigure this[int index] { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="System.Object" /> at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>System.Object.</returns>
        object this[int index, string propertyName] { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="System.Object" /> at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="fieldId">The field identifier.</param>
        /// <returns>System.Object.</returns>
        object this[int index, int fieldId] { get; set; }

        /// <summary>
        /// Gets or sets the rubrics.
        /// </summary>
        /// <value>The rubrics.</value>
        IRubrics Rubrics { get; set; }

        /// <summary>
        /// Gets or sets the key rubrics.
        /// </summary>
        /// <value>The key rubrics.</value>
        IRubrics KeyRubrics { get; set; }

        /// <summary>
        /// Creates new figure.
        /// </summary>
        /// <returns>IFigure.</returns>
        IFigure NewFigure();

        /// <summary>
        /// Creates new sleeve.
        /// </summary>
        /// <returns>ISleeve.</returns>
        ISleeve NewSleeve();

        /// <summary>
        /// Gets or sets the type of the figure.
        /// </summary>
        /// <value>The type of the figure.</value>
        Type FigureType { get; set; }

        /// <summary>
        /// Gets or sets the size of the figure.
        /// </summary>
        /// <value>The size of the figure.</value>
        int FigureSize { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        Type Type { get; set; }

        /// <summary>
        /// Gets or sets the view.
        /// </summary>
        /// <value>The view.</value>
        IQueryable<IFigure> View { get; set; }

        /// <summary>
        /// Gets or sets the summary.
        /// </summary>
        /// <value>The summary.</value>
        IFigure Summary { get; set; }

        /// <summary>
        /// Gets or sets the filter.
        /// </summary>
        /// <value>The filter.</value>
        FigureFilter Filter { get; set; }

        /// <summary>
        /// Gets or sets the sort.
        /// </summary>
        /// <value>The sort.</value>
        FigureSort Sort { get; set; }

        /// <summary>
        /// Gets or sets the predicate.
        /// </summary>
        /// <value>The predicate.</value>
        Func<IFigure, bool> Predicate { get; set; }

        /// <summary>
        /// Gets or sets the treatment.
        /// </summary>
        /// <value>The treatment.</value>
        Treatment Treatment { get; set; }

        /// <summary>
        /// Gets or sets the linker.
        /// </summary>
        /// <value>The linker.</value>
        Linker Linker { get; set; }

        /// <summary>
        /// Gets or sets the computations.
        /// </summary>
        /// <value>The computations.</value>
        IDeck<IComputation> Computations { get; set; }
    }
}
