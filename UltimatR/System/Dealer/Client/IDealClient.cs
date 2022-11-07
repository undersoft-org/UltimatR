/// <summary>
/// The Deal namespace.
/// </summary>
namespace System.Deal
{
    using System;
    using System.Instant;




    /// <summary>
    /// Interface IDealClient
    /// Implements the <see cref="System.IDisposable" />
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public interface IDealClient : IDisposable
    {
        #region Properties




        /// <summary>
        /// Gets or sets the connected.
        /// </summary>
        /// <value>The connected.</value>
        IDeputy Connected { get; set; }




        /// <summary>
        /// Gets or sets the context.
        /// </summary>
        /// <value>The context.</value>
        ITransferContext Context { get; set; }




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
        /// Gets or sets the message received.
        /// </summary>
        /// <value>The message received.</value>
        IDeputy MessageReceived { get; set; }




        /// <summary>
        /// Gets or sets the message sent.
        /// </summary>
        /// <value>The message sent.</value>
        IDeputy MessageSent { get; set; }

        #endregion

        #region Methods




        /// <summary>
        /// Connects this instance.
        /// </summary>
        void Connect();





        /// <summary>
        /// Determines whether this instance is connected.
        /// </summary>
        /// <returns><c>true</c> if this instance is connected; otherwise, <c>false</c>.</returns>
        bool IsConnected();





        /// <summary>
        /// Receives the specified message part.
        /// </summary>
        /// <param name="messagePart">The message part.</param>
        void Receive(MessagePart messagePart);





        /// <summary>
        /// Sends the specified message part.
        /// </summary>
        /// <param name="messagePart">The message part.</param>
        void Send(MessagePart messagePart);

        #endregion
    }
}
