
// <copyright file="SeededSet.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The Basedeck namespace.
/// </summary>
namespace System.Series.Basedeck
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Uniques;

    /// <summary>
    /// Class SeededSet.
    /// Implements the <see cref="System.Uniques.Uniqueness" />
    /// Implements the <see cref="System.Series.IMassDeck{V}" />
    /// </summary>
    /// <typeparam name="V"></typeparam>
    /// <seealso cref="System.Uniques.Uniqueness" />
    /// <seealso cref="System.Series.IMassDeck{V}" />
    public abstract class SeededSet<V> : Uniqueness, IMassDeck<V> where V : IUnique
    {
        /// <summary>
        /// The resizing vector
        /// </summary>
        static protected readonly float RESIZING_VECTOR = 1.766f;
        /// <summary>
        /// The conflicts percent limit
        /// </summary>
        static protected readonly float CONFLICTS_PERCENT_LIMIT = 0.22f;
        /// <summary>
        /// The removed percent limit
        /// </summary>
        static protected readonly float REMOVED_PERCENT_LIMIT = 0.15f;

        /// <summary>
        /// The serialcode
        /// </summary>
        protected Ussn serialcode;
        /// <summary>
        /// The first
        /// </summary>
        protected ICard<V> first, last;
        /// <summary>
        /// The table
        /// </summary>
        protected ICard<V>[] table;
        /// <summary>
        /// The count
        /// </summary>
        protected int count, conflicts, removed, minSize, size, mincount;
        /// <summary>
        /// The maximum identifier
        /// </summary>
        protected uint maxId;

        /// <summary>
        /// Nexts the size.
        /// </summary>
        /// <returns>System.Int32.</returns>
        protected int nextSize()
        {
            
            return (((int)(size * RESIZING_VECTOR)) ^ 3); 
        }

        /// <summary>
        /// Previouses the size.
        /// </summary>
        /// <returns>System.Int32.</returns>
        protected int previousSize()
        {
            
            return (int)(size * (1 - REMOVED_PERCENT_LIMIT)) ^ 3; 
        }

        /// <summary>
        /// Counts the increment.
        /// </summary>
        protected void countIncrement()
        {
            if ((++count + 7) > size)
                Rehash(nextSize());            
        }
        /// <summary>
        /// Conflicts the increment.
        /// </summary>
        protected void conflictIncrement()
        {
            countIncrement();
            if (++conflicts > (size * CONFLICTS_PERCENT_LIMIT))
                Rehash(nextSize());
        }
        /// <summary>
        /// Removeds the increment.
        /// </summary>
        protected void removedIncrement()
        {
            --count;
            if (++removed > ((size * REMOVED_PERCENT_LIMIT) - 1))
            {
                if (size < (size * 0.5))
                    Rehash(previousSize());
                else
                    Rehash(size);
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
        /// Initializes a new instance of the <see cref="SeededSet{V}" /> class.
        /// </summary>
        /// <param name="capacity">The capacity.</param>
        /// <param name="bits">The bits.</param>
        public SeededSet(int capacity = 17, HashBits bits = HashBits.bit64) : base(bits)
        {
            size = capacity;
            minSize = capacity;
            maxId = (uint)(size - 1);
            table = EmptyCardTable(capacity);
            first = EmptyCard();
            last = first;
            ValueEquals = getValueComparer();
            serialcode = new Ussn(typeof(V).UniqueKey64());
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="SeededSet{V}" /> class.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="capacity">The capacity.</param>
        /// <param name="bits">The bits.</param>
        public SeededSet(IList<ICard<V>> collection, int capacity = 17, HashBits bits = HashBits.bit64) : this(capacity > collection.Count ? capacity : collection.Count, bits)
        {
            this.Add(collection);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="SeededSet{V}" /> class.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="capacity">The capacity.</param>
        /// <param name="bits">The bits.</param>
        public SeededSet(IList<IUnique<V>> collection, int capacity = 17, HashBits bits = HashBits.bit64) : this(capacity > collection.Count ? capacity : collection.Count, bits)
        {
            foreach (var c in collection)
                this.Add(c);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="SeededSet{V}" /> class.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="capacity">The capacity.</param>
        /// <param name="bits">The bits.</param>
        public SeededSet(IEnumerable<ICard<V>> collection, int capacity = 17, HashBits bits = HashBits.bit64) : this(capacity, bits)
        {
            this.Add(collection);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="SeededSet{V}" /> class.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="capacity">The capacity.</param>
        /// <param name="bits">The bits.</param>
        public SeededSet(IEnumerable<IUnique<V>> collection, int capacity = 17, HashBits bits = HashBits.bit64) : this(capacity, bits)
        {
            foreach (var c in collection)
                this.Add(c);
        }

        /// <summary>
        /// Gets the first.
        /// </summary>
        /// <value>The first.</value>
        public virtual ICard<V> First
        { get { return first; } }
        /// <summary>
        /// Gets the last.
        /// </summary>
        /// <value>The last.</value>
        public virtual ICard<V> Last
        { get { return last; } }

        /// <summary>
        /// Gets the size.
        /// </summary>
        /// <value>The size.</value>
        public virtual int Size { get => size; }
        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>The count.</value>
        public virtual int Count { get => count; }
        /// <summary>
        /// Gets or sets the minimum count.
        /// </summary>
        /// <value>The minimum count.</value>
        public virtual int MinCount
        {
            get => mincount;
            set => mincount = value;
        }
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
            get => InnerGet(hashkey); 
            set => InnerPut(hashkey, value); 
        }
        /// <summary>
        /// Gets or sets the <see cref="V" /> with the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>V.</returns>
        /// <exception cref="System.NotSupportedException"></exception>
        public virtual V this[object key]
        {
            get
            {
                if (key is IUnique)
                {
                    IUnique ukey = (IUnique)key;
                    return InnerGet(unique.Key(ukey, ukey.UniqueSeed));
                }
                else
                    throw new NotSupportedException();
            }
            set
            {
                if (key is IUnique)
                {
                    IUnique ukey = (IUnique)key;
                    InnerPut(unique.Key(ukey, ukey.UniqueSeed), value);
                }
                else
                    throw new NotSupportedException();
            }
        }
        /// <summary>
        /// Gets or sets the <see cref="System.Object" /> with the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>System.Object.</returns>
        /// <exception cref="System.NotSupportedException"></exception>
        object IFindable.this[object key]
        {
            get
            {
                if (key is IUnique)
                {
                    IUnique ukey = (IUnique)key;
                    return InnerGet(unique.Key(ukey, ukey.UniqueSeed));
                }
                else
                    throw new NotSupportedException();
            }
            set
            {
                if (key is IUnique)
                {
                    IUnique ukey = (IUnique)key;
                    InnerPut(unique.Key(ukey, ukey.UniqueSeed), (V)value);
                }
                else
                    throw new NotSupportedException();
            }
        }
        /// <summary>
        /// Gets or sets the <see cref="V" /> with the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>V.</returns>
        public virtual V this[IUnique key]
        {
            get => InnerGet(unique.Key(key, key.UniqueSeed));
            set => InnerPut(unique.Key(key, key.UniqueSeed), value);
        }
        /// <summary>
        /// Gets or sets the <see cref="V" /> with the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>V.</returns>
        public virtual V this[IUnique<V> key]
        {
            get => InnerGet(unique.Key(key, key.UniqueSeed));
            set => InnerPut(unique.Key(key, key.UniqueSeed), value);
        }
        /// <summary>
        /// Gets or sets the <see cref="V" /> with the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>V.</returns>
        public virtual V this[object key, ulong seed]
        {
            get => InnerGet(unique.Key(key, seed)); 
            set => InnerPut(unique.Key(key, seed), value); 
        }
        /// <summary>
        /// Gets or sets the <see cref="V" /> with the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>V.</returns>
        public virtual V this[IUnique key, ulong seed]
        {
            get => InnerGet(unique.Key(key, seed)); 
            set => InnerPut(unique.Key(key, seed), value); 
        }
        /// <summary>
        /// Gets or sets the <see cref="V" /> with the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>V.</returns>
        public virtual V this[IUnique<V> key, ulong seed]
        {
            get => InnerGet(unique.Key(key, seed));
            set => InnerPut(unique.Key(key, seed), value);
        }

        /// <summary>
        /// Inners the get.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>V.</returns>
        protected virtual V InnerGet(ulong key)
        {
            if (key == 0)
                return default(V);

            ICard<V> mem = table[getPosition(key)];

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
        public virtual V Get(ulong key)
        {
            return InnerGet(key);
        }
        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>V.</returns>
        /// <exception cref="System.NotSupportedException"></exception>
        public virtual V Get(object key)
        {
            if (key is IUnique)
            {
                IUnique ukey = (IUnique)key;
                return InnerGet(unique.Key(ukey, ukey.UniqueSeed));
            }
            else
                throw new NotSupportedException();
        }
        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>V.</returns>
        public virtual V Get(object key, ulong seed)
        {
            return InnerGet(unique.Key(key, seed));
        }
        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>V.</returns>
        public virtual V Get(IUnique key)
        {
            return InnerGet(unique.Key(key, key.UniqueSeed));
        }
        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>V.</returns>
        public virtual V Get(IUnique<V> key)
        {
            return InnerGet(unique.Key(key, key.UniqueSeed));
        }

        /// <summary>
        /// Inners the try get.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="output">The output.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        protected virtual bool InnerTryGet(ulong key, out ICard<V> output)
        {
            output = null;
            if (key == 0)
                return false;

            ICard<V> mem = table[getPosition(key)];
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
        public virtual bool TryGet(ulong key, out ICard<V> output)
        {
            return InnerTryGet(key, out output);
        }
        /// <summary>
        /// Tries the get.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="output">The output.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        /// <exception cref="System.NotSupportedException"></exception>
        public virtual bool TryGet(object key, out ICard<V> output)
        {
            if (key is IUnique)
            {
                IUnique ukey = (IUnique)key;
                return InnerTryGet(unique.Key(ukey, ukey.UniqueSeed), out output);
            }
            else
                throw new NotSupportedException();
        }
        /// <summary>
        /// Tries the get.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="output">The output.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        /// <exception cref="System.NotSupportedException"></exception>
        public virtual bool TryGet(object key, out V output)
        {
            if (key is IUnique)
            {
                output = default(V);
                ICard<V> card = null;
                IUnique ukey = (IUnique)key;
                if (InnerTryGet(unique.Key(ukey, ukey.UniqueSeed), out card))
                {
                    output = card.Value;
                    return true;
                }
                return false;
            }
            else
                throw new NotSupportedException();
        }
        /// <summary>
        /// Tries the get.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="seed">The seed.</param>
        /// <param name="output">The output.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public virtual bool TryGet(object key, ulong seed, out ICard<V> output)
        {
            return InnerTryGet(unique.Key(key, seed), out output);
        }
        /// <summary>
        /// Tries the get.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="seed">The seed.</param>
        /// <param name="output">The output.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public virtual bool TryGet(object key, ulong seed, out V output)
        {
            output = default(V);
            ICard<V> card = null;
            if (InnerTryGet(unique.Key(key, seed), out card))
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
        public virtual bool TryGet(ulong key, out V output)
        {
            if (InnerTryGet(key, out ICard<V> card))
            {
                output = card.Value;
                return true;
            }
            output = default(V);
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
            return InnerTryGet(unique.Key(key, key.UniqueSeed), out output);
        }
        /// <summary>
        /// Tries the get.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="output">The output.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool TryGet(IUnique<V> key, out ICard<V> output)
        {
            return InnerTryGet(unique.Key(key, key.UniqueSeed), out output);
        }

        /// <summary>
        /// Inners the get card.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        protected virtual ICard<V> InnerGetCard(ulong key)
        {
            if (key == 0)
                return null;

            ICard<V> mem = table[getPosition(key)];

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
        /// <param name="key">The key.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        public virtual ICard<V> GetCard(ulong key)
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
            return InnerGetCard(unique.Key(key, key.UniqueSeed));
        }
        /// <summary>
        /// Gets the card.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        public ICard<V> GetCard(IUnique<V> key)
        {
            return InnerGetCard(unique.Key(key, key.UniqueSeed));
        }

        /// <summary>
        /// Inners the set.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        protected virtual ICard<V> InnerSet(ulong key, V value)
        {
            var card = InnerGetCard(key);
            if (card != null) card.Value = value;
            return card;
        }
        /// <summary>
        /// Inners the set.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        protected virtual ICard<V> InnerSet(ICard<V> value)
        {
            var card = GetCard(value);
            if (card != null) card.Value = value.Value;
            return card;
        }
        /// <summary>
        /// Sets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        public ICard<V> Set(object key, V value)
        {
            return InnerSet(unique.Key(key, value.UniqueSeed), value);
        }
        /// <summary>
        /// Sets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        public ICard<V> Set(ulong key, V value)
        {
            return massDeckImplementation.Set(key, value);
        }
        /// <summary>
        /// Sets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        public ICard<V> Set(IUnique key, V value)
        {
            return InnerSet(unique.Key(key, key.UniqueSeed), value);
        }
        /// <summary>
        /// Sets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        public ICard<V> Set(IUnique<V> key, V value)
        {
            return InnerSet(unique.Key(key, key.UniqueSeed),  value);
        }
        /// <summary>
        /// Sets the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        public ICard<V> Set(V value)
        {
            return InnerSet(unique.Key(value, value.UniqueSeed), value);
        }
        /// <summary>
        /// Sets the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        public ICard<V> Set(IUnique<V> value)
        {
            return InnerSet(unique.Key(value, value.UniqueSeed), value.UniqueObject);
        }
        /// <summary>
        /// Sets the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        public ICard<V> Set(ICard<V> value)
        {
            return InnerSet(value);
        }
        /// <summary>
        /// Sets the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>System.Int32.</returns>
        public int Set(IEnumerable<V> values)
        {
            int count = 0;
            foreach (var value in values)
            {
                if (Set(value) != null)
                    count++;
            }

            return count;
        }
        /// <summary>
        /// Sets the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>System.Int32.</returns>
        public int Set(IList<V> values)
        {
            int count = 0;
            foreach (var value in values)
            {
                if (Set(value) != null)
                    count++;
            }

            return count;
        }
        /// <summary>
        /// Sets the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>System.Int32.</returns>
        public int Set(IEnumerable<ICard<V>> values)
        {
            int count = 0;
            foreach (var value in values)
            {
                if (InnerSet(value) != null)
                    count++;
            }

            return count;
        }
        /// <summary>
        /// Sets the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>System.Int32.</returns>
        public int Set(IEnumerable<IUnique<V>> values)
        {
            int count = 0;
            foreach (var value in values)
            {
                if (Set(value) != null)
                    count++;
            }

            return count;
        }

        /// <summary>
        /// Sures the get.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="sureaction">The sureaction.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        /// <exception cref="System.NotSupportedException"></exception>
        public ICard<V> SureGet(object key, Func<ulong, V> sureaction)
        {
            if (key is IUnique)
            {
                IUnique ukey = (IUnique)key;
                ulong _key = unique.Key(ukey, ukey.UniqueSeed);
                return (!TryGet(_key, out ICard<V> item)) ?
                     Put(key, sureaction.Invoke(_key)) : item;
            }
            else
                throw new NotSupportedException();        
        }
        /// <summary>
        /// Sures the get.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="sureaction">The sureaction.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        public ICard<V> SureGet(ulong key, Func<ulong, V> sureaction)
        {
            return (!TryGet(key, out ICard<V> item)) ?
              Put(key, sureaction.Invoke(key)) : item;
        }
        /// <summary>
        /// Sures the get.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="sureaction">The sureaction.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        public ICard<V> SureGet(IUnique key, Func<ulong, V> sureaction)
        {
            ulong _key = unique.Key(key, key.UniqueSeed);
            return (!TryGet(_key, out ICard<V> item)) ?
                Put(key, sureaction.Invoke(_key)) : item;
        }
        /// <summary>
        /// Sures the get.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="sureaction">The sureaction.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        public ICard<V> SureGet(IUnique<V> key, Func<ulong, V> sureaction)
        {
            ulong _key = unique.Key(key, key.UniqueSeed);
            return (!TryGet(_key, out ICard<V> item)) ?
                Put(key, sureaction.Invoke(_key)) : item;
        }

        /// <summary>
        /// Gets the card.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        /// <exception cref="System.NotSupportedException"></exception>
        public virtual ICard<V> GetCard(object key)
        {
            if (key is IUnique)
            {
                IUnique ukey = (IUnique)key;
                return InnerGetCard(unique.Key(ukey, ukey.UniqueSeed));
            }
            else
                throw new NotSupportedException();
        }
        /// <summary>
        /// Gets the card.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        public virtual ICard<V> GetCard(object key, ulong seed)
        {
            return InnerGetCard(unique.Key(key, seed));
        }
        /// <summary>
        /// Gets the card.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        public abstract ICard<V> GetCard(int index);

        /// <summary>
        /// Inners the put.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="seed">The seed.</param>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        protected virtual ICard<V> InnerPut(ulong key, ulong seed, V value)
        {
            value.UniqueSeed = seed;
            value.UniqueKey = key;
            return InnerPut(value);
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
        /// <param name="seed">The seed.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        protected virtual ICard<V> InnerPut(V value, ulong seed)
        {
            value.UniqueSeed = seed;
            return InnerPut(value);
        }
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
        /// Puts the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        public virtual ICard<V> Put(object key, V value)
        {
            return InnerPut(unique.Key(key, value.UniqueSeed), value);
        }
        /// <summary>
        /// Puts the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="seed">The seed.</param>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        public virtual ICard<V> Put(object key, ulong seed, V value)
        {
            return InnerPut(unique.Key(key, seed), value);
        }
        /// <summary>
        /// Puts the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="seed">The seed.</param>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        public virtual ICard<V> Put(object key, ulong seed, object value)
        {
            if (value is V)
            {
                V o = (V)value;
                return InnerPut(unique.Key(key, seed), (V)value);
            }
            return null;
        }
        /// <summary>
        /// Puts the specified card.
        /// </summary>
        /// <param name="card">The card.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        public virtual ICard<V> Put(ICard<V> card)
        {
            return InnerPut(card);
        }
        /// <summary>
        /// Puts the specified cards.
        /// </summary>
        /// <param name="cards">The cards.</param>
        public virtual void Put(IList<ICard<V>> cards)
        {
            int i = 0, c = cards.Count;
            while(i < c)           
                InnerPut(cards[i++]);           
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
        /// Puts the specified cards.
        /// </summary>
        /// <param name="cards">The cards.</param>
        public virtual void Put(IList<V> cards)
        {
            int i = 0, c = cards.Count;
            while (i < c)
                InnerPut(cards[i++]);
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
        /// <param name="seed">The seed.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        public virtual ICard<V> Put(V value, ulong seed)
        {
            return InnerPut(value, seed);
        }
        /// <summary>
        /// Puts the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="seed">The seed.</param>
        public virtual void Put(object value, ulong seed)
        {
            if (value is IUnique)
            {
                IUnique v = (IUnique)value;
                Put(v, seed);
            }
            else if (value is V)
                Put((V)value, seed);
        }
        /// <summary>
        /// Puts the specified cards.
        /// </summary>
        /// <param name="cards">The cards.</param>
        /// <param name="seed">The seed.</param>
        public virtual void Put(IList<V> cards, ulong seed)
        {
            int c = cards.Count;
            for (int i = 0; i < c; i++)
            {
                InnerPut(cards[i], seed);
            }
        }
        /// <summary>
        /// Puts the specified cards.
        /// </summary>
        /// <param name="cards">The cards.</param>
        /// <param name="seed">The seed.</param>
        public virtual void Put(IEnumerable<V> cards, ulong seed)
        {
            foreach (V card in cards)
                InnerPut(card, seed);
        }
        /// <summary>
        /// Puts the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        public virtual ICard<V> Put(IUnique<V> value)
        {
            return InnerPut(unique.Key(value, value.UniqueSeed), value.UniqueObject);
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
        /// <param name="seed">The seed.</param>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        protected virtual bool InnerAdd(ulong key, ulong seed, V value)
        {
            value.UniqueSeed = seed;
            value.UniqueKey = key;
            return InnerAdd(value);
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
        /// <param name="seed">The seed.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        protected virtual bool InnerAdd(V value, ulong seed)
        {
            value.UniqueSeed = seed;
            return InnerAdd(value);
        }
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
            V o = (V)value;
            return InnerAdd(key, o.UniqueSeed, o);
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
            return InnerAdd(unique.Key(key, value.UniqueSeed), value);
        }
        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="seed">The seed.</param>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public virtual bool Add(object key, ulong seed, V value)
        {
            value.UniqueSeed = seed;
            return InnerAdd(unique.Key(key, seed), value);
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
        /// <param name="seed">The seed.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public virtual bool Add(V value, ulong seed)
        {
            return InnerAdd(value, seed);
        }
        /// <summary>
        /// Adds the specified cards.
        /// </summary>
        /// <param name="cards">The cards.</param>
        /// <param name="seed">The seed.</param>
        public virtual void Add(IList<V> cards, ulong seed)
        {
            int c = cards.Count;
            for (int i = 0; i < c; i++)
            {
                Add(cards[i], seed);

            }
        }
        /// <summary>
        /// Adds the specified cards.
        /// </summary>
        /// <param name="cards">The cards.</param>
        /// <param name="seed">The seed.</param>
        public virtual void Add(IEnumerable<V> cards, ulong seed)
        {
            foreach (V card in cards)
                Add(card, seed);
        }
        /// <summary>
        /// Adds the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        public virtual void Add(IUnique<V> value)
        {
            InnerAdd(unique.Key(value, value.UniqueSeed), value.UniqueObject);
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
        /// Tries the add.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="seed">The seed.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public virtual bool TryAdd(V value, ulong seed)
        {
            return InnerAdd(value, seed);
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
        /// <exception cref="System.NotSupportedException"></exception>
        public virtual ICard<V> New(object key)
        {
            if (key is IUnique)
            {
                IUnique ukey = (IUnique)key;
                ulong _key = unique.Key(ukey, ukey.UniqueSeed);
                ICard<V> newCard = NewCard(_key, default(V));
                if (InnerAdd(newCard))
                    return newCard;
                return null;
            }
            else
                throw new NotSupportedException();
           
        }
        /// <summary>
        /// News the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        public virtual ICard<V> New(object key, ulong seed)
        {
            ICard<V> newCard = NewCard(unique.Key(key, seed), default(V));
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
        /// Inserts the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="item">The item.</param>
        /// <exception cref="System.Logs.ILogSate.Exception">Item exist</exception>
        public virtual void Insert(int index, ICard<V> item)
        {
            
            ulong key = item.Key;
            ulong pos = getPosition(key);

            ICard<V> card = table[pos]; 
            
            if (card == null)
            {
                card = NewCard(item);
                table[pos] = card;
                InnerInsert(index, card);
                countIncrement();
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
                        conflictIncrement();
                        return;
                    }
                    throw new Exception("Item exist");

                }
                
                if (card.Extended == null)
                {
                    var newcard = NewCard(item);
                    card.Extended = newcard;
                    InnerInsert(index, newcard);
                    conflictIncrement();
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
            Insert(index, NewCard(item));
        }

        /// <summary>
        /// Enqueues the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public virtual bool Enqueue(V value)
        {
            return InnerAdd(value);
        }
        /// <summary>
        /// Enqueues the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public virtual bool Enqueue(object key, V value)
        {
            return Add(key, value);
        }
        /// <summary>
        /// Enqueues the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="seed">The seed.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public virtual bool Enqueue(V value, ulong seed)
        {
            return InnerAdd(value, seed);
        }
        /// <summary>
        /// Enqueues the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="seed">The seed.</param>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public virtual bool Enqueue(object key, ulong seed, V value)
        {
            return Add(key, seed, value);
        }
        /// <summary>
        /// Enqueues the specified card.
        /// </summary>
        /// <param name="card">The card.</param>
        public virtual void Enqueue(ICard<V> card)
        {
            InnerAdd(card);
        }

        /// <summary>
        /// Tries the pick.
        /// </summary>
        /// <param name="skip">The skip.</param>
        /// <param name="output">The output.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public virtual bool TryPick(int skip, out V output)
        {
            output = default(V);
            bool check = false;
            if (check = TryPick(skip, out ICard<V> _output))
                output = _output.Value;
            return check;
        }
        /// <summary>
        /// Tries the pick.
        /// </summary>
        /// <param name="skip">The skip.</param>
        /// <param name="output">The output.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public virtual bool TryPick(int skip, out ICard<V> output)
        {
            output = this.AsCards().Skip(skip).FirstOrDefault();
            if (output != null)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Dequeues this instance.
        /// </summary>
        /// <returns>V.</returns>
        public virtual V Dequeue()
        {
            var card = Next(first);
            if (card != null)
            {
                card.Removed = true;
                removedIncrement();
                first = card;
                return card.Value;
            }
            return default(V);
        }

        /// <summary>
        /// Tries the dequeue.
        /// </summary>
        /// <param name="output">The output.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public virtual bool TryDequeue(out V output)
        {
            output = default(V);
            if (count < mincount)
                return false;

            var card = Next(first);
            if (card != null)
            {
                card.Removed = true;
                removedIncrement();
                first = card;
                output = card.Value;
                return true;
            }
            return false;
        }
        /// <summary>
        /// Tries the dequeue.
        /// </summary>
        /// <param name="output">The output.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public virtual bool TryDequeue(out ICard<V> output)
        {
            output = null;
            if (count < mincount)
                return false;

            output = Next(first);
            if (output != null)
            {
                output.Removed = true;
                removedIncrement();
                first = output;
                return true;
            }
            return false;
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
        /// Renews the clear.
        /// </summary>
        /// <param name="capacity">The capacity.</param>
        protected virtual void renewClear(int capacity)
        {
            if (capacity != size || count > 0)
            {
                size = capacity;
                maxId = (uint)(capacity - 1);
                conflicts = 0;
                removed = 0;
                count = 0;
                table = EmptyCardTable(size);
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
            ICard<V> mem = table[getPosition(key)];

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
        /// <exception cref="System.NotSupportedException"></exception>
        public virtual bool ContainsKey(object key)
        {
            if (key is IUnique)
            {
                IUnique ukey = (IUnique)key;
                ulong _key = unique.Key(ukey, ukey.UniqueSeed);
                return InnerContainsKey(_key);
            }
            else
                throw new NotSupportedException();
           
        }
        /// <summary>
        /// Determines whether the specified key contains key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="seed">The seed.</param>
        /// <returns><c>true</c> if the specified key contains key; otherwise, <c>false</c>.</returns>
        public virtual bool ContainsKey(object key, ulong seed)
        {
            return InnerContainsKey(unique.Key(key, seed));
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
            return InnerContainsKey(unique.Key(key, key.UniqueSeed));
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
            return InnerContainsKey(unique.Key(item, item.UniqueSeed));
        }
        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1" /> contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        /// <returns><see langword="true" /> if <paramref name="item" /> is found in the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, <see langword="false" />.</returns>
        public virtual bool Contains(V item)
        {
            return InnerContainsKey(unique.Key(item, item.UniqueSeed));
        }
        /// <summary>
        /// Determines whether this instance contains the object.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="seed">The seed.</param>
        /// <returns><c>true</c> if [contains] [the specified item]; otherwise, <c>false</c>.</returns>
        public virtual bool Contains(V item, ulong seed)
        {
            return InnerContainsKey(unique.Key(item, seed));
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
        /// Inners the remove.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>V.</returns>
        protected virtual V InnerRemove(ulong key)
        {
            ICard<V> mem = table[getPosition(key)];

            while (mem != null)
            {
                if (mem.Equals(key))
                {
                    if (mem.Removed)
                        return default(V);

                    mem.Removed = true;
                    removedIncrement();
                    return mem.Value;
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
            ICard<V> mem = table[getPosition(key)];

            while (mem != null)
            {
                if (mem.Equals(key))
                {
                    if (mem.Removed)
                        return default(V);

                    if (ValueEquals(mem.Value, item))
                    {
                        mem.Removed = true;
                        removedIncrement();
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
            return InnerRemove(unique.Key(item, item.UniqueSeed)).Equals(default(V)) ? false : true;
        }
        /// <summary>
        /// Removes the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>V.</returns>
        /// <exception cref="System.NotSupportedException"></exception>
        public virtual    V Remove(object key)
        {
            if (key is IUnique)
            {
                IUnique ukey = (IUnique)key;
                ulong _key = unique.Key(ukey, ukey.UniqueSeed);
                return InnerRemove(_key);
            }
            else
                throw new NotSupportedException();          
        }
        /// <summary>
        /// Removes the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>V.</returns>
        public virtual    V Remove(object key, ulong seed)
        {
            return InnerRemove(unique.Key(key, seed));
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
            return TryRemove(unique.Key(item, item.UniqueSeed));
        }
        /// <summary>
        /// Tries the remove.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        /// <exception cref="System.NotSupportedException"></exception>
        public virtual bool TryRemove(object key)
        {
            if (key is IUnique)
            {
                IUnique ukey = (IUnique)key;
                ulong _key = unique.Key(ukey, ukey.UniqueSeed);
                V result = InnerRemove(unique.Key(key));
                if (result != null &&
                    !result.Equals(default(V)))
                    return true;
                return false;
            }
            else
                throw new NotSupportedException();          
        }
        /// <summary>
        /// Tries the remove.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="seed">The seed.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public virtual bool TryRemove(object key, ulong seed)
        {
            return InnerRemove(unique.Key(key, seed)).Equals(default(V)) ? false : true;
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
        /// Clears this instance.
        /// </summary>
        public virtual void Clear()
        {
            size = minSize;
            maxId = (uint)(size - 1);
            conflicts = 0;
            removed = 0;
            count = 0;
            table = EmptyCardTable(size);
            first = EmptyCard();
            last = first;
        }

        /// <summary>
        /// Flushes this instance.
        /// </summary>
        public virtual void Flush()
        {
            conflicts = 0;
            removed = 0;
            count = 0;
            table = null;
            table = EmptyCardTable(size);
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
        /// Converts to array.
        /// </summary>
        /// <returns>V[].</returns>
        public virtual V[] ToArray()
        {
            return this.AsValues().ToArray();
        }
        /// <summary>
        /// Converts to objectarray.
        /// </summary>
        /// <returns>System.Object[].</returns>
        public virtual object[] ToObjectArray()
        {
            return this.AsValues().Select((x) => (object)x).ToArray();
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
        public virtual void Resize(int size)
        {
            Rehash(size);
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
        /// <param name="seed">The seed.</param>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        public virtual ICard<V>  NewCard(ulong key, ulong seed, V value)
        {
            value.UniqueSeed = seed;
            value.UniqueKey = key;
            return NewCard(value);
        }
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
        /// <param name="key">The key.</param>
        /// <param name="seed">The seed.</param>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        public virtual ICard<V>  NewCard(object key, ulong seed, V value)
        {
            value.UniqueSeed = seed;
            return NewCard(unique.Key(key, seed), value);
        }
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
        /// <param name="seed">The seed.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        public virtual ICard<V>  NewCard(V card, ulong seed)
        {
            card.UniqueSeed = seed;
            return NewCard(card);
        }
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
        /// Indexes the of.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>System.Int32.</returns>
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
        /// Indexes the of.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="item">The item.</param>
        /// <returns>System.Int32.</returns>
        protected virtual int IndexOf(ulong key, V item)
        {
            var card = GetCard(key);
            if (ValueEquals(card.Value, item))
                return card.Index;
            return -1;
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
            foreach(ICard<V> card in this)
            {
                yield return card;
            }            
        }

        /// <summary>
        /// Gets the unique enumerator.
        /// </summary>
        /// <returns>IEnumerator&lt;IUnique&lt;V&gt;&gt;.</returns>
        public virtual IEnumerator<IUnique<V>> GetUniqueEnumerator()
        {
            return new CardKeySeries<V>(this);
        }

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns>IEnumerator&lt;ICard&lt;V&gt;&gt;.</returns>
        public virtual IEnumerator<ICard<V>> GetEnumerator()
        {
            return new CardSeries<V>(this);
        }

        /// <summary>
        /// Gets the key enumerator.
        /// </summary>
        /// <returns>IEnumerator&lt;System.UInt64&gt;.</returns>
        public virtual IEnumerator<ulong> GetKeyEnumerator()
        {
            return new CardUniqueKeySeries<V>(this);
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
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new CardSeries<V>(this);
        }

        /// <summary>
        /// Gets the position.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>System.UInt64.</returns>
        protected ulong getPosition(ulong key)
        {
            

            return (key % maxId);

            
            
            

            
        }
        /// <summary>
        /// Gets the position.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="tableMaxId">The table maximum identifier.</param>
        /// <returns>System.UInt64.</returns>
        protected static ulong getPosition(ulong key, uint tableMaxId)
        {
            

            return (key % tableMaxId);

            
            
            

            
        }

        /// <summary>
        /// Rehashes the specified new size.
        /// </summary>
        /// <param name="newSize">The new size.</param>
        protected virtual void Rehash(int newSize)
        {
            int finish = count;
            int newsize = newSize;
            uint newMaxId = (uint)(newsize - 1);
            ICard<V>[] newCardTable = EmptyCardTable(newsize);
            ICard<V> card = first;
            card = card.Next;
            if (removed > 0)
            {
                rehashAndReindex(card, newCardTable, newMaxId);
            }
            else
            {
                rehash(card, newCardTable, newMaxId);
            }

            table = newCardTable;
            maxId = newMaxId;
            size = newsize;
        }

        /// <summary>
        /// Rehashes the and reindex.
        /// </summary>
        /// <param name="card">The card.</param>
        /// <param name="newCardTable">The new card table.</param>
        /// <param name="newMaxId">The new maximum identifier.</param>
        private void rehashAndReindex(ICard<V> card, ICard<V>[] newCardTable, uint newMaxId)
        {
            int _conflicts = 0;
            uint _newMaxId = newMaxId;
            ICard<V>[] _newCardTable = newCardTable;
            ICard<V> _firstcard = EmptyCard();
            ICard<V> _lastcard = _firstcard;
            do
            {
                if (!card.Removed)
                {
                    ulong pos = getPosition(card.Key, _newMaxId);

                    ICard<V> mem = _newCardTable[pos];

                    if (mem == null)
                    {
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

                card = card.Next;

            } while (card != null);

            conflicts = _conflicts;
            removed = 0;
            first = _firstcard;
            last = _lastcard;
        }

        /// <summary>
        /// Rehashes the specified card.
        /// </summary>
        /// <param name="card">The card.</param>
        /// <param name="newCardTable">The new card table.</param>
        /// <param name="newMaxId">The new maximum identifier.</param>
        private void rehash(ICard<V> card, ICard<V>[] newCardTable, uint newMaxId)
        {
            int _conflicts = 0;
            uint _newMaxId = newMaxId;
            ICard<V>[] _newCardTable = newCardTable;
            do
            {
                if (!card.Removed)
                {
                    ulong pos = getPosition(card.Key, _newMaxId);

                    ICard<V> mem = _newCardTable[pos];

                    if (mem == null)
                    {
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
            conflicts = _conflicts;
        }

        /// <summary>
        /// The disposed value
        /// </summary>
        protected bool disposedValue = false;

        /// <summary>
        /// The mass deck implementation
        /// </summary>
        private IMassDeck<V> massDeckImplementation;

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
                table = null;

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
        public virtual ulong UniqueKey
        {
            get => serialcode.UniqueKey;
            set => serialcode.UniqueKey = value;
        }

        /// <summary>
        /// Gets or sets the unique seed.
        /// </summary>
        /// <value>The unique seed.</value>
        public virtual ulong UniqueSeed
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
    }  
}
