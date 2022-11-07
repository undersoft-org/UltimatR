
// <copyright file="ITraceable.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The Instant namespace.
/// </summary>
namespace System.Instant
{
    /// <summary>
    /// Interface ITraceable
    /// </summary>
    public interface ITraceable
    {
        /// <summary>
        /// Gets the variator.
        /// </summary>
        /// <value>The variator.</value>
        IVariety Variator { get; }

        /// <summary>
        /// Gets the notice change.
        /// </summary>
        /// <value>The notice change.</value>
        IDeputy NoticeChange { get; }

        /// <summary>
        /// Gets the notice changing.
        /// </summary>
        /// <value>The notice changing.</value>
        IDeputy NoticeChanging { get; }
    }
}
