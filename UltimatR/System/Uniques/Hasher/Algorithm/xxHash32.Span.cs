/// <summary>
/// The Uniques namespace.
/// </summary>
namespace System.Uniques
{
    using System.Diagnostics;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Class xxHash32.
    /// </summary>
    public static partial class xxHash32
    {
        #region Methods








        /// <summary>
        /// Computes the hash.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="length">The length.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>System.UInt32.</returns>
        public static unsafe uint ComputeHash(ReadOnlySpan<byte> data, int length, uint seed = 0)
        {
            Debug.Assert(data != null);
            Debug.Assert(length >= 0);
            Debug.Assert(length <= data.Length);

            fixed (byte* pData = &MemoryMarshal.GetReference(data))
            {
                return UnsafeComputeHash(pData, length, seed);
            }
        }








        /// <summary>
        /// Computes the hash.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="length">The length.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>System.UInt32.</returns>
        public static unsafe uint ComputeHash(Span<byte> data, int length, uint seed = 0)
        {
            Debug.Assert(data != null);
            Debug.Assert(length >= 0);
            Debug.Assert(length <= data.Length);

            fixed (byte* pData = &MemoryMarshal.GetReference(data))
            {
                return UnsafeComputeHash(pData, length, seed);
            }
        }

        #endregion
    }
}
