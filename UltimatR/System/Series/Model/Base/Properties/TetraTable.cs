
// <copyright file="TetraTable.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The Basedeck namespace.
/// </summary>
namespace System.Series.Basedeck
{
    /// <summary>
    /// Struct TetraTable
    /// Implements the <see cref="System.IDisposable" />
    /// </summary>
    /// <typeparam name="V"></typeparam>
    /// <seealso cref="System.IDisposable" />
    public struct TetraTable<V> : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TetraTable{V}" /> struct.
        /// </summary>
        /// <param name="hashdeck">The hashdeck.</param>
        /// <param name="size">The size.</param>
        public TetraTable(TetraSet<V> hashdeck, int size = 8)
        {
            EvenPositiveSize = hashdeck.EmptyCardTable(size);
            OddPositiveSize = hashdeck.EmptyCardTable(size);
            EvenNegativeSize = hashdeck.EmptyCardTable(size);
            OddNegativeSize = hashdeck.EmptyCardTable(size);
            tetraTable = new ICard<V>[4][] { EvenPositiveSize, OddPositiveSize, EvenNegativeSize, OddNegativeSize };

        }

        /// <summary>
        /// Gets or sets the <see cref="ICard{V}[]" /> with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ICard&lt;V&gt;[].</returns>
        public unsafe ICard<V>[] this[uint id]
        {
            get
            {
                return tetraTable[id];
            }
            set
            {
                tetraTable[id] = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="ICard{V}" /> with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="pos">The position.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        public unsafe ICard<V> this[uint id, uint pos]
        {
            get
            {
                return this[id][pos];
            }
            set
            {
                this[id][pos] = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="ICard{V}[]" /> with the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>ICard&lt;V&gt;[].</returns>
        public unsafe ICard<V>[] this[ulong key]
        {
            get
            {
                return this[(uint)((key & 1) | ((key >> 62) & 2))];
            }
            set
            {
                this[(uint)((key & 1) | ((key >> 62) & 2))] = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="ICard{V}" /> with the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="size">The size.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        public unsafe ICard<V> this[ulong key, long size]
        {
            get
            {
                return this[(uint)((key & 1) | ((key >> 62) & 2))]
                                 [(int)(key % (uint)size)];
            }
            set
            {
                this[(uint)((key & 1) | ((key >> 62) & 2))]
                                 [(int)(key % (uint)size)] = value;
            }
        }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>System.Int32.</returns>
        public static int GetId(ulong key)
        {
            ulong ukey = (ulong)key;
            return (int)((ukey & 1) | ((ukey >> 62) & 2));
        }

        /// <summary>
        /// Gets the position.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="size">The size.</param>
        /// <returns>System.Int32.</returns>
        public static int GetPosition(ulong key, long size)
        {
            return (int)(key % (ulong)size);
        }

        /// <summary>
        /// The even positive size
        /// </summary>
        private ICard<V>[] EvenPositiveSize;
        /// <summary>
        /// The odd positive size
        /// </summary>
        private ICard<V>[] OddPositiveSize;
        /// <summary>
        /// The even negative size
        /// </summary>
        private ICard<V>[] EvenNegativeSize;
        /// <summary>
        /// The odd negative size
        /// </summary>
        private ICard<V>[] OddNegativeSize;

        /// <summary>
        /// The tetra table
        /// </summary>
        private ICard<V>[][] tetraTable;

        /// <summary>
        /// Disposes this instance.
        /// </summary>
        public void Dispose()
        {
            EvenPositiveSize = null;
            OddPositiveSize = null;
            EvenNegativeSize = null;
            OddNegativeSize = null;
            tetraTable = null;
        }
    }
}
