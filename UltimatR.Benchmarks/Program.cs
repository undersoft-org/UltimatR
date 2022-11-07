using System;
using System.Instant.Mathset;
using System.Series.Tests;
using BenchmarkDotNet.Running;

namespace BenefitSystems.Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            //  BenchmarkRunner.Run<MathsetBenchmark>();

           // var metod = new Catalog64Benchmark();

          //  metod.Dictionary_Add_Test();

            BenchmarkRunner.Run<Catalog64Benchmark>();

        }
    }
}
