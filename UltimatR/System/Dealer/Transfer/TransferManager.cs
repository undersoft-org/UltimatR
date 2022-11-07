
// <copyright file="TransferManager.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The Deal namespace.
/// </summary>
namespace System.Deal
{
    using System;
    using System.Linq;




    /// <summary>
    /// Class TransferManager.
    /// </summary>
    public class TransferManager
    {
        #region Fields

        /// <summary>
        /// The context
        /// </summary>
        private DealContext context;
        /// <summary>
        /// The site
        /// </summary>
        private ServiceSite site;
        /// <summary>
        /// The transaction
        /// </summary>
        private DealTransfer transaction;
        /// <summary>
        /// The transfer context
        /// </summary>
        private ITransferContext transferContext;
        /// <summary>
        /// The treatment
        /// </summary>
        private DealManager treatment;

        #endregion

        #region Constructors





        /// <summary>
        /// Initializes a new instance of the <see cref="TransferManager" /> class.
        /// </summary>
        /// <param name="_transaction">The transaction.</param>
        public TransferManager(DealTransfer _transaction)
        {
            transaction = _transaction;
            transferContext = transaction.Context;
            context = transaction.MyHeader.Context;
            site = context.IdentitySite;
            treatment = new DealManager(_transaction);
        }

        #endregion

        #region Methods







        /// <summary>
        /// Headers the content.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="value">The value.</param>
        /// <param name="_direction">The direction.</param>
        public void HeaderContent(object content, object value, DirectionType _direction)
        {
            DirectionType direction = _direction;
            object _content = value;
            if (_content != null)
            {
                Type[] ifaces = _content.GetType().GetInterfaces();
                if (ifaces.Contains(typeof(ISerialFormatter)))
                {
                    transaction.MyHeader.Context.ContentType = _content.GetType();

                    if (direction == DirectionType.Send)
                        _content = ((ISerialFormatter)value).GetHeader();

                    object[] messages_ = null;
                    if (treatment.Assign(_content, direction, out messages_)                               
                    )
                    {
                        if (messages_.Length > 0)
                        {
                            context.ObjectsCount = messages_.Length;
                            for (int i = 0; i < context.ObjectsCount; i++)
                            {
                                ISerialFormatter message = ((ISerialFormatter[])messages_)[i];
                                ISerialFormatter head = (ISerialFormatter)((ISerialFormatter[])messages_)[i].GetHeader();
                                message.SerialCount = message.ItemsCount;
                                head.SerialCount = message.ItemsCount;
                            }

                            if (direction == DirectionType.Send)
                                transaction.MyMessage.Content = messages_;
                            else
                                transaction.MyMessage.Content = ((ISerialFormatter)_content).GetHeader();
                        }
                    }
                }
            }
            content = _content;
        }







        /// <summary>
        /// Messages the content.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="value">The value.</param>
        /// <param name="_direction">The direction.</param>
        public void MessageContent(ref object content, object value, DirectionType _direction)
        {
            DirectionType direction = _direction;
            object _content = value;
            if (_content != null)
            {
                if (direction == DirectionType.Receive)
                {
                    Type[] ifaces = _content.GetType().GetInterfaces();
                    if (ifaces.Contains(typeof(ISerialFormatter)))
                    {
                        object[] messages_ = ((ISerialFormatter)value).GetMessage();
                        if (messages_ != null)
                        {
                            int length = messages_.Length;
                            for (int i = 0; i < length; i++)
                            {
                                ISerialFormatter message = ((ISerialFormatter[])messages_)[i];
                                ISerialFormatter head = (ISerialFormatter)((ISerialFormatter[])messages_)[i].GetHeader();
                                message.SerialCount = head.SerialCount;
                                message.DeserialCount = head.DeserialCount;
                            }

                            _content = messages_;
                        }
                    }
                }
            }
            content = _content;
        }

        #endregion
    }
}
