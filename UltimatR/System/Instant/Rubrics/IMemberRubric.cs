
// <copyright file="IMemberRubric.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



using System.Reflection;

/// <summary>
/// The Instant namespace.
/// </summary>
namespace System.Instant
{



    /// <summary>
    /// Interface IMemberRubric
    /// </summary>
    public interface IMemberRubric
    {
        #region Properties


        /// <summary>
        /// Gets the member information.
        /// </summary>
        /// <value>The member information.</value>
        MemberInfo MemberInfo { get; }



        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="IMemberRubric" /> is editable.
        /// </summary>
        /// <value><c>true</c> if editable; otherwise, <c>false</c>.</value>
        bool Editable { get; set; }




        /// <summary>
        /// Gets or sets the rubric attributes.
        /// </summary>
        /// <value>The rubric attributes.</value>
        object[] RubricAttributes { get; set; }




        /// <summary>
        /// Gets or sets the rubric identifier.
        /// </summary>
        /// <value>The rubric identifier.</value>
        int RubricId { get; set; }




        /// <summary>
        /// Gets or sets the name of the rubric.
        /// </summary>
        /// <value>The name of the rubric.</value>
        string RubricName { get; set; }




        /// <summary>
        /// Gets or sets the rubric offset.
        /// </summary>
        /// <value>The rubric offset.</value>
        int RubricOffset { get; set; }




        /// <summary>
        /// Gets or sets the size of the rubric.
        /// </summary>
        /// <value>The size of the rubric.</value>
        int RubricSize { get; set; }




        /// <summary>
        /// Gets or sets the type of the rubric.
        /// </summary>
        /// <value>The type of the rubric.</value>
        Type RubricType { get; set; }




        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="IMemberRubric" /> is visible.
        /// </summary>
        /// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
        bool Visible { get; set; }

        #endregion
    }
}
