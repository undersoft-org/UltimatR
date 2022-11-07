
// <copyright file="BoardBase.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The Series namespace.
/// </summary>
namespace System.Series
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Uniques;

    /// <summary>
    /// Class BoardBase.
    /// Implements the <see cref="System.Series.DeckBase{V}" />
    /// </summary>
    /// <typeparam name="V"></typeparam>
    /// <seealso cref="System.Series.DeckBase{V}" />
    public abstract class BoardBase<V> : DeckBase<V>
    {
        #region Constants

        /// <summary>
        /// The wait read timeout
        /// </summary>
        internal const int WAIT_READ_TIMEOUT = 5000;

        /// <summary>
        /// The wait rehash timeout
        /// </summary>
        internal const int WAIT_REHASH_TIMEOUT = 5000;

        /// <summary>
        /// The wait write timeout
        /// </summary>
        internal const int WAIT_WRITE_TIMEOUT = 5000;

        #endregion

        #region Fields

        /// <summary>
        /// The read access
        /// </summary>
        internal readonly ManualResetEventSlim readAccess = new ManualResetEventSlim(true, 128);
        /// <summary>
        /// The rehash access
        /// </summary>
        internal readonly ManualResetEventSlim rehashAccess = new ManualResetEventSlim(true, 128);
        /// <summary>
        /// The write access
        /// </summary>
        internal readonly ManualResetEventSlim writeAccess = new ManualResetEventSlim(true, 128);
        /// <summary>
        /// The write pass
        /// </summary>
        internal readonly SemaphoreSlim writePass = new SemaphoreSlim(1);
        /// <summary>
        /// The readers
        /// </summary>
        private int readers;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BoardBase{V}" /> class.
        /// </summary>
        protected BoardBase() : base()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="BoardBase{V}" /> class.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="capacity">The capacity.</param>
        /// <param name="bits">The bits.</param>
        protected BoardBase(IEnumerable<IUnique<V>> collection, int capacity = 17, HashBits bits = HashBits.bit64) : this(capacity, bits)
        {
            foreach (var c in collection)
                this.Add(c);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="BoardBase{V}" /> class.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="capacity">The capacity.</param>
        /// <param name="bits">The bits.</param>
        protected BoardBase(IEnumerable<V> collection, int capacity = 17, HashBits bits = HashBits.bit64) : this(capacity, bits)
        {
            foreach (var c in collection)
                this.Add(c);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="BoardBase{V}" /> class.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="capacity">The capacity.</param>
        /// <param name="bits">The bits.</param>
        protected BoardBase(IList<IUnique<V>> collection, int capacity = 17, HashBits bits = HashBits.bit64) : this(capacity > collection.Count ? capacity : collection.Count, bits)
        {
            foreach (var c in collection)
                this.Add(c);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="BoardBase{V}" /> class.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="capacity">The capacity.</param>
        /// <param name="bits">The bits.</param>
        protected BoardBase(IList<V> collection, int capacity = 17, HashBits bits = HashBits.bit64) : this(capacity > collection.Count ? capacity : collection.Count, bits)
        {
            foreach (var c in collection)
                this.Add(c);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="BoardBase{V}" /> class.
        /// </summary>
        /// <param name="capacity">The capacity.</param>
        /// <param name="bits">The bits.</param>
        protected BoardBase(int capacity = 17, HashBits bits = HashBits.bit64) : base(capacity, bits)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public override void Clear()
        {
            acquireWriter();
            acquireRehash();

            base.Clear();

            releaseRehash();
            releaseWriter();
        }

        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="index">The index.</param>
        public override void CopyTo(Array array, int index)
        {
            acquireReader();
            base.CopyTo(array, index);
            releaseReader();
        }

        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="index">The index.</param>
        public override void CopyTo(ICard<V>[] array, int index)
        {
            acquireReader();
            base.CopyTo(array, index);
            releaseReader();
        }

        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="index">The index.</param>
        public override void CopyTo(V[] array, int index)
        {
            acquireReader();
            base.CopyTo(array, index);
            releaseReader();
        }

        /// <summary>
        /// Gets the card.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        public override ICard<V> GetCard(int index)
        {
            if (index < count)
            {
                acquireReader();
                if (removed > 0)
                {
                    releaseReader();
                    acquireWriter();
                    Reindex();
                    releaseWriter();
                    acquireReader();
                }

                int i = -1;
                int id = index;
                var card = first.Next;
                for (; ; )
                {
                    if (++i == id)
                    {
                        releaseReader();
                        return card;
                    }
                    card = card.Next;
                }
            }
            return null;
        }

        /// <summary>
        /// Indexes the of.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>System.Int32.</returns>
        public override int IndexOf(ICard<V> item)
        {
            int id = 0;
            acquireReader();
            id = base.IndexOf(item);
            releaseReader();
            return id;
        }

        /// <summary>
        /// Indexes the of.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>System.Int32.</returns>
        public override int IndexOf(V item)
        {
            int id = 0;
            acquireReader();
            id = base.IndexOf(item);
            releaseReader();
            return id;
        }

        /// <summary>
        /// Inserts the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="item">The item.</param>
        public override void Insert(int index, ICard<V> item)
        {
            acquireWriter();
            base.Insert(index, item);
            releaseWriter();
        }

        /// <summary>
        /// Converts to array.
        /// </summary>
        /// <returns>V[].</returns>
        public override V[] ToArray()
        {
            acquireReader();
            V[] array = base.ToArray();
            releaseReader();
            return array;
        }

        /// <summary>
        /// Tries the dequeue.
        /// </summary>
        /// <param name="output">The output.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public override bool TryDequeue(out ICard<V> output)
        {
            acquireWriter();
            var temp = base.TryDequeue(out output);
            releaseWriter();
            return temp;
        }

        /// <summary>
        /// Tries the dequeue.
        /// </summary>
        /// <param name="output">The output.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public override bool TryDequeue(out V output)
        {
            acquireWriter();
            var temp = base.TryDequeue(out output);
            releaseWriter();
            return temp;
        }

        /// <summary>
        /// Tries the pick.
        /// </summary>
        /// <param name="skip">The skip.</param>
        /// <param name="output">The output.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public override bool TryPick(int skip, out V output)
        {
            acquireWriter();
            var temp = base.TryPick(skip, out output);
            releaseWriter();
            return temp;
        }

        /// <summary>
        /// Acquires the reader.
        /// </summary>
        /// <exception cref="System.TimeoutException">Wait write Timeout</exception>
        protected void acquireReader()
        {
            Interlocked.Increment(ref readers);
            rehashAccess.Reset();
            if (!readAccess.Wait(WAIT_READ_TIMEOUT))
                throw new TimeoutException("Wait write Timeout");
        }

        /// <summary>
        /// Acquires the rehash.
        /// </summary>
        /// <exception cref="System.TimeoutException">Wait write Timeout</exception>
        protected void acquireRehash()
        {
            if (!rehashAccess.Wait(WAIT_REHASH_TIMEOUT))
                throw new TimeoutException("Wait write Timeout");
            readAccess.Reset();
        }

        /// <summary>
        /// Acquires the writer.
        /// </summary>
        /// <exception cref="System.TimeoutException">Wait write Timeout</exception>
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

        /// <summary>
        /// Inners the add.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        protected override bool InnerAdd(ICard<V> value)
        {
            acquireWriter();
            var temp = base.InnerAdd(value);
            releaseWriter();
            return temp;
        }

        /// <summary>
        /// Inners the add.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        protected override bool InnerAdd(ulong key, V value)
        {
            acquireWriter();
            var temp = base.InnerAdd(key, value);
            releaseWriter();
            return temp;
        }

        /// <summary>
        /// Inners the add.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        protected override bool InnerAdd(V value)
        {
            acquireWriter();
            var temp = base.InnerAdd(value);
            releaseWriter();
            return temp;
        }

        /// <summary>
        /// Inners the get.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>V.</returns>
        protected override V InnerGet(ulong key)
        {
            acquireReader();
            var v = base.InnerGet(key);
            releaseReader();
            return v;
        }

        /// <summary>
        /// Inners the get card.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        protected override ICard<V> InnerGetCard(ulong key)
        {
            acquireReader();
            var card = base.InnerGetCard(key);
            releaseReader();
            return card;
        }

        /// <summary>
        /// Inners the put.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        protected override ICard<V> InnerPut(ICard<V> value)
        {
            acquireWriter();
            var temp = base.InnerPut(value);
            releaseWriter();
            return temp;
        }

        /// <summary>
        /// Inners the put.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        protected override ICard<V> InnerPut(ulong key, V value)
        {
            acquireWriter();
            var temp = base.InnerPut(key, value);
            releaseWriter();
            return temp;
        }

        /// <summary>
        /// Inners the put.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        protected override ICard<V> InnerPut(V value)
        {
            acquireWriter();
            var temp = base.InnerPut(value);
            releaseWriter();
            return temp;
        }

        /// <summary>
        /// Inners the remove.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>V.</returns>
        protected override V InnerRemove(ulong key)
        {
            acquireWriter();
            var temp = base.InnerRemove(key);
            releaseWriter();
            return temp;
        }

        /// <summary>
        /// Inners the try get.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="output">The output.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        protected override bool InnerTryGet(ulong key, out ICard<V> output)
        {
            acquireReader();
            var test = base.InnerTryGet(key, out output);
            releaseReader();
            return test;
        }

        /// <summary>
        /// Rehashes the specified newsize.
        /// </summary>
        /// <param name="newsize">The newsize.</param>
        protected override void Rehash(int newsize)
        {
            acquireRehash();
            base.Rehash(newsize);
            releaseRehash();
        }

        /// <summary>
        /// Reindexes this instance.
        /// </summary>
        protected override void Reindex()
        {
            acquireRehash();
            base.Reindex();
            releaseRehash();
        }

        /// <summary>
        /// Releases the reader.
        /// </summary>
        protected void releaseReader()
        {
            if (0 == Interlocked.Decrement(ref readers))
                rehashAccess.Set();
        }

        /// <summary>
        /// Releases the rehash.
        /// </summary>
        protected void releaseRehash()
        {
            readAccess.Set();
        }

        /// <summary>
        /// Releases the writer.
        /// </summary>
        protected void releaseWriter()
        {
            writePass.Release();
            writeAccess.Set();
        }

        #endregion
    }
}
