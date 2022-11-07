/// <summary>
/// The Generic namespace.
/// </summary>
namespace System.Collections.Generic
{
    using System;



    /// <summary>
    /// Class SortedListExtension.
    /// </summary>
    public static class SortedListExtension
    {
        #region Methods

        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="S"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="collection">The collection.</param>
        /// <exception cref="System.ArgumentNullException">Collection is null</exception>
        public static void AddRange<T, S>(this SortedList<T, S> source, IDictionary<T, S> collection)
        {
            if (collection == null)
                throw new ArgumentNullException("Collection is null");

            foreach (var item in collection)
            {
                if (!source.ContainsKey(item.Key))
                    source.Add(item.Key, item.Value);
            }
        }








        /// <summary>
        /// Adds the range log.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="S"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="collection">The collection.</param>
        /// <returns>SortedList&lt;T, S&gt;.</returns>
        /// <exception cref="System.ArgumentNullException">Collection is null</exception>
        public static SortedList<T, S> AddRangeLog<T, S>(this SortedList<T, S> source, IDictionary<T, S> collection)
        {
            if (collection == null)
                throw new ArgumentNullException("Collection is null");
            SortedList<T, S> result = new SortedList<T, S>();
            foreach (var item in collection)
            {
                if (!source.ContainsKey(item.Key))
                    source.Add(item.Key, item.Value);
                else
                    result.Add(item.Key, item.Value);
            }
            return result;
        }









        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="S"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="key">The key.</param>
        /// <returns>S.</returns>
        /// <exception cref="System.ArgumentNullException">Collection is null</exception>
        public static S Get<T, S>(this SortedList<T, S> source, T key)
        {
            if (key == null)
                throw new ArgumentNullException("Collection is null");
            S result = default(S);
            if (source.ContainsKey(key))
                result = source[key];

            return result;
        }









        /// <summary>
        /// Gets the dictionary.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="S"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="key">The key.</param>
        /// <returns>SortedList&lt;T, S&gt;.</returns>
        /// <exception cref="System.ArgumentNullException">Collection is null</exception>
        public static SortedList<T, S> GetDictionary<T, S>(this SortedList<T, S> source, T key)
        {
            if (key == null)
                throw new ArgumentNullException("Collection is null");
            SortedList<T, S> result = new SortedList<T, S>();
            if (source.ContainsKey(key))
                result.Add(key, source[key]);

            return result;
        }









        /// <summary>
        /// Gets the range.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="S"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="collection">The collection.</param>
        /// <returns>SortedList&lt;T, S&gt;.</returns>
        /// <exception cref="System.ArgumentNullException">Collection is null</exception>
        public static SortedList<T, S> GetRange<T, S>(this SortedList<T, S> source, IDictionary<T, S> collection)
        {
            if (collection == null)
                throw new ArgumentNullException("Collection is null");
            SortedList<T, S> result = new SortedList<T, S>();
            foreach (var item in collection)
            {
                if (source.ContainsKey(item.Key))
                    result.Add(item.Key, source[item.Key]);
            }
            return result;
        }









        /// <summary>
        /// Gets the range.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="S"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="collection">The collection.</param>
        /// <returns>List&lt;S&gt;.</returns>
        /// <exception cref="System.ArgumentNullException">Collection is null</exception>
        public static List<S> GetRange<T, S>(this SortedList<T, S> source, IList<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException("Collection is null");
            List<S> result = new List<S>();
            foreach (var item in collection)
            {
                if (source.ContainsKey(item))
                    result.Add(source[item]);
            }
            return result;
        }









        /// <summary>
        /// Gets the range log.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="S"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="collection">The collection.</param>
        /// <returns>SortedList&lt;T, S&gt;[].</returns>
        /// <exception cref="System.ArgumentNullException">Collection is null</exception>
        public static SortedList<T, S>[] GetRangeLog<T, S>(this SortedList<T, S> source, IDictionary<T, S> collection)
        {
            if (collection == null)
                throw new ArgumentNullException("Collection is null");
            SortedList<T, S>[] result = new SortedList<T, S>[2];
            foreach (var item in collection)
            {
                if (source.ContainsKey(item.Key))
                    result[0].Add(item.Key, source[item.Key]);
                else
                    result[1].Add(item.Key, item.Value);
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
        /// <param name="item">The item.</param>
        public static void Put<T, S>(this SortedList<T, S> source, T key, S item)
        {
            if (!source.ContainsKey(key))
                source.Add(key, item);
            else
                source[key] = item;
        }








        /// <summary>
        /// Puts the range.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="S"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="collection">The collection.</param>
        /// <exception cref="System.ArgumentNullException">Collection is null</exception>
        public static void PutRange<T, S>(this SortedList<T, S> source, IDictionary<T, S> collection)
        {
            if (collection == null)
                throw new ArgumentNullException("Collection is null");
            foreach (var item in collection)
            {
                if (!source.ContainsKey(item.Key))
                    source.Add(item.Key, item.Value);
                else
                    source[item.Key] = item.Value;
            }
        }








        /// <summary>
        /// Removes the range.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="S"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="collection">The collection.</param>
        /// <exception cref="System.ArgumentNullException">Collection is null</exception>
        public static void RemoveRange<T, S>(this SortedList<T, S> source, IDictionary<T, S> collection)
        {
            if (collection == null)
                throw new ArgumentNullException("Collection is null");

            foreach (var item in collection)
            {
                if (source.ContainsKey(item.Key))
                    source.Remove(item.Key);
            }
        }








        /// <summary>
        /// Removes the range.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="S"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="collection">The collection.</param>
        /// <exception cref="System.ArgumentNullException">Collection is null</exception>
        public static void RemoveRange<T, S>(this SortedList<T, S> source, IList<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException("Collection is null");

            foreach (var item in collection)
            {
                if (source.ContainsKey(item))
                    source.Remove(item);
            }
        }

        #endregion
    }
}
