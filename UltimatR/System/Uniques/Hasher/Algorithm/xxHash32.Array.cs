/// <summary>
/// The Uniques namespace.
/// </summary>
namespace System.Uniques
{
    using System.Diagnostics;




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
        /// <param name="seed">The seed.</param>
        /// <returns>System.UInt64.</returns>
        public static unsafe ulong ComputeHash(ArraySegment<byte> data, uint seed = 0)
        {
            Debug.Assert(data != null);

            return ComputeHash(data.Array, data.Offset, data.Count, seed);
        }








        /// <summary>
        /// Computes the hash.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="length">The length.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>System.UInt32.</returns>
        public static unsafe uint ComputeHash(byte[] data, int length, uint seed = 0)
        {
            Debug.Assert(data != null);
            Debug.Assert(length >= 0);
            Debug.Assert(length <= data.Length);

            fixed (byte* pData = &data[0])
            {
                return UnsafeComputeHash(pData, length, seed);
            }
        }









        /// <summary>
        /// Computes the hash.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="length">The length.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>System.UInt32.</returns>
        public static unsafe uint ComputeHash(byte[] data, int offset, int length, uint seed = 0)
        {
            Debug.Assert(data != null);
            Debug.Assert(length >= 0);
            Debug.Assert(offset < data.Length);
            Debug.Assert(length <= data.Length - offset);

            fixed (byte* pData = &data[0 + offset])
            {
                return UnsafeComputeHash(pData, length, seed);
            }
        }

        #endregion
    }
}
