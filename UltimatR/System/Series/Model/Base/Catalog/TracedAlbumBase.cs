
// <copyright file="TracedAlbumBase.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The Series namespace.
/// </summary>
namespace System.Series
{
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Instant;
    using System.Threading;
    using System.Uniques;

    /// <summary>
    /// Class TracedAlbumBase.
    /// Implements the <see cref="System.Series.AlbumBase{V}" />
    /// Implements the <see cref="System.Instant.ITraceable" />
    /// Implements the <see cref="System.ComponentModel.INotifyPropertyChanged" />
    /// Implements the <see cref="System.ComponentModel.INotifyPropertyChanging" />
    /// Implements the <see cref="System.Collections.Specialized.INotifyCollectionChanged" />
    /// </summary>
    /// <typeparam name="V"></typeparam>
    /// <seealso cref="System.Series.AlbumBase{V}" />
    /// <seealso cref="System.Instant.ITraceable" />
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanging" />
    /// <seealso cref="System.Collections.Specialized.INotifyCollectionChanged" />
    public class TracedAlbumBase<V> : AlbumBase<V>, ITraceable, INotifyPropertyChanged, INotifyPropertyChanging, INotifyCollectionChanged where V : class, ITraceable
    {
        #region Fields

        /// <summary>
        /// The readers
        /// </summary>
        private int readers;

        /// <summary>
        /// Gets the variator.
        /// </summary>
        /// <value>The variator.</value>
        public IVariety Variator { get; set; }
        /// <summary>
        /// Gets the notice change.
        /// </summary>
        /// <value>The notice change.</value>
        public IDeputy  NoticeChange { get; set; }
        /// <summary>
        /// Gets the notice changing.
        /// </summary>
        /// <value>The notice changing.</value>
        public IDeputy  NoticeChanging { get; set; }

        #endregion

