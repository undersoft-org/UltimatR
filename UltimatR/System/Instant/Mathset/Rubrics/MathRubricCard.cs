
// <copyright file="MathRubricCard.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>




/// <summary>
/// The Mathset namespace.
/// </summary>
namespace System.Instant.Mathset
{
    using System.Runtime.InteropServices;
    using System.Series;
    using System.Uniques;




    /// <summary>
    /// Class MathRubricCard.
    /// Implements the <see cref="System.Series.Card{System.Instant.Mathset.MathRubric}" />
    /// </summary>
    /// <seealso cref="System.Series.Card{System.Instant.Mathset.MathRubric}" />
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public class MathRubricCard : Card<MathRubric>
    {
        #region Fields

        /// <summary>
        /// The key
        /// </summary>
        private ulong _key;

        #endregion

        #region Constructors




        /// <summary>
        /// Initializes a new instance of the <see cref="MathRubricCard" /> class.
        /// </summary>
        public MathRubricCard()
        {
        }




        /// <summary>
        /// Initializes a new instance of the <see cref="MathRubricCard" /> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public MathRubricCard(ICard<MathRubric> value) : base(value)
        {
        }





        /// <summary>
        /// Initializes a new instance of the <see cref="MathRubricCard" /> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public MathRubricCard(long key, MathRubric value) : base(key, value)
        {
        }




        /// <summary>
        /// Initializes a new instance of the <see cref="MathRubricCard" /> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public MathRubricCard(MathRubric value) : base(value)
        {
        }





        /// <summary>
        /// Initializes a new instance of the <see cref="MathRubricCard" /> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public MathRubricCard(object key, MathRubric value) : base(key.UniqueKey64(), value)
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
        /// Compares to.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>System.Int32.</returns>
        public override int CompareTo(ICard<MathRubric> other)
        {
            return (int)(_key - other.Key);
        }






        /// <summary>
        /// Compares to.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>System.Int32.</returns>
        public override int CompareTo(object other)
        {
            return (int)(_key - other.UniqueKey64());
        }






        /// <summary>
        /// Compares to.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>System.Int32.</returns>
        public override int CompareTo(ulong key)
        {
            return (int)(_key - key);
        }






        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="y">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object y)
        {
            return _key.Equals(y.UniqueKey64());
        }






        /// <summary>
        /// Equalses the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public override bool Equals(ulong key)
        {
            return _key == key;
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
            byte[] b = new byte[8];
            fixed (byte* s = b)
                *(ulong*)s = _key;
            return b;
        }





        /// <summary>
        /// Sets the specified card.
        /// </summary>
        /// <param name="card">The card.</param>
        public override void Set(ICard<MathRubric> card)
        {
            this.value = card.Value;
            _key = card.Key;
        }





        /// <summary>
        /// Sets the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        public override void Set(MathRubric value)
        {
            this.value = value;
            _key = value.UniqueKey;
        }






        /// <summary>
        /// Sets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public override void Set(object key, MathRubric value)
        {
            this.value = value;
            _key = key.UniqueKey64();
        }

        #endregion
    }
}
