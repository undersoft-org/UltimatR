
// <copyright file="ISerialObject.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The System namespace.
/// </summary>
namespace System
{



    /// <summary>
    /// Interface ISerialObject
    /// </summary>
    public interface ISerialObject
    {
        #region Methods






        /// <summary>
        /// Locates the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>System.Object.</returns>
        object Locate(object path = null);







        /// <summary>
        /// Merges the specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="name">The name.</param>
        /// <returns>System.Object.</returns>
        object Merge(object source, string name = null);

        #endregion
    }
}