        /// <summary>
        /// Handles the <see cref="E:PropertyChanged" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="PropertyChangedEventArgs" /> instance containing the event data.</param>
        protected void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChanged.Invoke(sender, e);
        }
        /// <summary>
        /// Handles the <see cref="E:PropertyChanging" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="PropertyChangingEventArgs" /> instance containing the event data.</param>
        protected void OnPropertyChanging(object sender, PropertyChangingEventArgs e)
        {
            PropertyChanging.Invoke(sender, e);
        }
        /// <summary>
        /// Handles the <see cref="E:CollectionChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="NotifyCollectionChangedEventArgs" /> instance containing the event data.</param>
        protected void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            CollectionChanged.Invoke(this, e);
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Occurs when a property value is changing.
        /// </summary>
        public event PropertyChangingEventHandler PropertyChanging;
        /// <summary>
        /// Occurs when the collection changes.
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TracedAlbumBase{V}" /> class.
        /// </summary>
        public TracedAlbumBase() : this(false, 17, HashBits.bit64)
        {
        
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="TracedAlbumBase{V}" /> class.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="capacity">The capacity.</param>
        /// <param name="repeatable">if set to <c>true</c> [repeatable].</param>
        /// <param name="bits">The bits.</param>
        public TracedAlbumBase(IEnumerable<IUnique<V>> collection, int capacity = 17, bool repeatable = false, HashBits bits = HashBits.bit64) : this(repeatable, capacity, bits)
        {
            Initialize();

            foreach(var c in collection)
                this.Add(c);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="TracedAlbumBase{V}" /> class.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="capacity">The capacity.</param>
        /// <param name="repeatable">if set to <c>true</c> [repeatable].</param>
        /// <param name="bits">The bits.</param>
        public TracedAlbumBase(IEnumerable<V> collection, int capacity = 17, bool repeatable = false, HashBits bits = HashBits.bit64) : this(repeatable, capacity, bits)
        {
            Initialize();

            foreach (var c in collection)
                this.Add(c);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="TracedAlbumBase{V}" /> class.
        /// </summary>
        /// <param name="repeatable">if set to <c>true</c> [repeatable].</param>
        /// <param name="capacity">The capacity.</param>
        /// <param name="bits">The bits.</param>
        public TracedAlbumBase(bool repeatable, int capacity = 17, HashBits bits = HashBits.bit64) : base(repeatable, capacity, bits)
        {
            Initialize();
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="TracedAlbumBase{V}" /> class.
        /// </summary>
        /// <param name="capacity">The capacity.</param>
        /// <param name="bits">The bits.</param>
        public TracedAlbumBase(int capacity = 17, HashBits bits = HashBits.bit64) : base(capacity, bits)
        {
            Initialize();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public void Initialize()
        {
            NoticeChange = new Deputy<TracedAlbumBase<V>>(this, a => nameof(a.OnPropertyChanged));
            NoticeChanging = new Deputy<TracedAlbumBase<V>>(this, a => nameof(a.OnPropertyChanging));
        }

        /// <summary>
        /// Empties the card.
        /// </summary>
        /// <returns>ICard&lt;V&gt;.</returns>
        public override ICard<V> EmptyCard()
        {
            return new TracedCard<V>();
        }

        /// <summary>
        /// Empties the card table.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <returns>ICard&lt;V&gt;[].</returns>
        public override ICard<V>[] EmptyCardTable(int size)
        {
            return new TracedCard<V>[size];
        }

        /// <summary>
        /// Empties the deck.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <returns>ICard&lt;V&gt;[].</returns>
        public override ICard<V>[] EmptyDeck(int size)
        {
            return new TracedCard<V>[size];
        }

        /// <summary>
        /// Creates new card.
        /// </summary>
        /// <param name="card">The card.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        public override ICard<V> NewCard(ICard<V> card)
        {
            return new TracedCard<V>(card);
        }

        /// <summary>
        /// Creates new card.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        public override ICard<V> NewCard(object key, V value)
        {
            return new TracedCard<V>(key, value);
        }

        /// <summary>
        /// Creates new card.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        public override ICard<V> NewCard(ulong key, V value)
        {
            return new TracedCard<V>(key, value);
        }

        /// <summary>
        /// Creates new card.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        public override ICard<V> NewCard(V value)
        {
            return new TracedCard<V>(value);
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public override void Clear()
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
           base.Clear();           
        }

        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="index">The index.</param>
        public override void CopyTo(Array array, int index)
        {
           
            base.CopyTo(array, index);
           
        }

        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="index">The index.</param>
        public override void CopyTo(ICard<V>[] array, int index)
        {
           
            base.CopyTo(array, index);
           
        }

        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="index">The index.</param>
        public override void CopyTo(V[] array, int index)
        {
           
            base.CopyTo(array, index);
           
        }

        /// <summary>
        /// Gets the card.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        /// <exception cref="System.IndexOutOfRangeException">Index out of range</exception>
        public override ICard<V> GetCard(int index)
        {
            if(index < count)
            {
               
                if(removed > 0)
                {
                   
                    
                    Reindex();
                    
                   
                }

                var temp = list[index];
               
                return temp;
            }
            throw new IndexOutOfRangeException("Index out of range");
        }

        /// <summary>
        /// Gets the card.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="item">The item.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        protected override ICard<V> GetCard(ulong key, V item)
        {           
           
            var card = base.GetCard(key, item);
           
            return card;
        }

        /// <summary>
        /// Indexes the of.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>System.Int32.</returns>
        public override int IndexOf(ICard<V> item)
        {
            int id = 0;
           
            id = base.IndexOf(item);
           
            return id;
        }

        /// <summary>
        /// Indexes the of.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="item">The item.</param>
        /// <returns>System.Int32.</returns>
        protected override int IndexOf(ulong key, V item)
        {
            int id = 0;
           
            id = base.IndexOf(key, item);
           
            return id;
        }

        /// <summary>
        /// Inserts the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="item">The item.</param>
        public override void Insert(int index, ICard<V> item)
        {
            
            base.InnerInsert(index, item);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item.Value));
            
        }

        /// <summary>
        /// Converts to array.
        /// </summary>
        /// <returns>V[].</returns>
        public override V[] ToArray()
        {
           
            V[] array = base.ToArray();
           
            return array;
        }

        /// <summary>
        /// Dequeues this instance.
        /// </summary>
        /// <returns>V.</returns>
        public override V Dequeue()
        {           
            var temp = base.Dequeue();
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, temp));
            
            return temp;
        }

        /// <summary>
        /// Tries the dequeue.
        /// </summary>
        /// <param name="output">The output.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public override bool TryDequeue(out ICard<V> output)
        {            
            var temp = base.TryDequeue(out output);
            RemoveNotifier(output.Value);            
            return temp;
        }

        /// <summary>
        /// Tries the dequeue.
        /// </summary>
        /// <param name="output">The output.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public override bool TryDequeue(out V output)
        {
            
            var temp = base.TryDequeue(out output);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, temp));
            
