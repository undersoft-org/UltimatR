/// The System namespace.
/// </summary>
namespace System
{
    /// <summary>
    /// Interface IUnique
    /// Implements the <see cref="System.IEquatable{System.IUnique}" />
    /// Implements the <see cref="System.IComparable{System.IUnique}" />
    /// </summary>
    /// <seealso cref="System.IEquatable{System.IUnique}" />
    /// <seealso cref="System.IComparable{System.IUnique}" />
    public interface IUnique : IEquatable<IUnique>, IComparable<IUnique>
    {
        #region Properties

        /// <summary>
        /// Gets or sets the unique key.
        /// </summary>
        /// <value>The unique key.</value>
        ulong UniqueKey { get; set; }

        /// <summary>
        /// Gets or sets the unique seed.
        /// </summary>
        /// <value>The unique seed.</value>
        ulong UniqueSeed { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the bytes.
        /// </summary>
        /// <returns>System.Byte[].</returns>
        byte[] GetBytes();

        /// <summary>
        /// Gets the unique bytes.
        /// </summary>
        /// <returns>System.Byte[].</returns>
        byte[] GetUniqueBytes();

        #endregion
    }
}
