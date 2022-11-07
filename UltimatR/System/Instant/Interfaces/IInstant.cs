
// <copyright file="IInstant.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The Instant namespace.
/// </summary>
namespace System.Instant
{



    /// <summary>
    /// Interface IInstant
    /// </summary>
    public interface IInstant
    {
        #region Properties




        /// <summary>
        /// Gets or sets the type of the base.
        /// </summary>
        /// <value>The type of the base.</value>
        Type BaseType { get; set; }




        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        string Name { get; set; }




        /// <summary>
        /// Gets the rubrics.
        /// </summary>
        /// <value>The rubrics.</value>
        IRubrics Rubrics { get; }




        /// <summary>
        /// Gets the size.
        /// </summary>
        /// <value>The size.</value>
        int Size { get; }




        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        Type Type { get; set; }

        #endregion

        #region Methods





        /// <summary>
        /// News this instance.
        /// </summary>
        /// <returns>System.Object.</returns>
        object New();

        #endregion
    }
}
