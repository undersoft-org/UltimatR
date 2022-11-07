
// <copyright file="IRubrics.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The Instant namespace.
/// </summary>
namespace System.Instant
{
    using System.Series;




    /// <summary>
    /// Interface IRubrics
    /// Implements the <see cref="System.IUnique" />
    /// Implements the <see cref="System.Series.IDeck{System.Instant.MemberRubric}" />
    /// </summary>
    /// <seealso cref="System.IUnique" />
    /// <seealso cref="System.Series.IDeck{System.Instant.MemberRubric}" />
    public interface IRubrics : IUnique, IDeck<MemberRubric>
    {
        #region Properties




        /// <summary>
        /// Gets the size of the binary.
        /// </summary>
        /// <value>The size of the binary.</value>
        int BinarySize { get; }




        /// <summary>
        /// Gets the binary sizes.
        /// </summary>
        /// <value>The binary sizes.</value>
        int[] BinarySizes { get; }




        /// <summary>
        /// Gets or sets the figures.
        /// </summary>
        /// <value>The figures.</value>
        IFigures Figures { get; set; }




        /// <summary>
        /// Gets or sets the key rubrics.
        /// </summary>
        /// <value>The key rubrics.</value>
        IRubrics KeyRubrics { get; set; }




        /// <summary>
        /// Gets or sets the mappings.
        /// </summary>
        /// <value>The mappings.</value>
        FieldMappings Mappings { get; set; }




        /// <summary>
        /// Gets the ordinals.
        /// </summary>
        /// <value>The ordinals.</value>
        int[] Ordinals { get; }

        #endregion

        #region Methods






        /// <summary>
        /// Gets the bytes.
        /// </summary>
        /// <param name="figure">The figure.</param>
        /// <returns>System.Byte[].</returns>
        byte[] GetBytes(IFigure figure);







        /// <summary>
        /// Gets the unique bytes.
        /// </summary>
        /// <param name="figure">The figure.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>System.Byte[].</returns>
        byte[] GetUniqueBytes(IFigure figure, uint seed = 0);







        /// <summary>
        /// Gets the unique key.
        /// </summary>
        /// <param name="figure">The figure.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>System.UInt64.</returns>
        ulong GetUniqueKey(IFigure figure, uint seed = 0);






        /// <summary>
        /// Sets the unique key.
        /// </summary>
        /// <param name="figure">The figure.</param>
        /// <param name="seed">The seed.</param>
        void SetUniqueKey(IFigure figure, uint seed = 0);




        /// <summary>
        /// Updates this instance.
        /// </summary>
        void Update();

        #endregion
    }
}
