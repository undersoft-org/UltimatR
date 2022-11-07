
// <copyright file="CardKeyBlockSeries.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>




/// <summary>
/// The Series namespace.
/// </summary>
namespace System.Series
{
    using System.Collections;
    using System.Collections.Generic;





    /// <summary>
    /// Class CardUniqueKeySeries.
    /// Implements the <see cref="System.Collections.Generic.IEnumerator{System.UInt64}" />
    /// Implements the <see cref="System.Collections.IEnumerator" />
    /// </summary>
    /// <typeparam name="V"></typeparam>
    /// <seealso cref="System.Collections.Generic.IEnumerator{System.UInt64}" />
    /// <seealso cref="System.Collections.IEnumerator" />
    public class CardUniqueKeySeries<V> : IEnumerator<ulong>, IEnumerator
    {
        #region Fields

        /// <summary>
        /// The entry
        /// </summary>
        public ICard<V> Entry;
        /// <summary>
        /// The map
        /// </summary>
        private IDeck<V> map;

        #endregion

        #region Constructors





        /// <summary>
        /// Initializes a new instance of the <see cref="CardUniqueKeySeries{V}" /> class.
        /// </summary>
        /// <param name="Map">The map.</param>
        public CardUniqueKeySeries(IDeck<V> Map)
        {
            map = Map;
            Entry = map.First;
        }

        #endregion

        #region Properties




        /// <summary>
        /// Gets the element in the collection at the current position of the enumerator.
        /// </summary>
        /// <value>The current.</value>
        public object Current => Entry.Key;




        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <value>The key.</value>
        public ulong Key
        {
            get { return Entry.Key; }
        }




        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>The value.</value>
        public V Value
        {
            get { return Entry.Value; }
        }




        /// <summary>
        /// Gets the element in the collection at the current position of the enumerator.
        /// </summary>
        /// <value>The current.</value>
        ulong IEnumerator<ulong>.Current => Entry.Key;

        #endregion

        #region Methods




        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Entry = map.First;
        }





        /// <summary>
        /// Advances the enumerator to the next element of the collection.
        /// </summary>
        /// <returns><see langword="true" /> if the enumerator was successfully advanced to the next element; <see langword="false" /> if the enumerator has passed the end of the collection.</returns>
        public bool MoveNext()
        {
            Entry = Entry.Next;
            if (Entry != null)
            {
                if (Entry.Removed)
                    return MoveNext();
                return true;
            }
            return false;
        }




        /// <summary>
        /// Resets this instance.
        /// </summary>
        public void Reset()
        {
            Entry = map.First;
        }

        #endregion
    }
}
