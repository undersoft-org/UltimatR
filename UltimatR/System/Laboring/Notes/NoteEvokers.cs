
// <copyright file="NoteEvokers.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The Laboring namespace.
/// </summary>
namespace System.Laboring
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Series;


    /// <summary>
    /// Class NoteEvokers.
    /// Implements the <see cref="System.Series.Catalog{System.Laboring.NoteEvoker}" />
    /// </summary>
    /// <seealso cref="System.Series.Catalog{System.Laboring.NoteEvoker}" />
    public class NoteEvokers : Catalog<NoteEvoker>
    {
        /// <summary>
        /// Determines whether this instance contains the object.
        /// </summary>
        /// <param name="objectives">The objectives.</param>
        /// <returns><c>true</c> if [contains] [the specified objectives]; otherwise, <c>false</c>.</returns>
        public bool Contains(IEnumerable<Labor> objectives)
        {
            return this.AsValues().Any(t => t.RelatedLabors.Any(ro => objectives.All(o => ReferenceEquals(ro, o))));
        }
        /// <summary>
        /// Determines whether this instance contains the object.
        /// </summary>
        /// <param name="relayNames">The relay names.</param>
        /// <returns><c>true</c> if [contains] [the specified relay names]; otherwise, <c>false</c>.</returns>
        public bool Contains(IEnumerable<string> relayNames)
        {
            return this.AsValues().Any(t => t.RelatedLaborNames.SequenceEqual(relayNames));
        }

        /// <summary>
        /// Gets the <see cref="NoteEvoker" /> with the specified related labor name.
        /// </summary>
        /// <param name="relatedLaborName">Name of the related labor.</param>
        /// <returns>NoteEvoker.</returns>
        public NoteEvoker this[string relatedLaborName]
        {
            get
            {
                return this.AsValues().FirstOrDefault(c => c.RelatedLaborNames.Contains(relatedLaborName));
            }
        }
        /// <summary>
        /// Gets the <see cref="NoteEvoker" /> with the specified related labor.
        /// </summary>
        /// <param name="relatedLabor">The related labor.</param>
        /// <returns>NoteEvoker.</returns>
        public NoteEvoker this[Labor relatedLabor]
        {
            get
            {
                return this.AsValues().FirstOrDefault(c => c.RelatedLabors.Contains(relatedLabor));
            }
        }
    }
}
