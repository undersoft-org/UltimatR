
// <copyright file="vEBTreeNode.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The Spectrum namespace.
/// </summary>
namespace System.Series.Spectrum
{
    /// <summary>
    /// Class vEBTreeNode.
    /// </summary>
    public class vEBTreeNode
    {
        #region Properties

        /// <summary>
        /// Gets or sets the index offset.
        /// </summary>
        /// <value>The index offset.</value>
        public int IndexOffset { get; set; }

        /// <summary>
        /// Gets or sets the node counter.
        /// </summary>
        /// <value>The node counter.</value>
        public int NodeCounter { get; set; }

        /// <summary>
        /// Gets or sets the size of the node.
        /// </summary>
        /// <value>The size of the node.</value>
        public int NodeSize { get; set; }

        #endregion
    }
}
