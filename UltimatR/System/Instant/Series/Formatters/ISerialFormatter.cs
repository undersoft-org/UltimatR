
// <copyright file="ISerialFormatter.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The System namespace.
/// </summary>
namespace System
{
    using System.IO;




    /// <summary>
    /// Interface ISerialFormatter
    /// </summary>
    public interface ISerialFormatter
    {
        #region Properties




        /// <summary>
        /// Gets or sets the deserial count.
        /// </summary>
        /// <value>The deserial count.</value>
        int DeserialCount { get; set; }




        /// <summary>
        /// Gets the items count.
        /// </summary>
        /// <value>The items count.</value>
        int ItemsCount { get; }




        /// <summary>
        /// Gets or sets the progress count.
        /// </summary>
        /// <value>The progress count.</value>
        int ProgressCount { get; set; }




        /// <summary>
        /// Gets or sets the serial count.
        /// </summary>
        /// <value>The serial count.</value>
        int SerialCount { get; set; }

        #endregion

        #region Methods







        /// <summary>
        /// Deserializes the specified buffer.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="serialFormat">The serial format.</param>
        /// <returns>System.Object.</returns>
        object Deserialize(ISerialBuffer buffer, SerialFormat serialFormat = SerialFormat.Binary);







        /// <summary>
        /// Deserializes the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="serialFormat">The serial format.</param>
        /// <returns>System.Object.</returns>
        object Deserialize(Stream stream, SerialFormat serialFormat = SerialFormat.Binary);





        /// <summary>
        /// Gets the header.
        /// </summary>
        /// <returns>System.Object.</returns>
        object GetHeader();





        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <returns>System.Object[].</returns>
        object[] GetMessage();









        /// <summary>
        /// Serializes the specified buffer.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="batchSize">Size of the batch.</param>
        /// <param name="serialFormat">The serial format.</param>
        /// <returns>System.Int32.</returns>
        int Serialize(ISerialBuffer buffer, int offset, int batchSize, SerialFormat serialFormat = SerialFormat.Binary);









        /// <summary>
        /// Serializes the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="batchSize">Size of the batch.</param>
        /// <param name="serialFormat">The serial format.</param>
        /// <returns>System.Int32.</returns>
        int Serialize(Stream stream, int offset, int batchSize, SerialFormat serialFormat = SerialFormat.Binary);

        #endregion
    }
}
