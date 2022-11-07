
// <copyright file="DeckBase.cs" company="UltimatR.Core">
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
    /// Class DeckBase.
    /// Implements the <see cref="System.Series.Basedeck.KeyedSet{V}" />
    /// </summary>
    /// <typeparam name="V"></typeparam>
    /// <seealso cref="System.Series.Basedeck.KeyedSet{V}" />
    public abstract class DeckBase<V> : KeyedSet<V>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DeckBase{V}" /> class.
        /// </summary>
        protected DeckBase() : base()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="DeckBase{V}" /> class.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="capacity">The capacity.</param>
        /// <param name="bits">The bits.</param>
        protected DeckBase(IEnumerable<IUnique<V>> collection, int capacity = 17, HashBits bits = HashBits.bit64) : base(capacity, bits)
        {
            foreach (var c in collection)
                this.Add(c);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="DeckBase{V}" /> class.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="capacity">The capacity.</param>
        /// <param name="bits">The bits.</param>
        protected DeckBase(IEnumerable<V> collection, int capacity = 17, HashBits bits = HashBits.bit64) : base(capacity, bits)
        {
            foreach (var c in collection)
                this.Add(c);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="DeckBase{V}" /> class.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="capacity">The capacity.</param>
        /// <param name="bits">The bits.</param>
        protected DeckBase(IList<IUnique<V>> collection, int capacity = 17, HashBits bits = HashBits.bit64) : base(capacity > collection.Count ? capacity : collection.Count, bits)
        {
            foreach (var c in collection)
                this.Add(c);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="DeckBase{V}" /> class.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="capacity">The capacity.</param>
        /// <param name="bits">The bits.</param>
        protected DeckBase(IList<V> collection, int capacity = 17, HashBits bits = HashBits.bit64) : base(capacity > collection.Count ? capacity : collection.Count, bits)
        {
            foreach (var c in collection)
                this.Add(c);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="DeckBase{V}" /> class.
        /// </summary>
        /// <param name="capacity">The capacity.</param>
        /// <param name="bits">The bits.</param>
        protected DeckBase(int capacity = 17, HashBits bits = HashBits.bit64) : base(capacity, bits)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the card.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        public override ICard<V> GetCard(int index)
        {
            if (index < count)
            {
                if (removed > 0)
                    Reindex();

                int i = -1;
                int id = index;
                var card = first.Next;
                for (; ; )
                {
                    if (++i == id)
                        return card;
                    card = card.Next;
                }
            }
            return null;
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
            ulong key = unique.Key(value);
            
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
            if (index < count - 1)
            {
                if (index == 0)
                {
                    item.Index = 0;
                    item.Next = first.Next;
                    first.Next = item;
                }
                else
                {

                    ICard<V> prev = GetCard(index - 1);
                    ICard<V> next = prev.Next;
                    prev.Next = item;
                    item.Next = next;
                    item.Index = index;
                }
            }
            else
            {
                last = last.Next = item;
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
            
            if (card == null)
            {
                card = createNew(value);
                table[pos] = card;
                countIncrement();
                return card;
            }

            
            for (; ; )
            {
                
                if (card.Equals(key))
                {
                    
                    if (card.Removed)
                    {
                        card.Removed = false;
                        removedDecrement();
                    }
                    
                    card.Value = value.Value;
                    return card;
                }
                
                if (card.Extended == null)
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
            
            if (card == null)
            {
                card = createNew(key, value);
                table[pos] = card;
                countIncrement();
                return card;
            }

            
            for (; ; )
            {
                
                if (card.Equals(key))
                {
                    
                    if (card.Removed)
                    {
                        card.Removed = false;
                        removedDecrement();
                    }
                    
                    card.Value = value;
                    return card;
                }
                
                if (card.Extended == null)
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
            ulong key = unique.Key(value);
            
            ulong pos = getPosition(key);

            ICard<V> card = table[pos]; 
            
            if (card == null)
            {
                card = createNew(key, value);
                table[pos] = card;
                countIncrement();
                return card;
            }

            
            for (; ; )
            {
                
                if (card.Equals(key))
                {
                    
                    if (card.Removed)
                    {
                        card.Removed = false;
                        removedDecrement();
                    }
                    
                    card.Value = value;
                    return card;
                }
                
                if (card.Extended == null)
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
        /// Reindexes this instance.
        /// </summary>
        protected virtual void Reindex()
        {
            ICard<V> _firstcard = EmptyCard();
            ICard<V> _lastcard = _firstcard;
            ICard<V> card = first.Next;
            do
            {
                if (!card.Removed)
                {
                    _lastcard = _lastcard.Next = card;
                }

                card = card.Next;

            } while (card != null);
            removed = 0;
            first = _firstcard;
            last = _lastcard;
        }

        /// <summary>
        /// Creates the new.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        private ICard<V> createNew(ICard<V> value)
        {
            last.Next = value;
            last = value;
            return value;
        }

        /// <summary>
        /// Creates the new.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        private ICard<V> createNew(ulong key, V value)
        {
            var newcard = NewCard(key, value);      
            last.Next = newcard;
            last = newcard;
            return newcard;
        }

        #endregion
    }
}
