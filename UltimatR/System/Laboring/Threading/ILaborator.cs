
// <copyright file="ILaborator.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>

/// <summary>
/// The Laboring namespace.
/// </summary>
namespace System.Laboring
{
    /// <summary>
    /// Interface ILaborator
    /// </summary>
    public interface ILaborator
    {




        /// <summary>
        /// Closes the specified safe close.
        /// </summary>
        /// <param name="SafeClose">if set to <c>true</c> [safe close].</param>
        void Close(bool SafeClose);





        /// <summary>
        /// Allocates the specified antcount.
        /// </summary>
        /// <param name="antcount">The antcount.</param>
        /// <returns>Aspect.</returns>
        Aspect Allocate(int antcount = 1);





        /// <summary>
        /// Runs the specified labor.
        /// </summary>
        /// <param name="labor">The labor.</param>
        void Run(Labor labor);





        /// <summary>
        /// Resets the specified antcount.
        /// </summary>
        /// <param name="antcount">The antcount.</param>
        void Reset(int antcount = 1);
    }
}