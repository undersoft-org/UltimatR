
// <copyright file="ISerialBuffer.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The System namespace.
/// </summary>
namespace System
{



    /// <summary>
    /// Interface ISerialBuffer
    /// Implements the <see cref="System.IDisposable" />
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public interface ISerialBuffer : IDisposable
    {
        #region Properties




        /// <summary>
        /// Gets or sets the block offset.
        /// </summary>
        /// <value>The block offset.</value>
        int BlockOffset { get; set; }




        /// <summary>
        /// Gets or sets the size of the block.
        /// </summary>
        /// <value>The size of the block.</value>
        long BlockSize { get; set; }




        /// <summary>
        /// Gets the deserial block.
        /// </summary>
        /// <value>The deserial block.</value>
        byte[] DeserialBlock { get; }




        /// <summary>
        /// Gets or sets the deserial block identifier.
        /// </summary>
        /// <value>The deserial block identifier.</value>
        int DeserialBlockId { get; set; }




        /// <summary>
        /// Gets the deserial block PTR.
        /// </summary>
        /// <value>The deserial block PTR.</value>
        IntPtr DeserialBlockPtr { get; }




        /// <summary>
        /// Gets or sets the serial block.
        /// </summary>
        /// <value>The serial block.</value>
        byte[] SerialBlock { get; set; }




        /// <summary>
        /// Gets or sets the serial block identifier.
        /// </summary>
        /// <value>The serial block identifier.</value>
        int SerialBlockId { get; set; }




        /// <summary>
        /// Gets the serial block PTR.
        /// </summary>
        /// <value>The serial block PTR.</value>
        IntPtr SerialBlockPtr { get; }




        /// <summary>
        /// Gets the site.
        /// </summary>
        /// <value>The site.</value>
        ServiceSite Site { get; }

        #endregion
    }
}
