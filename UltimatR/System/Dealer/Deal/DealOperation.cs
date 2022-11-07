
// <copyright file="DealOperation.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The Deal namespace.
/// </summary>
namespace System.Deal
{
    #region Enums

    /// <summary>
    /// Enum StateFlag
    /// </summary>
    public enum StateFlag : ushort
    {
        /// <summary>
        /// The synced
        /// </summary>
        Synced = 1,
        /// <summary>
        /// The edited
        /// </summary>
        Edited = 2,
        /// <summary>
        /// The added
        /// </summary>
        Added = 4,
        /// <summary>
        /// The quered
        /// </summary>
        Quered = 8,
        /// <summary>
        /// The saved
        /// </summary>
        Saved = 16,
        /// <summary>
        /// The canceled
        /// </summary>
        Canceled = 32
    }

    #endregion

    /// <summary>
    /// Class DealOperation.
    /// Implements the <see cref="System.IDisposable" />
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public class DealOperation : IDisposable
    {
        #region Fields

        /// <summary>
        /// The transfer context
        /// </summary>
        [NonSerialized]
        public ITransferContext transferContext;
        /// <summary>
        /// The content
        /// </summary>
        private ISerialFormatter content;
        /// <summary>
        /// The deal context
        /// </summary>
        private DealContext dealContext;
        /// <summary>
        /// The direction
        /// </summary>
        private DirectionType direction;
        /// <summary>
        /// The site
        /// </summary>
        private ServiceSite site;
        /// <summary>
        /// The state
        /// </summary>
        private ushort state;

        #endregion

        #region Constructors





        /// <summary>
        /// Initializes a new instance of the <see cref="DealOperation" /> class.
        /// </summary>
        /// <param name="dealContent">Content of the deal.</param>
        public DealOperation(object dealContent)
        {
            site = ServiceSite.Server;
            direction = DirectionType.None;
            state = ((ISerialNumber)dealContent).FlagsBlock;
            content = (ISerialFormatter)dealContent;
        }






        /// <summary>
        /// Initializes a new instance of the <see cref="DealOperation" /> class.
        /// </summary>
        /// <param name="dealContent">Content of the deal.</param>
        /// <param name="directionType">Type of the direction.</param>
        /// <param name="transfer">The transfer.</param>
        public DealOperation(object dealContent, DirectionType directionType, DealTransfer transfer) : this(dealContent)
        {
            direction = directionType;
            transferContext = transfer.Context;
            dealContext = transfer.MyHeader.Context;
        }

        #endregion

        #region Methods




        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
        }





        /// <summary>
        /// Resolves the specified messages.
        /// </summary>
        /// <param name="messages">The messages.</param>
        public void Resolve(out object[] messages)
        {
            messages = null;
            switch (site)
            {
                case ServiceSite.Server:
                    switch (direction)
                    {
                        case DirectionType.Receive:
                            
                            break;
                        case DirectionType.Send:
                            switch (state & (int)StateFlag.Synced)
                            {
                                case 0:
                                    SrvSendSync(out messages);
                                    break;
                            }
                            break;
                        case DirectionType.None:
                            switch (state & (int)StateFlag.Synced)
                            {
                                case 0:
                                    SrvSendSync(out messages);
                                    break;
                            }
                            break;
                    }
                    break;
                case ServiceSite.Client:
                    switch (direction)
                    {
                        case DirectionType.Receive:
                            
                            break;
                        case DirectionType.Send:
                            switch (state & (int)StateFlag.Synced)
                            {
                                case 0:
                                    CltSendSync(out messages);
                                    break;
                            }
                            break;
                    }
                    break;
            }
        }





        /// <summary>
        /// CLTs the send synchronize.
        /// </summary>
        /// <param name="messages">The messages.</param>
        private void CltSendSync(out object[] messages)
        {
            if (direction != DirectionType.None)
                if (((state & (int)StateFlag.Edited) > 0) ||
                    ((state & (int)StateFlag.Saved) > 0) ||
                    ((state & (int)StateFlag.Quered) > 0) ||
                    ((state & (int)StateFlag.Canceled) > 0))
                {
                    transferContext.Synchronic = true;
                    dealContext.Synchronic = true;
                }

            messages = content.GetMessage();
        }





        /// <summary>
        /// SRVs the send cancel.
        /// </summary>
        /// <param name="messages">The messages.</param>
        private void SrvSendCancel(out object[] messages)
        {
            messages = content.GetMessage();
        }





        /// <summary>
        /// SRVs the send edit.
        /// </summary>
        /// <param name="messages">The messages.</param>
        private void SrvSendEdit(out object[] messages)
        {
            messages = content.GetMessage();
        }





        /// <summary>
        /// SRVs the send query.
        /// </summary>
        /// <param name="messages">The messages.</param>
        private void SrvSendQuery(out object[] messages)
        {
            messages = content.GetMessage();
        }





        /// <summary>
        /// SRVs the send save.
        /// </summary>
        /// <param name="messages">The messages.</param>
        private void SrvSendSave(out object[] messages)
        {
            messages = content.GetMessage();
        }





        /// <summary>
        /// SRVs the send synchronize.
        /// </summary>
        /// <param name="messages">The messages.</param>
        private void SrvSendSync(out object[] messages)
        {
            if (direction != DirectionType.None)
                if (((state & (int)StateFlag.Edited) > 0) ||
                    ((state & (int)StateFlag.Saved) > 0) ||
                    ((state & (int)StateFlag.Quered) > 0) ||
                    ((state & (int)StateFlag.Canceled) > 0))
                {
                    transferContext.Synchronic = true;
                    dealContext.Synchronic = true;
                }

            messages = null;
            switch (state & (int)StateFlag.Edited)
            {
                case 2:
                    SrvSendEdit(out messages);
                    break;
            }
            switch (state & (int)StateFlag.Canceled)
            {
                case 32:
                    SrvSendCancel(out messages);
                    break;
            }
            switch ((int)state & (int)StateFlag.Saved)
            {
                case 16:
                    SrvSendSave(out messages);
                    break;
            }
            switch (state & (int)StateFlag.Quered)
            {
                case 8:
                    SrvSendQuery(out messages);
                    break;
            }
            if (messages == null)
            {
                messages = content.GetMessage();
            }
        }

        #endregion
    }
}
