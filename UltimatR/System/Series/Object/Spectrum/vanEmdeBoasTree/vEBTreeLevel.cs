
// <copyright file="vEBTreeLevel.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The Spectrum namespace.
/// </summary>
namespace System.Series.Spectrum
{
    using System.Collections.Generic;

    /// <summary>
    /// Class vEBTreeLevel.
    /// </summary>
    public class vEBTreeLevel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="vEBTreeLevel" /> class.
        /// </summary>
        public vEBTreeLevel()
        {
            Level = 0;
            BaseOffset = 0;
            Nodes = null;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the base offset.
        /// </summary>
        /// <value>The base offset.</value>
        public int BaseOffset { get; set; }

        /// <summary>
        /// Gets or sets the count.
        /// </summary>
        /// <value>The count.</value>
        public byte Count { get; set; }

        /// <summary>
        /// Gets or sets the level.
        /// </summary>
        /// <value>The level.</value>
        public byte Level { get; set; }

        /// <summary>
        /// Gets or sets the nodes.
        /// </summary>
        /// <value>The nodes.</value>
        public IList<vEBTreeNode> Nodes { get; set; }

        #endregion
    }
}
