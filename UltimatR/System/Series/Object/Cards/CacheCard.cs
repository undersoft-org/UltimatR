
// <copyright file="CacheCard.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The Series namespace.
/// </summary>
namespace System.Series
{
    using System.Runtime.InteropServices;
    using System.Uniques;
    using System.Extract;
    using System.Logs;

    /// <summary>
    /// Class CacheCard.
    /// Implements the <see cref="System.Series.CardBase{V}" />
    /// </summary>
    /// <typeparam name="V"></typeparam>
    /// <seealso cref="System.Series.CardBase{V}" />
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public class CacheCard<V> : CardBase<V>
    {
        #region Fields

        /// <summary>
        /// The key
        /// </summary>
        private ulong _key;
        /// <summary>
        /// The duration
        /// </summary>
        private TimeSpan duration;
        /// <summary>
        /// The expiration
        /// </summary>
        private DateTime expiration;
        /// <summary>
        /// The callback
        /// </summary>
        private Deputy callback;

        #endregion

        /// <summary>
        /// Setups the expiration.
        /// </summary>
        /// <param name="lifetime">The lifetime.</param>
        /// <param name="callback">The callback.</param>
        public void SetupExpiration(TimeSpan? lifetime, Deputy callback = null)
        {
            duration = (lifetime != null) ? lifetime.Value : TimeSpan.FromMinutes(15);
            expiration = Log.Clock + duration;
            this.callback = callback;
        }
        /// <summary>
        /// Setups the expiration.
        /// </summary>
        private void setupExpiration()
        {
            expiration = Log.Clock + duration;
        }

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheCard{V}" /> class.
        /// </summary>
        public CacheCard() : base()
        {
            SetupExpiration(TimeSpan.FromMinutes(15));
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="CacheCard{V}" /> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="lifeTime">The life time.</param>
        /// <param name="deputy">The deputy.</param>
        public CacheCard(ICard<V> value, TimeSpan? lifeTime = null, Deputy deputy = null) : base(value)
        {
            SetupExpiration(lifeTime, deputy);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="CacheCard{V}" /> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="lifeTime">The life time.</param>
        /// <param name="deputy">The deputy.</param>
        public CacheCard(object key, V value, TimeSpan? lifeTime = null, Deputy deputy = null) : base(key, value)
        {
            SetupExpiration(lifeTime, deputy);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="CacheCard{V}" /> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="lifeTime">The life time.</param>
        /// <param name="deputy">The deputy.</param>
        public CacheCard(ulong key, V value, TimeSpan? lifeTime = null, Deputy deputy = null) : base(key, value)
        {
            SetupExpiration(lifeTime, deputy);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="CacheCard{V}" /> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="lifeTime">The life time.</param>
        /// <param name="deputy">The deputy.</param>
        public CacheCard(V value, TimeSpan? lifeTime = null, Deputy deputy = null) : base(value)
        {
            SetupExpiration(lifeTime, deputy);
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

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public override V Value
        {
            get
            {
                if (Log.Clock > expiration)
                {
                    Removed = true;
                    if (callback != null)
                        _ = callback.ExecuteAsync(value);
                    return default(V);
                }
                return value;
            }
            set
            {
                setupExpiration();
                this.value = value;
            }
        }

        /// <summary>
        /// Gets or sets the unique object.
        /// </summary>
        /// <value>The unique object.</value>
        public override V UniqueObject 
        { 
            get => this.Value; 
            set => this.Value = value; 
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
            return (int)(Key - other.UniqueKey64(UniqueSeed));
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
            return Key.Equals(y.UniqueKey64(UniqueSeed));
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
            return this.value.GetBytes();
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            return (int)Key.UniqueKey32();
        }

        /// <summary>
        /// Gets the unique bytes.
        /// </summary>
        /// <returns>System.Byte[].</returns>
        public unsafe override byte[] GetUniqueBytes()
        {
            byte[] b = new byte[8];
            fixed(byte* s = b)
                *(ulong*)s = _key;
            return b;
        }

        /// <summary>
        /// Sets the specified card.
        /// </summary>
        /// <param name="card">The card.</param>
        public override void Set(ICard<V> card)
        {
            value = card.Value;
            _key = card.Key;
        }

        /// <summary>
        /// Sets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public override void Set(object key, V value)
        {
            this.value = value;
            _key = key.UniqueKey64(UniqueSeed);
        }

        /// <summary>
        /// Sets the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        public override void Set(V value)
        {
            this.value = value;
            if(this.value is IUnique<V>)
            _key = ((IUnique<V>)value).CompactKey();
        }

        #endregion
    }
}
