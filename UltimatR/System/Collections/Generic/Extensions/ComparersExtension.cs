/// <summary>
/// The Generic namespace.
/// </summary>
namespace System.Collections.Generic
{
    using System.Linq;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Class HashCode32Comparer.
    /// Implements the <see cref="System.Collections.Generic.IComparer{System.Int32}" />
    /// </summary>
    /// <seealso cref="System.Collections.Generic.IComparer{System.Int32}" />
    [Serializable]
    public class HashCode32Comparer : IComparer<int>
    {
        #region Methods

        /// <summary>
        /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of <paramref name="x" /> and <paramref name="y" />, as shown in the following table.
        /// <list type="table"><listheader><term> Value</term><description> Meaning</description></listheader><item><term> Less than zero</term><description><paramref name="x" /> is less than <paramref name="y" />.</description></item><item><term> Zero</term><description><paramref name="x" /> equals <paramref name="y" />.</description></item><item><term> Greater than zero</term><description><paramref name="x" /> is greater than <paramref name="y" />.</description></item></list></returns>
        public int Compare(int x, int y)
        {
            return x - y;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public int GetHashCode(int obj)
        {
            return obj;
        }

        #endregion
    }

    /// <summary>
    /// Class HashCode32Equality.
    /// Implements the <see cref="System.Collections.Generic.IEqualityComparer{System.Int32}" />
    /// </summary>
    /// <seealso cref="System.Collections.Generic.IEqualityComparer{System.Int32}" />
    [Serializable]
    public class HashCode32Equality : IEqualityComparer<int>
    {
        #region Methods







        /// <summary>
        /// Determines whether the specified objects are equal.
        /// </summary>
        /// <param name="x">The first object of type <paramref name="T" /> to compare.</param>
        /// <param name="y">The second object of type <paramref name="T" /> to compare.</param>
        /// <returns><see langword="true" /> if the specified objects are equal; otherwise, <see langword="false" />.</returns>
        public bool Equals(int x, int y)
        {
            return x == y;
        }






        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object" /> for which a hash code is to be returned.</param>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public int GetHashCode(int obj)
        {
            return obj;
        }

        #endregion
    }

    /// <summary>
    /// Class HashCode64Equality.
    /// Implements the <see cref="System.Collections.Generic.IEqualityComparer{System.Int64}" />
    /// </summary>
    /// <seealso cref="System.Collections.Generic.IEqualityComparer{System.Int64}" />
    [Serializable]
    public class HashCode64Equality : IEqualityComparer<long>
    {
        #region Methods







        /// <summary>
        /// Determines whether the specified objects are equal.
        /// </summary>
        /// <param name="x">The first object of type <paramref name="T" /> to compare.</param>
        /// <param name="y">The second object of type <paramref name="T" /> to compare.</param>
        /// <returns><see langword="true" /> if the specified objects are equal; otherwise, <see langword="false" />.</returns>
        public bool Equals(long x, long y)
        {
            return x == y;
        }






        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object" /> for which a hash code is to be returned.</param>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public unsafe int GetHashCode(long obj)
        {
            unchecked
            {
                byte* pkey = ((byte*)&obj);
                return (((17 + *(int*)(pkey + 4)) * 23) + *(int*)(pkey)) * 23;
            }
        }

        #endregion
    }

    /// <summary>
    /// Class IntArrayEquality.
    /// Implements the <see cref="System.Collections.Generic.IEqualityComparer{System.Int32[]}" />
    /// </summary>
    /// <seealso cref="System.Collections.Generic.IEqualityComparer{System.Int32[]}" />
    [Serializable]
    public class IntArrayEquality : IEqualityComparer<int[]>
    {
        #region Methods







        /// <summary>
        /// Determines whether the specified objects are equal.
        /// </summary>
        /// <param name="x">The first object of type <paramref name="T" /> to compare.</param>
        /// <param name="y">The second object of type <paramref name="T" /> to compare.</param>
        /// <returns><see langword="true" /> if the specified objects are equal; otherwise, <see langword="false" />.</returns>
        public bool Equals(int[] x, int[] y)
        {
            return x.SequenceEqual(y);
        }






        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object" /> for which a hash code is to be returned.</param>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public int GetHashCode(int[] obj)
        {
            unchecked
            {
                return obj.Select(o => o).Aggregate(17, (a, b) => (a + b) * 23); ;
            }
        }

        #endregion
    }


    /// <summary>
    /// Class ParellelHashCode32Equality.
    /// Implements the <see cref="System.Collections.Generic.IEqualityComparer{System.Linq.ParallelQuery{System.Collections.Generic.IEnumerable{System.Int32}}}" />
    /// </summary>
    /// <seealso cref="System.Collections.Generic.IEqualityComparer{System.Linq.ParallelQuery{System.Collections.Generic.IEnumerable{System.Int32}}}" />
    [Serializable]
    public class ParellelHashCode32Equality : IEqualityComparer<ParallelQuery<IEnumerable<int>>>
    {
        #region Methods







        /// <summary>
        /// Determines whether the specified objects are equal.
        /// </summary>
        /// <param name="x">The first object of type <paramref name="T" /> to compare.</param>
        /// <param name="y">The second object of type <paramref name="T" /> to compare.</param>
        /// <returns><see langword="true" /> if the specified objects are equal; otherwise, <see langword="false" />.</returns>
        public bool Equals(ParallelQuery<IEnumerable<int>> x, ParallelQuery<IEnumerable<int>> y)
        {
            return x.SelectMany(a => a.Select(b => b)).SequenceEqual(y.SelectMany(a => a.Select(b => b)));
        }






        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object" /> for which a hash code is to be returned.</param>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public int GetHashCode(ParallelQuery<IEnumerable<int>> obj)
        {
            unchecked
            {
                return obj.SelectMany(o => o.Select(x => x)).Aggregate(17, (a, b) => (a + b) * 23);
            }
        }

        #endregion
    }

    /// <summary>
    /// Class ParellelHashCode64Equality.
    /// Implements the <see cref="System.Collections.Generic.IEqualityComparer{System.Linq.ParallelQuery{System.Collections.Generic.IEnumerable{System.Int64}}}" />
    /// </summary>
    /// <seealso cref="System.Collections.Generic.IEqualityComparer{System.Linq.ParallelQuery{System.Collections.Generic.IEnumerable{System.Int64}}}" />
    [Serializable]
    public class ParellelHashCode64Equality : IEqualityComparer<ParallelQuery<IEnumerable<long>>>
    {
        #region Methods







        /// <summary>
        /// Determines whether the specified objects are equal.
        /// </summary>
        /// <param name="x">The first object of type <paramref name="T" /> to compare.</param>
        /// <param name="y">The second object of type <paramref name="T" /> to compare.</param>
        /// <returns><see langword="true" /> if the specified objects are equal; otherwise, <see langword="false" />.</returns>
        public bool Equals(ParallelQuery<IEnumerable<long>> x, ParallelQuery<IEnumerable<long>> y)
        {
            return x.SelectMany(a => a.Select(b => b)).SequenceEqual(y.SelectMany(a => a.Select(b => b)));
        }






        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object" /> for which a hash code is to be returned.</param>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public int GetHashCode(ParallelQuery<IEnumerable<long>> obj)
        {
            unchecked
            {
                return obj.SelectMany(o => o.Select(x => x)).Aggregate(17, (a, b) => (a + (int)b) * 23);
            }
        }

        #endregion
    }
}
