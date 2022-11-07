/// <summary>
/// The System namespace.
/// </summary>
namespace System
{
    using System.Collections.Specialized;
    using System.Reflection;

    /// <summary>
    /// Interface ISerialNumber
    /// Implements the <see cref="System.ISerialNumber" />
    /// </summary>
    /// <typeparam name="V"></typeparam>
    /// <seealso cref="System.ISerialNumber" />
    public interface ISerialNumber<V> : ISerialNumber
    {
        #region Properties




        /// <summary>
        /// Gets the type of the identifier.
        /// </summary>
        /// <value>The type of the identifier.</value>
        Type IdentifierType { get; }




        /// <summary>
        /// Gets the key fields.
        /// </summary>
        /// <value>The key fields.</value>
        FieldInfo[] KeyFields { get; }




        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>The value.</value>
        V Value { get; }

        #endregion
    }

    /// <summary>
    /// Interface ISerialNumber
    /// Implements the <see cref="System.ISerialNumber" />
    /// </summary>
    /// <seealso cref="System.ISerialNumber" />
    public interface ISerialNumber : IUnique, IEquatable<BitVector32>, IEquatable<DateTime>, IEquatable<ISerialNumber>
    {
        #region Properties




        /// <summary>
        /// Gets or sets the block x.
        /// </summary>
        /// <value>The block x.</value>
        ushort BlockX { get; set; }




        /// <summary>
        /// Gets or sets the block y.
        /// </summary>
        /// <value>The block y.</value>
        ushort BlockY { get; set; }




        /// <summary>
        /// Gets or sets the block z.
        /// </summary>
        /// <value>The block z.</value>
        ushort BlockZ { get; set; }


        /// <summary>
        /// Gets or sets the priority block.
        /// </summary>
        /// <value>The priority block.</value>
        byte PriorityBlock { get; set; }



        /// <summary>
        /// Gets or sets the flags block.
        /// </summary>
        /// <value>The flags block.</value>
        byte FlagsBlock { get; set; }




        /// <summary>
        /// Gets or sets the time block.
        /// </summary>
        /// <value>The time block.</value>
        long TimeBlock { get; set; }

        #endregion

        #region Methods







        /// <summary>
        /// Values from xyz.
        /// </summary>
        /// <param name="vectorZ">The vector z.</param>
        /// <param name="vectorY">The vector y.</param>
        /// <returns>System.UInt64.</returns>
        ulong ValueFromXYZ(uint vectorZ, uint vectorY);








        /// <summary>
        /// Values to xyz.
        /// </summary>
        /// <param name="vectorZ">The vector z.</param>
        /// <param name="vectorY">The vector y.</param>
        /// <param name="value">The value.</param>
        /// <returns>System.UInt16[].</returns>
        ushort[] ValueToXYZ(ulong vectorZ, ulong vectorY, ulong value);

        #endregion
    }
}
