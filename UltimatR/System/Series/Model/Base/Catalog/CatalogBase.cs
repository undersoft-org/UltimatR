
namespace System.Series
{
    using Collections.Generic;
    using Threading;
    using Uniques;

    public class CatalogBase<V> : AlbumBase<V>
    {
        #region Fields

        private const int WAIT_READ_TIMEOUT = 5000;
        private const int WAIT_REHASH_TIMEOUT = 5000;
        private const int WAIT_WRITE_TIMEOUT = 5000;
        private readonly ManualResetEventSlim readAccess = new ManualResetEventSlim(true, 128);
        private readonly ManualResetEventSlim rehashAccess = new ManualResetEventSlim(true, 128);
        private readonly ManualResetEventSlim writeAccess = new ManualResetEventSlim(true, 128);
        private readonly SemaphoreSlim writePass = new SemaphoreSlim(1);
        private int readers;

        #endregion

        #region Constructors

        public CatalogBase() : this(false, 17, HashBits.bit64)
        {
        }
        public CatalogBase(IEnumerable<IUnique<V>> collection, int capacity = 17, bool repeatable = false, HashBits bits = HashBits.bit64) : this(repeatable, capacity, bits)
        {
            foreach(var c in collection)
                this.Add(c);
        }
        public CatalogBase(IEnumerable<V> collection, int capacity = 17, bool repeatable = false, HashBits bits = HashBits.bit64) : this(repeatable, capacity, bits)
        {
            foreach(var c in collection)
                this.Add(c);
        }
        public CatalogBase(bool repeatable, int capacity = 17, HashBits bits = HashBits.bit64) : base(repeatable, capacity, bits)
        {
        }
        public CatalogBase(int capacity = 17, HashBits bits = HashBits.bit64) : base(capacity, bits)
        {
        }

        #endregion

        #region Methods

        public override ICard<V> EmptyCard()
        {
            return new Card<V>();
        }

        public override ICard<V>[] EmptyCardTable(int size)
        {
            return new Card<V>[size];
        }

        public override ICard<V>[] EmptyDeck(int size)
        {
            return new Card<V>[size];
        }

        public override ICard<V> NewCard(ICard<V> card)
        {
            return new Card<V>(card);
        }

        public override ICard<V> NewCard(object key, V value)
        {
            return new Card<V>(key, value);
        }

        public override ICard<V> NewCard(ulong key, V value)
        {
            return new Card<V>(key, value);
        }

        public override ICard<V> NewCard(V value)
        {
            return new Card<V>(value);
        }

        public override void Clear()
        {
            acquireWriter();
            acquireRehash();

            base.Clear();

            releaseRehash();
            releaseWriter();
        }

        public override void CopyTo(Array array, int index)
        {
            acquireReader();
            base.CopyTo(array, index);
            releaseReader();
        }

        public override void CopyTo(ICard<V>[] array, int index)
        {
            acquireReader();
            base.CopyTo(array, index);
            releaseReader();
        }

        public override void CopyTo(V[] array, int index)
        {
            acquireReader();
            base.CopyTo(array, index);
            releaseReader();
        }

        public override ICard<V> GetCard(int index)
        {
            if(index < count)
            {
                acquireReader();
                if(removed > 0)
                {
                    releaseReader();
                    acquireWriter();
                    Reindex();
                    releaseWriter();
                    acquireReader();
                }

                var temp = list[index];
                releaseReader();
                return temp;
            }
            throw new IndexOutOfRangeException("Index out of range");
        }

        protected override ICard<V> GetCard(ulong key, V item)
        {           
            acquireReader();
            var card = base.GetCard(key, item);
            releaseReader();
            return card;
        }

        public override int IndexOf(ICard<V> item)
        {
            int id = 0;
            acquireReader();
            id = base.IndexOf(item);
            releaseReader();
            return id;
        }

        protected override int IndexOf(ulong key, V item)
        {
            int id = 0;
            acquireReader();
            id = base.IndexOf(key, item);
            releaseReader();
            return id;
        }

        public override void Insert(int index, ICard<V> item)
        {
            acquireWriter();
            base.InnerInsert(index, item);
            releaseWriter();
        }

        public override V[] ToArray()
        {
            acquireReader();
            V[] array = base.ToArray();
            releaseReader();
            return array;
        }

