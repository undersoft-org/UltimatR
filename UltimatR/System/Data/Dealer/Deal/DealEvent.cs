
// <copyright file="DealEvent.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The Deal namespace.
/// </summary>
namespace System.Deal
{
    using System.Instant;




    /// <summary>
    /// Class DealEvent.
    /// Implements the <see cref="System.Deputy" />
    /// </summary>
    /// <seealso cref="System.Deputy" />
    [Serializable]
    public class DealEvent : Deputy
    {
        #region Constructors







        /// <summary>
        /// Initializes a new instance of the <see cref="DealEvent" /> class.
        /// </summary>
        /// <param name="MethodName">Name of the method.</param>
        /// <param name="TargetClassObject">The target class object.</param>
        /// <param name="parameters">The parameters.</param>
        public DealEvent(string MethodName, object TargetClassObject, params object[] parameters) : base(TargetClassObject, MethodName)
        {
            base.ParameterValues = parameters;
        }






        /// <summary>
        /// Initializes a new instance of the <see cref="DealEvent" /> class.
        /// </summary>
        /// <param name="MethodName">Name of the method.</param>
        /// <param name="TargetClassName">Name of the target class.</param>
        /// <param name="parameters">The parameters.</param>
        public DealEvent(string MethodName, string TargetClassName, params object[] parameters) : base(TargetClassName, MethodName)
        {
            base.ParameterValues = parameters;
        }

        #endregion
    }
}
