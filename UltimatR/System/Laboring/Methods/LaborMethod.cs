/// <summary>
/// The Laboring namespace.
/// </summary>
namespace System.Laboring
{
    using System.Extract;
    using System.Series;
    using System.Uniques;




    /// <summary>
    /// Class LaborMethod.
    /// Implements the <see cref="System.Series.Card{System.IDeputy}" />
    /// </summary>
    /// <seealso cref="System.Series.Card{System.IDeputy}" />
    public class LaborMethod : Card<IDeputy>
    {
        #region Fields




        /// <summary>
        /// The key
        /// </summary>
        private ulong key;

        #endregion

        #region Constructors




        /// <summary>
        /// Initializes a new instance of the <see cref="LaborMethod" /> class.
        /// </summary>
        public LaborMethod()
        {
        }




        /// <summary>
        /// Initializes a new instance of the <see cref="LaborMethod" /> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public LaborMethod(IDeputy value)
        {
            Value = value;
        }





        /// <summary>
        /// Initializes a new instance of the <see cref="LaborMethod" /> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public LaborMethod(long key, IDeputy value) : base(key, value)
        {
        }





        /// <summary>
        /// Initializes a new instance of the <see cref="LaborMethod" /> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public LaborMethod(object key, IDeputy value) : base(key.UniqueKey64(), value)
        {
        }

        #endregion

        #region Properties




        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public override ulong Key { get => key; set => key = value; }

        #endregion

        #region Methods






        /// <summary>
        /// Compares to.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>System.Int32.</returns>
        public override int CompareTo(object other)
        {
            return (int)(UniqueKey - other.UniqueKey());
        }






        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="y">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object y)
        {
            return UniqueKey == y.UniqueKey64();
        }





        /// <summary>
        /// Gets the bytes.
        /// </summary>
        /// <returns>System.Byte[].</returns>
        public override byte[] GetBytes()
        {
            return Key.GetBytes();
        }





        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            return Key.GetBytes().BitAggregate64to32().ToInt32();
        }





        /// <summary>
        /// Gets the unique bytes.
        /// </summary>
        /// <returns>System.Byte[].</returns>
        public override byte[] GetUniqueBytes()
        {
            return Key.GetBytes();
        }





        /// <summary>
        /// Sets the specified card.
        /// </summary>
        /// <param name="card">The card.</param>
        public override void Set(ICard<IDeputy> card)
        {
            Key = card.Key;
            Value = card.Value;
            Removed = false;
        }





        /// <summary>
        /// Sets the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        public override void Set(IDeputy value)
        {
            Value = value;
            Removed = false;
        }






        /// <summary>
        /// Sets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public override void Set(object key, IDeputy value)
        {
            Key = key.UniqueKey64();
            Value = value;
            Removed = false;
        }

        #endregion
    }
}
