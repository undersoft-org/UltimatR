// ***********************************************************************
// Assembly         : UltimatR.Core
// Authors          : darisuz.hanc < undersoft.org >
// Participants
// Patronate        : m.krzetowski (architect), k.reszka (team-leader)
// Contribution     : d.hanc (r&d.soft.developer), p.grys (senior.soft.engineer)
// Development      : p.gasowski (jr.soft.developer)
// Business         : k.golos (po) m.rafalski (pm), m.korzeniewski (analyst) 
// QA               : a.urbanek
/// <summary>
/// The Generic namespace.
/// </summary>
namespace System.Collections.Generic
{
    using System.Collections.Concurrent;

    /// <summary>
    /// Class ConcurrentDictionaryExtension.
    /// </summary>
    public static class ConcurrentDictionaryExtension
    {
        #region Methods

        /// <summary>
        /// Adds the specified collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="S"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="collection">The collection.</param>
        /// <returns>ConcurrentDictionary&lt;T, S&gt;.</returns>
        /// <exception cref="System.ArgumentNullException">Collection is null</exception>
        public static ConcurrentDictionary<T, S> Add<T, S>(this ConcurrentDictionary<T, S> source, IDictionary<T, S> collection)
        {
            if (collection == null)
                throw new ArgumentNullException("Collection is null");
            ConcurrentDictionary<T, S> result = new ConcurrentDictionary<T, S>();
            foreach (var item in collection)
            {
                if (!source.ContainsKey(item.Key))
                    source.AddOrUpdate(item.Key, item.Value, (k, v) => item.Value);
                else
                    result.AddOrUpdate(item.Key, item.Value, (k, v) => item.Value);
            }
            return result;
        }

        /// <summary>
        /// Puts the specified key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="S"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="key">The key.</param>
        /// <param name="key2">The key2.</param>
        /// <param name="item">The item.</param>
        /// <exception cref="System.ArgumentNullException">Collection is null</exception>
        public static void Put<T, S>(this ConcurrentDictionary<T, Dictionary<T, S>> source, T key, T key2, S item)
        {
            if (key == null || item == null)
                throw new ArgumentNullException("Collection is null");

            if (!source.TryAdd(key, new Dictionary<T, S>() { { key2, item } }))
            {
                if (!source[key].ContainsKey(key2))
                    source[key].Add(key2, item);
                else
                    source[key][key2] = item;
            }
        }

        /// <summary>
        /// Puts the specified collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="S"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="collection">The collection.</param>
        /// <exception cref="System.ArgumentNullException">Collection is null</exception>
        public static void Put<T, S>(this ConcurrentDictionary<T, S> source, IDictionary<T, S> collection)
        {
            if (collection == null)
                throw new ArgumentNullException("Collection is null");

            foreach (var item in collection)
            {
                source.AddOrUpdate(item.Key, item.Value, (k, v) => v = item.Value);
            }
        }

        #endregion
    }
}
