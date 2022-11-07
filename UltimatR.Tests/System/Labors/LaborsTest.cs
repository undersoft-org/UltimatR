/*************************************************
   Copyright (c) 2021 Undersoft

   System.Laboring.LaborsTest.cs.Tests
   
   @project: Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Laboring.Tests
{
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics;
    using System.Laboring.Rules;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading;

    using Xunit;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="ComputeCurrency" />.
    /// </summary>
    public class ComputeCurrency
    {
        #region Methods

        /// <summary>
        /// The Compute.
        /// </summary>
        /// <param name="currency1">The currency1<see cref="string"/>.</param>
        /// <param name="rate1">The rate1<see cref="double"/>.</param>
        /// <param name="currency2">The currency2<see cref="string"/>.</param>
        /// <param name="rate2">The rate2<see cref="double"/>.</param>
        /// <returns>The <see cref="object"/>.</returns>
        public object Compute(string currency1, double rate1, string currency2, double rate2)
        {
            try
            {
                double _rate1 = rate1;
                double _rate2 = rate2;
                double result = _rate2 / _rate1;
                Debug.WriteLine("Result : " + result.ToString());

                return new object[] { _rate1, _rate2, result };
            }

            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                throw new InvalidLaborException(ex.ToString());
            }
        }

        #endregion
    }

    /// <summary>
    /// Defines the <see cref="FirstCurrency" />.
    /// </summary>
    public class FirstCurrency
    {
        #region Methods

        /// <summary>
        /// The GetFirstCurrency.
        /// </summary>
        /// <param name="currency">The currency<see cref="string"/>.</param>
        /// <param name="days">The days<see cref="int"/>.</param>
        /// <returns>The <see cref="object"/>.</returns>
        public object GetCurrency(string currency, int days)
        {
            //Thread.Sleep(2000);

            NBPSource kurKraju = new NBPSource(days);

            try
            {
                double rate = kurKraju.LoadRate(currency);
                Debug.WriteLine("Rate 1 : " + currency + "  days past: " + days.ToString() + " amount : " + rate.ToString("#.####"));

                return new object[] { currency, rate };
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                throw new InvalidLaborException(ex.ToString());
            }
        }

        #endregion
    }

    /// <summary>
    /// Defines the <see cref="LaborsTest" />.
    /// </summary>
    public class LaborsTest
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LaborsTest"/> class.
        /// </summary>
        public LaborsTest()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        #endregion

        #region Methods

        /// <summary>
        /// The Labors_Inductive_MultiThreading_Integration_Test.
        /// </summary>
        [Fact]
        public void Labors_Inductive_MultiThreading_Integration_Test()
        {
            var rule = new Rule<LaborCase>();

            var download = rule
                .Aspect<LaborsTest>()
                .Include<FirstCurrency>()
                .Include<SecondCurrency>()
                .Allocate(2);  /// e.g. 8 workers/consumers for downloading 2 different currency rates

            var compute = rule
                .Aspect<NBPSource>()
                .Include<ComputeCurrency>()
                .Include<PresentResult>()
                .Allocate(1); /// e.g. 4 workers/consumers for computing and presenting results

            download
                .Operation<FirstCurrency>()
                .ResultTo(compute
                .Operation<ComputeCurrency>());

            download
                .Operation<SecondCurrency>()
                .ResultTo(compute
                .Operation<ComputeCurrency>());

            compute
                .Operation<ComputeCurrency>()
                .ResultTo(compute
                .Operation<PresentResult>());


            for (int i = 1; i < 10; i++)
            {
                download.Operation<FirstCurrency>().Run("EUR", i);
                download.Operation<SecondCurrency>().Run("USD", i);
            }

            Thread.Sleep(10000);

            download.Close(true);
            compute.Close(true);
        }

        /// <summary>
        /// The Labors_QuickLabor_Integration_Test.
        /// </summary>
        [Fact]
        public void Labors_QuickLabor_Integration_Test()
        {

            var ql0 = new Laborate(new Deputy<FirstCurrency>(), "EUR", 1);
            var ql1 = new Laborate(new Deputy<SecondCurrency>(), "USD", 1);

            ql0 = Laborate.Run<FirstCurrency>(true, "EUR", 1); 
            ql1 = Laborate.Run<SecondCurrency>(false, "USD", 1);
             
            Thread.Sleep(5000);
        }

        #endregion 
    }

    /// <summary>
    /// Defines the <see cref="NBPSource" />.
    /// </summary>
    public class NBPSource
    {
        #region Constants

        private const string file_dir = "http://www.nbp.pl/Kursy/xml/dir.txt";
        private const string xml_url = "http://www.nbp.pl/kursy/xml/";

        #endregion

        #region Fields

        public string file_name;
        public DateTime rate_date;
        private int start_int = 1;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NBPSource"/> class.
        /// </summary>
        /// <param name="daysbefore">The daysbefore<see cref="int"/>.</param>
        public NBPSource(int daysbefore)
        {
            GetFileName(daysbefore);
        }

        #endregion

        #region Methods

        /// <summary>
        /// The GetCurrenciesRate.
        /// </summary>
        /// <param name="currency_names">The currency_names<see cref="List{string}"/>.</param>
        /// <returns>The <see cref="Dictionary{string, double}"/>.</returns>
        public Dictionary<string, double> GetCurrenciesRate(List<string> currency_names)
        {
            Dictionary<string, double> result = new Dictionary<string, double>();

            foreach (var item in currency_names)
            {
                result.Add(item, LoadRate(item));
            }
            return result;
        }

        /// <summary>
        /// The LoadRate.
        /// </summary>
        /// <param name="currency_name">The currency_name<see cref="string"/>.</param>
        /// <returns>The <see cref="double"/>.</returns>
        public double LoadRate(string currency_name)
        {

            try
            {
                string file = xml_url + file_name + ".xml";
                DataSet ds = new DataSet(); 
                ds.ReadXml(file);
                var tabledate = ds.Tables["tabela_kursow"].Rows.Cast<DataRow>().AsEnumerable();
                var before_rate_date = (from k in tabledate select new { Data = k["data_publikacji"].ToString() }).First();
                var tabela = ds.Tables["pozycja"].Rows.Cast<DataRow>().AsEnumerable();
                var rate = (from k in tabela where k["kod_waluty"].ToString() == currency_name select new { Kurs = k["kurs_sredni"].ToString() }).First();
                rate_date = Convert.ToDateTime(before_rate_date.Data);
                return Convert.ToDouble(rate.Kurs);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                throw new InvalidLaborException(ex.ToString());
            }
        }

        /// <summary>
        /// The GetFileName.
        /// </summary>
        /// <param name="daysbefore">The daysbefore<see cref="int"/>.</param>
        private void GetFileName(int daysbefore)
        {

            try
            {
                int minusdays = daysbefore * -1;
                WebClient client = new WebClient();
                Stream strm = client.OpenRead(file_dir);
                StreamReader sr = new StreamReader(strm);
                string file_list = sr.ReadToEnd();
                string date_str = string.Empty;
                bool has_this_rate = false;
                DateTime date_of_rate = DateTime.Now.AddDays(minusdays);
                while (!has_this_rate)
                {
                    date_str = "a" + start_int.ToString().PadLeft(3, '0') + "z" + date_of_rate.ToString("yyMMdd");
                    if (file_list.Contains(date_str))
                    {
                        has_this_rate = true;
                    }

                    start_int++;

                    if (start_int > 365)
                    {
                        start_int = 1;
                        date_of_rate = date_of_rate.AddDays(-1);
                    }
                }
                file_name = date_str;
                rate_date = date_of_rate;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                throw new InvalidLaborException(ex.ToString());
            }
        }

        #endregion
    }

    /// <summary>
    /// Defines the <see cref="PresentResult" />.
    /// </summary>
    public class PresentResult
    {
        #region Methods

        /// <summary>
        /// The Present.
        /// </summary>
        /// <param name="rate1">The rate1<see cref="double"/>.</param>
        /// <param name="rate2">The rate2<see cref="double"/>.</param>
        /// <param name="result">The result<see cref="double"/>.</param>
        /// <returns>The <see cref="object"/>.</returns>
        public object Present(double rate1, double rate2, double result)
        {

            try
            {
                string present = "Rate USD : " + rate1.ToString() + " EUR : " + rate2.ToString() + " EUR->USD : " + result.ToString();
                Debug.WriteLine(present);
                return present;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                throw new InvalidLaborException(ex.ToString());
            }
        }

        #endregion
    }

    /// <summary>
    /// Defines the <see cref="SecondCurrency" />.
    /// </summary>
    public class SecondCurrency
    {
        #region Methods

        /// <summary>
        /// The GetSecondCurrency.
        /// </summary>
        /// <param name="currency">The currency<see cref="string"/>.</param>
        /// <param name="days">The days<see cref="int"/>.</param>
        /// <returns>The <see cref="object"/>.</returns>
        public object GetCurrency(string currency, int days)
        {
            NBPSource kurKraju = new NBPSource(days);

            try
            {
                double rate = kurKraju.LoadRate(currency);
                Debug.WriteLine("Rate 2 : " + currency + " days past : " + days.ToString() + " amount : " + rate.ToString("#.####"));

                return new object[] { currency, rate };
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                throw new InvalidLaborException(ex.ToString());
            }
        }

        #endregion
    }
}
