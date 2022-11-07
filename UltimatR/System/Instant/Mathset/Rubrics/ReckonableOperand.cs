
// <copyright file="ReckonableOperand.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The Mathset namespace.
/// </summary>
namespace System.Instant.Mathset
{
    using System;

    #region Enums

    /// <summary>
    /// Enum ComputeableOperand
    /// </summary>
    [Serializable]
    public enum ComputeableOperand
    {
        /// <summary>
        /// The none
        /// </summary>
        None,
        /// <summary>
        /// The add
        /// </summary>
        Add,
        /// <summary>
        /// The subtract
        /// </summary>
        Subtract,
        /// <summary>
        /// The multiply
        /// </summary>
        Multiply,
        /// <summary>
        /// The divide
        /// </summary>
        Divide
    }

    #endregion
}
