
// <copyright file="Card32.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>


/// <summary>
/// The Series namespace.
/// </summary>
namespace System.Series
{
    using System.Runtime.InteropServices;
    using System.Uniques;

    /// <summary>
    /// Class Card32.
    /// Implements the <see cref="System.Series.CardBase{V}" />
    /// </summary>
    /// <typeparam name="V"></typeparam>
    /// <seealso cref="System.Series.CardBase{V}" />
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public class Card32<V> : CardBase<V>
    {
        #region Fields

        /// <summary>
        /// The key
        /// </summary>
        private uint _key;

        #endregion

        #region Constructors




        /// <summary>
        /// Initializes a new instance of the <see cref="Card32{V}" /> class.
        /// </summary>
        public Card32()
        {
        }




        /// <summary>
        /// Initializes a new instance of the <see cref="Card32{V}" /> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public Card32(ICard<V> value) : base(value)
        {
        }





        /// <summary>
        /// Initializes a new instance of the <see cref="Card32{V}" /> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public Card32(object key, V value) : base(key, value)
        {
        }





        /// <summary>
        /// Initializes a new instance of the <see cref="Card32{V}" /> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public Card32(ulong key, V value) : base(key, value)
        {
        }




        /// <summary>
        /// Initializes a new instance of the <see cref="Card32{V}" /> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public Card32(V value) : base(value)
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
                _key = (uint)value;
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
            return (int)(_key - other.UniqueKey32());
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
            return _key.Equals(y.UniqueKey32());
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
            return (int)_key;
        }





        /// <summary>
        /// Gets the unique bytes.
        /// </summary>
        /// <returns>System.Byte[].</returns>
        public unsafe override byte[] GetUniqueBytes()
        {
            byte[] b = new byte[4];
            fixed (byte* s = b)
                *(uint*)s = _key;
            return b;
        }





        /// <summary>
        /// Sets the specified card.
        /// </summary>
        /// <param name="card">The card.</param>
        public override void Set(ICard<V> card)
        {
            this.value = card.Value;
            _key = (uint)card.Key;
        }






        /// <summary>
        /// Sets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public override void Set(object key, V value)
        {
            this.value = value;
            _key = key.UniqueKey32();
        }





        /// <summary>
        /// Sets the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        public override void Set(V value)
        {
            this.value = value;
            _key = value.UniqueKey32();
        }

        #endregion
    }
}
