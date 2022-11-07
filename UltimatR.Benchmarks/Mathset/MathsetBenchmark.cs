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
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using BenchmarkDotNet;
    using BenchmarkDotNet.Attributes;

    [MemoryDiagnoser] 
    [RankColumn]
    [Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.FastestToSlowest)]
    [RPlotExporter]  
    public class MathsetBenchmark
    {
        #region Fields

        private Figure instFig;
        private Figures instMtic;
        private Computation rck;
        private Computation rck2;
        private IFigures spcMtic;

        #endregion

        #region Constructors

  
        public MathsetBenchmark()
        {
            instMtic = new Figures(typeof(MathsetMockModel), "Figures_Mathset_Test");

            spcMtic = instMtic.Combine();

            MathsetMockModel fom = new MathsetMockModel();
           

            for (int i = 0; i < 2000 * 1000; i++)
            {
                ISleeve f = spcMtic.NewSleeve();
                f.Devisor = new MathsetMockModel();               

                f["NetPrice"] = (double)f["NetPrice"] + i;
                f["SellFeeRate"] = (double)f["SellFeeRate"] / 2;
                spcMtic.Add(i, f);
            }

            rck2 = new Computation(spcMtic);

            Mathset ml = rck2.GetMathset("SellNetPrice");

            ml.Formula = (ml["NetPrice"] * (ml["SellFeeRate"] / 100D)) + ml["NetPrice"];

            Mathset ml2 = rck2.GetMathset("SellGrossPrice");

            ml2.Formula = ml * ml2["TaxRate"];

            rck2.Compute(); // first with formula compilation
        }

        #endregion

        #region Methods

        /// <summary>
        /// The Mathset_Computation_Formula_Test.
        /// </summary>   
        public void Mathset_With_Compilation()
        {
            rck = new Computation(spcMtic);

            Mathset ml = rck.GetMathset("SellNetPrice");

            ml.Formula = (ml["NetPrice"] * (ml["SellFeeRate"] / 100D)) + ml["NetPrice"];

            Mathset ml2 = rck.GetMathset("SellGrossPrice");

            ml2.Formula = ml * ml2["TaxRate"];
          
            rck.Compute();                 
        }

        [Benchmark]
        public void Mathset_Without_Compilation()
        { 
            rck2.Compute();   
        }

        [Benchmark]
        public void Parellel_ForEach_Loop()
        {            

            spcMtic.AsParallel().ForEach((c) => {

                c["SellNetPrice"] = ((double)c["NetPrice"] * ((double)c["SellFeeRate"] / 100D)) + (double)c["NetPrice"];
                
                c["SellGrossPrice"] = (double)c["SellNetPrice"] * (double)c["TaxRate"];
            });
        }      

        #endregion
    }
}