        public override V Dequeue()
        {
            acquireWriter();
            var temp = base.Dequeue();
            releaseWriter();
            return temp;
        }

        public override bool TryDequeue(out ICard<V> output)
        {
            acquireWriter();
            var temp = base.TryDequeue(out output);
            releaseWriter();
            return temp;
        }

        public override bool TryDequeue(out V output)
        {
            acquireWriter();
            var temp = base.TryDequeue(out output);
            releaseWriter();
            return temp;
        }

        public override bool TryPick(int skip, out V output)
        {
            acquireWriter();
            var temp = base.TryPick(skip, out output);
            releaseWriter();
            return temp;
        }

        protected override bool InnerAdd(ICard<V> value)
        {
            acquireWriter();
            var temp = base.InnerAdd(value);
            releaseWriter();
            return temp;
        }

        protected override bool InnerAdd(ulong key, V value)
        {
            acquireWriter();
            var temp = base.InnerAdd(key, value);
            releaseWriter();
            return temp;
        }

        protected override bool InnerAdd(V value)
        {
            acquireWriter();
            var temp = base.InnerAdd(value);
            releaseWriter();
            return temp;
        }

        protected override V InnerGet(ulong key)
        {
            acquireReader();
            var v = base.InnerGet(key);
            releaseReader();
            return v;
        }

        protected override ICard<V> InnerGetCard(ulong key)
        {
            acquireReader();
            var card = base.InnerGetCard(key);
            releaseReader();
            return card;
        }

        protected override ICard<V> InnerPut(ICard<V> value)
        {
            acquireWriter();
            var temp = base.InnerPut(value);
            releaseWriter();
            return temp;
        }

        protected override ICard<V> InnerPut(ulong key, V value)
        {
            acquireWriter();
            var temp = base.InnerPut(key, value);
            releaseWriter();
            return temp;
        }

        protected override ICard<V> InnerPut(V value)
        {
            acquireWriter();
            var temp = base.InnerPut(value);
            releaseWriter();
            return temp;
        }

        protected override V InnerRemove(ulong key)
        {
            acquireWriter();
            var temp = base.InnerRemove(key);
            releaseWriter();
            return temp;
        }

        protected override ICard<V> InnerSet(ICard<V> value)
        {
            acquireWriter();
            var temp = base.InnerSet(value);
            releaseWriter();
            return temp;
        }

        protected override ICard<V> InnerSet(ulong key, V value)
        {
            acquireWriter();
            var temp = base.InnerSet(key, value);
            releaseWriter();
            return temp;
        }

        protected override ICard<V> InnerSet(V value)
        {
            acquireWriter();
            var temp = base.InnerSet(value);
            releaseWriter();
            return temp;
        }

        protected override bool InnerTryGet(ulong key, out ICard<V> output)
        {
            acquireReader();
            var test = base.InnerTryGet(key, out output);
            releaseReader();
            return test;
        }

        protected override void Rehash(int newsize)
        {
            acquireRehash();
            base.Rehash(newsize);
            releaseRehash();
        }

        protected override void Reindex()
        {
            acquireRehash();
            base.Reindex();
            releaseRehash();
        }

        protected void acquireReader()
        {
            Interlocked.Increment(ref readers);
            rehashAccess.Reset();    
            if (!readAccess.Wait(WAIT_READ_TIMEOUT))
                throw new TimeoutException("Wait read timeout");
        }

        protected void acquireRehash()
        {
            if (!rehashAccess.Wait(WAIT_REHASH_TIMEOUT))
                throw new TimeoutException("Wait write Timeout");
            readAccess.Reset();
        }

        protected void acquireWriter()
        {
            do
            {
                if (!writeAccess.Wait(WAIT_WRITE_TIMEOUT))
                    throw new TimeoutException("Wait write Timeout");
                writeAccess.Reset();
            }
            while (!writePass.Wait(0));
        }

        protected void releaseReader()
        {
            if(0 == Interlocked.Decrement(ref readers))
                rehashAccess.Set();
        }

        protected void releaseRehash()
        {
            readAccess.Set();
        }

        protected void releaseWriter()
        {
            writePass.Release();
            writeAccess.Set();
        }

        #endregion
    }
}
