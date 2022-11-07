
// <copyright file="DealServer.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The Deal namespace.
/// </summary>
namespace System.Deal
{
    using System;
    using System.Instant;
    using System.Threading;




    /// <summary>
    /// Class DealServer.
    /// </summary>
    public class DealServer
    {
        #region Fields

        /// <summary>
        /// The security
        /// </summary>
        public static IMemberSecurity Security;
        /// <summary>
        /// The server
        /// </summary>
        private IDealListener server;

        #endregion

        #region Methods




        /// <summary>
        /// Clears the clients.
        /// </summary>
        public void ClearClients()
        {
            WriteEcho("Client registry cleaned");
            if (server != null)
                server.ClearClients();
        }




        /// <summary>
        /// Clears the resources.
        /// </summary>
        public void ClearResources()
        {
            WriteEcho("Resource buffer cleaned");
            if (server != null)
                server.ClearResources();
        }




        /// <summary>
        /// Closes this instance.
        /// </summary>
        public void Close()
        {
            if (server != null)
            {
                WriteEcho("Server instance shutdown ");
                server.CloseListener();
                server = null;
            }
            else
            {
                WriteEcho("Server instance doesn't exist ");
            }
        }






        /// <summary>
        /// Headers the received.
        /// </summary>
        /// <param name="inetdealcontext">The inetdealcontext.</param>
        /// <returns>System.Object.</returns>
        public object HeaderReceived(object inetdealcontext)
        {
            string clientEcho = ((ITransferContext)inetdealcontext).Transfer.HeaderReceived.Context.Echo;
            WriteEcho(string.Format("Client header received"));
            if (clientEcho != null && clientEcho != "")
                WriteEcho(string.Format("Client echo: {0}", clientEcho));

            DealContext trctx = ((ITransferContext)inetdealcontext).Transfer.MyHeader.Context;
            if (trctx.Echo == null || trctx.Echo == "")
                trctx.Echo = "Server say Hello";
            if (!((ITransferContext)inetdealcontext).Synchronic)
                server.Send(MessagePart.Header, ((ITransferContext)inetdealcontext).Id);
            else
                server.Receive(MessagePart.Message, ((ITransferContext)inetdealcontext).Id);

            return ((ITransferContext)inetdealcontext);
        }






        /// <summary>
        /// Headers the sent.
        /// </summary>
        /// <param name="inetdealcontext">The inetdealcontext.</param>
        /// <returns>System.Object.</returns>
        public object HeaderSent(object inetdealcontext)
        {

            WriteEcho("Server header sent");

            ITransferContext context = (ITransferContext)inetdealcontext;
            if (context.Close)
            {
                context.Transfer.Dispose();
                server.CloseClient(context.Id);
            }
            else
            {
                if (!context.Synchronic)
                {
                    if (context.ReceiveMessage)
                        server.Receive(MessagePart.Message, context.Id);
                }
                if (context.SendMessage)
                    server.Send(MessagePart.Message, context.Id);
            }
            return context;
        }





        /// <summary>
        /// Determines whether this instance is active.
        /// </summary>
        /// <returns><c>true</c> if this instance is active; otherwise, <c>false</c>.</returns>
        public bool IsActive()
        {

            if (server != null)
            {
                WriteEcho("Server Instance Is Active");
                return true;
            }
            else
            {
                WriteEcho("Server Instance Doesn't Exist");
                return false;
            }
        }






        /// <summary>
        /// Messages the received.
        /// </summary>
        /// <param name="inetdealcontext">The inetdealcontext.</param>
        /// <returns>System.Object.</returns>
        public object MessageReceived(object inetdealcontext)
        {
            WriteEcho(string.Format("Client message received"));
            if (((ITransferContext)inetdealcontext).Synchronic)
                server.Send(MessagePart.Header, ((ITransferContext)inetdealcontext).Id);
            return (ITransferContext)inetdealcontext;
        }






        /// <summary>
        /// Messages the sent.
        /// </summary>
        /// <param name="inetdealcontext">The inetdealcontext.</param>
        /// <returns>System.Object.</returns>
        public object MessageSent(object inetdealcontext)
        {
            WriteEcho("Server message sent");
            ITransferContext result = (ITransferContext)inetdealcontext;
            if (result.Close)
            {
                result.Transfer.Dispose();
                server.CloseClient(result.Id);
            }
            return result;
        }







        /// <summary>
        /// Starts the specified server identity.
        /// </summary>
        /// <param name="ServerIdentity">The server identity.</param>
        /// <param name="security">The security.</param>
        /// <param name="OnEchoEvent">The on echo event.</param>
        public void Start(MemberIdentity ServerIdentity, IMemberSecurity security = null, IDeputy OnEchoEvent = null)
        {
            server = DealListener.Instance;
            server.Identity = ServerIdentity;
            Security = security;

            new Thread(new ThreadStart(server.StartListening)).Start();

            server.HeaderSent = new DealEvent("HeaderSent", this);
            server.MessageSent = new DealEvent("MessageSent", this);
            server.HeaderReceived = new DealEvent("HeaderReceived", this);
            server.MessageReceived = new DealEvent("MessageReceived", this);
            server.SendEcho = OnEchoEvent;

            WriteEcho("Dealer instance started");
        }





        /// <summary>
        /// Writes the echo.
        /// </summary>
        /// <param name="message">The message.</param>
        public void WriteEcho(string message)
        {
            if (server != null)
                server.Echo(message);
        }

        #endregion
    }
}
