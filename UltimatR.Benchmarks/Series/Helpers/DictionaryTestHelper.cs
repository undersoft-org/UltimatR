/*************************************************
   Copyright (c) 2021 Undersoft

   System.Series.SharedCatalogTestHelper.cs.Tests
   
   @project: Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Series.Tests
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Series;
    using NetTopologySuite.Utilities;



    /// <summary>
    /// Defines the <see cref="LogCatalogTestHelper" />.
    /// </summary>
    public class BenchmarkDictionaryHelper
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LogCatalogTestHelper"/> class.
        /// </summary>
        public BenchmarkDictionaryHelper()
        {
            stringKeyTestCollection = PrepareTestListings.prepareStringKeyTestCollection();
            intKeyTestCollection = PrepareTestListings.prepareIntKeyTestCollection();
            longKeyTestCollection = PrepareTestListings.prepareLongKeyTestCollection();
            identifierKeyTestCollection = PrepareTestListings.prepareIdentifierKeyTestCollection();
           
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the identifierKeyTestCollection.
        /// </summary>
        public IList<KeyValuePair<object, string>> identifierKeyTestCollection { get; set; }

        /// <summary>
        /// Gets or sets the intKeyTestCollection.
        /// </summary>
        public IList<KeyValuePair<object, string>> intKeyTestCollection { get; set; }

        /// <summary>
        /// Gets or sets the longKeyTestCollection.
        /// </summary>
        public IList<KeyValuePair<object, string>> longKeyTestCollection { get; set; }

        /// <summary>
        /// Gets or sets the registry.
        /// </summary>
        public IDictionary<string, string> registry { get; set; }

        /// <summary>
        /// Gets or sets the stringKeyTestCollection.
        /// </summary>
        public IList<KeyValuePair<object, string>> stringKeyTestCollection { get; set; }

        #endregion

        #region Methods

        public void Add_Test(IList<KeyValuePair<object, string>> testCollection)
        {
            registry = new Dictionary<string, string>();
            foreach(var item in testCollection)
            {
                registry.Add((string)item.Key.ToString(), item.Value);
            }
        }

        public void Contains_Test(IList<KeyValuePair<object, string>> testCollection)
        {
            registry = new Dictionary<string, string>();
            foreach(var item in testCollection)
            {
                registry.Add((string)item.Key.ToString(), item.Value);
            }
            foreach(var item in testCollection)
            {
                registry.Contains(new KeyValuePair<string, string>((string)item.Key.ToString(), item.Value));
            }
        }

        public void ContainsKey_Test(IList<KeyValuePair<object, string>> testCollection)
        {
            registry = new Dictionary<string, string>();
            foreach(var item in testCollection)
            {
                registry.Add((string)item.Key.ToString(), item.Value);
            }
            foreach(var item in testCollection)
            {
                registry.ContainsKey((string)item.Key.ToString());
            }
        }

        public void GetByKey_Test(IList<KeyValuePair<object, string>> testCollection)
        {
            registry = new Dictionary<string, string>();
            foreach(var item in testCollection)
            {
                registry.Add((string)item.Key.ToString(), item.Value);
            }
            foreach(var item in testCollection)
            {
                string r = registry[(string)item.Key];
            }
        }

        public void GetByIndex_Test(IList<KeyValuePair<object, string>> testCollection)
        {
            registry = new Dictionary<string, string>();
            foreach(var item in testCollection)
            {
                registry.Add((string)item.Key.ToString(), item.Value);
            }
            int i = 0;
            foreach(var item in testCollection)
            {
                string r = registry.Values.ElementAt(i++);
            }
        }

        public void GetLast_Test(IList<KeyValuePair<object, string>> testCollection)
        {
            registry = new Dictionary<string, string>();
            foreach(var item in testCollection)
            {
                registry.Add((string)item.Key.ToString(), item.Value);
            }
            string r = registry.Last().Value;
        }

        public void Remove_Test(IList<KeyValuePair<object, string>> testCollection)
        {
            registry = new Dictionary<string, string>();
            foreach(var item in testCollection)
            {
                registry.Add((string)item.Key.ToString(), item.Value);
            }
            foreach(var item in testCollection.Skip(100000))
            {

                registry.Remove((string)item.Key.ToString());
            }
        }

        public void Iteration_Test(IList<KeyValuePair<object, string>> testCollection)
        {
            registry = new Dictionary<string, string>();
            foreach(var item in testCollection)
            {
                registry.Add((string)item.Key.ToString(), item.Value);
            }
            foreach(var item in registry)
            {
                object r = item.Value;
            }
        }

        #endregion
    }
}
