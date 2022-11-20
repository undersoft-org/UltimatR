﻿using System;

namespace System.Deal
{
    public class DealManager : IDisposable
    {
        public ITransferContext transferContext;
        private DealContext dealContext;
        private ServiceSite site;
        private DealTransfer transfer;

        public DealManager(DealTransfer dealTransfer)
        {
            transfer = dealTransfer;
            transferContext = dealTransfer.Context;
            dealContext = dealTransfer.MyHeader.Context;
            site = dealContext.IdentitySite;
        }

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

        public void Dispose()
        {
            if (transferContext != null)
                transferContext.Dispose();
        }
    }
}
