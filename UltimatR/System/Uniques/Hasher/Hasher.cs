/// <summary>
/// The Uniques namespace.
/// </summary>
namespace System.Uniques
{

    /// <summary>
    /// Class Hasher32.
    /// </summary>
    public static class Hasher32
    {
        #region Methods


        /// <summary>
        /// Computes the bytes.
        /// </summary>
        /// <param name="ptr">The PTR.</param>
        /// <param name="length">The length.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>Byte[].</returns>
        public static unsafe Byte[] ComputeBytes(byte* ptr, int length, ulong seed = 0)
        {
            byte[] b = new byte[4];
            fixed (byte* pb = b)
            {
                *((uint*)pb) = xxHash32.UnsafeComputeHash(ptr, length, (uint)seed);
            }
            return b;
        }

        /// <summary>
        /// Computes the bytes.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>Byte[].</returns>
        public static unsafe Byte[] ComputeBytes(byte[] bytes, ulong seed = 0)
        {
            byte[] b = new byte[4];
            fixed (byte* pb = b, pa = bytes)
            {
                *((uint*)pb) = xxHash32.UnsafeComputeHash(pa, bytes.Length, (uint)seed);
            }
            return b;
        }

        /// <summary>
        /// Computes the key.
        /// </summary>
        /// <param name="ptr">The PTR.</param>
        /// <param name="length">The length.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>System.UInt32.</returns>
        public static unsafe uint ComputeKey(byte* ptr, int length, ulong seed = 0)
        {
            return xxHash32.UnsafeComputeHash(ptr, length, (uint)seed);
        }


        /// <summary>
        /// Computes the key.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>System.UInt32.</returns>
        public static unsafe uint ComputeKey(byte[] bytes, ulong seed = 0)
        {
            fixed (byte* pa = bytes)
            {
                return xxHash32.UnsafeComputeHash(pa, bytes.Length, (uint)seed);
            }
        }

        #endregion
    }


    /// <summary>
    /// Class Hasher64.
    /// </summary>
    public static class Hasher64
    {
        #region Methods


        /// <summary>
        /// Computes the bytes.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="length">The length.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>Byte[].</returns>
        public static unsafe Byte[] ComputeBytes(byte* bytes, int length, ulong seed = 0)
        {
            byte[] b = new byte[8];
            fixed (byte* pb = b)
            {
                *((ulong*)pb) = (xxHash64.UnsafeComputeHash(bytes, length, seed) << 12) >> 12;
            }
            return b;
        }

        /// <summary>
        /// Computes the bytes.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>Byte[].</returns>
        public static unsafe Byte[] ComputeBytes(byte[] bytes, ulong seed = 0)
        {
            byte[] b = new byte[8];
            fixed (byte* pb = b, pa = bytes)
            {
                *((ulong*)pb) = (xxHash64.UnsafeComputeHash(pa, bytes.Length, seed) << 12) >> 12;
            }
            return b;
        }


        /// <summary>
        /// Computes the key.
        /// </summary>
        /// <param name="ptr">The PTR.</param>
        /// <param name="length">The length.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>System.UInt64.</returns>
        public static unsafe ulong ComputeKey(byte* ptr, int length, ulong seed = 0)
        {
            return (xxHash64.UnsafeComputeHash(ptr, length, seed) << 12) >> 12;
        }

        /// <summary>
        /// Computes the key.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>System.UInt64.</returns>
        public static unsafe ulong ComputeKey(byte[] bytes, ulong seed = 0)
        {
            fixed (byte* pa = bytes)
            {
                return (xxHash64.UnsafeComputeHash(pa, bytes.Length, seed) << 12) >> 12;
            }
        }

        #endregion
    }
}
