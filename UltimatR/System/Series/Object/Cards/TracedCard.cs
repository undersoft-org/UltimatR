
// <copyright file="TracedCard.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The Series namespace.
/// </summary>
namespace System.Series
{
    using System.Instant;
    using System.Runtime.InteropServices;
    using System.Uniques;

    /// <summary>
    /// Class TracedCard.
    /// Implements the <see cref="System.Series.CardBase{V}" />
    /// </summary>
    /// <typeparam name="V"></typeparam>
    /// <seealso cref="System.Series.CardBase{V}" />
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public class TracedCard<V> : CardBase<V> where V : class, ITraceable
    {
        #region Fields

        /// <summary>
        /// The key
        /// </summary>
        private ulong _key;
        /// <summary>
        /// The proxy
        /// </summary>
        private Variety<V> _proxy;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TracedCard{V}" /> class.
        /// </summary>
        public TracedCard()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="TracedCard{V}" /> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public TracedCard(ICard<V> value) : base(value)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="TracedCard{V}" /> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public TracedCard(object key, V value) : base(key, value)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="TracedCard{V}" /> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public TracedCard(ulong key, V value) : base(key, value)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="TracedCard{V}" /> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public TracedCard(V value) : base(value)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public override ulong Key
        {
            get
            {
                return _key;
            }
            set
            {
                _key = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="other">An object to compare with this instance.</param>
        /// <returns>A value that indicates the relative order of the objects being compared. The return value has these meanings:
        /// <list type="table"><listheader><term> Value</term><description> Meaning</description></listheader><item><term> Less than zero</term><description> This instance precedes <paramref name="other" /> in the sort order.</description></item><item><term> Zero</term><description> This instance occurs in the same position in the sort order as <paramref name="other" />.</description></item><item><term> Greater than zero</term><description> This instance follows <paramref name="other" /> in the sort order.</description></item></list></returns>
        public override int CompareTo(ICard<V> other)
        {
            return (int)(Key - other.Key);
        }

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="other">An object to compare with this instance.</param>
        /// <returns>A value that indicates the relative order of the objects being compared. The return value has these meanings:
        /// <list type="table"><listheader><term> Value</term><description> Meaning</description></listheader><item><term> Less than zero</term><description> This instance precedes <paramref name="other" /> in the sort order.</description></item><item><term> Zero</term><description> This instance occurs in the same position in the sort order as <paramref name="other" />.</description></item><item><term> Greater than zero</term><description> This instance follows <paramref name="other" /> in the sort order.</description></item></list></returns>
        public override int CompareTo(object other)
        {
            return (int)(Key - other.UniqueKey64());
        }

        /// <summary>
        /// Compares to.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>System.Int32.</returns>
        public override int CompareTo(ulong key)
        {
            return (int)(Key - key);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="y">The <see cref="T:System.Object" /> to compare with this instance.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object y)
        {
            return Key.Equals(y.UniqueKey64());
        }

        /// <summary>
        /// Equalses the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public override bool Equals(ulong key)
        {
            return Key == key;
        }

        /// <summary>
        /// Gets the bytes.
        /// </summary>
        /// <returns>System.Byte[].</returns>
        public override byte[] GetBytes()
        {
            return GetUniqueBytes();
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            return (int)Key;
        }

        /// <summary>
        /// Gets the unique bytes.
        /// </summary>
        /// <returns>System.Byte[].</returns>
        public unsafe override byte[] GetUniqueBytes()
        {
            byte[] b = new byte[8];
            fixed (byte* s = b)
                *(ulong*)s = _key;
            return b;
        }

        /// <summary>
        /// Sets the specified card.
        /// </summary>
        /// <param name="card">The card.</param>
        public override void Set(ICard<V> card) 
        {           
            if (this.value == null)
            {
                _proxy = new Variety<V>(card.Value);
                value = _proxy.Preset;
            }
            else 
            {
                value.PatchTo(_proxy.EntryProxy);
            }
            
            _key = card.Key;
        }

        /// <summary>
        /// Sets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public override void Set(object key, V value)
        {
            if (this.value == null)
            {
                _proxy = new Variety<V>(value);
                this.value = _proxy.Entry;
            }
            else
            {
                value.PatchTo(_proxy.EntryProxy);
            }

            _key = key.UniqueKey64();
        }

        /// <summary>
        /// Sets the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        public override void Set(V value)
        {
            Set(value.UniqueKey64(), value);            
        }

        /// <summary>
        /// Gets or sets the unique object.
        /// </summary>
        /// <value>The unique object.</value>
        public override V UniqueObject 
        { 
            get => base.UniqueObject; 
            set => value.PatchTo(_proxy.EntryProxy);
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public override V Value 
        { 
            get => base.Value; 
            set => value.PatchTo(_proxy.EntryProxy);
        }

        #endregion
    }
}
