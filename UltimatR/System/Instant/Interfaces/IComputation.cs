
// <copyright file="IComputation.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The Instant namespace.
/// </summary>
namespace System.Instant
{



    /// <summary>
    /// Interface IComputation
    /// Implements the <see cref="System.IUnique" />
    /// </summary>
    /// <seealso cref="System.IUnique" />
    public interface IComputation : IUnique
    {
        #region Methods





        /// <summary>
        /// Computes this instance.
        /// </summary>
        /// <returns>IFigures.</returns>
        IFigures Compute();

        #endregion
    }
}