            return temp;
        }

        /// <summary>
        /// Tries the pick.
        /// </summary>
        /// <param name="skip">The skip.</param>
        /// <param name="output">The output.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public override bool TryPick(int skip, out V output)
        {
            
            var temp = base.TryPick(skip, out output);
            
            return temp;
        }

        /// <summary>
        /// Inners the add.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        protected override bool InnerAdd(ICard<V> value)
        {
            
            var temp = base.InnerAdd(value);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, temp));
            
            return temp;
        }

        /// <summary>
        /// Inners the add.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        protected override bool InnerAdd(ulong key, V value)
        {
            
            var temp = base.InnerAdd(key, value);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, temp));
            
            return temp;
        }

        /// <summary>
        /// Inners the add.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        protected override bool InnerAdd(V value)
        {
            
            var temp = base.InnerAdd(value);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, temp));
            
            return temp;
        }

        /// <summary>
        /// Inners the get.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>V.</returns>
        protected override V InnerGet(ulong key)
        {
           
            var v = base.InnerGet(key);
           
            return v;
        }

        /// <summary>
        /// Inners the get card.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        protected override ICard<V> InnerGetCard(ulong key)
        {           
            return base.InnerGetCard(key);          
        }

        /// <summary>
        /// Inners the put.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        protected override ICard<V> InnerPut(ICard<V> value)
        {
            var _count = count;

            var temp = base.InnerPut(value);

            
            return temp;
        }

        /// <summary>
        /// Inners the put.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        protected override ICard<V> InnerPut(ulong key, V value)
        {
            
            var temp = base.InnerPut(key, value);
            
            return temp;
        }

        /// <summary>
        /// Inners the put.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        protected override ICard<V> InnerPut(V value)
        {
            
            var temp = base.InnerPut(value);
            
            return temp;
        }

        /// <summary>
        /// Inners the remove.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>V.</returns>
        protected override V InnerRemove(ulong key)
        {            
            var temp = base.InnerRemove(key);
            RemoveNotifier(temp);
            return temp;
        }

        /// <summary>
        /// Inners the set.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        protected override ICard<V> InnerSet(ICard<V> value)
        {
            var card = InnerGetCard(value.Key);
            if (card != null)
            {
                var oldItem = card.Value;
                var newItem = value.Value;
                card.Value  = newItem;
                ChangeNotiifer(newItem, oldItem);
            }
            return card;
        }

        /// <summary>
        /// Inners the set.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        protected override ICard<V> InnerSet(ulong key, V value)
        {
            var card = InnerGetCard(key);
            if (card != null)
            {
                var oldItem = card.Value;
                var newItem = value;
                card.Value  = newItem;
                ChangeNotiifer(newItem, oldItem);
            }
            return card;
        }

        /// <summary>
        /// Inners the set.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>ICard&lt;V&gt;.</returns>
        protected override ICard<V> InnerSet(V value)
        {
            return InnerSet(unique.Key(value), value);
        }

        /// <summary>
        /// Inners the try get.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="output">The output.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        protected override bool InnerTryGet(ulong key, out ICard<V> output)
        {           
           return base.InnerTryGet(key, out output);          
        }

        /// <summary>
        /// Rehashes the specified newsize.
        /// </summary>
        /// <param name="newsize">The newsize.</param>
        protected override void Rehash(int newsize)
        {
            base.Rehash(newsize);
        }

        /// <summary>
        /// Reindexes this instance.
        /// </summary>
        protected override void Reindex()
        {
            base.Reindex();
        }

        /// <summary>
        /// Changes the notiifer.
        /// </summary>
        /// <param name="newItem">The new item.</param>
        /// <param name="oldItem">The old item.</param>
        protected void ChangeNotiifer(V newItem, V oldItem)
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, newItem, oldItem));
        }

        /// <summary>
        /// Replaces the notifier.
        /// </summary>
        /// <param name="itemsMoved">The items moved.</param>
        protected void ReplaceNotifier(V itemsMoved)
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, itemsMoved));
        }

        /// <summary>
        /// Resets the notifier.
        /// </summary>
        /// <param name="itemsReset">The items reset.</param>
        protected void ResetNotifier(IEnumerable<V> itemsReset)
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset, itemsReset));
        }

        /// <summary>
        /// Removes the notifier.
        /// </summary>
        /// <param name="itemRemoved">The item removed.</param>
        protected void RemoveNotifier(V itemRemoved)
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, itemRemoved));
        }

        /// <summary>
        /// Adds the notifier.
        /// </summary>
        /// <param name="itemAdded">The item added.</param>
        protected void AddNotifier(V itemAdded)
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, itemAdded));
        }

        /// <summary>
        /// Adds the notifier.
        /// </summary>
        /// <param name="itemsAdded">The items added.</param>
        protected void AddNotifier(IEnumerable<V> itemsAdded)
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, itemsAdded));
        }

        #endregion
    }
}
