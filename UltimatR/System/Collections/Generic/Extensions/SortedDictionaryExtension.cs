/// <summary>
/// The Generic namespace.
/// </summary>
namespace System.Collections.Generic
{
    /// <summary>
    /// Class SortedDictionaryExtension.
    /// </summary>
    public static class SortedDictionaryExtension
    {
        #region Methods

        /// <summary>
        /// Adds the dictionary.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="S"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="collection">The collection.</param>
        /// <exception cref="System.ArgumentNullException">Collection is null</exception>
        public static void AddDictionary<T, S>(this SortedDictionary<T, S> source, IDictionary<T, S> collection)
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
        /// Adds the or update.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="S"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="Key">The key.</param>
        /// <param name="Value">The value.</param>
        /// <param name="func">The function.</param>
        /// <returns>S.</returns>
        /// <exception cref="System.ArgumentNullException">Collection is null</exception>
        public static S AddOrUpdate<T, S>(this SortedDictionary<T, S> source, T Key, S Value, Func<T, S, S> func)
        {
            if (Key == null || Value == null)
                throw new ArgumentNullException("Collection is null");

            if (source.ContainsKey(Key))
                return source[Key] = func(Key, Value);
            else
            {
                source.Add(Key, Value);
                return Value;
            }
        }


        /// <summary>
        /// Adds the or update range.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="S"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="collection">The collection.</param>
        /// <exception cref="System.ArgumentNullException">Collection is null</exception>
        public static void AddOrUpdateRange<T, S>(this SortedDictionary<T, S> source, IDictionary<T, S> collection)
        {
            if (collection == null)
                throw new ArgumentNullException("Collection is null");

            foreach (var item in collection)
            {
                source.AddOrUpdate(item.Key, item.Value, (k, v) => v = item.Value);
            }
        }

        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="S"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="collection">The collection.</param>
        /// <exception cref="System.ArgumentNullException">Collection is null</exception>
        public static void AddRange<T, S>(this SortedDictionary<T, S> source, IDictionary<T, S> collection)
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
        /// <returns>SortedDictionary&lt;T, S&gt;.</returns>
        /// <exception cref="System.ArgumentNullException">Collection is null</exception>
        public static SortedDictionary<T, S> AddRangeLog<T, S>(this SortedDictionary<T, S> source, IDictionary<T, S> collection)
        {
            if (collection == null)
                throw new ArgumentNullException("Collection is null");
            SortedDictionary<T, S> result = new SortedDictionary<T, S>();
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
        public static S Get<T, S>(this SortedDictionary<T, S> source, T key)
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
        /// <returns>SortedDictionary&lt;T, S&gt;.</returns>
        /// <exception cref="System.ArgumentNullException">Collection is null</exception>
        public static SortedDictionary<T, S> GetDictionary<T, S>(this SortedDictionary<T, S> source, T key)
        {
            if (key == null)
                throw new ArgumentNullException("Collection is null");
            SortedDictionary<T, S> result = new SortedDictionary<T, S>();
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
        /// <returns>SortedDictionary&lt;T, S&gt;.</returns>
        /// <exception cref="System.ArgumentNullException">Collection is null</exception>
        public static SortedDictionary<T, S> GetRange<T, S>(this SortedDictionary<T, S> source, IDictionary<T, S> collection)
        {
            if (collection == null)
                throw new ArgumentNullException("Collection is null");
            SortedDictionary<T, S> result = new SortedDictionary<T, S>();
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
        public static List<S> GetRange<T, S>(this SortedDictionary<T, S> source, IList<T> collection)
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
        /// <returns>SortedDictionary&lt;T, S&gt;[].</returns>
        /// <exception cref="System.ArgumentNullException">Collection is null</exception>
        public static SortedDictionary<T, S>[] GetRangeLog<T, S>(this SortedDictionary<T, S> source, IDictionary<T, S> collection)
        {
            if (collection == null)
                throw new ArgumentNullException("Collection is null");
            SortedDictionary<T, S>[] result = new SortedDictionary<T, S>[2];
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
        /// <param name="key2">The key2.</param>
        /// <param name="item">The item.</param>
        /// <exception cref="System.ArgumentNullException">Collection is null</exception>
        public static void Put<T, S>(this SortedDictionary<T, Dictionary<T, S>> source, T key, T key2, S item)
        {
            if (key == null || item == null)
                throw new ArgumentNullException("Collection is null");

            if (!source.ContainsKey(key))
                source.Add(key, new Dictionary<T, S>() { { key2, item } });
            else if (!source[key].ContainsKey(key2))
                source[key].Add(key2, item);
            else
                source[key][key2] = item;
        }


        /// <summary>
        /// Puts the specified key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="S"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="key">The key.</param>
        /// <param name="item">The item.</param>
        /// <exception cref="System.ArgumentNullException">Collection is null</exception>
        public static void Put<T, S>(this SortedDictionary<T, S> source, T key, S item)
        {
            if (key == null || item == null)
                throw new ArgumentNullException("Collection is null");

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
        public static void PutRange<T, S>(this SortedDictionary<T, S> source, IDictionary<T, S> collection)
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
        public static void RemoveRange<T, S>(this SortedDictionary<T, S> source, IDictionary<T, S> collection)
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
        public static void RemoveRange<T, S>(this SortedDictionary<T, S> source, IList<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException("Collection is null");

            foreach (var item in collection)
            {
                if (source.ContainsKey(item))
                    source.Remove(item);
            }
        }

        /// <summary>
        /// Tries the add.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="S"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="Key">The key.</param>
        /// <param name="Value">The value.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        /// <exception cref="System.ArgumentNullException">Collection is null</exception>
        public static bool TryAdd<T, S>(this SortedDictionary<T, S> source, T Key, S Value)
        {
            if (Key == null || Value == null)
                throw new ArgumentNullException("Collection is null");

            if (source.ContainsKey(Key))
                return false;
            else
                source.Add(Key, Value);
            return true;
        }

        /// <summary>
        /// Tries the remove.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="S"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="Key">The key.</param>
        /// <param name="Value">The value.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        /// <exception cref="System.ArgumentNullException">Collection is null</exception>
        public static bool TryRemove<T, S>(this SortedDictionary<T, S> source, T Key, out S Value)
        {
            if (Key == null)
                throw new ArgumentNullException("Collection is null");

            if (source.TryGetValue(Key, out Value))
            {
                source.Remove(Key);
                return true;
            }
            return false;
        }

        #endregion
    }
}
