/*************************************************
   Copyright (c) 2021 Undersoft

   System.Series.Catalog64Test.cs.Tests
   
   @project: Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Series.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Series;
    using System.Threading.Tasks;
    using BenchmarkDotNet.Attributes;

    public class Catalog64Test : BenchmarkHelper
    {
        #region Fields

        public static object holder = new object();
        public static int threadCount = 0;
        public Task[] s1 = new Task[10];

        #endregion

        #region Constructors

        public Catalog64Test() : base()
        {
            registry = new Catalog64<string>();
            DefaultTraceListener Logfile = new DefaultTraceListener();
            Logfile.Name = "Logfile";
            Trace.Listeners.Add(Logfile);
            Logfile.LogFileName = $"Catalog64_{DateTime.Now.ToFileTime().ToString()}_Test.log";
        }

        #endregion

        #region Methods

        [Benchmark]
        public async Task Catalog64_Concurrent_IndentifierKeys_TestAsync()
        {
            await catalog64_MultiThread_Test(identifierKeyTestCollection).ConfigureAwait(true);
        }

        [Benchmark]
        public async Task Catalog64_Concurrent_IntKeys_Test()
        {
            await catalog64_MultiThread_Test(intKeyTestCollection).ConfigureAwait(true);
        }

        [Benchmark]
        public async Task Catalog64_Concurrent_LongKeys_Test()
        {
            await catalog64_MultiThread_Test(longKeyTestCollection).ConfigureAwait(true);
        }

        [Benchmark]
        public async Task Catalog64_Concurrent_StringKeys_Test()
        {
            await catalog64_MultiThread_Test(stringKeyTestCollection).ConfigureAwait(true);
        }

        [Benchmark]
        public void Catalog64_IndentifierKeys_Test()
        {
            LogIntegrated_Test(identifierKeyTestCollection.Take(100000).ToArray());
        }

        [Benchmark]
        public void Catalog64_IntKeys_Test()
        {
            LogIntegrated_Test(intKeyTestCollection.Take(100000).ToArray());
        }

        [Benchmark]
        public void Catalog64_LongKeys_Test()
        {
            LogIntegrated_Test(longKeyTestCollection.Take(100000).ToArray());
        }

        [Benchmark]
        public void Catalog64_StringKeys_Test()
        {
            LogIntegrated_Test(stringKeyTestCollection.Take(100000).ToArray());
        }

        private void catalog64_MultiThread_TCallback_Test(Task[] t)
        {
            Debug.WriteLine($"Test Finished");
        }

        private Task catalog64_MultiThread_Test(IList<KeyValuePair<object, string>> collection)
        {
            Action publicTest = () =>
            {
                int c = 0;
                lock (holder)
                    c = threadCount++;

                LogThreadIntegrated_Test(collection.Skip(c * 10000).Take(10000).ToArray());
            };


            for(int i = 0; i < 10; i++)
            {
                s1[i] = Task.Factory.StartNew(publicTest);
            }

            return Task.Factory.ContinueWhenAll(s1, new Action<Task[]>(a => { catalog64_MultiThread_TCallback_Test(a); }));
        }

        #endregion
    }
}
