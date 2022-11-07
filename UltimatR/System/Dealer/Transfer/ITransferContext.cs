
// <copyright file="ITransferContext.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The Deal namespace.
/// </summary>
namespace System.Deal
{
    using System.Collections;
    using System.IO;
    using System.Net.Sockets;
    using System.Series;
    using System.Text;
    using System.Threading;




    /// <summary>
    /// Interface ITransferContext
    /// Implements the <see cref="System.ISerialBuffer" />
    /// </summary>
    /// <seealso cref="System.ISerialBuffer" />
    public interface ITransferContext : ISerialBuffer
    {
        #region Properties




        /// <summary>
        /// Gets or sets the batches received notice.
        /// </summary>
        /// <value>The batches received notice.</value>
        ManualResetEvent BatchesReceivedNotice { get; set; }




        /// <summary>
        /// Gets the size of the buffer.
        /// </summary>
        /// <value>The size of the buffer.</value>
        int BufferSize { get; }




        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ITransferContext" /> is close.
        /// </summary>
        /// <value><c>true</c> if close; otherwise, <c>false</c>.</value>
        bool Close { get; set; }




        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ITransferContext" /> is denied.
        /// </summary>
        /// <value><c>true</c> if denied; otherwise, <c>false</c>.</value>
        bool Denied { get; set; }




        /// <summary>
        /// Gets the echo.
        /// </summary>
        /// <value>The echo.</value>
        string Echo { get; }




        /// <summary>
        /// Gets the header buffer.
        /// </summary>
        /// <value>The header buffer.</value>
        byte[] HeaderBuffer { get; }




        /// <summary>
        /// Gets or sets the header received notice.
        /// </summary>
        /// <value>The header received notice.</value>
        ManualResetEvent HeaderReceivedNotice { get; set; }




        /// <summary>
        /// Gets or sets the header sent notice.
        /// </summary>
        /// <value>The header sent notice.</value>
        ManualResetEvent HeaderSentNotice { get; set; }




        /// <summary>
        /// Gets or sets the HTTP headers.
        /// </summary>
        /// <value>The HTTP headers.</value>
        Hashtable HttpHeaders { get; set; }




        /// <summary>
        /// Gets or sets the HTTP options.
        /// </summary>
        /// <value>The HTTP options.</value>
        Hashtable HttpOptions { get; set; }




        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        int Id { get; set; }




        /// <summary>
        /// Gets or sets the listener.
        /// </summary>
        /// <value>The listener.</value>
        Socket Listener { get; set; }




        /// <summary>
        /// Gets the message buffer.
        /// </summary>
        /// <value>The message buffer.</value>
        byte[] MessageBuffer { get; }




        /// <summary>
        /// Gets or sets the message received notice.
        /// </summary>
        /// <value>The message received notice.</value>
        ManualResetEvent MessageReceivedNotice { get; set; }




        /// <summary>
        /// Gets or sets the message sent notice.
        /// </summary>
        /// <value>The message sent notice.</value>
        ManualResetEvent MessageSentNotice { get; set; }




        /// <summary>
        /// Gets or sets the method.
        /// </summary>
        /// <value>The method.</value>
        ProtocolMethod Method { get; set; }




        /// <summary>
        /// Gets or sets the object position.
        /// </summary>
        /// <value>The object position.</value>
        int ObjectPosition { get; set; }




        /// <summary>
        /// Gets or sets the objects left.
        /// </summary>
        /// <value>The objects left.</value>
        int ObjectsLeft { get; set; }




        /// <summary>
        /// Gets or sets the protocol.
        /// </summary>
        /// <value>The protocol.</value>
        DealProtocol Protocol { get; set; }




        /// <summary>
        /// Gets or sets a value indicating whether [receive message].
        /// </summary>
        /// <value><c>true</c> if [receive message]; otherwise, <c>false</c>.</value>
        bool ReceiveMessage { get; set; }




        /// <summary>
        /// Gets or sets the request builder.
        /// </summary>
        /// <value>The request builder.</value>
        StringBuilder RequestBuilder { get; set; }




        /// <summary>
        /// Gets or sets the resources.
        /// </summary>
        /// <value>The resources.</value>
        IDeck<byte[]> Resources { get; set; }




        /// <summary>
        /// Gets or sets the response builder.
        /// </summary>
        /// <value>The response builder.</value>
        StringBuilder ResponseBuilder { get; set; }




        /// <summary>
        /// Gets or sets a value indicating whether [send message].
        /// </summary>
        /// <value><c>true</c> if [send message]; otherwise, <c>false</c>.</value>
        bool SendMessage { get; set; }




        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ITransferContext" /> is synchronic.
        /// </summary>
        /// <value><c>true</c> if synchronic; otherwise, <c>false</c>.</value>
        bool Synchronic { get; set; }




        /// <summary>
        /// Gets or sets the transfer.
        /// </summary>
        /// <value>The transfer.</value>
        DealTransfer Transfer { get; set; }

        #endregion

        #region Methods





        /// <summary>
        /// Appends the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        void Append(string text);




        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        void Dispose();




        /// <summary>
        /// Handles the denied request.
        /// </summary>
        void HandleDeniedRequest();





        /// <summary>
        /// Handles the get request.
        /// </summary>
        /// <param name="content_type">Type of the content.</param>
        void HandleGetRequest(string content_type = "text/html");





        /// <summary>
        /// Handles the options request.
        /// </summary>
        /// <param name="content_type">Type of the content.</param>
        void HandleOptionsRequest(string content_type = "text/html");





        /// <summary>
        /// Handles the post request.
        /// </summary>
        /// <param name="content_type">Type of the content.</param>
        void HandlePostRequest(string content_type = "text/html");





        /// <summary>
        /// Identifies the protocol.
        /// </summary>
        /// <returns>DealProtocol.</returns>
        DealProtocol IdentifyProtocol();






        /// <summary>
        /// Incomings the header.
        /// </summary>
        /// <param name="received">The received.</param>
        /// <returns>MarkupType.</returns>
        MarkupType IncomingHeader(int received);






        /// <summary>
        /// Incomings the message.
        /// </summary>
        /// <param name="received">The received.</param>
        /// <returns>MarkupType.</returns>
        MarkupType IncomingMessage(int received);




        /// <summary>
        /// Resets this instance.
        /// </summary>
        void Reset();

        #endregion
    }
}
