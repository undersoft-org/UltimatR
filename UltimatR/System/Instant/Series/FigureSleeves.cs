
// <copyright file="FigureSleeves.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The Instant namespace.
/// </summary>
namespace System.Instant
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Instant.Linking;
    using System.Instant.Treatments;
    using System.IO;
    using System.Linq;
    using System.Series;
    using System.Uniques;

    /// <summary>
    /// Class FigureSleeves.
    /// Implements the <see cref="System.Instant.ISleeves" />
    /// </summary>
    /// <seealso cref="System.Instant.ISleeves" />
    public abstract class FigureSleeves : ISleeves
    {
        /// <summary>
        /// Gets or sets the instant.
        /// </summary>
        /// <value>The instant.</value>
        public IInstant Instant { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="IFigures" /> is prime.
        /// </summary>
        /// <value><c>true</c> if prime; otherwise, <c>false</c>.</value>
        public bool Prime { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="System.Object" /> at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>System.Object.</returns>
        public abstract object this[int index, string propertyName] { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="System.Object" /> at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="fieldId">The field identifier.</param>
        /// <returns>System.Object.</returns>
        public abstract object this[int index, int fieldId] { get; set; }

        /// <summary>
        /// Gets or sets the sleeves.
        /// </summary>
        /// <value>The sleeves.</value>
        public abstract IFigures Sleeves { get; set; }

        /// <summary>
        /// Gets or sets the figures.
        /// </summary>
        /// <value>The figures.</value>
        public abstract IFigures Figures { get; set; }

        /// <summary>
        /// Gets or sets the view.
        /// </summary>
        /// <value>The view.</value>
        public IQueryable<IFigure> View { get => Sleeves.View; set => Sleeves.View = value; }

        /// <summary>
        /// Gets or sets the filter.
        /// </summary>
        /// <value>The filter.</value>
        public FigureFilter Filter { get => Sleeves.Filter; set => Sleeves.Filter = value; }
        /// <summary>
        /// Gets or sets the sort.
        /// </summary>
        /// <value>The sort.</value>
        public FigureSort Sort { get => Sleeves.Sort; set => Sleeves.Sort = value; }
        /// <summary>
        /// Gets or sets the predicate.
        /// </summary>
        /// <value>The predicate.</value>
        public Func<IFigure, bool> Predicate { get => Sleeves.Predicate; set => Sleeves.Predicate = value; }

        /// <summary>
        /// Gets a value indicating whether this instance is repeatable.
        /// </summary>
        /// <value><c>true</c> if this instance is repeatable; otherwise, <c>false</c>.</value>
        public virtual bool IsRepeatable { get => false; }

        /// <summary>
        /// Serializes the specified stream.
        /// </summary>
        /// <param name="tostream">The tostream.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="batchSize">Size of the batch.</param>
        /// <param name="serialFormat">The serial format.</param>
        /// <returns>System.Int32.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public int Serialize(Stream tostream, int offset, int batchSize, SerialFormat serialFormat = SerialFormat.Binary)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Serializes the specified buffer.
        /// </summary>
        /// <param name="buffor">The buffor.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="batchSize">Size of the batch.</param>
        /// <param name="serialFormat">The serial format.</param>
        /// <returns>System.Int32.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public int Serialize(ISerialBuffer buffor, int offset, int batchSize, SerialFormat serialFormat = SerialFormat.Binary)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Deserializes the specified stream.
        /// </summary>
        /// <param name="fromstream">The fromstream.</param>
        /// <param name="serialFormat">The serial format.</param>
        /// <returns>System.Object.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public object Deserialize(Stream fromstream, SerialFormat serialFormat = SerialFormat.Binary)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Deserializes the specified buffer.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="serialFormat">The serial format.</param>
        /// <returns>System.Object.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public object Deserialize(ISerialBuffer buffer, SerialFormat serialFormat = SerialFormat.Binary)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <returns>System.Object[].</returns>
        public object[] GetMessage()
        {
            return new[] { this };
        }
        /// <summary>
        /// Gets the header.
        /// </summary>
        /// <returns>System.Object.</returns>
        public object GetHeader()
        {
            return Figures;
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            Sleeves.Clear();
        }

        /// <summary>
        /// Flushes this instance.
        /// </summary>
        public void Flush()
        {
            Sleeves.Flush();
        }

        /// <summary>
        /// Creates new figure.
        /// </summary>
        /// <returns>IFigure.</returns>
        public IFigure NewFigure()
        {
            return Figures.NewFigure();
        }

        /// <summary>
        /// Creates new sleeve.
        /// </summary>
        /// <returns>ISleeve.</returns>
        public ISleeve NewSleeve()
        {
            return Figures.NewSleeve();
        }

        /// <summary>
        /// Nexts the specified card.
        /// </summary>
        /// <param name="card">The card.</param>
        /// <returns>ICard&lt;IFigure&gt;.</returns>
        public ICard<IFigure> Next(ICard<IFigure> card)
        {
            return Sleeves.Next(card);
        }

        /// <summary>
        /// Determines whether the specified key contains key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns><c>true</c> if the specified key contains key; otherwise, <c>false</c>.</returns>
        public bool ContainsKey(object key)
        {
            return Sleeves.ContainsKey(key);
        }
        /// <summary>
        /// Determines whether the specified key contains key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns><c>true</c> if the specified key contains key; otherwise, <c>false</c>.</returns>
        public bool ContainsKey(IUnique key)
        {
            return Sleeves.ContainsKey(key);
        }
        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1" /> contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        /// <returns><see langword="true" /> if <paramref name="item" /> is found in the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, <see langword="false" />.</returns>
        public bool Contains(IFigure item)
        {
            return Sleeves.Contains(item);
        }
        /// <summary>
        /// Determines whether this instance contains the object.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns><c>true</c> if [contains] [the specified item]; otherwise, <c>false</c>.</returns>
        public bool Contains(ICard<IFigure> item)
        {
            return Sleeves.Contains(item);
        }
        /// <summary>
        /// Determines whether this instance contains the object.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns><c>true</c> if [contains] [the specified item]; otherwise, <c>false</c>.</returns>
        public bool Contains(IUnique<IFigure> item)
        {
            return Sleeves.Contains(item);
        }
        /// <summary>
        /// Determines whether this instance contains the object.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="item">The item.</param>
        /// <returns><c>true</c> if [contains] [the specified key]; otherwise, <c>false</c>.</returns>
        public virtual bool Contains(ulong key, IFigure item)
        {
            return Sleeves.Contains(key, item);
        }

        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>IFigure.</returns>
        public IFigure Get(object key)
        {
            return Sleeves.Get(key);
        }
        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>IFigure.</returns>
        public IFigure Get(IUnique<IFigure> key)
        {
            return Sleeves.Get(key);
        }
        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>IFigure.</returns>
        public IFigure Get(IUnique key)
        {
            return Sleeves.Get(key);
        }

        /// <summary>
        /// Tries the get.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="output">The output.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool TryGet(object key, out ICard<IFigure> output)
        {
            return Sleeves.TryGet(key, out output);
        }
        /// <summary>
        /// Tries the get.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="output">The output.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool TryGet(object key, out IFigure output)
        {
            return Sleeves.TryGet(key, out output);
        }

        /// <summary>
        /// Gets the card.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>ICard&lt;IFigure&gt;.</returns>
        public ICard<IFigure> GetCard(object key)
        {
            return Sleeves.GetCard(key);
        }

        /// <summary>
        /// News this instance.
        /// </summary>
        /// <returns>ICard&lt;IFigure&gt;.</returns>
        public ICard<IFigure> New()
        {
            ICard<IFigure> item = Figures.New();
            if (item != null)
            {
                Sleeves.Add(item);
            }
            return item;
        }
        /// <summary>
        /// News the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>ICard&lt;IFigure&gt;.</returns>
        public ICard<IFigure> New(ulong key)
        {
            ICard<IFigure> item = Figures.New(key);
            if (item != null)
            {
                Sleeves.Add(item);
            }
            return item;
        }
        /// <summary>
        /// News the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>ICard&lt;IFigure&gt;.</returns>
        public ICard<IFigure> New(object key)
        {
            ICard<IFigure> item = Figures.New();
            if (item != null)
            {
                Sleeves.Add(item);
            }
            return item;
        }

        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="item">The item.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool Add(ulong key, IFigure item)
        {
            IFigure _item;
            if (Figures.TryGet(key, out _item))
            {
                if (!ReferenceEquals(_item, item))
                {
                    _item.ValueArray = item.ValueArray;
                    _item.UniqueKey = item.UniqueKey;
                }
                return Sleeves.Add(key, _item);
            }
            else
            {
                return Sleeves.TryAdd(Figures.Put(item).Value);
            }
        }
        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        public void Add(IFigure item)
        {
            ulong key = item.UniqueKey;
            IFigure _item;
            if (Figures.TryGet(key, out _item))
            {
                if (!ReferenceEquals(_item, item))
                {
                    _item.ValueArray = item.ValueArray;
                    _item.UniqueKey = item.UniqueKey;
                }
                Sleeves.Add(key, _item);
            }
            else
            {
                Sleeves.Add(Figures.Put(item).Value);
            }

        }
        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="item">The item.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool Add(object key, IFigure item)
        {
            IFigure _item;
            ulong _key = key.UniqueKey64();
            if (Figures.TryGet(_key, out _item))
            {
                if (!ReferenceEquals(_item, item))
                {
                    _item.ValueArray = item.ValueArray;
                    _item.UniqueKey = item.UniqueKey;
                }
                return Sleeves.Add(_key, _item);
            }
            else
                return Sleeves.TryAdd(Figures.Put(item).Value);
        }
        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Add(ICard<IFigure> item)
        {
            ulong key = item.Key;
            ICard<IFigure> _item;
            if (Figures.TryGet(key, out _item))
            {
                if (!ReferenceEquals(_item.Value, item.Value))
                {
                    _item.Value.ValueArray = item.Value.ValueArray;
                    _item.UniqueKey = item.UniqueKey;
                }
                Sleeves.Add(_item);
            }
            else
                Sleeves.Add(Figures.Put(item));
        }
        /// <summary>
        /// Adds the specified card list.
        /// </summary>
        /// <param name="cardList">The card list.</param>
        public void Add(IList<ICard<IFigure>> cardList)
        {
            foreach (var card in cardList)
                Sleeves.Add(card);
        }
        /// <summary>
        /// Adds the specified cards.
        /// </summary>
        /// <param name="cards">The cards.</param>
        public void Add(IEnumerable<ICard<IFigure>> cards)
        {
            foreach (var card in cards)
                Add(card);
        }
        /// <summary>
        /// Adds the specified cards.
        /// </summary>
        /// <param name="cards">The cards.</param>
        public void Add(IList<IFigure> cards)
        {
            foreach (var card in cards)
                Add(card);
        }
        /// <summary>
        /// Adds the specified cards.
        /// </summary>
        /// <param name="cards">The cards.</param>
        public void Add(IEnumerable<IFigure> cards)
        {
            foreach (var card in cards)
                Add(card);
        }
        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Add(IUnique<IFigure> item)
        {
            IFigure _item;
            if (Figures.TryGet(item, out _item))
            {
                IFigure value = item.UniqueObject;
                if (!ReferenceEquals(_item, value))
                {
                    _item.ValueArray = value.ValueArray;
                    _item.UniqueKey = value.UniqueKey;
                }
                Sleeves.Add(_item);
            }
            else
                Sleeves.Add(Figures.Put(item).Value);
        }
        /// <summary>
        /// Adds the specified items.
        /// </summary>
        /// <param name="items">The items.</param>
        public void Add(IList<IUnique<IFigure>> items)
        {
            foreach (IUnique<IFigure> item in items)
                Add(item);
        }
        /// <summary>
        /// Adds the specified items.
        /// </summary>
        /// <param name="items">The items.</param>
        public void Add(IEnumerable<IUnique<IFigure>> items)
        {
            foreach (IUnique<IFigure> item in items)
                Add(item);
        }
        /// <summary>
        /// Attempts to add an object to the <see cref="T:System.Collections.Concurrent.IProducerConsumerCollection`1" />.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Concurrent.IProducerConsumerCollection`1" />.</param>
        /// <returns><see langword="true" /> if the object was added successfully; otherwise, <see langword="false" />.</returns>
        public bool TryAdd(IFigure item)
        {
            ulong key = item.UniqueKey;
            IFigure _item;
            if (Figures.TryGet(key, out _item))
            {
                if (!ReferenceEquals(_item, item))
                {
                    _item.ValueArray = item.ValueArray;
                    _item.UniqueKey = item.UniqueKey;
                }
                return Sleeves.Add(key, _item);
            }
            else
                return Sleeves.TryAdd(Figures.Put(item).Value);
        }

        /// <summary>
        /// Enqueues the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool Enqueue(object key, IFigure value)
        {
            return Sleeves.Enqueue(key, value);
        }
        /// <summary>
        /// Enqueues the specified card.
        /// </summary>
        /// <param name="card">The card.</param>
        public void Enqueue(ICard<IFigure> card)
        {
            Sleeves.Enqueue(card);
        }
        /// <summary>
        /// Enqueues the specified card.
        /// </summary>
        /// <param name="card">The card.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool Enqueue(IFigure card)
        {
            return Sleeves.Enqueue(card);
        }

        /// <summary>
        /// Dequeues this instance.
        /// </summary>
        /// <returns>IFigure.</returns>
        public IFigure Dequeue()
        {
            return Sleeves.Dequeue();
        }
        /// <summary>
        /// Tries the dequeue.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool TryDequeue(out ICard<IFigure> item)
        {
            return Sleeves.TryDequeue(out item);
        }
        /// <summary>
        /// Tries the dequeue.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool TryDequeue(out IFigure item)
        {
            return Sleeves.TryDequeue(out item);
        }

        /// <summary>
        /// Tries the take.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool TryTake(out IFigure item)
        {
            return Sleeves.TryTake(out item);
        }

        /// <summary>
        /// Puts the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="item">The item.</param>
        /// <returns>ICard&lt;IFigure&gt;.</returns>
        public ICard<IFigure> Put(ulong key, IFigure item)
        {
            IFigure _item;
            if (Figures.TryGet(key, out _item))
            {
                if (!ReferenceEquals(_item, item))
                {
                    _item.ValueArray = item.ValueArray;
                    _item.UniqueKey = item.UniqueKey;
                }
                return Sleeves.Put(key, _item);
            }
            else
                return Sleeves.Put(Figures.Put(key, item).Value);
        }

        /// <summary>
        /// Puts the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="item">The item.</param>
        /// <returns>ICard&lt;IFigure&gt;.</returns>
        public ICard<IFigure> Put(object key, IFigure item)
        {
            ulong _key = key.UniqueKey();
            IFigure _item;
            if (Figures.TryGet(_key, out _item))
            {
                if (!ReferenceEquals(_item, item))
                {
                    _item.ValueArray = item.ValueArray;
                    _item.UniqueKey = item.UniqueKey;
                }
                return Sleeves.Put(_key, _item);
            }
            else
                return Sleeves.Put(Figures.Put(key, item).Value);
        }
        /// <summary>
        /// Puts the specified card.
        /// </summary>
        /// <param name="card">The card.</param>
        /// <returns>ICard&lt;IFigure&gt;.</returns>
        public ICard<IFigure> Put(ICard<IFigure> card)
        {
            IFigure item = card.Value;
            IFigure _item;
            if (Figures.TryGet(card.Key, out _item))
            {
                if (!ReferenceEquals(_item, item))
                {
                    _item.ValueArray = item.ValueArray;
                    _item.UniqueKey = item.UniqueKey;
                }
                return Sleeves.Put(card);
            }
            else
                return Sleeves.Put(Figures.Put(card));
        }

        /// <summary>
        /// Puts the specified card list.
        /// </summary>
        /// <param name="cardList">The card list.</param>
        public void Put(IList<ICard<IFigure>> cardList)
        {
            foreach (var card in cardList)
                Put(card);
        }
        /// <summary>
        /// Puts the specified cards.
        /// </summary>
        /// <param name="cards">The cards.</param>
        public void Put(IEnumerable<ICard<IFigure>> cards)
        {
            foreach (var card in cards)
                Put(card);
        }
        /// <summary>
        /// Puts the specified cards.
        /// </summary>
        /// <param name="cards">The cards.</param>
        public void Put(IList<IFigure> cards)
        {
            foreach (var card in cards)
                Put(card);
        }
        /// <summary>
        /// Puts the specified cards.
        /// </summary>
        /// <param name="cards">The cards.</param>
        public void Put(IEnumerable<IFigure> cards)
        {
            foreach (var card in cards)
                Put(card);
        }
        /// <summary>
        /// Puts the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>ICard&lt;IFigure&gt;.</returns>
        public ICard<IFigure> Put(IFigure item)
        {
            ulong key = item.UniqueKey;
            IFigure _item;
            if (Figures.TryGet(key, out _item))
            {
                if (!ReferenceEquals(_item, item))
                {
                    _item.ValueArray = item.ValueArray;
                    _item.UniqueKey = item.UniqueKey;
                }
                return Sleeves.Put(key, _item);
            }
            else
                return Sleeves.Put(Figures.Put(item).Value);
        }
        /// <summary>
        /// Puts the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;IFigure&gt;.</returns>
        public ICard<IFigure> Put(IUnique<IFigure> value)
        {
            ulong key = value.UniqueKey;
            IFigure item = value.UniqueObject;
            IFigure _item;
            if (Figures.TryGet(key, out _item))
            {
                if (!ReferenceEquals(_item, item))
                {
                    _item.ValueArray = item.ValueArray;
                    _item.UniqueKey = item.UniqueKey;
                }
                return Sleeves.Put(key, _item);
            }
            else
                return Sleeves.Put(Figures.Put(item).Value);
        }
        /// <summary>
        /// Puts the specified items.
        /// </summary>
        /// <param name="items">The items.</param>
        public void Put(IList<IUnique<IFigure>> items)
        {
            foreach (var item in items)
                Put(item);
        }
        /// <summary>
        /// Puts the specified items.
        /// </summary>
        /// <param name="items">The items.</param>
        public void Put(IEnumerable<IUnique<IFigure>> items)
        {
            foreach (var item in items)
                Put(item);
        }

        /// <summary>
        /// Removes the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>IFigure.</returns>
        public IFigure Remove(object key)
        {
            return Sleeves.Remove(key);
        }
        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        /// <returns><see langword="true" /> if <paramref name="item" /> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, <see langword="false" />. This method also returns <see langword="false" /> if <paramref name="item" /> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1" />.</returns>
        public bool Remove(IFigure item)
        {
            if (Sleeves.Remove(item) != null)
                return true;
            return false;
        }
        /// <summary>
        /// Removes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool Remove(ICard<IFigure> item)
        {
            return Sleeves.Remove(item);
        }
        /// <summary>
        /// Removes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool Remove(IUnique<IFigure> item)
        {
            return Sleeves.Remove(item);
        }
        /// <summary>
        /// Tries the remove.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool TryRemove(object key)
        {
            return Sleeves.TryRemove(key);
        }
        /// <summary>
        /// Removes the <see cref="T:System.Collections.Generic.IList`1" /> item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param>
        public void RemoveAt(int index)
        {
            Sleeves.RemoveAt(index);
        }

        /// <summary>
        /// Renews the specified cards.
        /// </summary>
        /// <param name="cards">The cards.</param>
        public void Renew(IEnumerable<IFigure> cards)
        {
            Sleeves.Renew(cards);
        }
        /// <summary>
        /// Renews the specified cards.
        /// </summary>
        /// <param name="cards">The cards.</param>
        public void Renew(IList<IFigure> cards)
        {
            Sleeves.Renew(cards);
        }
        /// <summary>
        /// Renews the specified cards.
        /// </summary>
        /// <param name="cards">The cards.</param>
        public void Renew(IList<ICard<IFigure>> cards)
        {
            Sleeves.Renew(cards);
        }
        /// <summary>
        /// Renews the specified cards.
        /// </summary>
        /// <param name="cards">The cards.</param>
        public void Renew(IEnumerable<ICard<IFigure>> cards)
        {
            Sleeves.Renew(cards);
        }

        /// <summary>
        /// Converts to array.
        /// </summary>
        /// <returns>IFigure[].</returns>
        public IFigure[] ToArray()
        {
            return Sleeves.ToArray();
        }

        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1" /> to an <see cref="T:System.Array" />, starting at a particular <see cref="T:System.Array" /> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins.</param>
        public void CopyTo(IFigure[] array, int arrayIndex)
        {
            Sleeves.CopyTo(array, arrayIndex);
        }
        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="arrayIndex">Index of the array.</param>
        public void CopyTo(Array array, int arrayIndex)
        {
            Sleeves.CopyTo(array, arrayIndex);
        }
        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="destIndex">Index of the dest.</param>
        public void CopyTo(ICard<IFigure>[] array, int destIndex)
        {
            Sleeves.CopyTo(array, destIndex);
        }

        /// <summary>
        /// Creates new card.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;IFigure&gt;.</returns>
        public ICard<IFigure> NewCard(IFigure value)
        {
            return Sleeves.NewCard(value);
        }
        /// <summary>
        /// Creates new card.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;IFigure&gt;.</returns>
        public ICard<IFigure> NewCard(ICard<IFigure> value)
        {
            return Sleeves.NewCard(value);
        }
        /// <summary>
        /// Creates new card.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;IFigure&gt;.</returns>
        public ICard<IFigure> NewCard(object key, IFigure value)
        {
            return Sleeves.NewCard(key, value);
        }
        /// <summary>
        /// Creates new card.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;IFigure&gt;.</returns>
        public ICard<IFigure> NewCard(ulong key, IFigure value)
        {
            return Sleeves.NewCard(key, value);
        }

        /// <summary>
        /// Determines the index of a specific item in the <see cref="T:System.Collections.Generic.IList`1" />.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.IList`1" />.</param>
        /// <returns>The index of <paramref name="item" /> if found in the list; otherwise, -1.</returns>
        public int IndexOf(IFigure item)
        {
            return Sleeves.IndexOf(item);
        }

        /// <summary>
        /// Inserts an item to the <see cref="T:System.Collections.Generic.IList`1" /> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="item" /> should be inserted.</param>
        /// <param name="item">The object to insert into the <see cref="T:System.Collections.Generic.IList`1" />.</param>
        public void Insert(int index, IFigure item)
        {
            Figures.Add(item);
            Sleeves.Insert(index, item);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<IFigure> GetEnumerator()
        {
            return  ((IEnumerable<IFigure>)Sleeves).GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)Sleeves).GetEnumerator();
        }

        /// <summary>
        /// Gets the bytes.
        /// </summary>
        /// <returns>System.Byte[].</returns>
        public byte[] GetBytes()
        {
            return Sleeves.GetBytes();
        }

        /// <summary>
        /// Gets the unique bytes.
        /// </summary>
        /// <returns>System.Byte[].</returns>
        public byte[] GetUniqueBytes()
        {
            return Sleeves.GetUniqueBytes();
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><see langword="true" /> if the current object is equal to the <paramref name="other" /> parameter; otherwise, <see langword="false" />.</returns>
        public bool Equals(IUnique other)
        {
            return Sleeves.Equals(other);
        }

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="other">An object to compare with this instance.</param>
        /// <returns>A value that indicates the relative order of the objects being compared. The return value has these meanings:
        /// <list type="table"><listheader><term> Value</term><description> Meaning</description></listheader><item><term> Less than zero</term><description> This instance precedes <paramref name="other" /> in the sort order.</description></item><item><term> Zero</term><description> This instance occurs in the same position in the sort order as <paramref name="other" />.</description></item><item><term> Greater than zero</term><description> This instance follows <paramref name="other" /> in the sort order.</description></item></list></returns>
        public int CompareTo(IUnique other)
        {
            return Sleeves.CompareTo(other);
        }

        /// <summary>
        /// Ases the cards.
        /// </summary>
        /// <returns>IEnumerable&lt;ICard&lt;IFigure&gt;&gt;.</returns>
        public IEnumerable<ICard<IFigure>> AsCards()
        {
            return Sleeves.AsCards();
        }

        /// <summary>
        /// Ases the values.
        /// </summary>
        /// <returns>IEnumerable&lt;IFigure&gt;.</returns>
        public IEnumerable<IFigure> AsValues()
        {
            return Sleeves.AsValues();
        }

        /// <summary>
        /// Determines whether the specified key contains key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns><c>true</c> if the specified key contains key; otherwise, <c>false</c>.</returns>
        public bool ContainsKey(ulong key)
        {
            return Sleeves.ContainsKey(key);
        }

        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>IFigure.</returns>
        public IFigure Get(ulong key)
        {
            return Sleeves.Get(key);
        }

        /// <summary>
        /// Tries the get.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="output">The output.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool TryGet(ulong key, out IFigure output)
        {
            return Sleeves.TryGet(key, out output);
        }

        /// <summary>
        /// Gets the card.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>ICard&lt;IFigure&gt;.</returns>
        public ICard<IFigure> GetCard(ulong key)
        {
            return Sleeves.GetCard(key);
        }

        /// <summary>
        /// Gets or sets the serial count.
        /// </summary>
        /// <value>The serial count.</value>
        public int SerialCount { get; set; }
        /// <summary>
        /// Gets or sets the deserial count.
        /// </summary>
        /// <value>The deserial count.</value>
        public int DeserialCount { get; set; }
        /// <summary>
        /// Gets or sets the progress count.
        /// </summary>
        /// <value>The progress count.</value>
        public int ProgressCount { get; set; }

        /// <summary>
        /// Gets the items count.
        /// </summary>
        /// <value>The items count.</value>
        public int ItemsCount => Sleeves.Count;

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>The count.</value>
        public int Count => Sleeves.Count;

        /// <summary>
        /// Gets or sets the minimum count.
        /// </summary>
        /// <value>The minimum count.</value>
        public int MinCount { get => Sleeves.MinCount; set => Sleeves.MinCount = value; }

        /// <summary>
        /// Gets or sets the rubrics.
        /// </summary>
        /// <value>The rubrics.</value>
        public IRubrics Rubrics { get => Sleeves.Rubrics; set => Sleeves.Rubrics = value; }

        /// <summary>
        /// Gets or sets the key rubrics.
        /// </summary>
        /// <value>The key rubrics.</value>
        public IRubrics KeyRubrics { get => Sleeves.KeyRubrics; set => Sleeves.KeyRubrics = value; }

        /// <summary>
        /// Gets or sets the type of the figure.
        /// </summary>
        /// <value>The type of the figure.</value>
        public Type FigureType { get => Figures.FigureType; set => Figures.FigureType = value; }

        /// <summary>
        /// Gets or sets the size of the figure.
        /// </summary>
        /// <value>The size of the figure.</value>
        public int FigureSize { get => Figures.FigureSize; set => Figures.FigureSize = value; }

        /// <summary>
        /// Gets the first.
        /// </summary>
        /// <value>The first.</value>
        public ICard<IFigure> First => Sleeves.First;

        /// <summary>
        /// Gets the last.
        /// </summary>
        /// <value>The last.</value>
        public ICard<IFigure> Last => Sleeves.Last;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is synchronized.
        /// </summary>
        /// <value><c>true</c> if this instance is synchronized; otherwise, <c>false</c>.</value>
        public bool IsSynchronized { get => Sleeves.IsSynchronized; set => Sleeves.IsSynchronized = value; }
        /// <summary>
        /// Gets or sets the synchronize root.
        /// </summary>
        /// <value>The synchronize root.</value>
        public object SyncRoot { get => Sleeves.SyncRoot; set => Sleeves.SyncRoot = value; }

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only.
        /// </summary>
        /// <value><c>true</c> if this instance is read only; otherwise, <c>false</c>.</value>
        public bool IsReadOnly => Sleeves.IsReadOnly;

        /// <summary>
        /// Gets a value indicating whether this instance is synchronized.
        /// </summary>
        /// <value><c>true</c> if this instance is synchronized; otherwise, <c>false</c>.</value>
        bool ICollection.IsSynchronized => Sleeves.IsSynchronized;

        /// <summary>
        /// Gets the synchronize root.
        /// </summary>
        /// <value>The synchronize root.</value>
        object ICollection.SyncRoot => Sleeves.SyncRoot;

        /// <summary>
        /// Gets or sets the value array.
        /// </summary>
        /// <value>The value array.</value>
        public object[] ValueArray { get => Sleeves.ValueArray; set => Sleeves.ValueArray = value; }

        /// <summary>
        /// Gets or sets the serial code.
        /// </summary>
        /// <value>The serial code.</value>
        public Ussn SerialCode { get => Sleeves.SerialCode; set => Sleeves.SerialCode = value; }

        /// <summary>
        /// Gets or sets the unique key.
        /// </summary>
        /// <value>The unique key.</value>
        public ulong UniqueKey { get => Sleeves.UniqueKey; set => Sleeves.UniqueKey = value; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public Type Type { get => Figures.Type; set => Figures.Type = value; }

        /// <summary>
        /// Gets or sets the linker.
        /// </summary>
        /// <value>The linker.</value>
        public Linker Linker { get => Sleeves.Linker; set => Sleeves.Linker = value; }

        /// <summary>
        /// Gets or sets the treatment.
        /// </summary>
        /// <value>The treatment.</value>
        public Treatment Treatment
        {
            get => Sleeves.Treatment;
            set => Sleeves.Treatment = value;
        }

        /// <summary>
        /// Gets or sets the summary.
        /// </summary>
        /// <value>The summary.</value>
        public IFigure Summary
        {
            get => Sleeves.Summary;
            set => Sleeves.Summary = value;
        }

        /// <summary>
        /// Gets or sets the computations.
        /// </summary>
        /// <value>The computations.</value>
        public IDeck<IComputation> Computations { get => Figures.Computations; set => Figures.Computations = value; }
        /// <summary>
        /// Gets or sets the unique seed.
        /// </summary>
        /// <value>The unique seed.</value>
        public ulong UniqueSeed { get => Sleeves.UniqueSeed; set => Sleeves.UniqueSeed = value; }

        /// <summary>
        /// Gets or sets the <see cref="IFigure" /> at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>IFigure.</returns>
        public IFigure this[int index] { get => Sleeves[index]; set => Sleeves[index] = value; }

        /// <summary>
        /// Gets or sets the <see cref="System.Object" /> with the specified field identifier.
        /// </summary>
        /// <param name="fieldId">The field identifier.</param>
        /// <returns>System.Object.</returns>
        object IFigure.this[int fieldId] { get => Sleeves[fieldId]; set => Sleeves[fieldId] = (IFigure)value; }

        /// <summary>
        /// Gets or sets the <see cref="System.Object" /> with the specified property name.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>System.Object.</returns>
        public object this[string propertyName] { get => Sleeves[propertyName]; set => Sleeves[propertyName] = value; }

        /// <summary>
        /// Gets or sets the <see cref="IFigure" /> with the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>IFigure.</returns>
        public IFigure this[object key] { get => Sleeves[key]; set => Sleeves[key] = value; }

        /// <summary>
        /// Gets or sets the <see cref="System.Object" /> with the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>System.Object.</returns>
        object IFindable.this[object key] { get => Sleeves[key]; set => Sleeves[key] = (IFigure)value; }

        /// <summary>
        /// The disposed value
        /// </summary>
        private bool disposedValue = false;

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Sleeves.Dispose();
                }
                Sleeves = null;
                disposedValue = true;
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Tries the get.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="output">The output.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool TryGet(IUnique key, out ICard<IFigure> output)
        {
            return Sleeves.TryGet(key, out output);
        }
        /// <summary>
        /// Tries the get.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="output">The output.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool TryGet(IUnique<IFigure> key, out ICard<IFigure> output)
        {
            return Sleeves.TryGet(key, out output);
        }

        /// <summary>
        /// Gets the card.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>ICard&lt;IFigure&gt;.</returns>
        public ICard<IFigure> GetCard(IUnique key)
        {
            return Sleeves.GetCard(key);
        }
        /// <summary>
        /// Gets the card.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>ICard&lt;IFigure&gt;.</returns>
        public ICard<IFigure> GetCard(IUnique<IFigure> key)
        {
            return Sleeves.GetCard(key);
        }

        /// <summary>
        /// Sets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;IFigure&gt;.</returns>
        public ICard<IFigure> Set(object key, IFigure value)
        {
            return Sleeves.Set(key, value);
        }
        /// <summary>
        /// Sets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;IFigure&gt;.</returns>
        public ICard<IFigure> Set(ulong key, IFigure value)
        {
            return Sleeves.Set(key, value);
        }
        /// <summary>
        /// Sets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;IFigure&gt;.</returns>
        public ICard<IFigure> Set(IUnique key, IFigure value)
        {
            return Sleeves.Set(key, value);
        }
        /// <summary>
        /// Sets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;IFigure&gt;.</returns>
        public ICard<IFigure> Set(IUnique<IFigure> key, IFigure value)
        {
            return Sleeves.Set(key, value);
        }
        /// <summary>
        /// Sets the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;IFigure&gt;.</returns>
        public ICard<IFigure> Set(IFigure value)
        {
            return Sleeves.Set(value);
        }
        /// <summary>
        /// Sets the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;IFigure&gt;.</returns>
        public ICard<IFigure> Set(IUnique<IFigure> value)
        {
            return Sleeves.Set(value);
        }
        /// <summary>
        /// Sets the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;IFigure&gt;.</returns>
        public ICard<IFigure> Set(ICard<IFigure> value)
        {
            return Sleeves.Set(value);
        }

        /// <summary>
        /// Sets the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>System.Int32.</returns>
        public int Set(IEnumerable<IFigure> values)
        {
            return Sleeves.Set(values);
        }
        /// <summary>
        /// Sets the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>System.Int32.</returns>
        public int Set(IList<IFigure> values)
        {
            return Sleeves.Set(values);
        }
        /// <summary>
        /// Sets the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>System.Int32.</returns>
        public int Set(IEnumerable<ICard<IFigure>> values)
        {
            return Sleeves.Set(values);
        }
        /// <summary>
        /// Sets the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>System.Int32.</returns>
        public int Set(IEnumerable<IUnique<IFigure>> values)
        {
            return Sleeves.Set(values);
        }

        /// <summary>
        /// Sures the get.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="sureaction">The sureaction.</param>
        /// <returns>ICard&lt;IFigure&gt;.</returns>
        public ICard<IFigure> SureGet(object key, Func<ulong, IFigure> sureaction)
        {
            return Sleeves.SureGet(key, sureaction);
        }
        /// <summary>
        /// Sures the get.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="sureaction">The sureaction.</param>
        /// <returns>ICard&lt;IFigure&gt;.</returns>
        public ICard<IFigure> SureGet(ulong key, Func<ulong, IFigure> sureaction)
        {
            return Sleeves.SureGet(key, sureaction);
        }
        /// <summary>
        /// Sures the get.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="sureaction">The sureaction.</param>
        /// <returns>ICard&lt;IFigure&gt;.</returns>
        public ICard<IFigure> SureGet(IUnique key, Func<ulong, IFigure> sureaction)
        {
            return Sleeves.SureGet(key, sureaction);
        }
        /// <summary>
        /// Sures the get.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="sureaction">The sureaction.</param>
        /// <returns>ICard&lt;IFigure&gt;.</returns>
        public ICard<IFigure> SureGet(IUnique<IFigure> key, Func<ulong, IFigure> sureaction)
        {
            return Sleeves.SureGet(key, sureaction);
        }

        /// <summary>
        /// Tries the pick.
        /// </summary>
        /// <param name="skip">The skip.</param>
        /// <param name="output">The output.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool TryPick(int skip, out IFigure output)
        {
            return Sleeves.TryPick(skip, out output);
        }

        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="arrayIndex">Index of the array.</param>
        public void CopyTo(IUnique<IFigure>[] array, int arrayIndex)
        {
            Sleeves.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Resizes the specified size.
        /// </summary>
        /// <param name="size">The size.</param>
        public void Resize(int size)
        {
            Sleeves.Resize(size);
        }

        /// <summary>
        /// Removes the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="item">The item.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public bool Remove(object key, IFigure item)
        {
            throw new NotImplementedException();
        }
    }
}
