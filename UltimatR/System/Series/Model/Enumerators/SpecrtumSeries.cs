
// <copyright file="SpecrtumSeries.cs" company="UltimatR.Core">
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
    /// Class SpectrumSeries.
    /// Implements the <see cref="System.Collections.Generic.IEnumerator{System.Series.CardBase{V}}" />
    /// Implements the <see cref="System.Collections.IEnumerator" />
    /// </summary>
    /// <typeparam name="V"></typeparam>
    /// <seealso cref="System.Collections.Generic.IEnumerator{System.Series.CardBase{V}}" />
    /// <seealso cref="System.Collections.IEnumerator" />
    public class SpectrumSeries<V> : IEnumerator<CardBase<V>>, IEnumerator
    {
        #region Fields

        /// <summary>
        /// The entry
        /// </summary>
        public CardBase<V> Entry;
        /// <summary>
        /// The iterated
        /// </summary>
        private int iterated = 0;
        /// <summary>
        /// The last returned
        /// </summary>
        private int lastReturned;
        /// <summary>
        /// The map
        /// </summary>
        private Spectrum<V> map;

        #endregion

        #region Constructors





        /// <summary>
        /// Initializes a new instance of the <see cref="SpectrumSeries{V}" /> class.
        /// </summary>
        /// <param name="Map">The map.</param>
        public SpectrumSeries(Spectrum<V> Map)
        {
            map = Map;
            Entry = new Card64<V>();
        }

        #endregion

        #region Properties




        /// <summary>
        /// Gets the element in the collection at the current position of the enumerator.
        /// </summary>
        /// <value>The current.</value>
        public object Current => Entry;




        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <value>The key.</value>
        public int Key
        {
            get { return (int)Entry.Key; }
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
        CardBase<V> IEnumerator<CardBase<V>>.Current => Entry;

        #endregion

        #region Methods




        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            iterated = 0;
            Entry = null;
        }





        /// <summary>
        /// Determines whether this instance has next.
        /// </summary>
        /// <returns><c>true</c> if this instance has next; otherwise, <c>false</c>.</returns>
        public bool HasNext()
        {
            return iterated < map.Count;
        }





        /// <summary>
        /// Advances the enumerator to the next element of the collection.
        /// </summary>
        /// <returns><see langword="true" /> if the enumerator was successfully advanced to the next element; <see langword="false" /> if the enumerator has passed the end of the collection.</returns>
        public bool MoveNext()
        {
            return Next();
        }





        /// <summary>
        /// Nexts this instance.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool Next()
        {
            if (!HasNext())
            {
                return false;
            }

            if (iterated == 0)
            {
                lastReturned = map.IndexMin;
                iterated++;
                Entry.Key = (uint)lastReturned;
                Entry.Value = map.Get(lastReturned);
            }
            else
            {
                lastReturned = map.Next(lastReturned); ;
                iterated++;
                Entry.Key = (uint)lastReturned;
                Entry.Value = map.Get(lastReturned);
            }
            return true;
        }




        /// <summary>
        /// Resets this instance.
        /// </summary>
        public void Reset()
        {
            Entry = new Card64<V>();
            iterated = 0;
        }

        #endregion
    }
}
