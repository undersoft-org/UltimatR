


using NLog;

/// <summary>
/// The Logs namespace.
/// </summary>
namespace System.Logs
{



    /// <summary>
    /// Interface ILogHandler
    /// </summary>
    public interface ILogHandler
    {
        #region Methods






        /// <summary>
        /// Cleans the specified older then.
        /// </summary>
        /// <param name="olderThen">The older then.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool Clean(DateTime olderThen);






        /// <summary>
        /// Writes the specified log.
        /// </summary>
        /// <param name="log">The log.</param>
        void Write(Starlog log);

        #endregion
    }

  
}
