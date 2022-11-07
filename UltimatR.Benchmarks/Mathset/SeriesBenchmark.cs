/*************************************************
   Copyright (c) 2021 Undersoft

   System.Instant.Mathset.MathsetTest.cs.Tests
   
   @project: Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Instant.Mathset
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Series;
    using System.Series.Tests;
    using System.Threading.Tasks;
    using BenchmarkDotNet.Attributes;

    [MemoryDiagnoser] 
    [RankColumn]
    [Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.FastestToSlowest)]
    [RPlotExporter]
    public class SeriesBenchmark : BoardTestHelper
    {
        #region Fields

        public static object holder = new object();
        public static int threadCount = 0;
        public Task[] s1 = new Task[10];

        #endregion
        #region Constructors
        public SeriesBenchmark()
        {
            registry = new Catalog64<string>();
            DefaultTraceListener Logfile = new DefaultTraceListener();
            Logfile.Name = "Logfile";
            Trace.Listeners.Add(Logfile);
            Logfile.LogFileName = $"Catalog64_{DateTime.Now.ToFileTime().ToString()}_Benchmark.log";
        }
           

        #endregion

        #region Methods

        /// <summary>
        /// The Mathset_Computation_Formula_Test.
        /// </summary>
        [Benchmark]
        public void Catalog_Adding()
        {
         
        }

        [Benchmark]
        public void DictionaryAdding()
        { 
        }

        [Benchmark]
        public void Mathset_ForEach()
        {            

          
        }      

        #endregion
    }
}
