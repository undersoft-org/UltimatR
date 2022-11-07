
// <copyright file="DealTransfer.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The Deal namespace.
/// </summary>
namespace System.Deal
{
    using System;




    /// <summary>
    /// Class DealTransfer.
    /// Implements the <see cref="System.IDisposable" />
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public class DealTransfer : IDisposable
    {
        #region Fields

        /// <summary>
        /// The context
        /// </summary>
        public ITransferContext Context;
        /// <summary>
        /// The header received
        /// </summary>
        public DealHeader HeaderReceived;
        /// <summary>
        /// The identity
        /// </summary>
        public MemberIdentity Identity;
        /// <summary>
        /// The manager
        /// </summary>
        public TransferManager Manager;
        /// <summary>
        /// The message received
        /// </summary>
        public DealMessage MessageReceived;
        /// <summary>
        /// My header
        /// </summary>
        public DealHeader MyHeader;
        /// <summary>
        /// The mymessage
        /// </summary>
        private DealMessage mymessage;

        #endregion

        #region Constructors




        /// <summary>
        /// Initializes a new instance of the <see cref="DealTransfer" /> class.
        /// </summary>
        public DealTransfer()
        {
            MyHeader = new DealHeader(this);
            Manager = new TransferManager(this);
            MyMessage = new DealMessage(this, DirectionType.Send, null);
        }






        /// <summary>
        /// Initializes a new instance of the <see cref="DealTransfer" /> class.
        /// </summary>
        /// <param name="identity">The identity.</param>
        /// <param name="message">The message.</param>
        /// <param name="context">The context.</param>
        public DealTransfer(MemberIdentity identity, object message = null, ITransferContext context = null)
        {
            Context = context;
            if (Context != null)
                MyHeader = new DealHeader(this, Context, identity);
            else
                MyHeader = new DealHeader(this, identity);

            Identity = identity;
            Manager = new TransferManager(this);
            MyMessage = new DealMessage(this, DirectionType.Send, message);
        }

        #endregion

        #region Properties




        /// <summary>
        /// Gets or sets my message.
        /// </summary>
        /// <value>My message.</value>
        public DealMessage MyMessage
        {
            get
            {
                return mymessage;
            }
            set
            {
                mymessage = value;
            }
        }

        #endregion

        #region Methods




        /// <summary>
        /// Disposes this instance.
        /// </summary>
        public void Dispose()
        {
            if (MyHeader != null)
                MyHeader.Dispose();
            if (mymessage != null)
                mymessage.Dispose();
            if (HeaderReceived != null)
                HeaderReceived.Dispose();
            if (MessageReceived != null)
                MessageReceived.Dispose();
            if (Context != null)
                Context.Dispose();
        }

        #endregion
    }
}
