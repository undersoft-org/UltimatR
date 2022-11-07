
// <copyright file="CardBase.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The Series namespace.
/// </summary>
namespace System.Series
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Uniques;

    /// <summary>
    /// Class CardBase.
    /// Implements the <see cref="System.Series.ICard{V}" />
    /// </summary>
    /// <typeparam name="V"></typeparam>
    /// <seealso cref="System.Series.ICard{V}" />
    [StructLayout(LayoutKind.Sequential)]
    public abstract class CardBase<V> : ICard<V>
    {
        #region Fields

        /// <summary>
        /// The value
        /// </summary>
        protected V value;
        /// <summary>
        /// The is unique
        /// </summary>
        protected bool? isUnique;
        /// <summary>
        /// The disposed value
        /// </summary>
        private bool disposedValue = false;
        /// <summary>
        /// The extended
        /// </summary>
        private ICard<V> extended;
        /// <summary>
        /// The next
        /// </summary>
        private ICard<V> next;
        /// <summary>
        /// The deck
        /// </summary>
        private IDeck<V> deck;


        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CardBase{V}" /> class.
        /// </summary>
        public CardBase()
        {            
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="CardBase{V}" /> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public CardBase(ICard<V> value) : base()
        {
            Set(value);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="CardBase{V}" /> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public CardBase(object key, V value) : base()
        {
            Set(key, value);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="CardBase{V}" /> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public CardBase(ulong key, V value) : base()
        {
            Set(key, value);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="CardBase{V}" /> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public CardBase(V value) : base()
        {
            Set(value);            
        }

        #endregion

        #region Properties       

        /// <summary>
        /// Gets the empty.
        /// </summary>
        /// <value>The empty.</value>
        /// <exception cref="System.NotImplementedException"></exception>
        public virtual     IUnique Empty => throw new NotImplementedException();

        /// <summary>
        /// Gets or sets the extended.
        /// </summary>
        /// <value>The extended.</value>
        public virtual ICard<V> Extended { get => extended; set => extended = value; }

        /// <summary>
        /// Gets or sets the index.
        /// </summary>
        /// <value>The index.</value>
        public virtual         int Index { get; set; } = -1;

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public abstract        ulong Key { get; set; }

        /// <summary>
        /// Gets or sets the next.
        /// </summary>
        /// <value>The next.</value>
        public virtual     ICard<V> Next { get => next; set => next = value; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="CardBase{V}" /> is removed.
        /// </summary>
        /// <value><c>true</c> if removed; otherwise, <c>false</c>.</value>
        public virtual      bool Removed { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="CardBase{V}" /> is repeated.
        /// </summary>
        /// <value><c>true</c> if repeated; otherwise, <c>false</c>.</value>
        public virtual     bool Repeated { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is unique.
        /// </summary>
        /// <value><c>true</c> if this instance is unique; otherwise, <c>false</c>.</value>
        public virtual     bool IsUnique 
        { 
            get => isUnique ??= typeof(V).IsAssignableTo(typeof(IUnique)); 
            set => isUnique = value; 
        }

        /// <summary>
        /// Gets or sets the unique key.
        /// </summary>
        /// <value>The unique key.</value>
        public virtual   ulong UniqueKey { get => Key; set => Key = value; }

        /// <summary>
        /// Gets or sets the unique object.
        /// </summary>
        /// <value>The unique object.</value>
        public virtual    V UniqueObject
        {
            get => value;
            set => this.value = value;
        }

        /// <summary>
        /// Gets or sets the unique seed.
        /// </summary>
        /// <value>The unique seed.</value>
        public virtual ulong  UniqueSeed
        {
            get
            {
                if (IsUnique)
                {
                    var uniqueValue = (IUnique)UniqueObject;
                    if (uniqueValue.UniqueSeed == 0)
                        uniqueValue.UniqueSeed = typeof(V).UniqueKey32();
                    return uniqueValue.UniqueSeed;
                }
                return typeof(V).UniqueKey32();
            }
            set
            {
                if (IsUnique)
                {
                    var uniqueValue = (IUnique)UniqueObject;
                    uniqueValue.UniqueSeed = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public virtual          V Value { get => value; set => this.value = value; }

        #endregion

        #region Methods

        /// <summary>
        /// Compacts the key.
        /// </summary>
        /// <returns>System.UInt64.</returns>
        public virtual ulong CompactKey()
        {
            return (IsUnique) ? ((IUnique)UniqueObject).UniqueKey : Key;
        }

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="other">An object to compare with this instance.</param>
        /// <returns>A value that indicates the relative order of the objects being compared. The return value has these meanings:
        /// <list type="table"><listheader><term> Value</term><description> Meaning</description></listheader><item><term> Less than zero</term><description> This instance precedes <paramref name="other" /> in the sort order.</description></item><item><term> Zero</term><description> This instance occurs in the same position in the sort order as <paramref name="other" />.</description></item><item><term> Greater than zero</term><description> This instance follows <paramref name="other" /> in the sort order.</description></item></list></returns>
        public virtual int CompareTo(ICard<V> other)
        {
            return (int)(Key - other.Key);
        }

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="other">An object to compare with this instance.</param>
        /// <returns>A value that indicates the relative order of the objects being compared. The return value has these meanings:
        /// <list type="table"><listheader><term> Value</term><description> Meaning</description></listheader><item><term> Less than zero</term><description> This instance precedes <paramref name="other" /> in the sort order.</description></item><item><term> Zero</term><description> This instance occurs in the same position in the sort order as <paramref name="other" />.</description></item><item><term> Greater than zero</term><description> This instance follows <paramref name="other" /> in the sort order.</description></item></list></returns>
        public virtual int CompareTo(IUnique other)
        {
            return (int)(Key - other.UniqueKey);
        }

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="other">An object to compare with this instance.</param>
        /// <returns>A value that indicates the relative order of the objects being compared. The return value has these meanings:
        /// <list type="table"><listheader><term> Value</term><description> Meaning</description></listheader><item><term> Less than zero</term><description> This instance precedes <paramref name="other" /> in the sort order.</description></item><item><term> Zero</term><description> This instance occurs in the same position in the sort order as <paramref name="other" />.</description></item><item><term> Greater than zero</term><description> This instance follows <paramref name="other" /> in the sort order.</description></item></list></returns>
        public abstract int CompareTo(object other);

        /// <summary>
        /// Compares to.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>System.Int32.</returns>
        public virtual int CompareTo(ulong key)
        {
            return (int)(Key - key);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Equalses the specified y.
        /// </summary>
        /// <param name="y">The y.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public virtual bool Equals(ICard<V> y)
        {
            return this.Equals(y.Key);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><see langword="true" /> if the current object is equal to the <paramref name="other" /> parameter; otherwise, <see langword="false" />.</returns>
        public virtual bool Equals(IUnique other)
        {
            return Key == other.UniqueKey;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="y">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override abstract bool Equals(object y);

        /// <summary>
        /// Equalses the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public virtual bool Equals(ulong key)
        {
            return Key == key;
        }

        /// <summary>
        /// Gets the bytes.
        /// </summary>
        /// <returns>System.Byte[].</returns>
        public abstract byte[] GetBytes();

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override abstract int GetHashCode();

        /// <summary>
        /// Gets the unique bytes.
        /// </summary>
        /// <returns>System.Byte[].</returns>
        public abstract byte[] GetUniqueBytes();

        /// <summary>
        /// Gets the type of the unique.
        /// </summary>
        /// <returns>Type.</returns>
        public virtual Type GetUniqueType()
        {
            return typeof(V);
        }

        /// <summary>
        /// Sets the specified card.
        /// </summary>
        /// <param name="card">The card.</param>
        public abstract void Set(ICard<V> card);

        /// <summary>
        /// Sets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public abstract void Set(object key, V value);

        /// <summary>
        /// Sets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public virtual void Set(ulong key, V value)
        {
            this.value = value;
            Key = key;
        }

        /// <summary>
        /// Sets the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        public abstract void Set(V value);

        /// <summary>
        /// Uniques the ordinals.
        /// </summary>
        /// <returns>System.Int32[].</returns>
        public virtual int[] UniqueOrdinals()
        {
            return null;
        }

        /// <summary>
        /// Uniques the values.
        /// </summary>
        /// <returns>System.Object[].</returns>
        public virtual object[] UniqueValues()
        {
            return new object[] { Key };
        }

        /// <summary>
        /// Moves the next.
        /// </summary>
        /// <param name="card">The card.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        public virtual ICard<V> MoveNext(ICard<V> card)
        {
            ICard<V> _card = card.Next;
            if (_card != null)
            {
                if (!_card.Removed)
                    return _card;
                return MoveNext(_card);
            }
            return null;
        }

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
                    Value = default(V);
                }

                disposedValue = true;
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        IEnumerator<V> IEnumerable<V>.GetEnumerator()
        {
            return new CardSubSeries<V>(this);
        }
        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new CardSubSeries<V>(this);
        }

        /// <summary>
        /// Ases the values.
        /// </summary>
        /// <returns>IEnumerable&lt;V&gt;.</returns>
        public virtual IEnumerable<V> AsValues()
        {
            return this;
        }
        /// <summary>
        /// Ases the cards.
        /// </summary>
        /// <returns>IEnumerable&lt;ICard&lt;V&gt;&gt;.</returns>
        public virtual IEnumerable<ICard<V>> AsCards()
        {
            foreach (ICard<V> card in this)
            {
                yield return card;
            }
        }
        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns>IEnumerator&lt;ICard&lt;V&gt;&gt;.</returns>
        public virtual IEnumerator<ICard<V>> GetEnumerator()
        {
            return new CardSubSeries<V>(this);
        }

        #endregion
    }
}
