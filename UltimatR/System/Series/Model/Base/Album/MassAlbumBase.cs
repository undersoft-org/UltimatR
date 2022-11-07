
// <copyright file="MassAlbumBase.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The Series namespace.
/// </summary>
namespace System.Series
{
    using System.Collections.Generic;
    using System.Series.Basedeck;
    using System.Uniques;

    /// <summary>
    /// Class MassAlbumBase.
    /// Implements the <see cref="System.Series.Basedeck.SeededSet{V}" />
    /// </summary>
    /// <typeparam name="V"></typeparam>
    /// <seealso cref="System.Series.Basedeck.SeededSet{V}" />
    public abstract class MassAlbumBase<V> : SeededSet<V> where V : IUnique
    {
        #region Fields

        /// <summary>
        /// The list
        /// </summary>
        protected ICard<V>[] list;
        /// <summary>
        /// The repeatable
        /// </summary>
        protected bool repeatable;
        /// <summary>
        /// The repeated
        /// </summary>
        protected int repeated;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MassAlbumBase{V}" /> class.
        /// </summary>
        public MassAlbumBase() : this(17, HashBits.bit64)
        {          
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="MassAlbumBase{V}" /> class.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="capacity">The capacity.</param>
        /// <param name="repeatable">if set to <c>true</c> [repeatable].</param>
        /// <param name="bits">The bits.</param>
        public MassAlbumBase(IEnumerable<IUnique<V>> collection, int capacity = 17, bool repeatable = false, HashBits bits = HashBits.bit64) : this(repeatable, capacity, bits)
        {            
            foreach(var c in collection)
                this.Add(c);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="MassAlbumBase{V}" /> class.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="capacity">The capacity.</param>
        /// <param name="repeatable">if set to <c>true</c> [repeatable].</param>
        /// <param name="bits">The bits.</param>
        public MassAlbumBase(IEnumerable<V> collection, int capacity = 17, bool repeatable = false, HashBits bits = HashBits.bit64) : this(repeatable, capacity, bits)
        {            
            foreach(var c in collection)
                this.Add(c);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="MassAlbumBase{V}" /> class.
        /// </summary>
        /// <param name="repeatable">if set to <c>true</c> [repeatable].</param>
        /// <param name="capacity">The capacity.</param>
        /// <param name="bits">The bits.</param>
        public MassAlbumBase(bool repeatable, int capacity = 17, HashBits bits = HashBits.bit64) : base(capacity, bits)
        {
            this.repeatable = repeatable;
            list = EmptyDeck(capacity);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="MassAlbumBase{V}" /> class.
        /// </summary>
        /// <param name="capacity">The capacity.</param>
        /// <param name="bits">The bits.</param>
        public MassAlbumBase(int capacity = 17, HashBits bits = HashBits.bit64) : base(capacity, bits)
        {
            list = EmptyDeck(capacity);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the first.
        /// </summary>
        /// <value>The first.</value>
        public override ICard<V> First
        {
            get { return first; }
        }

        /// <summary>
        /// Gets the last.
        /// </summary>
        /// <value>The last.</value>
        public override ICard<V> Last
        {
            get { return list[(count + removed) - 1]; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is repeatable.
        /// </summary>
        /// <value><c>true</c> if this instance is repeatable; otherwise, <c>false</c>.</value>
        public override bool IsRepeatable { get => repeatable; }

        #endregion

        #region Methods

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public override void Clear()
        {
            base.Clear();
            list = EmptyDeck(minSize);
        }

        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="index">The index.</param>
        public override void CopyTo(Array array, int index)
        {
            int c = count, i = index, l = array.Length;

            if(l - i < c) c = l - i;

            for(int j = 0; j < c; j++)
                array.SetValue(GetCard(j).Value, i++);
        }

        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="index">The index.</param>
        public override void CopyTo(ICard<V>[] array, int index)
        {
            int c = count, i = index, l = array.Length;

            if(l - i < c) c = l - i;

            for(int j = 0; j < c; j++)
            {
                array[i++] = GetCard(j);
            }
        }

        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="index">The index.</param>
        public override void CopyTo(V[] array, int index)
        {
            int c = count, i = index, l = array.Length;

            if(l - i < c) c = l - i;

            for(int j = 0; j < c; j++)
                array[i++] = GetCard(j).Value;
        }

        /// <summary>
        /// Empties the deck.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <returns>ICard&lt;V&gt;[].</returns>
        public abstract ICard<V>[] EmptyDeck(int size);

        /// <summary>
        /// Flushes this instance.
        /// </summary>
        public override void Flush()
        {
            base.Flush();
            list = EmptyDeck(size);
        }

        /// <summary>
        /// Gets the card.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="item">The item.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        protected virtual ICard<V> GetCard(ulong key, V item)
        {
            if (key == 0)
                return null;

            ICard<V> mem = table[getPosition(key)];

            while (mem != null)
            {
                if (mem.Equals(key))
                {
                    if (repeatable)
                        while (mem != null ||
                            !ValueEquals(mem.Value, item))
                            mem = mem.Next;

                    if (!mem.Removed)
                        return mem;
                    return null;
                }
                mem = mem.Extended;
            }

            return mem;
        }
        /// <summary>
        /// Gets the card.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        /// <exception cref="System.IndexOutOfRangeException">Index out of range</exception>
        public override   ICard<V> GetCard(int index)
        {
            if(index < count)
            {
                if(removed > 0)
                    Reindex();

                return list[index];
            }
            throw new IndexOutOfRangeException("Index out of range");
        }

        /// <summary>
        /// Nexts the specified card.
        /// </summary>
        /// <param name="card">The card.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        public override ICard<V> Next(ICard<V> card)
        {
            ICard<V> _card = list[card.Index + 1];
            if(_card != null)
            {
                if(!_card.Removed)
                    return _card;
                return Next(_card);
            }
            return null;
        }

        /// <summary>
        /// Converts to array.
        /// </summary>
        /// <returns>V[].</returns>
        public override V[] ToArray()
        {
            V[] array = new V[count];
            CopyTo(array, 0);
            return array;
        }

        /// <summary>
        /// Creates the new.
        /// </summary>
        /// <param name="card">The card.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        protected ICard<V> createNew(ICard<V> card)
        {
            int id = count + removed;
            card.Index = id;           
            list[id] = card;
            return card;
        }
        /// <summary>
        /// Creates the new.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        protected ICard<V> createNew(ulong key, V value)
        {
            int id = count + removed;
            var newcard = NewCard(key, value);
            newcard.Index = id;           
            list[id] = newcard;
            return newcard;
        }

        /// <summary>
        /// Creates the repeated.
        /// </summary>
        /// <param name="card">The card.</param>
        /// <param name="value">The value.</param>
        protected void createRepeated(ICard<V> card, V value)
        {
            var _card = createNew(card.Key, card.Value);
            card.Value = value;
            _card.Next = card.Next;
            card.Next = _card;
            _card.Repeated = true;
        }
        /// <summary>
        /// Creates the repeated.
        /// </summary>
        /// <param name="card">The card.</param>
        /// <param name="newcard">The newcard.</param>
        protected void createRepeated(ICard<V> card, ICard<V> newcard)
        {
            var _card = createNew(newcard);
            var val = card.Value;
            card.Value = _card.Value;
            _card.Value = val;
            _card.Next = card.Next;
            card.Next = _card;
            _card.Repeated = true;
        }

        /// <summary>
        /// Swaps the repeated.
        /// </summary>
        /// <param name="card">The card.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        protected ICard<V> swapRepeated(ICard<V> card)
        {
            var value = card.Value;
            var _card = card.Next;
            card.Value = _card.Value;
            _card.Value = value;
            card.Next = _card.Next;
            _card.Next = _card;
            return _card;
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if(!disposedValue)
            {
                if(disposing)
                {
                    first = null;
                    last = null;
                }
                table = null;
                list = null;

                disposedValue = true;
            }
        }

        /// <summary>
        /// Inners the add.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        protected override bool InnerAdd(ICard<V> value)
        {
            
            ulong key = value.Key;
            ulong pos = getPosition(key);

            ICard<V> card = table[pos]; 
            
            if (card == null)
            {
                table[pos] = createNew(value);
                countIncrement();
                return true;
            }

            
            for (; ; )
            {
                
                if (card.Equals(key))
                {
                    
                    if (card.Removed)
                    {
                        
                        card.Removed = false;
                        card.Value = value.Value;
                        removedDecrement();
                        return true;
                    }
                    
                    if (repeatable)
                    {
                        createRepeated(card, value);
                        countIncrement();
                        return true;
                    }
                    return false;
                }
                
                if (card.Extended == null)
                {
                    card.Extended = createNew(value);
                    conflictIncrement();
                    return true;
                }
                card = card.Extended;
            }
        }
        /// <summary>
        /// Inners the add.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        protected override bool InnerAdd(ulong key, V value)
        {
            
            ulong pos = getPosition(key);

            ICard<V> card = table[pos];
            
            if (card == null)
            {
                table[pos] = createNew(key, value);
                countIncrement();
                return true;
            }
            
            for (; ; )
            {
                
                if (card.Equals(key))
                {
                    
                    if (card.Removed)
                    {
                        
                        card.Removed = false;
                        card.Value = value;
                        removedDecrement();
                        return true;
                    }
                    
                    if (repeatable)
                    {
                        createRepeated(card, value);
                        countIncrement();
                        return true;
                    }

                    return false;
                }
                
                if (card.Extended == null)
                {
                    
                    card.Extended = createNew(key, value);
                    conflictIncrement();
                    return true;
                }
                
                card = card.Extended;
            }
        }
        /// <summary>
        /// Inners the add.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        protected override bool InnerAdd(V value)
        {
            ulong key = unique.Key(value, value.UniqueSeed);
            
            ulong pos = getPosition(key);
            ICard<V> card = table[pos];
            
            if (card == null)
            {
                table[pos] = createNew(key, value);
                countIncrement();
                return true;
            }
            
            for (; ; )
            {
                
                if (card.Equals(key))
                {
                    
                    if (card.Removed)
                    {
                        
                        card.Removed = false;
                        card.Value = value;
                        removedDecrement();
                        return true;
                    }
                    
                    if (repeatable)
                    {
                        createRepeated(card, value);
                        countIncrement();
                        return true;
                    }

                    return false;
                }
                
                if (card.Extended == null)
                {
                    
                    card.Extended = createNew(key, value);
                    conflictIncrement();
                    return true;
                }
                
                card = card.Extended;
            }
        }

        /// <summary>
        /// Inners the insert.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="item">The item.</param>
        protected override void InnerInsert(int index, ICard<V> item)
        {

            int c = count - index;
            if(c > 0)
            {
                
                if(removed > 0)
                    reindexWithInsert(index, item);
                else
                {

                    var replaceCard = GetCard(index);

                    while(replaceCard != null)
                    {
                        int id = ++replaceCard.Index;
                        var _replaceCard = list[id];
                        list[id] = replaceCard;
                        replaceCard = _replaceCard;
                    }

                    item.Index = index;
                    list[index] = item;
                }
            }
            else
            {
                
                int id = count + removed;
                item.Index = id;
                list[id] = item;
            }
        }

        /// <summary>
        /// Inners the put.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        protected override ICard<V> InnerPut(ICard<V> value)
        {
            ulong key = value.Key;
            
            ulong pos = getPosition(key);

            ICard<V> card = table[pos]; 
            
            if(card == null)
            {
                card = createNew(value);
                table[pos] = card;
                countIncrement();
                return card;
            }

            
            for(; ; )
            {
                
                if(card.Equals(key))
                {
                    
                    card.Value = value.Value;
                    
                    if(card.Removed)
                    {
                        card.Removed = false;
                        removedDecrement();
                    }
                    return card;
                }
                
                if(card.Extended == null)
                {
                    var newcard = createNew(value);
                    card.Extended = newcard;
                    conflictIncrement();
                    return newcard;
                }
                card = card.Extended;
            }
        }
        /// <summary>
        /// Inners the put.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        protected override ICard<V> InnerPut(ulong key, V value)
        {
            ulong pos = getPosition(key);
            ICard<V> card = table[pos]; 
            
            if(card == null)
            {
                card = createNew(key, value);
                table[pos] = card;
                countIncrement();
                return card;
            }

            
            for(; ; )
            {
                
                if(card.Equals(key))
                {
                    
                    card.Value = value;
                    if(card.Removed)
                    {
                        card.Removed = false;
                        removedDecrement();
                    }
                    

                    return card;
                }
                
                if(card.Extended == null)
                {
                    var newcard = createNew(key, value);
                    card.Extended = newcard;
                    conflictIncrement();
                    return newcard;
                }
                
                card = card.Extended;
            }
        }
        /// <summary>
        /// Inners the put.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        protected override ICard<V> InnerPut(V value)
        {
            ulong key = unique.Key(value, value.UniqueSeed);
            ulong pos = getPosition(key);
            ICard<V> card = table[pos]; 
            
            if(card == null)
            {
                card = createNew(key, value);
                table[pos] = card;
                countIncrement();
                return card;
            }

            
            for(; ; )
            {
                
                if(card.Equals(key))
                {
                    
                    card.Value = value;
                    if(card.Removed)
                    {
                        card.Removed = false;
                        removedDecrement();
                    }
                    

                    return card;
                }
                
                if(card.Extended == null)
                {
                    var newcard = createNew(key, value);
                    card.Extended = newcard;
                    conflictIncrement();
                    return newcard;
                }
                
                card = card.Extended;
            }
        }

        /// <summary>
        /// Inners the remove.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>V.</returns>
        protected override V InnerRemove(ulong key)
        {
            ICard<V> _card = table[getPosition(key)];

            while (_card != null)
            {
                if (_card.Equals(key))
                {
                    if (_card.Removed)
                        return default(V);

                    if (repeatable && _card.Next != null)
                        _card = swapRepeated(_card);

                    _card.Removed = true;
                    removedIncrement();
                    return _card.Value;
                }
                _card = _card.Extended;
            }
            return default(V);
        }
        /// <summary>
        /// Inners the remove.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="item">The item.</param>
        /// <returns>V.</returns>
        protected override V InnerRemove(ulong key, V item)
        {
            ICard<V> _card = table[getPosition(key)];

            while (_card != null)
            {
                if (_card.Equals(key))
                {
                    if (_card.Removed)
                        return default(V);
                    do
                    {
                        if (ValueEquals(_card.Value, item))
                        {
                            if (_card.Next != null)
                                _card = swapRepeated(_card);

                            _card.Removed = true;
                            removedIncrement();
                            return _card.Value;
                        }
                        _card = _card.Next;
                    }
                    while (_card != null);
                    return default(V);
                }
                _card = _card.Extended;
            }
            return default(V);
        }

        /// <summary>
        /// Tries the remove.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="item">The item.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public virtual bool TryRemove(ulong key, V item)
        {
            var output = InnerRemove(key, item);
            return (output != null) ? true : false;
        }

        /// <summary>
        /// Indexes the of.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>System.Int32.</returns>
        public override int IndexOf(V item)
        {
            return IndexOf(unique.Key(item), item);
        }
        /// <summary>
        /// Indexes the of.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="item">The item.</param>
        /// <returns>System.Int32.</returns>
        protected override int IndexOf(ulong key, V item)
        {
            if (!repeatable)
                return base.IndexOf(key, item);

            var card = GetCard(key);
            if (card == null)
                return -1;

            do
            {
                if (ValueEquals(card.Value, item))
                    return card.Index;

                card = card.Next;
            }
            while (card != null);

            return -1;
        }

        /// <summary>
        /// Determines whether this instance contains the object.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns><c>true</c> if [contains] [the specified item]; otherwise, <c>false</c>.</returns>
        public override bool Contains(ICard<V> item)
        {
            return IndexOf(item.Key, item.Value) > -1;
        }
        /// <summary>
        /// Determines whether this instance contains the object.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns><c>true</c> if [contains] [the specified item]; otherwise, <c>false</c>.</returns>
        public override bool Contains(IUnique<V> item)
        {
            return IndexOf(unique.Key(item), item.UniqueObject) > -1;
        }
        /// <summary>
        /// Determines whether this instance contains the object.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns><c>true</c> if [contains] [the specified item]; otherwise, <c>false</c>.</returns>
        public override bool Contains(V item)
        {
            return IndexOf(item) > -1;
        }
        /// <summary>
        /// Determines whether this instance contains the object.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="item">The item.</param>
        /// <returns><c>true</c> if [contains] [the specified key]; otherwise, <c>false</c>.</returns>
        public override bool Contains(ulong key, V item)
        {
            return IndexOf(key, item) > -1;
        }

        /// <summary>
        /// Dequeues this instance.
        /// </summary>
        /// <returns>V.</returns>
        public override V Dequeue()
        {
            var _output = Next(first);
            if (_output == null)
                return default(V);

            if (repeatable && _output.Next != null)
                _output = swapRepeated(_output);
            else
                first = _output;

            _output.Removed = true;
            removedIncrement();
            return _output.Value;
        }

        /// <summary>
        /// Tries the dequeue.
        /// </summary>
        /// <param name="output">The output.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public override bool TryDequeue(out V output)
        {
            output = default(V);
            if (count < mincount)
                return false;

            var _output = Next(first);
            if (_output == null)
                return false;

            if (repeatable && _output.Next != null)
                _output = swapRepeated(_output);
            else
                first = _output;

            _output.Removed = true;
            removedIncrement();
            output = _output.Value;
            return true;
        }
        /// <summary>
        /// Tries the dequeue.
        /// </summary>
        /// <param name="output">The output.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public override bool TryDequeue(out ICard<V> output)
        {
            output = null;
            if (count < mincount)
                return false;

            output = Next(first);
            if (output == null)
                return false;

            if (repeatable && output.Next != null)
                output = swapRepeated(output);
            else
                first = output;

            output.Removed = true;
            removedIncrement();
            return true;
        }

        /// <summary>
        /// Rehashes the specified newsize.
        /// </summary>
        /// <param name="newsize">The newsize.</param>
        protected override void Rehash(int newsize)
        {
            int finish = count;
            int _newsize = newsize; 
            uint newMaxId = (uint)(_newsize - 1);
            ICard<V>[] newCardTable = EmptyCardTable(_newsize);
            if(removed != 0)
            {
                ICard<V>[] newBaseDeck = EmptyDeck(_newsize);
                rehashAndReindex(newCardTable, newBaseDeck, newMaxId);
                list = newBaseDeck;
            }
            else
            {
                ICard<V>[] newBaseDeck = EmptyDeck(_newsize);
                rehash(newCardTable, newMaxId);
                Array.Copy(list, 0, newBaseDeck, 0, finish);
                list = newBaseDeck;
            }
            table = newCardTable;
            maxId = newMaxId;
            size = newsize;
        }

        /// <summary>
        /// Reindexes this instance.
        /// </summary>
        protected virtual void Reindex()
        {
            ICard<V> card = null;
            first = EmptyCard();
            int total = count + removed;
            int _counter = 0;
            for(int i = 0; i < total; i++)
            {
                card = list[i];
                if(card != null && !card.Removed)
                {
                    card.Index = _counter;
                    list[_counter++] = card;
                }

            }
            removed = 0;
        }

        /// <summary>
        /// Renews the clear.
        /// </summary>
        /// <param name="capacity">The capacity.</param>
        protected override void renewClear(int capacity)
        {
            if(capacity != size || count > 0)
            {
                size = capacity;
                maxId = (uint)(capacity - 1);
                conflicts = 0;
                removed = 0;
                count = 0;
                table = EmptyCardTable(size);
                list = EmptyDeck(size);
                first = EmptyCard();
                last = first;
            }
        }

        /// <summary>
        /// Rehashes the specified new card table.
        /// </summary>
        /// <param name="newCardTable">The new card table.</param>
        /// <param name="newMaxId">The new maximum identifier.</param>
        private void rehash(ICard<V>[] newCardTable, uint newMaxId)
        {
            int _conflicts = 0;
            int total = count + removed;
            uint _newMaxId = newMaxId;
            ICard<V>[] _newCardTable = newCardTable;
            ICard<V> card = null;
            ICard<V> mem = null;
            for (int i = 0; i < total; i++)
            {
                card = list[i];

                if (card == null || card.Removed ||
                     (repeatable && card.Repeated))
                         continue;

                ulong pos = getPosition(card.Key, _newMaxId);

                mem = _newCardTable[pos];

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
                        mem = mem.Extended;
                    }
                }
            }
            conflicts = _conflicts;
        }

        /// <summary>
        /// Rehashes the and reindex.
        /// </summary>
        /// <param name="newCardTable">The new card table.</param>
        /// <param name="newBaseDeck">The new base deck.</param>
        /// <param name="newMaxId">The new maximum identifier.</param>
        private void rehashAndReindex(ICard<V>[] newCardTable, ICard<V>[] newBaseDeck, uint newMaxId)
        {
            int _conflicts = 0;
            int _counter = 0;
            int total = count + removed;
            uint _newMaxId = newMaxId;
            ICard<V>[] _newCardTable = newCardTable;
            ICard<V>[] _newBaseDeck = newBaseDeck;
            ICard<V> card = null;
            ICard<V> mem = null;
            for (int i = 0; i < total; i++)
            {
                card = list[i];

                if (card == null ||
                    card.Removed) 
                    continue;

                if (card.Repeated)
                {
                    card.Index = _counter;
                    _newBaseDeck[_counter++] = card;
                    continue;
                }

                ulong pos = getPosition(card.Key, _newMaxId);

                mem = _newCardTable[pos];

                if (mem == null)
                {
                    card.Extended = null;
                    card.Index = _counter;
                    _newCardTable[pos] = card;
                    _newBaseDeck[_counter++] = card;
                }
                else
                {
                    for (; ; )
                    {
                        if (mem.Extended == null)
                        {
                            card.Extended = null;
                            mem.Extended = card;
                            card.Index = _counter;
                            _newBaseDeck[_counter++] = card;
                            _conflicts++;
                            break;
                        }
                        mem = mem.Extended;
                    }
                }

            }
            first = EmptyCard();
            conflicts = _conflicts;
            removed = 0;
        }

        /// <summary>
        /// Reindexes the with insert.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="item">The item.</param>
        private void reindexWithInsert(int index, ICard<V> item)
        {
            ICard<V> card = null;
            first = EmptyCard();
            int _counter = 0;
            int total = count + removed;
            for(int i = 0; i < total; i++)
            {
                card = list[i];
                if(card != null && !card.Removed)
                {
                    card.Index = _counter;
                    list[_counter++] = card;
                    if(_counter == index)
                    {
                        item.Index = _counter;
                        list[_counter++] = item;
                    }
                }

            }
            removed = 0;
        }

        #endregion
    }
}
