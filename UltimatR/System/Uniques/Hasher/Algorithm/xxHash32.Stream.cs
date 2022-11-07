﻿/// <summary>
/// The Uniques namespace.
/// </summary>
namespace System.Uniques
{
    using System.Buffers;
    using System.Diagnostics;
    using System.Extract;
    using System.IO;

    /// <summary>
    /// Class xxHash32.
    /// </summary>
    public static partial class xxHash32
    {
        #region Methods








        /// <summary>
        /// Computes the hash.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="bufferSize">Size of the buffer.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>System.UInt32.</returns>
        public static uint ComputeHash(Stream stream, int bufferSize = 4096, uint seed = 0)
        {
            Debug.Assert(stream != null);
            Debug.Assert(bufferSize > 16);

            byte[] buffer = ArrayPool<byte>.Shared.Rent(bufferSize + 16);
            int readBytes;
            int offset = 0;
            long length = 0;

            uint v1 = seed + p1 + p2;
            uint v2 = seed + p2;
            uint v3 = seed + 0;
            uint v4 = seed - p1;

            try
            {
                while ((readBytes = stream.Read(buffer, offset, bufferSize)) > 0)
                {
                    length = length + readBytes;
                    offset = offset + readBytes;

                    if (offset < 16) continue;

                    int r = offset % 16;
                    int l = offset - r;

                    UnsafeAlign(buffer, l, ref v1, ref v2, ref v3, ref v4);

                    Extractor.CopyBlock(buffer, 0, buffer, l, (uint)r);
                    offset = r;
                }

                uint h32 = UnsafeFinal(buffer, offset, ref v1, ref v2, ref v3, ref v4, length, seed);

                return h32;
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer);
            }
        }

        #endregion
    }
}
