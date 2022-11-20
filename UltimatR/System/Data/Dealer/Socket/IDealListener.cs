
// <copyright file="IDealListener.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The Deal namespace.
/// </summary>
namespace System.Deal
{
    using System;
    using System.Instant;




    /// <summary>
    /// Interface IDealListener
    /// Implements the <see cref="System.IDisposable" />
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public interface IDealListener : IDisposable
    {
        #region Properties





        /// <summary>
        /// Gets or sets the header received.
        /// </summary>
        /// <value>The header received.</value>
        IDeputy HeaderReceived { get; set; }




        /// <summary>
        /// Gets or sets the header sent.
        /// </summary>
        /// <value>The header sent.</value>
        IDeputy HeaderSent { get; set; }




        /// <summary>
        /// Gets or sets the identity.
        /// </summary>
        /// <value>The identity.</value>
        MemberIdentity Identity { get; set; }




        /// <summary>
        /// Gets or sets the message received.
        /// </summary>
        /// <value>The message received.</value>
        IDeputy MessageReceived { get; set; }




        /// <summary>
        /// Gets or sets the message sent.
        /// </summary>
        /// <value>The message sent.</value>
        IDeputy MessageSent { get; set; }




        /// <summary>
        /// Gets or sets the send echo.
        /// </summary>
        /// <value>The send echo.</value>
        IDeputy SendEcho { get; set; }

        #endregion

        #region Methods




        /// <summary>
        /// Clears the clients.
        /// </summary>
        void ClearClients();




        /// <summary>
        /// Clears the resources.
        /// </summary>
        void ClearResources();





        /// <summary>
        /// Closes the client.
        /// </summary>
        /// <param name="id">The identifier.</param>
        void CloseClient(int id);




        /// <summary>
        /// Closes the listener.
        /// </summary>
        void CloseListener();





        /// <summary>
        /// Echoes the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        void Echo(string message);





        /// <summary>
        /// Headers the received callback.
        /// </summary>
        /// <param name="result">The result.</param>
        void HeaderReceivedCallback(IAsyncResult result);





        /// <summary>
        /// Headers the sent callback.
        /// </summary>
        /// <param name="result">The result.</param>
        void HeaderSentCallback(IAsyncResult result);






        /// <summary>
        /// Determines whether the specified identifier is connected.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if the specified identifier is connected; otherwise, <c>false</c>.</returns>
        bool IsConnected(int id);





        /// <summary>
        /// Messages the received callback.
        /// </summary>
        /// <param name="result">The result.</param>
        void MessageReceivedCallback(IAsyncResult result);





        /// <summary>
        /// Messages the sent callback.
        /// </summary>
        /// <param name="result">The result.</param>
        void MessageSentCallback(IAsyncResult result);





        /// <summary>
        /// Called when [connect callback].
        /// </summary>
        /// <param name="result">The result.</param>
        void OnConnectCallback(IAsyncResult result);






        /// <summary>
        /// Receives the specified message part.
        /// </summary>
        /// <param name="messagePart">The message part.</param>
        /// <param name="id">The identifier.</param>
        void Receive(MessagePart messagePart, int id);






        /// <summary>
        /// Sends the specified message part.
        /// </summary>
        /// <param name="messagePart">The message part.</param>
        /// <param name="id">The identifier.</param>
        void Send(MessagePart messagePart, int id);




        /// <summary>
        /// Starts the listening.
        /// </summary>
        void StartListening();

        #endregion
    }
}
