/// <summary>
/// The System namespace.
/// </summary>
namespace System
{
    /// <summary>
    /// Interface IUnique
    /// Implements the <see cref="System.IUnique" />
    /// </summary>
    /// <typeparam name="V"></typeparam>
    /// <seealso cref="System.IUnique" />
    public interface IUnique<V> : IUnique
    {
        #region Properties




        /// <summary>
        /// Gets or sets the unique object.
        /// </summary>
        /// <value>The unique object.</value>
        V UniqueObject { get; set; }

        #endregion

        #region Methods





        /// <summary>
        /// Uniques the ordinals.
        /// </summary>
        /// <returns>System.Int32[].</returns>
        int[] UniqueOrdinals();





        /// <summary>
        /// Compacts the key.
        /// </summary>
        /// <returns>System.UInt64.</returns>
        ulong CompactKey();





        /// <summary>
        /// Uniques the values.
        /// </summary>
        /// <returns>System.Object[].</returns>
        object[] UniqueValues();

        #endregion
    }
}
