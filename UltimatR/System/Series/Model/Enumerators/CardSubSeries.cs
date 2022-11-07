
// <copyright file="CardSubSeries.cs" company="UltimatR.Core">
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
    /// Class CardSubSeries.
    /// Implements the <see cref="System.Collections.Generic.IEnumerator{System.Series.ICard{V}}" />
    /// Implements the <see cref="System.Collections.Generic.IEnumerator{V}" />
    /// Implements the <see cref="System.Collections.IEnumerator" />
    /// </summary>
    /// <typeparam name="V"></typeparam>
    /// <seealso cref="System.Collections.Generic.IEnumerator{System.Series.ICard{V}}" />
    /// <seealso cref="System.Collections.Generic.IEnumerator{V}" />
    /// <seealso cref="System.Collections.IEnumerator" />
    public class CardSubSeries<V> : IEnumerator<ICard<V>>, IEnumerator<V>, IEnumerator
    {
        #region Fields

        /// <summary>
        /// The entry
        /// </summary>
        public ICard<V> Entry;
        /// <summary>
        /// The map
        /// </summary>
        private ICard<V> map;

        #endregion

        #region Constructors





        /// <summary>
        /// Initializes a new instance of the <see cref="CardSubSeries{V}" /> class.
        /// </summary>
        /// <param name="map">The map.</param>
        public CardSubSeries(ICard<V> map)
        {
            this.map = map;
            Entry = map;
        }

        #endregion

        #region Properties




        /// <summary>
        /// Gets the element in the collection at the current position of the enumerator.
        /// </summary>
        /// <value>The current.</value>
        public object Current => Entry.Value;

        /// <summary>
        /// Gets the index.
        /// </summary>
        /// <value>The index.</value>
        public int Index => Entry.Index;




        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <value>The key.</value>
        public ulong Key => Entry.Key;




        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>The value.</value>
        public V Value => Entry.Value;




        /// <summary>
        /// Gets the element in the collection at the current position of the enumerator.
        /// </summary>
        /// <value>The current.</value>
        ICard<V> IEnumerator<ICard<V>>.Current => Entry;




        /// <summary>
        /// Gets the element in the collection at the current position of the enumerator.
        /// </summary>
        /// <value>The current.</value>
        V IEnumerator<V>.Current => Entry.Value;

        #endregion

        #region Methods




        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Entry = map;
        }





        /// <summary>
        /// Advances the enumerator to the next element of the collection.
        /// </summary>
        /// <returns><see langword="true" /> if the enumerator was successfully advanced to the next element; <see langword="false" /> if the enumerator has passed the end of the collection.</returns>
        public bool MoveNext()
        {
            Entry = map.MoveNext(Entry);
            if (Entry != null)
            {
                return true;
            }
            return false;
        }




        /// <summary>
        /// Resets this instance.
        /// </summary>
        public void Reset()
        {
            Entry = map;
        }

        #endregion
    }

}
