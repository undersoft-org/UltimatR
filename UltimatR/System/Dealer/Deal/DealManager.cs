
// <copyright file="DealManager.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The Deal namespace.
/// </summary>
namespace System.Deal
{
    using System;




    /// <summary>
    /// Class DealManager.
    /// Implements the <see cref="System.IDisposable" />
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public class DealManager : IDisposable
    {
        #region Fields

        /// <summary>
        /// The transfer context
        /// </summary>
        public ITransferContext transferContext;
        /// <summary>
        /// The deal context
        /// </summary>
        private DealContext dealContext;
        /// <summary>
        /// The site
        /// </summary>
        private ServiceSite site;
        /// <summary>
        /// The transfer
        /// </summary>
        private DealTransfer transfer;

        #endregion

        #region Constructors





        /// <summary>
        /// Initializes a new instance of the <see cref="DealManager" /> class.
        /// </summary>
        /// <param name="dealTransfer">The deal transfer.</param>
        public DealManager(DealTransfer dealTransfer)
        {
            transfer = dealTransfer;
            transferContext = dealTransfer.Context;
            dealContext = dealTransfer.MyHeader.Context;
            site = dealContext.IdentitySite;
        }

        #endregion

        #region Methods








        /// <summary>
        /// Assigns the specified content.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="direction">The direction.</param>
        /// <param name="messages">The messages.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool Assign(object content, DirectionType direction, out object[] messages)
        {
            messages = null;

            DealOperation operation = new DealOperation(content, direction, transfer);
            operation.Resolve(out messages);

            if (messages != null)
                return true;
            else
                return false;
        }




        /// <summary>
        /// Disposes this instance.
        /// </summary>
        public void Dispose()
        {
            if (transferContext != null)
                transferContext.Dispose();
        }

        #endregion
    }
}
