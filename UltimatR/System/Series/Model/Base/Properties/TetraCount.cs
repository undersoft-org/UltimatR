
// <copyright file="TetraCount.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The Basedeck namespace.
/// </summary>
namespace System.Series.Basedeck
{
    /// <summary>
    /// Struct TetraCount
    /// </summary>
    public struct TetraCount
    {
        /// <summary>
        /// Gets or sets the <see cref="System.Int32" /> with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>System.Int32.</returns>
        public unsafe int this[uint id]
        {
            get
            {
                fixed (TetraCount* a = &this)
                    return *&((int*)a)[id];
            }
            set
            {
                fixed (TetraCount* a = &this)
                    *&((int*)a)[id] = value;
            }
        }

        /// <summary>
        /// Increments the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>System.Int32.</returns>
        public unsafe int Increment(uint id)
        {
            fixed (TetraCount* a = &this)
                return ++(*&((int*)a)[id]);
        }
        /// <summary>
        /// Decrements the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>System.Int32.</returns>
        public unsafe int Decrement(uint id)
        {
            fixed (TetraCount* a = &this)
                return --(*&((int*)a)[id]);
        }

        /// <summary>
        /// Resets the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public unsafe void Reset(uint id)
        {
            fixed (TetraCount* a = &this)
            {
                (*&((int*)a)[id]) = 0;
            }
        }

        /// <summary>
        /// Resets all.
        /// </summary>
        public unsafe void ResetAll()
        {
            fixed (TetraCount* a = &this)
            {
                (*&((long*)a)[0]) = 0L;
                (*&((long*)a)[1]) = 0L;
            }
        }

        /// <summary>
        /// The even positive count
        /// </summary>
        public int EvenPositiveCount;
        /// <summary>
        /// The odd positive count
        /// </summary>
        public int OddPositiveCount;
        /// <summary>
        /// The even negative count
        /// </summary>
        public int EvenNegativeCount;
        /// <summary>
        /// The odd negative count
        /// </summary>
        public int OddNegativeCount;
    }
}
