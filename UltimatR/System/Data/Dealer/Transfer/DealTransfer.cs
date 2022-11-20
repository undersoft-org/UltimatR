using System;

namespace System.Deal
{
    public class DealTransfer : IDisposable
    {
        public ITransferContext Context;
        public DealHeader HeaderReceived;
        public MemberIdentity Identity;
        public TransferManager Manager;
        public DealMessage MessageReceived;
        public DealHeader MyHeader;
        private DealMessage mymessage;

        public DealTransfer()
        {
            MyHeader = new DealHeader(this);
            Manager = new TransferManager(this);
            MyMessage = new DealMessage(this, DirectionType.Send, null);
        }

        public DealTransfer(
            MemberIdentity identity,
            object message = null,
            ITransferContext context = null
        )
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

        public DealMessage MyMessage
        {
            get { return mymessage; }
            set { mymessage = value; }
        }

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
    }
}
