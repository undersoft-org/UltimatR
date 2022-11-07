
// <copyright file="IDeck.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The Series namespace.
/// </summary>
namespace System.Series
{
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    /// <summary>
    /// Interface IDeck
    /// Implements the <see cref="System.Collections.Generic.IEnumerable{V}" />
    /// Implements the <see cref="System.Collections.IEnumerable" />
    /// Implements the <see cref="System.Collections.ICollection" />
    /// Implements the <see cref="System.Collections.Generic.ICollection{V}" />
    /// Implements the <see cref="System.Collections.Generic.IList{V}" />
    /// Implements the <see cref="System.Collections.Concurrent.IProducerConsumerCollection{V}" />
    /// Implements the <see cref="System.IDisposable" />
    /// Implements the <see cref="System.IUnique" />
    /// Implements the <see cref="System.Series.IFindable{V}" />
    /// </summary>
    /// <typeparam name="V"></typeparam>
    /// <seealso cref="System.Collections.Generic.IEnumerable{V}" />
    /// <seealso cref="System.Collections.IEnumerable" />
    /// <seealso cref="System.Collections.ICollection" />
    /// <seealso cref="System.Collections.Generic.ICollection{V}" />
    /// <seealso cref="System.Collections.Generic.IList{V}" />
    /// <seealso cref="System.Collections.Concurrent.IProducerConsumerCollection{V}" />
    /// <seealso cref="System.IDisposable" />
    /// <seealso cref="System.IUnique" />
    /// <seealso cref="System.Series.IFindable{V}" />
    public interface IDeck<V> : IEnumerable<V>, IEnumerable, ICollection, ICollection<V>, IList<V>, 
                                IProducerConsumerCollection<V>, IDisposable, IUnique, IFindable<V>                            
    {


        /// <summary>
        /// Gets the first.
        /// </summary>
        /// <value>The first.</value>
        ICard<V> First { get; }
        /// <summary>
        /// Gets the last.
        /// </summary>
        /// <value>The last.</value>
        ICard<V> Last { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is repeatable.
        /// </summary>
        /// <value><c>true</c> if this instance is repeatable; otherwise, <c>false</c>.</value>
        bool IsRepeatable { get; }

        /// <summary>
        /// Nexts the specified card.
        /// </summary>
        /// <param name="card">The card.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        ICard<V> Next(ICard<V> card);

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>The count.</value>
        new int Count { get; }
        /// <summary>
        /// Gets or sets the minimum count.
        /// </summary>
        /// <value>The minimum count.</value>
        int MinCount { get; set; }

        /// <summary>
        /// Determines whether the specified key contains key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns><c>true</c> if the specified key contains key; otherwise, <c>false</c>.</returns>
        bool ContainsKey(ulong key);
        /// <summary>
        /// Determines whether the specified key contains key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns><c>true</c> if the specified key contains key; otherwise, <c>false</c>.</returns>
        bool ContainsKey(object key);
        /// <summary>
        /// Determines whether the specified key contains key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns><c>true</c> if the specified key contains key; otherwise, <c>false</c>.</returns>
        bool ContainsKey(IUnique key);

        /// <summary>
        /// Determines whether this instance contains the object.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns><c>true</c> if [contains] [the specified item]; otherwise, <c>false</c>.</returns>
        bool Contains(ICard<V> item);
        /// <summary>
        /// Determines whether this instance contains the object.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns><c>true</c> if [contains] [the specified item]; otherwise, <c>false</c>.</returns>
        bool Contains(IUnique<V> item);
        /// <summary>
        /// Determines whether this instance contains the object.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="item">The item.</param>
        /// <returns><c>true</c> if [contains] [the specified key]; otherwise, <c>false</c>.</returns>
        bool Contains(ulong key, V item);

        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>V.</returns>
        V Get(object key);
        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>V.</returns>
        V Get(ulong key);
        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>V.</returns>
        V Get(IUnique key);
        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>V.</returns>
        V Get(IUnique<V> key);

        /// <summary>
        /// Tries the get.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="output">The output.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool TryGet(object key, out ICard<V> output);
        /// <summary>
        /// Tries the get.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="output">The output.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool TryGet(object key, out V output);
        /// <summary>
        /// Tries the get.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="output">The output.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool TryGet(ulong key, out V output);
        /// <summary>
        /// Tries the get.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="output">The output.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool TryGet(IUnique key, out ICard<V> output);
        /// <summary>
        /// Tries the get.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="output">The output.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool TryGet(IUnique<V> key, out ICard<V> output);

        /// <summary>
        /// Gets the card.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        ICard<V> GetCard(object key);
        /// <summary>
        /// Gets the card.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        ICard<V> GetCard(ulong key);
        /// <summary>
        /// Gets the card.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        ICard<V> GetCard(IUnique key);
        /// <summary>
        /// Gets the card.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        ICard<V> GetCard(IUnique<V> key);

        /// <summary>
        /// Sets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        ICard<V> Set(object key, V value);
        /// <summary>
        /// Sets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        ICard<V> Set(ulong key, V value);
        /// <summary>
        /// Sets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        ICard<V> Set(IUnique key, V value);
        /// <summary>
        /// Sets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        ICard<V> Set(IUnique<V> key, V value);
        /// <summary>
        /// Sets the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        ICard<V> Set(V value);
        /// <summary>
        /// Sets the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        ICard<V> Set(IUnique<V> value);
        /// <summary>
        /// Sets the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        ICard<V> Set(ICard<V> value);
        /// <summary>
        /// Sets the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>System.Int32.</returns>
        int Set(IEnumerable<V> values);
        /// <summary>
        /// Sets the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>System.Int32.</returns>
        int Set(IList<V> values);
        /// <summary>
        /// Sets the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>System.Int32.</returns>
        int Set(IEnumerable<ICard<V>> values);
        /// <summary>
        /// Sets the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>System.Int32.</returns>
        int Set(IEnumerable<IUnique<V>> values);

        /// <summary>
        /// Sures the get.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="sureaction">The sureaction.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        ICard<V> SureGet(object key, Func<ulong, V> sureaction);
        /// <summary>
        /// Sures the get.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="sureaction">The sureaction.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        ICard<V> SureGet(ulong key, Func<ulong, V> sureaction);
        /// <summary>
        /// Sures the get.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="sureaction">The sureaction.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        ICard<V> SureGet(IUnique key, Func<ulong, V> sureaction);
        /// <summary>
        /// Sures the get.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="sureaction">The sureaction.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        ICard<V> SureGet(IUnique<V> key, Func<ulong, V> sureaction);

        /// <summary>
        /// News this instance.
        /// </summary>
        /// <returns>ICard&lt;V&gt;.</returns>
        ICard<V> New();
        /// <summary>
        /// News the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        ICard<V> New(ulong key);
        /// <summary>
        /// News the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        ICard<V> New(object key);

        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool Add(object key, V value);
        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool Add(ulong key, V value);
        /// <summary>
        /// Adds the specified card.
        /// </summary>
        /// <param name="card">The card.</param>
        void Add(ICard<V> card);
        /// <summary>
        /// Adds the specified card list.
        /// </summary>
        /// <param name="cardList">The card list.</param>
        void Add(IList<ICard<V>> cardList);
        /// <summary>
        /// Adds the specified cards.
        /// </summary>
        /// <param name="cards">The cards.</param>
        void Add(IEnumerable<ICard<V>> cards);
        /// <summary>
        /// Adds the specified cards.
        /// </summary>
        /// <param name="cards">The cards.</param>
        void Add(IList<V> cards);
        /// <summary>
        /// Adds the specified cards.
        /// </summary>
        /// <param name="cards">The cards.</param>
        void Add(IEnumerable<V> cards);
        /// <summary>
        /// Adds the specified cards.
        /// </summary>
        /// <param name="cards">The cards.</param>
        void Add(IUnique<V> cards);
        /// <summary>
        /// Adds the specified cards.
        /// </summary>
        /// <param name="cards">The cards.</param>
        void Add(IList<IUnique<V>> cards);
        /// <summary>
        /// Adds the specified cards.
        /// </summary>
        /// <param name="cards">The cards.</param>
        void Add(IEnumerable<IUnique<V>> cards);

        /// <summary>
        /// Enqueues the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool Enqueue(object key, V value);
        /// <summary>
        /// Enqueues the specified card.
        /// </summary>
        /// <param name="card">The card.</param>
        void Enqueue(ICard<V> card);
        /// <summary>
        /// Enqueues the specified card.
        /// </summary>
        /// <param name="card">The card.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool Enqueue(V card);

        /// <summary>
        /// Dequeues this instance.
        /// </summary>
        /// <returns>V.</returns>
        V Dequeue();
        /// <summary>
        /// Tries the dequeue.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool TryDequeue(out ICard<V> item);
        /// <summary>
        /// Tries the dequeue.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool TryDequeue(out V item);
        /// <summary>
        /// Tries the take.
        /// </summary>
        /// <param name="item">When this method returns, if the object was removed and returned successfully, <paramref name="item" /> contains the removed object. If no object was available to be removed, the value is unspecified.</param>
        /// <returns><see langword="true" /> if an object was removed and returned successfully; otherwise, <see langword="false" />.</returns>
        new bool TryTake(out V item);

        /// <summary>
        /// Tries the pick.
        /// </summary>
        /// <param name="skip">The skip.</param>
        /// <param name="output">The output.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool TryPick(int skip, out V output);

        /// <summary>
        /// Puts the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        ICard<V> Put(object key, V value);
        /// <summary>
        /// Puts the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        ICard<V> Put(ulong key, V value);
        /// <summary>
        /// Puts the specified card.
        /// </summary>
        /// <param name="card">The card.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        ICard<V> Put(ICard<V> card);
        /// <summary>
        /// Puts the specified card list.
        /// </summary>
        /// <param name="cardList">The card list.</param>
        void Put(IList<ICard<V>> cardList);
        /// <summary>
        /// Puts the specified cards.
        /// </summary>
        /// <param name="cards">The cards.</param>
        void Put(IEnumerable<ICard<V>> cards);
        /// <summary>
        /// Puts the specified cards.
        /// </summary>
        /// <param name="cards">The cards.</param>
        void Put(IList<V> cards);
        /// <summary>
        /// Puts the specified cards.
        /// </summary>
        /// <param name="cards">The cards.</param>
        void Put(IEnumerable<V> cards);
        /// <summary>
        /// Puts the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        ICard<V> Put(V value);
        /// <summary>
        /// Puts the specified cards.
        /// </summary>
        /// <param name="cards">The cards.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        ICard<V> Put(IUnique<V> cards);
        /// <summary>
        /// Puts the specified cards.
        /// </summary>
        /// <param name="cards">The cards.</param>
        void Put(IList<IUnique<V>> cards);
        /// <summary>
        /// Puts the specified cards.
        /// </summary>
        /// <param name="cards">The cards.</param>
        void Put(IEnumerable<IUnique<V>> cards);

        /// <summary>
        /// Removes the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>V.</returns>
        V Remove(object key);
        /// <summary>
        /// Removes the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="item">The item.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool Remove(object key, V item);
        /// <summary>
        /// Removes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool Remove(ICard<V> item);
        /// <summary>
        /// Removes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool Remove(IUnique<V> item);
        /// <summary>
        /// Tries the remove.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool TryRemove(object key);

        /// <summary>
        /// Renews the specified cards.
        /// </summary>
        /// <param name="cards">The cards.</param>
        void Renew(IEnumerable<V> cards);
        /// <summary>
        /// Renews the specified cards.
        /// </summary>
        /// <param name="cards">The cards.</param>
        void Renew(IList<V> cards);
        /// <summary>
        /// Renews the specified cards.
        /// </summary>
        /// <param name="cards">The cards.</param>
        void Renew(IList<ICard<V>> cards);
        /// <summary>
        /// Renews the specified cards.
        /// </summary>
        /// <param name="cards">The cards.</param>
        void Renew(IEnumerable<ICard<V>> cards);

        /// <summary>
        /// Converts to array.
        /// </summary>
        /// <returns>A new array containing the elements copied from the <see cref="T:System.Collections.Concurrent.IProducerConsumerCollection`1" />.</returns>
        new V[] ToArray();

        /// <summary>
        /// Ases the cards.
        /// </summary>
        /// <returns>IEnumerable&lt;ICard&lt;V&gt;&gt;.</returns>
        IEnumerable<ICard<V>> AsCards();

        /// <summary>
        /// Ases the values.
        /// </summary>
        /// <returns>IEnumerable&lt;V&gt;.</returns>
        IEnumerable<V> AsValues();

        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="arrayIndex">Index of the array.</param>
        new void CopyTo(Array array, int arrayIndex);

        /// <summary>
        /// Gets or sets a value indicating whether this instance is synchronized.
        /// </summary>
        /// <value><c>true</c> if this instance is synchronized; otherwise, <c>false</c>.</value>
        new bool IsSynchronized { get; set; }
        /// <summary>
        /// Gets or sets the synchronize root.
        /// </summary>
        /// <value>The synchronize root.</value>
        new object SyncRoot { get; set; }

        /// <summary>
        /// Creates new card.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        ICard<V> NewCard(V value);
        /// <summary>
        /// Creates new card.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        ICard<V> NewCard(object key, V value);
        /// <summary>
        /// Creates new card.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        ICard<V> NewCard(ulong key, V value);
        /// <summary>
        /// Creates new card.
        /// </summary>
        /// <param name="card">The card.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        ICard<V> NewCard(ICard<V> card);

        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="destIndex">Index of the dest.</param>
        void CopyTo(ICard<V>[] array, int destIndex);
        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="arrayIndex">Index of the array.</param>
        void CopyTo(IUnique<V>[] array, int arrayIndex);


        /// <summary>
        /// Clears this instance.
        /// </summary>
        new void Clear();

        /// <summary>
        /// Resizes the specified size.
        /// </summary>
        /// <param name="size">The size.</param>
        void Resize(int size);

        /// <summary>
        /// Flushes this instance.
        /// </summary>
        void Flush();
    }
}
