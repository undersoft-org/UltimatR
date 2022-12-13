
// <copyright file="DealEnums.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The Deal namespace.
/// </summary>
namespace System.Deal
{
    using System;

    #region Enums

    /// <summary>
    /// Enum DirectionType
    /// </summary>
    [Serializable]
    public enum DirectionType
    {
        /// <summary>
        /// The send
        /// </summary>
        Send,
        /// <summary>
        /// The receive
        /// </summary>
        Receive,
        /// <summary>
        /// The none
        /// </summary>
        None
    }
    /// <summary>
    /// Enum DealProtocol
    /// </summary>
    [Serializable]
    public enum DealProtocol
    {
        /// <summary>
        /// The none
        /// </summary>
        NONE,
        /// <summary>
        /// The dotp
        /// </summary>
        DOTP,
        /// <summary>
        /// The HTTP
        /// </summary>
        HTTP
    }
    /// <summary>
    /// Enum ProtocolMethod
    /// </summary>
    [Serializable]
    public enum ProtocolMethod
    {
        /// <summary>
        /// The none
        /// </summary>
        NONE,
        /// <summary>
        /// The deal
        /// </summary>
        DEAL,
        /// <summary>
        /// The synchronize
        /// </summary>
        SYNC,
        /// <summary>
        /// The get
        /// </summary>
        GET,
        /// <summary>
        /// The post
        /// </summary>
        POST,
        /// <summary>
        /// The options
        /// </summary>
        OPTIONS,
        /// <summary>
        /// The put
        /// </summary>
        PUT,
        /// <summary>
        /// The delete
        /// </summary>
        DELETE,
        /// <summary>
        /// The patch
        /// </summary>
        PATCH
    }
    /// <summary>
    /// Enum DealComplexity
    /// </summary>
    [Serializable]
    public enum DealComplexity
    {
        /// <summary>
        /// The guide
        /// </summary>
        Guide,
        /// <summary>
        /// The basic
        /// </summary>
        Basic,
        /// <summary>
        /// The standard
        /// </summary>
        Standard,
        /// <summary>
        /// The advanced
        /// </summary>
        Advanced
    }
    /// <summary>
    /// Enum MessagePart
    /// </summary>
    [Serializable]
    public enum MessagePart
    {
        /// <summary>
        /// The header
        /// </summary>
        Header,
        /// <summary>
        /// The message
        /// </summary>
        Message,
    }

    #endregion
}
