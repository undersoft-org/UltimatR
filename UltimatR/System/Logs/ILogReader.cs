
// <copyright file="ILogReader.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The System namespace.
/// </summary>
namespace System
{



    /// <summary>
    /// Interface ILogReader
    /// </summary>
    public interface ILogReader
    {
        #region Methods





        /// <summary>
        /// Clears the specified older then.
        /// </summary>
        /// <param name="olderThen">The older then.</param>
        void Clear(DateTime olderThen);






        /// <summary>
        /// Reads the specified after date.
        /// </summary>
        /// <param name="afterDate">The after date.</param>
        /// <returns>LogMessage[].</returns>
        LogMessage[] Read(DateTime afterDate);

        #endregion
    }
}
