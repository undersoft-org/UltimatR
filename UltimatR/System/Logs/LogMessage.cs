
// <copyright file="LogMessage.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The System namespace.
/// </summary>
namespace System
{
    using System.Threading;




    /// <summary>
    /// Class LogMessage.
    /// </summary>
    public class LogMessage
    {
        #region Fields

        /// <summary>
        /// The automatic identifier
        /// </summary>
        private static long autoId = DateTime.Now.Ticks;

        #endregion

        #region Constructors




        /// <summary>
        /// Initializes a new instance of the <see cref="LogMessage" /> class.
        /// </summary>
        public LogMessage()
        {
            Id = Interlocked.Increment(ref autoId);
        }

        #endregion

        #region Properties




        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public long Id { get; set; }




        /// <summary>
        /// Gets or sets the level.
        /// </summary>
        /// <value>The level.</value>
        public int Level { get; set; }




        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        public string Message { get; set; }




        /// <summary>
        /// Gets or sets the millis.
        /// </summary>
        /// <value>The millis.</value>
        public int Millis { get; set; }




        /// <summary>
        /// Gets or sets the time.
        /// </summary>
        /// <value>The time.</value>
        public DateTime Time { get; set; }




        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public string Type { get; set; }

        #endregion
    }
}
