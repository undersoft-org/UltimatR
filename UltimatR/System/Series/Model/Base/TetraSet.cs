
// <copyright file="TetraSet.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The Basedeck namespace.
/// </summary>
namespace System.Series.Basedeck
{
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Uniques;

    /// <summary>
    /// Class TetraSet.
    /// Implements the <see cref="System.Uniques.Uniqueness" />
    /// Implements the <see cref="System.Collections.Generic.ICollection{V}" />
    /// Implements the <see cref="System.Collections.Generic.IList{V}" />
    /// Implements the <see cref="System.Series.IDeck{V}" />
    /// Implements the <see cref="System.Collections.Generic.ICollection{System.Series.ICard{V}}" />
    /// Implements the <see cref="System.Collections.Generic.IList{System.Series.ICard{V}}" />
    /// Implements the <see cref="System.Collections.Concurrent.IProducerConsumerCollection{V}" />
    /// Implements the <see cref="System.IDisposable" />
    /// Implements the <see cref="System.Collections.Generic.ICollection{System.IUnique{V}}" />
    /// </summary>
    /// <typeparam name="V"></typeparam>
    /// <seealso cref="System.Uniques.Uniqueness" />
    /// <seealso cref="System.Collections.Generic.ICollection{V}" />
    /// <seealso cref="System.Collections.Generic.IList{V}" />
    /// <seealso cref="System.Series.IDeck{V}" />
    /// <seealso cref="System.Collections.Generic.ICollection{System.Series.ICard{V}}" />
    /// <seealso cref="System.Collections.Generic.IList{System.Series.ICard{V}}" />
    /// <seealso cref="System.Collections.Concurrent.IProducerConsumerCollection{V}" />
    /// <seealso cref="System.IDisposable" />
    /// <seealso cref="System.Collections.Generic.ICollection{System.IUnique{V}}" />
    public abstract class TetraSet<V> : Uniqueness, ICollection<V>, IList<V>, IDeck<V>, ICollection<ICard<V>>, IList<ICard<V>>,
                                                     IProducerConsumerCollection<V>, IDisposable, ICollection<IUnique<V>>
    {
        /// <summary>
        /// The conflicts percent limit
        /// </summary>
        static protected readonly float CONFLICTS_PERCENT_LIMIT = 0.25f;
        /// <summary>
        /// The removed percent limit
        /// </summary>
        static protected readonly float REMOVED_PERCENT_LIMIT = 0.15f;

        /// <summary>
        /// The serialcode
        /// </summary>
        protected Usid serialcode;
        /// <summary>
        /// The first
        /// </summary>
        protected ICard<V> first, last;
        /// <summary>
        /// The table
        /// </summary>
        protected TetraTable<V> table;
        /// <summary>
        /// The tsize
        /// </summary>
        protected TetraSize tsize;
        /// <summary>
        /// The tcount
        /// </summary>
        protected TetraCount tcount;
        /// <summary>
        /// The count
        /// </summary>
        protected int count, conflicts, removed, size, minSize;

        /// <summary>
        /// Counts the increment.
        /// </summary>
        /// <param name="tid">The tid.</param>
        protected void countIncrement(uint tid)
        {
            count++;
            if ((tcount.Increment(tid) + 3) > size)
                Rehash(tsize.NextSize(tid), tid);
        }
        /// <summary>
        /// Conflicts the increment.
        /// </summary>
        /// <param name="tid">The tid.</param>
        protected void conflictIncrement(uint tid)
        {
            countIncrement(tid);
            if (++conflicts > (size * CONFLICTS_PERCENT_LIMIT))
                Rehash(tsize.NextSize(tid), tid);
        }
        /// <summary>
        /// Removeds the increment.
        /// </summary>
        /// <param name="tid">The tid.</param>
        protected void removedIncrement(uint tid)
        {
            int _tsize = tsize[tid];
            --count;
            tcount.Decrement(tid);
            if (++removed > (_tsize * REMOVED_PERCENT_LIMIT))
            {
                if (_tsize < _tsize / 2)
                    Rehash(tsize.PreviousSize(tid), tid);
                else
                    Rehash(_tsize, tid);
            }
        }
        /// <summary>
        /// Removeds the decrement.
        /// </summary>
        protected void removedDecrement()
        {
            ++count;
            --removed;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TetraSet{V}" /> class.
        /// </summary>
        /// <param name="capacity">The capacity.</param>
        /// <param name="bits">The bits.</param>
        public TetraSet(int capacity = 16, HashBits bits = HashBits.bit64) : base(bits)
        {
            size = capacity;
            minSize = capacity;
            tsize = new TetraSize(capacity);
            tcount = new TetraCount();
            table = new TetraTable<V>(this, capacity);
            first = EmptyCard();
            last = first;
            ValueEquals = getValueComparer();
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="TetraSet{V}" /> class.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="capacity">The capacity.</param>
        /// <param name="bits">The bits.</param>
        public TetraSet(IList<ICard<V>> collection, int capacity = 16, HashBits bits = HashBits.bit64) : this(capacity > collection.Count ? capacity : collection.Count, bits)
        {
            this.Add(collection);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="TetraSet{V}" /> class.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="capacity">The capacity.</param>
        /// <param name="bits">The bits.</param>
        public TetraSet(IEnumerable<ICard<V>> collection, int capacity = 16, HashBits bits = HashBits.bit64) : this(capacity, bits)
        {
            this.Add(collection);
        }

        /// <summary>
        /// Gets the first.
        /// </summary>
        /// <value>The first.</value>
        public virtual ICard<V> First => first;
        /// <summary>
        /// Gets the last.
        /// </summary>
        /// <value>The last.</value>
        public virtual ICard<V> Last => last;

        /// <summary>
        /// Gets the size.
        /// </summary>
        /// <value>The size.</value>
        public virtual int Size => size;
        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <value>The count.</value>
        public virtual int Count => count;
        /// <summary>
        /// Gets or sets the minimum count.
        /// </summary>
        /// <value>The minimum count.</value>
        public virtual int MinCount { get; set; }
        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only.
        /// </summary>
        /// <value><c>true</c> if this instance is read only; otherwise, <c>false</c>.</value>
        public virtual bool IsReadOnly { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is synchronized.
        /// </summary>
        /// <value><c>true</c> if this instance is synchronized; otherwise, <c>false</c>.</value>
        public virtual bool IsSynchronized { get; set; }
        /// <summary>
        /// Gets a value indicating whether this instance is repeatable.
        /// </summary>
        /// <value><c>true</c> if this instance is repeatable; otherwise, <c>false</c>.</value>
        public virtual bool IsRepeatable { get => false; }
        /// <summary>
        /// Gets or sets the synchronize root.
        /// </summary>
        /// <value>The synchronize root.</value>
        public virtual object SyncRoot { get; set; }
        /// <summary>
        /// Gets the value equals.
        /// </summary>
        /// <value>The value equals.</value>
        public virtual Func<V, V, bool> ValueEquals { get; }

        /// <summary>
        /// Gets or sets the <see cref="ICard{V}" /> at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        ICard<V> IList<ICard<V>>.this[int index]
        {
            get => GetCard(index);
            set => GetCard(index).Set(value);
        }
        /// <summary>
        /// Gets or sets the <see cref="V" /> at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>V.</returns>
        public virtual V this[int index]
        {
            get => GetCard(index).Value;
            set => GetCard(index).Value = value;
        }
        /// <summary>
        /// Gets or sets the <see cref="V" /> with the specified hashkey.
        /// </summary>
        /// <param name="hashkey">The hashkey.</param>
        /// <returns>V.</returns>
        protected V this[ulong hashkey]
        {
            get { return InnerGet(hashkey); }
            set { InnerPut(hashkey, value); }
        }
        /// <summary>
        /// Gets or sets the <see cref="V" /> with the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>V.</returns>
        public virtual V this[object key]
        {
            get { return InnerGet(unique.Key(key)); }
            set { InnerPut(unique.Key(key), value); }
        }
        /// <summary>
        /// Gets or sets the <see cref="System.Object" /> with the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>System.Object.</returns>
        object IFindable.this[object key]
        {
            get => InnerGet(unique.Key(key));
            set => InnerPut(unique.Key(key), (V)value);
        }

        /// <summary>
        /// Inners the get.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>V.</returns>
        public virtual V InnerGet(ulong key)
        {
            uint tid = getTetraId(key);
            int _size = tsize[tid];
            uint pos = (uint)getPosition(key);

            ICard<V> mem = table[tid, pos];

            while (mem != null)
            {
                if (mem.Equals(key))
                {
                    if (!mem.Removed)
                        return mem.Value;
                    return default(V);
                }
                mem = mem.Extended;
            }

            return default(V);
        }
        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>V.</returns>
        public V Get(ulong key)
        {
            return InnerGet(key);
        }
        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>V.</returns>
        public virtual V Get(object key)
        {
            return InnerGet(unique.Key(key));
        }
        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>V.</returns>
        public virtual V Get(IUnique<V> key)
        {
            return InnerGet(unique.Key(key));
        }
        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>V.</returns>
        public virtual V Get(IUnique key)
        {
            return InnerGet(unique.Key(key));
        }

        /// <summary>
        /// Inners the try get.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="output">The output.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public virtual bool InnerTryGet(ulong key, out ICard<V> output)
        {
            output = null;
            uint tid = getTetraId(key);
            int _size = tsize[tid];
            uint pos = (uint)getPosition(key);

            ICard<V> mem = table[tid, pos];
            while (mem != null)
            {
                if (mem.Equals(key))
                {
                    if (!mem.Removed)
                    {
                        output = mem;
                        return true;
                    }
                    return false;
                }
                mem = mem.Extended;
            }
            return false;
        }
        /// <summary>
        /// Tries the get.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="output">The output.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public virtual bool TryGet(object key, out ICard<V> output)
        {
            return InnerTryGet(unique.Key(key), out output);
        }
        /// <summary>
        /// Tries the get.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="output">The output.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public virtual bool TryGet(object key, out V output)
        {
            output = default(V);
            ICard<V> card = null;
            if (InnerTryGet(unique.Key(key), out card))
            {
                output = card.Value;
                return true;
            }
            return false;
        }
        /// <summary>
        /// Tries the get.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="output">The output.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool TryGet(ulong key, out V output)
        {
            output = default(V);
            ICard<V> card = null;
            if (InnerTryGet(key, out card))
            {
                output = card.Value;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Tries the get.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="output">The output.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool TryGet(IUnique key, out ICard<V> output)
        {
            return deckImplementation.TryGet(key, out output);
        }

        /// <summary>
        /// Tries the get.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="output">The output.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool TryGet(IUnique<V> key, out ICard<V> output)
        {
            return deckImplementation.TryGet(key, out output);
        }

        /// <summary>
        /// Inners the get card.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        public virtual ICard<V> InnerGetCard(ulong key)
        {
            uint tid = getTetraId(key);
            int _size = tsize[tid];
            uint pos = (uint)getPosition(key);

            ICard<V> mem = table[tid, pos];
            while (mem != null)
            {
                if (mem.Equals(key))
                {
                    if (!mem.Removed)
                        return mem;
                    return null;
                }
                mem = mem.Extended;
            }

            return null;
        }
        /// <summary>
        /// Gets the card.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        public abstract ICard<V> GetCard(int index);
        /// <summary>
        /// Gets the card.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        public virtual ICard<V> GetCard(object key)
        {
            return InnerGetCard(unique.Key(key));
        }
        /// <summary>
        /// Gets the card.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        public ICard<V> GetCard(ulong key)
        {
            return InnerGetCard(key);
        }
        /// <summary>
        /// Gets the card.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        public ICard<V> GetCard(IUnique key)
        {
            return deckImplementation.GetCard(key);
        }
        /// <summary>
        /// Gets the card.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        public ICard<V> GetCard(IUnique<V> key)
        {
            return deckImplementation.GetCard(key);
        }

        /// <summary>
        /// Sets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        public ICard<V> Set(object key, V value)
        {
            return deckImplementation.Set(key, value);
        }
        /// <summary>
        /// Sets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        public ICard<V> Set(ulong key, V value)
        {
            return deckImplementation.Set(key, value);
        }
        /// <summary>
        /// Sets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        public ICard<V> Set(IUnique key, V value)
        {
            return deckImplementation.Set(key, value);
        }
        /// <summary>
        /// Sets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        public ICard<V> Set(IUnique<V> key, V value)
        {
            return deckImplementation.Set(key, value);
        }
        /// <summary>
        /// Sets the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        public ICard<V> Set(V value)
        {
            return deckImplementation.Set(value);
        }
        /// <summary>
        /// Sets the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        public ICard<V> Set(IUnique<V> value)
        {
            return deckImplementation.Set(value);
        }
        /// <summary>
        /// Sets the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        public ICard<V> Set(ICard<V> value)
        {
            return deckImplementation.Set(value);
        }
        /// <summary>
        /// Sets the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>System.Int32.</returns>
        public int Set(IEnumerable<V> values)
        {
            return deckImplementation.Set(values);
        }
        /// <summary>
        /// Sets the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>System.Int32.</returns>
        public int Set(IList<V> values)
        {
            return deckImplementation.Set(values);
        }
        /// <summary>
        /// Sets the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>System.Int32.</returns>
        public int Set(IEnumerable<ICard<V>> values)
        {
            return deckImplementation.Set(values);
        }
        /// <summary>
        /// Sets the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>System.Int32.</returns>
        public int Set(IEnumerable<IUnique<V>> values)
        {
            return deckImplementation.Set(values);
        }

        /// <summary>
        /// Sures the get.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="sureaction">The sureaction.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        public ICard<V> SureGet(object key, Func<ulong, V> sureaction)
        {
            return deckImplementation.SureGet(key, sureaction);
        }
        /// <summary>
        /// Sures the get.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="sureaction">The sureaction.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        public ICard<V> SureGet(ulong key, Func<ulong, V> sureaction)
        {
            return deckImplementation.SureGet(key, sureaction);
        }
        /// <summary>
        /// Sures the get.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="sureaction">The sureaction.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        public ICard<V> SureGet(IUnique key, Func<ulong, V> sureaction)
        {
            return deckImplementation.SureGet(key, sureaction);
        }
        /// <summary>
        /// Sures the get.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="sureaction">The sureaction.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        public ICard<V> SureGet(IUnique<V> key, Func<ulong, V> sureaction)
        {
            return deckImplementation.SureGet(key, sureaction);
        }

        /// <summary>
        /// Inners the put.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        protected abstract ICard<V> InnerPut(ulong key, V value);
        /// <summary>
        /// Inners the put.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        protected abstract ICard<V> InnerPut(V value);
        /// <summary>
        /// Inners the put.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        protected abstract ICard<V> InnerPut(ICard<V> value);
        /// <summary>
        /// Puts the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        public virtual ICard<V> Put(ulong key, object value)
        {
            return InnerPut(key, (V)value);
        }
        /// <summary>
        /// Puts the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        public virtual ICard<V> Put(ulong key, V value)
        {
            return InnerPut(key, value);
        }

        /// <summary>
        /// Gets the value comparer.
        /// </summary>
        /// <returns>Func&lt;V, V, System.Boolean&gt;.</returns>
        protected virtual Func<V, V, bool> getValueComparer()
        {
            if (typeof(V).IsValueType)
                return (o1, o2) => o1.Equals(o2);
            return (o1, o2) => ReferenceEquals(o1, o2);
        }

        /// <summary>
        /// Puts the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        public virtual ICard<V> Put(object key, V value)
        {
            return InnerPut(unique.Key(key), value);
        }
        /// <summary>
        /// Puts the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        public virtual ICard<V> Put(object key, object value)
        {
            if (value is V)
                return InnerPut(unique.Key(key), (V)value);
            return null;
        }
        /// <summary>
        /// Puts the specified card.
        /// </summary>
        /// <param name="_card">The card.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        public virtual ICard<V> Put(ICard<V> _card)
        {
            return InnerPut(_card);
        }
        /// <summary>
        /// Puts the specified cards.
        /// </summary>
        /// <param name="cards">The cards.</param>
        public virtual void Put(IList<ICard<V>> cards)
        {
            int c = cards.Count;
            for (int i = 0; i < c; i++)
            {
                InnerPut(cards[i]);
            }
        }
        /// <summary>
        /// Puts the specified cards.
        /// </summary>
        /// <param name="cards">The cards.</param>
        public virtual void Put(IEnumerable<ICard<V>> cards)
        {
            foreach (ICard<V> card in cards)
                InnerPut(card);
        }
        /// <summary>
        /// Puts the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        public virtual ICard<V> Put(V value)
        {
            return InnerPut(value);
        }
        /// <summary>
        /// Puts the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        public virtual void Put(object value)
        {
            if (value is IUnique<V>)
                Put((IUnique<V>)value);
            if (value is V)
                Put((V)value);
            else if (value is ICard<V>)
                Put((ICard<V>)value);
        }
        /// <summary>
        /// Puts the specified cards.
        /// </summary>
        /// <param name="cards">The cards.</param>
        public virtual void Put(IList<V> cards)
        {
            int c = cards.Count;
            for (int i = 0; i < c; i++)
            {
                Put(cards[i]);

            }
        }
        /// <summary>
        /// Puts the specified cards.
        /// </summary>
        /// <param name="cards">The cards.</param>
        public virtual void Put(IEnumerable<V> cards)
        {
            foreach (V card in cards)
                Put(card);
        }
        /// <summary>
        /// Puts the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        public virtual ICard<V> Put(IUnique<V> value)
        {
            if (value is ICard<V>)
                return InnerPut((ICard<V>)value);
            return InnerPut(value.CompactKey(), value.UniqueObject);
        }
        /// <summary>
        /// Puts the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        public virtual void Put(IList<IUnique<V>> value)
        {
            foreach (IUnique<V> item in value)
            {
                Put(item);
            }
        }
        /// <summary>
        /// Puts the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        public virtual void Put(IEnumerable<IUnique<V>> value)
        {
            foreach (IUnique<V> item in value)
            {
                Put(item);
            }
        }

        /// <summary>
        /// Inners the add.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        protected abstract bool InnerAdd(ulong key, V value);
        /// <summary>
        /// Inners the add.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        protected abstract bool InnerAdd(V value);
        /// <summary>
        /// Inners the add.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        protected abstract bool InnerAdd(ICard<V> value);
        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public virtual bool Add(ulong key, object value)
        {
            return InnerAdd(key, (V)value);
        }
        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public virtual bool Add(ulong key, V value)
        {
            return InnerAdd(key, value);
        }
        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public virtual bool Add(object key, V value)
        {
            return InnerAdd(unique.Key(key), value);
        }
        /// <summary>
        /// Adds the specified card.
        /// </summary>
        /// <param name="card">The card.</param>
        public virtual void Add(ICard<V> card)
        {
            InnerAdd(card);
        }
        /// <summary>
        /// Adds the specified card list.
        /// </summary>
        /// <param name="cardList">The card list.</param>
        public virtual void Add(IList<ICard<V>> cardList)
        {
            int c = cardList.Count;
            for (int i = 0; i < c; i++)
            {
                InnerAdd(cardList[i]);
            }
        }
        /// <summary>
        /// Adds the specified card table.
        /// </summary>
        /// <param name="cardTable">The card table.</param>
        public virtual void Add(IEnumerable<ICard<V>> cardTable)
        {
            foreach (ICard<V> card in cardTable)
                Add(card);
        }
        /// <summary>
        /// Adds the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        public virtual void Add(V value)
        {
            InnerAdd(value);
        }
        /// <summary>
        /// Adds the specified cards.
        /// </summary>
        /// <param name="cards">The cards.</param>
        public virtual void Add(IList<V> cards)
        {
            int c = cards.Count;
            for (int i = 0; i < c; i++)
            {
                Add(cards[i]);

            }
        }
        /// <summary>
        /// Adds the specified cards.
        /// </summary>
        /// <param name="cards">The cards.</param>
        public virtual void Add(IEnumerable<V> cards)
        {
            foreach (V card in cards)
                Add(card);
        }
        /// <summary>
        /// Adds the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        public virtual void Add(IUnique<V> value)
        {
            if (value is ICard<V>)
                InnerAdd((ICard<V>)value);
            InnerAdd(unique.Key(value), value.UniqueObject);
        }
        /// <summary>
        /// Adds the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        public virtual void Add(IList<IUnique<V>> value)
        {
            foreach (IUnique<V> item in value)
            {
                Add(item);
            }
        }
        /// <summary>
        /// Adds the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        public virtual void Add(IEnumerable<IUnique<V>> value)
        {
            foreach (IUnique<V> item in value)
            {
                Add(item);
            }
        }
        /// <summary>
        /// Tries the add.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public virtual bool TryAdd(V value)
        {
            return InnerAdd(value);
        }

        /// <summary>
        /// News this instance.
        /// </summary>
        /// <returns>ICard&lt;V&gt;.</returns>
        public virtual ICard<V> New()
        {
            ICard<V> newCard = NewCard(Unique.New, default(V));
            if (InnerAdd(newCard))
                return newCard;
            return null;
        }
        /// <summary>
        /// News the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        public virtual ICard<V> New(ulong key)
        {
            ICard<V> newCard = NewCard(key, default(V));
            if (InnerAdd(newCard))
                return newCard;
            return null;
        }
        /// <summary>
        /// News the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        public virtual ICard<V> New(object key)
        {
            ICard<V> newCard = NewCard(unique.Key(key), default(V));
            if (InnerAdd(newCard))
                return newCard;
            return null;
        }

        /// <summary>
        /// Inners the insert.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="item">The item.</param>
        protected abstract void InnerInsert(int index, ICard<V> item);
        /// <summary>
        /// Inserts an item to the <see cref="T:System.Collections.Generic.IList`1" /> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="item" /> should be inserted.</param>
        /// <param name="item">The object to insert into the <see cref="T:System.Collections.Generic.IList`1" />.</param>
        /// <exception cref="System.Logs.ILogSate.Exception">Item exist</exception>
        public virtual void Insert(int index, ICard<V> item)
        {
            
            ulong key = item.Key;
            uint tid = getTetraId(key);
            int _size = tsize[tid];
            ulong pos = getPosition(key);
            var _table = table[tid];
            ICard<V> card = _table[pos];
            if (card == null)
            {
                card = NewCard(item);
                _table[pos] = card;
                InnerInsert(index, card);
                countIncrement(tid);
                return;
            }

            for (; ; )
            {
                
                if (card.Equals(key))
                {
                    
                    if (card.Removed)
                    {
                        var newcard = NewCard(item);
                        card.Extended = newcard;
                        InnerInsert(index, newcard);
                        conflictIncrement(tid);
                        return;
                    }
                    throw new Exception("Item exist");

                }
                
                if (card.Extended == null)
                {
                    var newcard = NewCard(item);
                    card.Extended = newcard;
                    InnerInsert(index, newcard);
                    conflictIncrement(tid);
                    return;
                }
                card = card.Extended;
            }
        }
        /// <summary>
        /// Inserts an item to the <see cref="T:System.Collections.Generic.IList`1" /> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="item" /> should be inserted.</param>
        /// <param name="item">The object to insert into the <see cref="T:System.Collections.Generic.IList`1" />.</param>
        public virtual void Insert(int index, V item)
        {
            Insert(index, NewCard(unique.Key(item), item));
        }

        /// <summary>
        /// Enqueues the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public virtual bool Enqueue(V value)
        {
            return InnerAdd(unique.Key(value), value);
        }
        /// <summary>
        /// Enqueues the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public virtual bool Enqueue(object key, V value)
        {
            return InnerAdd(unique.Key(key), value);
        }
        /// <summary>
        /// Enqueues the specified card.
        /// </summary>
        /// <param name="card">The card.</param>
        public virtual void Enqueue(ICard<V> card)
        {
            InnerAdd(card.Key, card.Value);
        }

        /// <summary>
        /// Tries the take.
        /// </summary>
        /// <param name="output">The output.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public virtual bool TryTake(out V output)
        {
            return TryDequeue(out output);
        }

        /// <summary>
        /// Tries the pick.
        /// </summary>
        /// <param name="skip">The skip.</param>
        /// <param name="output">The output.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool TryPick(int skip, out V output)
        {
            return deckImplementation.TryPick(skip, out output);
        }

        /// <summary>
        /// Dequeues this instance.
        /// </summary>
        /// <returns>V.</returns>
        public virtual V Dequeue()
        {
            V card = default(V);
            TryDequeue(out card);
            return card;
        }

        /// <summary>
        /// Tries the dequeue.
        /// </summary>
        /// <param name="output">The output.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public virtual bool TryDequeue(out V output)
        {
            var _output = Next(first);
            if (_output != null)
            {
                _output.Removed = true;
                removedIncrement(getTetraId(_output.Key));
                output = _output.Value;
                return true;
            }
            output = default(V);
            return false;
        }
        /// <summary>
        /// Tries the dequeue.
        /// </summary>
        /// <param name="output">The output.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public virtual bool TryDequeue(out ICard<V> output)
        {
            output = Next(first);
            if (output != null)
            {
                output.Removed = true;
                removedIncrement(getTetraId(output.Key));
                return true;
            }
            return false;
        }

        /// <summary>
        /// Renews the clear.
        /// </summary>
        /// <param name="capacity">The capacity.</param>
        private void renewClear(int capacity)
        {
            if (capacity != size || count > 0)
            {
                size = capacity;
                conflicts = 0;
                removed = 0;
                count = 0;
                tcount.ResetAll();
                tsize.ResetAll();
                table = new TetraTable<V>(this, size);
                first = EmptyCard();
                last = first;
            }
        }

        /// <summary>
        /// Renews the specified cards.
        /// </summary>
        /// <param name="cards">The cards.</param>
        public virtual void Renew(IEnumerable<V> cards)
        {
            renewClear(minSize);
            Put(cards);
        }
        /// <summary>
        /// Renews the specified cards.
        /// </summary>
        /// <param name="cards">The cards.</param>
        public virtual void Renew(IList<V> cards)
        {
            int capacity = cards.Count;
            capacity += (int)(capacity * CONFLICTS_PERCENT_LIMIT);
            renewClear(capacity);
            Put(cards);
        }
        /// <summary>
        /// Renews the specified cards.
        /// </summary>
        /// <param name="cards">The cards.</param>
        public virtual void Renew(IList<ICard<V>> cards)
        {
            int capacity = cards.Count;
            capacity += (int)(capacity * CONFLICTS_PERCENT_LIMIT);
            renewClear(capacity);
            Put(cards);
        }
        /// <summary>
        /// Renews the specified cards.
        /// </summary>
        /// <param name="cards">The cards.</param>
        public virtual void Renew(IEnumerable<ICard<V>> cards)
        {
            renewClear(minSize);
            Put(cards);
        }

        /// <summary>
        /// Inners the contains key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        protected bool InnerContainsKey(ulong key)
        {
            uint tid = getTetraId(key);
            int _size = tsize[tid];
            uint pos = (uint)getPosition(key);

            ICard<V> mem = table[tid, pos];

            while (mem != null)
            {
                if (!mem.Removed && mem.Equals(key))
                {

                    return true;
                }
                mem = mem.Extended;
            }

            return false;
        }
        /// <summary>
        /// Determines whether the specified key contains key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns><c>true</c> if the specified key contains key; otherwise, <c>false</c>.</returns>
        public virtual bool ContainsKey(object key)
        {
            return InnerContainsKey(unique.Key(key));
        }
        /// <summary>
        /// Determines whether the specified key contains key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns><c>true</c> if the specified key contains key; otherwise, <c>false</c>.</returns>
        public virtual bool ContainsKey(ulong key)
        {
            return InnerContainsKey(key);
        }
        /// <summary>
        /// Determines whether the specified key contains key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns><c>true</c> if the specified key contains key; otherwise, <c>false</c>.</returns>
        public virtual bool ContainsKey(IUnique key)
        {
            return InnerContainsKey(unique.Key(key));
        }

        /// <summary>
        /// Determines whether this instance contains the object.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns><c>true</c> if [contains] [the specified item]; otherwise, <c>false</c>.</returns>
        public virtual bool Contains(ICard<V> item)
        {
            return InnerContainsKey(item.Key);
        }
        /// <summary>
        /// Determines whether this instance contains the object.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns><c>true</c> if [contains] [the specified item]; otherwise, <c>false</c>.</returns>
        public virtual bool Contains(IUnique<V> item)
        {
            return InnerContainsKey(unique.Key(item));
        }
        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1" /> contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        /// <returns><see langword="true" /> if <paramref name="item" /> is found in the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, <see langword="false" />.</returns>
        public virtual bool Contains(V item)
        {
            return InnerContainsKey(unique.Key(item));
        }
        /// <summary>
        /// Determines whether this instance contains the object.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="item">The item.</param>
        /// <returns><c>true</c> if [contains] [the specified key]; otherwise, <c>false</c>.</returns>
        public virtual bool Contains(ulong key, V item)
        {
            return InnerContainsKey(key);
        }

        /// <summary>
        /// Inners the remove.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>V.</returns>
        public virtual V InnerRemove(ulong key)
        {
            uint tid = getTetraId(key);
            int _size = tsize[tid];
            uint pos = (uint)getPosition(key);

            ICard<V> mem = table[tid, pos];

            while (mem != null)
            {
                if (mem.Equals(key))
                {
                    if (!mem.Removed)
                    {
                        mem.Removed = true;
                        removedIncrement(getTetraId(mem.Key));
                        return mem.Value;
                    }
                    return default(V);
                }

                mem = mem.Extended;
            }
            return default(V);
        }
        /// <summary>
        /// Inners the remove.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="item">The item.</param>
        /// <returns>V.</returns>
        protected virtual V InnerRemove(ulong key, V item)
        {
            uint tid = getTetraId(key);
            int _size = tsize[tid];
            uint pos = (uint)getPosition(key);

            ICard<V> mem = table[tid, pos];

            while (mem != null)
            {
                if (mem.Equals(key))
                {
                    if (mem.Removed)
                        return default(V);

                    if (ValueEquals(mem.Value, item))
                    {
                        mem.Removed = true;
                        removedIncrement(getTetraId(mem.Key));
                        return mem.Value;
                    }
                    return default(V);
                }
                mem = mem.Extended;
            }
            return default(V);
        }
        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        /// <returns><see langword="true" /> if <paramref name="item" /> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, <see langword="false" />. This method also returns <see langword="false" /> if <paramref name="item" /> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1" />.</returns>
        public virtual bool Remove(V item)
        {
            return InnerRemove(unique.Key(item)).Equals(default(V)) ? false : true;
        }
        /// <summary>
        /// Removes the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>V.</returns>
        public virtual V Remove(object key)
        {
            return InnerRemove(unique.Key(key));
        }
        /// <summary>
        /// Removes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public virtual bool Remove(ICard<V> item)
        {
            return InnerRemove(item.Key).Equals(default(V)) ? false : true;
        }
        /// <summary>
        /// Removes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public virtual bool Remove(IUnique<V> item)
        {
            return TryRemove(unique.Key(item));
        }
        /// <summary>
        /// Tries the remove.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public virtual bool TryRemove(object key)
        {
            return InnerRemove(unique.Key(key)).Equals(default(V)) ? false : true;
        }
        /// <summary>
        /// Removes the <see cref="T:System.Collections.Generic.IList`1" /> item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param>
        public virtual void RemoveAt(int index)
        {
            InnerRemove(GetCard(index).Key);
        }
        /// <summary>
        /// Removes the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="item">The item.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public virtual bool Remove(object key, V item)
        {
            return InnerRemove(unique.Key(key), item).Equals(default(V)) ? false : true;
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        public virtual void Clear()
        {
            size = minSize;
            conflicts = 0;
            removed = 0;
            count = 0;
            tcount.ResetAll();
            tsize.ResetAll();
            table = new TetraTable<V>(this, size);
            first = EmptyCard();
            last = first;
        }

        /// <summary>
        /// Resizes the specified size.
        /// </summary>
        /// <param name="size">The size.</param>
        public void Resize(int size)
        {
            deckImplementation.Resize(size);
        }

        /// <summary>
        /// Flushes this instance.
        /// </summary>
        public virtual void Flush()
        {
            conflicts = 0;
            removed = 0;
            count = 0;
            tcount.ResetAll();
            tsize.ResetAll();
            table = new TetraTable<V>(this, size);
            first = EmptyCard();
            last = first;
        }

        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="index">The index.</param>
        public virtual void CopyTo(ICard<V>[] array, int index)
        {
            int c = count, i = index, l = array.Length;
            if (l - i < c)
            {
                c = l - i;
                foreach (ICard<V> ves in this.AsCards().Take(c))
                    array[i++] = ves;
            }
            else
                foreach (ICard<V> ves in this)
                    array[i++] = ves;
        }
        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.ICollection" /> to an <see cref="T:System.Array" />, starting at a particular <see cref="T:System.Array" /> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.ICollection" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
        /// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins.</param>
        public virtual void CopyTo(Array array, int index)
        {
            int c = count, i = index, l = array.Length;
            if (l - i < c)
            {
                c = l - i;
                foreach (V ves in this.AsValues().Take(c))
                    array.SetValue(ves, i++);
            }
            else
                foreach (V ves in this.AsValues())
                    array.SetValue(ves, i++);
        }
        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.Concurrent.IProducerConsumerCollection`1" /> to an <see cref="T:System.Array" />, starting at a specified index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from the <see cref="T:System.Collections.Concurrent.IProducerConsumerCollection`1" />.
        /// The array must have zero-based indexing.</param>
        /// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins.</param>
        public virtual void CopyTo(V[] array, int index)
        {
            int c = count, i = index, l = array.Length;
            if (l - i < c)
            {
                c = l - i;
                foreach (V ves in this.AsValues().Take(c))
                    array[i++] = ves;
            }
            else
                foreach (V ves in this.AsValues())
                    array[i++] = ves;
        }
        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="arrayIndex">Index of the array.</param>
        public virtual void CopyTo(IUnique<V>[] array, int arrayIndex)
        {
            int c = count, i = arrayIndex, l = array.Length;
            if (l - i < c)
            {
                c = l - i;
                foreach (ICard<V> ves in this.AsCards().Take(c))
                    array[i++] = ves;
            }
            else
                foreach (ICard<V> ves in this)
                    array[i++] = ves;
        }
        /// <summary>
        /// Converts to array.
        /// </summary>
        /// <returns>V[].</returns>
        public virtual V[] ToArray()
        {
            return this.AsValues().ToArray();
        }

        /// <summary>
        /// Nexts the specified card.
        /// </summary>
        /// <param name="card">The card.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        public virtual ICard<V> Next(ICard<V> card)
        {
            ICard<V> _card = card.Next;
            if (_card != null)
            {
                if (!_card.Removed)
                    return _card;
                return Next(_card);
            }
            return null;
        }

        /// <summary>
        /// Resizes the specified size.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <param name="tid">The tid.</param>
        public virtual void Resize(int size, uint tid)
        {
            Rehash(size, tid);
        }

        /// <summary>
        /// Empties the card.
        /// </summary>
        /// <returns>ICard&lt;V&gt;.</returns>
        public abstract ICard<V> EmptyCard();

        /// <summary>
        /// Creates new card.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        public abstract ICard<V> NewCard(ulong key, V value);
        /// <summary>
        /// Creates new card.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        public abstract ICard<V> NewCard(object key, V value);
        /// <summary>
        /// Creates new card.
        /// </summary>
        /// <param name="card">The card.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        public abstract ICard<V> NewCard(ICard<V> card);
        /// <summary>
        /// Creates new card.
        /// </summary>
        /// <param name="card">The card.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        public abstract ICard<V> NewCard(V card);

        /// <summary>
        /// Empties the card table.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <returns>ICard&lt;V&gt;[].</returns>
        public abstract ICard<V>[] EmptyCardTable(int size);

        /// <summary>
        /// Determines the index of a specific item in the <see cref="T:System.Collections.Generic.IList`1" />.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.IList`1" />.</param>
        /// <returns>The index of <paramref name="item" /> if found in the list; otherwise, -1.</returns>
        public virtual int IndexOf(ICard<V> item)
        {
            return GetCard(item).Index;
        }
        /// <summary>
        /// Determines the index of a specific item in the <see cref="T:System.Collections.Generic.IList`1" />.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.IList`1" />.</param>
        /// <returns>The index of <paramref name="item" /> if found in the list; otherwise, -1.</returns>
        public virtual int IndexOf(V item)
        {
            return GetCard(item).Index;
        }

        /// <summary>
        /// Ases the values.
        /// </summary>
        /// <returns>IEnumerable&lt;V&gt;.</returns>
        public virtual IEnumerable<V> AsValues()
        {
            return (IEnumerable<V>)this;
        }

        /// <summary>
        /// Ases the cards.
        /// </summary>
        /// <returns>IEnumerable&lt;ICard&lt;V&gt;&gt;.</returns>
        public virtual IEnumerable<ICard<V>> AsCards()
        {
            return (IEnumerable<ICard<V>>)this;
        }

        /// <summary>
        /// Ases the identifiers.
        /// </summary>
        /// <returns>IEnumerable&lt;IUnique&lt;V&gt;&gt;.</returns>
        public virtual IEnumerable<IUnique<V>> AsIdentifiers()
        {
            return (IEnumerable<IUnique<V>>)this;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public virtual IEnumerator<ICard<V>> GetEnumerator()
        {
            return new CardSeries<V>(this);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        IEnumerator<V> IEnumerable<V>.GetEnumerator()
        {
            return new CardSeries<V>(this);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        IEnumerator<IUnique<V>>
                        IEnumerable<IUnique<V>>.GetEnumerator()
        {
            return new CardKeySeries<V>(this);
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new CardSeries<V>(this);
        }

        /// <summary>
        /// Gets the tetra identifier.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>System.UInt32.</returns>
        protected static uint getTetraId(ulong key)
        {
            return (uint)(((long)key & 1L) - (((long)key & -1L) * 2));
        }
        /// <summary>
        /// Gets the position.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>System.UInt64.</returns>
        protected ulong getPosition(ulong key)
        {
            

            return (key % (uint)(size - 1));

            
            
            

            
        }
        /// <summary>
        /// Gets the position.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="newsize">The newsize.</param>
        /// <returns>System.UInt64.</returns>
        protected static ulong getPosition(ulong key, int newsize)
        {
            

            return (key % (uint)(newsize - 1));

            
            
            

            
        }

        /// <summary>
        /// Rehashes the specified newsize.
        /// </summary>
        /// <param name="newsize">The newsize.</param>
        /// <param name="tid">The tid.</param>
        protected virtual void Rehash(int newsize, uint tid)
        {
            int finish = tcount[tid];
            int _tsize = tsize[tid];
            ICard<V>[] newCardTable = EmptyCardTable(newsize);
            ICard<V> card = first;
            card = card.Next;
            if (removed > 0)
            {
                rehashAndReindex(card, newCardTable, newsize, tid);
            }
            else
            {
                rehashOnly(card, newCardTable, newsize, tid);
            }

            table[tid] = newCardTable;
            size = newsize - _tsize;

        }

        /// <summary>
        /// Rehashes the and reindex.
        /// </summary>
        /// <param name="card">The card.</param>
        /// <param name="newCardTable">The new card table.</param>
        /// <param name="newsize">The newsize.</param>
        /// <param name="tid">The tid.</param>
        private void rehashAndReindex(ICard<V> card, ICard<V>[] newCardTable, int newsize, uint tid)
        {
            int _conflicts = 0;
            int _oldconflicts = 0;
            int _removed = 0;
            ICard<V>[] _newCardTable = newCardTable;
            ICard<V> _firstcard = EmptyCard();
            ICard<V> _lastcard = _firstcard;
            do
            {
                if (!card.Removed)
                {
                    ulong pos = getPosition(card.Key, newsize);

                    ICard<V> mem = _newCardTable[pos];

                    if (mem == null)
                    {
                        if (card.Extended != null)
                            _oldconflicts++;

                        card.Extended = null;
                        _newCardTable[pos] = _lastcard = _lastcard.Next = card;
                    }
                    else
                    {
                        for (; ; )
                        {
                            if (mem.Extended == null)
                            {
                                card.Extended = null; ;
                                _lastcard = _lastcard.Next = mem.Extended = card;
                                _conflicts++;
                                break;
                            }
                            else
                                mem = mem.Extended;
                        }
                    }
                }
                else
                    _removed++;

                card = card.Next;

            } while (card != null);
            conflicts -= _oldconflicts;
            removed -= _removed;
            first = _firstcard;
            last = _lastcard;
        }

        /// <summary>
        /// Rehashes the only.
        /// </summary>
        /// <param name="card">The card.</param>
        /// <param name="newCardTable">The new card table.</param>
        /// <param name="newsize">The newsize.</param>
        /// <param name="tid">The tid.</param>
        private void rehashOnly(ICard<V> card, ICard<V>[] newCardTable, int newsize, uint tid)
        {
            int _conflicts = 0;
            int _oldconflicts = 0;
            ICard<V>[] _newCardTable = newCardTable;
            do
            {
                if (!card.Removed)
                {
                    ulong pos = getPosition(card.Key, newsize);

                    ICard<V> mem = _newCardTable[pos];

                    if (mem == null)
                    {
                        if (card.Extended != null)
                            _oldconflicts++;

                        card.Extended = null;
                        _newCardTable[pos] = card;
                    }
                    else
                    {
                        for (; ; )
                        {
                            if (mem.Extended == null)
                            {
                                card.Extended = null;
                                mem.Extended = card;
                                _conflicts++;
                                break;
                            }
                            else
                                mem = mem.Extended;
                        }
                    }
                }

                card = card.Next;

            } while (card != null);
            conflicts -= _oldconflicts;
        }

        /// <summary>
        /// The disposed value
        /// </summary>
        private bool disposedValue = false;
        /// <summary>
        /// The deck implementation
        /// </summary>
        private IDeck<V> deckImplementation;

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    first = null;
                    last = null;
                }

                table.Dispose();

                disposedValue = true;
            }
        }








        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            
            Dispose(true);
            
            
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><see langword="true" /> if the current object is equal to the <paramref name="other" /> parameter; otherwise, <see langword="false" />.</returns>
        public bool Equals(IUnique? other)
        {
            return serialcode.Equals(other);
        }

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="other">An object to compare with this instance.</param>
        /// <returns>A value that indicates the relative order of the objects being compared. The return value has these meanings:
        /// <list type="table"><listheader><term> Value</term><description> Meaning</description></listheader><item><term> Less than zero</term><description> This instance precedes <paramref name="other" /> in the sort order.</description></item><item><term> Zero</term><description> This instance occurs in the same position in the sort order as <paramref name="other" />.</description></item><item><term> Greater than zero</term><description> This instance follows <paramref name="other" /> in the sort order.</description></item></list></returns>
        public int CompareTo(IUnique? other)
        {
            return serialcode.CompareTo(other);
        }

        /// <summary>
        /// Gets the empty.
        /// </summary>
        /// <value>The empty.</value>
        public IUnique Empty => Usid.Empty;

        /// <summary>
        /// Gets or sets the unique key.
        /// </summary>
        /// <value>The unique key.</value>
        public ulong UniqueKey
        {
            get => serialcode.UniqueKey;
            set => serialcode.UniqueKey = value;
        }

        /// <summary>
        /// Gets or sets the unique seed.
        /// </summary>
        /// <value>The unique seed.</value>
        public ulong UniqueSeed
        {
            get => serialcode.UniqueSeed;
            set => serialcode.UniqueSeed = value;
        }

        /// <summary>
        /// Gets the bytes.
        /// </summary>
        /// <returns>System.Byte[].</returns>
        public byte[] GetBytes()
        {
            return serialcode.GetBytes();
        }

        /// <summary>
        /// Gets the unique bytes.
        /// </summary>
        /// <returns>System.Byte[].</returns>
        public byte[] GetUniqueBytes()
        {
            return serialcode.GetUniqueBytes();
        }

        /// <summary>
        /// Gets the type of the element.
        /// </summary>
        /// <value>The type of the element.</value>
        public Type ElementType
        {
            get { return typeof(V); }
        }
        /// <summary>
        /// Gets or sets the expression.
        /// </summary>
        /// <value>The expression.</value>
        public Expression Expression { get; set; }
        /// <summary>
        /// Gets or sets the provider.
        /// </summary>
        /// <value>The provider.</value>
        public IQueryProvider Provider { get; protected set; }
    }
}
