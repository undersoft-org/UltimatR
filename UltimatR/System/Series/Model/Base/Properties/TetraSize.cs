
// <copyright file="TetraSize.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The Basedeck namespace.
/// </summary>
namespace System.Series.Basedeck
{
    using System.Uniques;

    /// <summary>
    /// Struct TetraSize
    /// </summary>
    public struct TetraSize
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TetraSize" /> struct.
        /// </summary>
        /// <param name="size">The size.</param>
        public TetraSize(int size = 8)
        {
            StartSize = size;
            EvenPositiveSize = size;
            OddPositiveSize = size;
            EvenNegativeSize = size;
            OddNegativeSize = size;

            EvenPositivePrimesId = 0;
            OddPositivePrimesId = 0;
            EvenNegativePrimesId = 0;
            OddNegativePrimesId = 0;
        }

        /// <summary>
        /// Gets or sets the <see cref="System.Int32" /> with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>System.Int32.</returns>
        public unsafe int this[uint id]
        {
            get
            {
                fixed (TetraSize* a = &this)
                    return *&((int*)a)[id];
            }
            set
            {
                fixed (TetraSize* a = &this)
                    *&((int*)a)[id] = value;
            }
        }

        /// <summary>
        /// Nexts the size.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>System.Int32.</returns>
        public unsafe int NextSize(uint id)
        {
            fixed (TetraSize* a = &this)
                return (*&((int*)a)[id]) = PRIMES_ARRAY.Get((*&((int*)a)[id + 4])++);
        }
        /// <summary>
        /// Previouses the size.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>System.Int32.</returns>
        public unsafe int PreviousSize(uint id)
        {
            fixed (TetraSize* a = &this)
                return (*&((int*)a)[id]) = PRIMES_ARRAY.Get(--(*&((int*)a)[id + 4]));
        }

        /// <summary>
        /// Gets the primes identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>System.Int32.</returns>
        public unsafe int GetPrimesId(uint id)
        {
            return this[id + 4];
        }

        /// <summary>
        /// Sets the primes identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="value">The value.</param>
        public unsafe void SetPrimesId(uint id, int value)
        {
            this[id + 4] = value;
        }

        /// <summary>
        /// Resets the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public unsafe void Reset(uint id)
        {
            fixed (TetraSize* a = &this)
            {
                (*&((int*)a)[id]) = StartSize;
            }
        }

        /// <summary>
        /// Resets all.
        /// </summary>
        public unsafe void ResetAll()
        {
            fixed (TetraSize* a = &this)
            {
                (*&((int*)a)[0]) = StartSize;
                (*&((int*)a)[1]) = StartSize;
                (*&((int*)a)[2]) = StartSize;
                (*&((int*)a)[3]) = StartSize;
            }
        }

        /// <summary>
        /// The even positive size
        /// </summary>
        public int EvenPositiveSize;
        /// <summary>
        /// The odd positive size
        /// </summary>
        public int OddPositiveSize;
        /// <summary>
        /// The even negative size
        /// </summary>
        public int EvenNegativeSize;
        /// <summary>
        /// The odd negative size
        /// </summary>
        public int OddNegativeSize;

        /// <summary>
        /// The even positive primes identifier
        /// </summary>
        public int EvenPositivePrimesId;
        /// <summary>
        /// The odd positive primes identifier
        /// </summary>
        public int OddPositivePrimesId;
        /// <summary>
        /// The even negative primes identifier
        /// </summary>
        public int EvenNegativePrimesId;
        /// <summary>
        /// The odd negative primes identifier
        /// </summary>
        public int OddNegativePrimesId;

        /// <summary>
        /// The start size
        /// </summary>
        public int StartSize;
    }
}
