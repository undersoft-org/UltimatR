
// <copyright file="Spectrum.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The Series namespace.
/// </summary>
namespace System.Series
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Series.Spectrum;

    /// <summary>
    /// Class Spectrum.
    /// Implements the <see cref="System.Series.ISpectrum{V}" />
    /// </summary>
    /// <typeparam name="V"></typeparam>
    /// <seealso cref="System.Series.ISpectrum{V}" />
    public class Spectrum<V> : ISpectrum<V>
    {
        #region Fields

        /// <summary>
        /// The levels
        /// </summary>
        private IList<vEBTreeLevel> levels;
        /// <summary>
        /// The registry
        /// </summary>
        private IDeck<V> registry;
        /// <summary>
        /// The root
        /// </summary>
        private SpectrumBase root;
        /// <summary>
        /// The scopes
        /// </summary>
        private IDeck<SpectrumBase> scopes;
        /// <summary>
        /// The sigma scopes
        /// </summary>
        private IDeck<SpectrumBase> sigmaScopes;
        /// <summary>
        /// The size
        /// </summary>
        private int size;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Spectrum{V}" /> class.
        /// </summary>
        public Spectrum() : this(int.MaxValue, false)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Spectrum{V}" /> class.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <param name="safeThread">if set to <c>true</c> [safe thread].</param>
        public Spectrum(int size, bool safeThread)
        {
            Initialize(size);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>The count.</value>
        public int Count => registry.Count;

        /// <summary>
        /// Gets the index maximum.
        /// </summary>
        /// <value>The index maximum.</value>
        public int IndexMax
        {
            get { return root.IndexMax; }
        }

        /// <summary>
        /// Gets the index minimum.
        /// </summary>
        /// <value>The index minimum.</value>
        public int IndexMin
        {
            get { return root.IndexMin; }
        }

        /// <summary>
        /// Gets the size.
        /// </summary>
        /// <value>The size.</value>
        public int Size { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="obj">The object.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool Add(int key, V obj)
        {
            if(registry.Add(key, obj))
            {
                root.Add(0, 1, 0, key);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Determines whether this instance contains the object.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns><c>true</c> if [contains] [the specified key]; otherwise, <c>false</c>.</returns>
        public bool Contains(int key)
        {
            return registry.ContainsKey(key);
        }

        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>V.</returns>
        public V Get(int key)
        {
            return registry.Get(key);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<CardBase<V>> GetEnumerator()
        {
            return new SpectrumSeries<V>(this);
        }

        /// <summary>
        /// Initializes the specified range.
        /// </summary>
        /// <param name="range">The range.</param>
        /// <param name="safeThred">if set to <c>true</c> [safe thred].</param>
        public void Initialize(int range = 0, bool safeThred = false)
        {
            scopes = new Deck64<SpectrumBase>();
            sigmaScopes = new Deck64<SpectrumBase>();

            if((range == 0) || (range > int.MaxValue))
            {
                range = int.MaxValue;
            }
            if(!safeThred)
                registry = new Deck64<V>(range);
            else
                registry = new Board64<V>(range);

            size = range;

            CreateLevels(range);   

            root = new ScopeValue(range, scopes, sigmaScopes, levels, 0, 0, 0);
        }

        /// <summary>
        /// Nexts the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>System.Int32.</returns>
        public int Next(int key)
        {
            return root.Next(0, 1, 0, key);
        }

        /// <summary>
        /// Previouses the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>System.Int32.</returns>
        public int Previous(int key)
        {
            return root.Previous(0, 1, 0, key);
        }

        /// <summary>
        /// Removes the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool Remove(int key)
        {
            if(registry.TryRemove(key))
            {
                root.Remove(0, 1, 0, key);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Sets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool Set(int key, V value)
        {
            return Add(key, value);
        }

        /// <summary>
        /// Tests the add.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool TestAdd(int key)
        {
            root.Add(0, 1, 0, key);
            return true;
        }

        /// <summary>
        /// Tests the contains.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool TestContains(int key)
        {
            return root.Contains(0, 1, 0, key);
        }

        /// <summary>
        /// Tests the remove.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool TestRemove(int key)
        {
            root.Remove(0, 1, 0, key);
            return true;
        }

        /// <summary>
        /// Builds the sigma scopes.
        /// </summary>
        /// <param name="range">The range.</param>
        /// <param name="level">The level.</param>
        /// <param name="nodeTypeIndex">Index of the node type.</param>
        /// <param name="nodeCounter">The node counter.</param>
        /// <param name="clusterSize">Size of the cluster.</param>
        private void BuildSigmaScopes(int range, byte level, byte nodeTypeIndex, int nodeCounter, int clusterSize)
        {
            int parentSqrt = ScopeValue.ParentSqrt(range);

            if(levels == null)
            {
                levels = new List<vEBTreeLevel>();
            }
            if(levels.Count <= level)
            {
                levels.Add(new vEBTreeLevel());
            }
            if(levels[level].Nodes == null)
            {
                levels[level].Nodes = new List<vEBTreeNode>();
                levels[level].Nodes.Add(new vEBTreeNode());
            }
            else
            {
                levels[level].Nodes.Add(new vEBTreeNode());
            }

            levels[level].Nodes[nodeTypeIndex].NodeCounter = nodeCounter;
            levels[level].Nodes[nodeTypeIndex].NodeSize = parentSqrt;

            if(parentSqrt > 4)
            {
                
                BuildSigmaScopes(parentSqrt, (byte)(level + 1), (byte)(2 * nodeTypeIndex), nodeCounter, parentSqrt);
                
                BuildSigmaScopes(parentSqrt, (byte)(level + 1), (byte)(2 * nodeTypeIndex + 1), nodeCounter * parentSqrt, parentSqrt);
            }
        }

        /// <summary>
        /// Creates the levels.
        /// </summary>
        /// <param name="range">The range.</param>
        private void CreateLevels(int range)
        {
            if(levels == null)
            {
                int parentSqrt = ScopeValue.ParentSqrt(size);
                BuildSigmaScopes(range, 0, 0, 1, parentSqrt);
            }

            int baseOffset = 0;
            for(int i = 1; i < levels.Count; i++)
            {
                levels[i].BaseOffset = baseOffset;
                for(int j = 0; j < levels[i].Nodes.Count - 1; j++)
                {
                    levels[i].Nodes[j].IndexOffset = baseOffset;
                    baseOffset += levels[i].Nodes[j].NodeCounter * levels[i].Nodes[j].NodeSize;
                }
            }
        }

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns>IEnumerator.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new SpectrumSeries<V>(this);
        }

        #endregion
    }
}
