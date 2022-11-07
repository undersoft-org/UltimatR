
// <copyright file="IRubric.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The Instant namespace.
/// </summary>
namespace System.Instant
{
    using System.Instant.Linking;
    using System.Instant.Treatments;




    /// <summary>
    /// Interface IRubric
    /// Implements the <see cref="System.Instant.IMemberRubric" />
    /// Implements the <see cref="System.IUnique" />
    /// </summary>
    /// <seealso cref="System.Instant.IMemberRubric" />
    /// <seealso cref="System.IUnique" />
    public interface IRubric : IMemberRubric, IUnique
    {
        #region Properties




        /// <summary>
        /// Gets or sets the aggregate link identifier.
        /// </summary>
        /// <value>The aggregate link identifier.</value>
        int AggregateLinkId { get; set; }




        /// <summary>
        /// Gets or sets the aggregate links.
        /// </summary>
        /// <value>The aggregate links.</value>
        Links AggregateLinks { get; set; }




        /// <summary>
        /// Gets or sets the aggregate operand.
        /// </summary>
        /// <value>The aggregate operand.</value>
        AggregateOperand AggregateOperand { get; set; }




        /// <summary>
        /// Gets or sets the aggregate ordinal.
        /// </summary>
        /// <value>The aggregate ordinal.</value>
        int AggregateOrdinal { get; set; }




        /// <summary>
        /// Gets or sets the aggregate rubric.
        /// </summary>
        /// <value>The aggregate rubric.</value>
        IRubric AggregateRubric { get; set; }




        /// <summary>
        /// Gets or sets the identity order.
        /// </summary>
        /// <value>The identity order.</value>
        short IdentityOrder { get; set; }




        /// <summary>
        /// Gets or sets a value indicating whether this instance is autoincrement.
        /// </summary>
        /// <value><c>true</c> if this instance is autoincrement; otherwise, <c>false</c>.</value>
        bool IsAutoincrement { get; set; }




        /// <summary>
        /// Gets or sets a value indicating whether this instance is expandable.
        /// </summary>
        /// <value><c>true</c> if this instance is expandable; otherwise, <c>false</c>.</value>
        bool IsExpandable { get; set; }




        /// <summary>
        /// Gets or sets a value indicating whether this instance is database null.
        /// </summary>
        /// <value><c>true</c> if this instance is database null; otherwise, <c>false</c>.</value>
        bool IsDBNull { get; set; }




        /// <summary>
        /// Gets or sets a value indicating whether this instance is identity.
        /// </summary>
        /// <value><c>true</c> if this instance is identity; otherwise, <c>false</c>.</value>
        bool IsIdentity { get; set; }




        /// <summary>
        /// Gets or sets a value indicating whether this instance is unique.
        /// </summary>
        /// <value><c>true</c> if this instance is unique; otherwise, <c>false</c>.</value>
        bool IsUnique { get; set; }




        /// <summary>
        /// Gets or sets a value indicating whether this instance is key.
        /// </summary>
        /// <value><c>true</c> if this instance is key; otherwise, <c>false</c>.</value>
        bool IsKey { get; set; }




        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="IRubric" /> is required.
        /// </summary>
        /// <value><c>true</c> if required; otherwise, <c>false</c>.</value>
        bool Required { get; set; }




        /// <summary>
        /// Gets or sets the summary operand.
        /// </summary>
        /// <value>The summary operand.</value>
        AggregateOperand SummaryOperand { get; set; }




        /// <summary>
        /// Gets or sets the summary ordinal.
        /// </summary>
        /// <value>The summary ordinal.</value>
        int SummaryOrdinal { get; set; }




        /// <summary>
        /// Gets or sets the summary rubric.
        /// </summary>
        /// <value>The summary rubric.</value>
        IRubric SummaryRubric { get; set; }

        #endregion
    }
}
