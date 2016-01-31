﻿namespace BitcoinPriceMonitor.Config
{
    using System.Configuration;
    using System.IO;
    using System.Reflection;

    public class Settings : ISettings
    {
        public string ProfileStoreDirectory => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        public string BitcoinAverageApiUrl => ConfigurationManager.AppSettings["BitcoinAverageApiUrl"];
        public string CoinbaseApiUrl => ConfigurationManager.AppSettings["CoinbaseApiUrl"];
        public string BtceApiUrl => ConfigurationManager.AppSettings["BtceApiUrl"];
        public string BitstampApiUrl => ConfigurationManager.AppSettings["BitstampApiUrl"];
        public string BitfinexApiUrl => ConfigurationManager.AppSettings["BitfinexApiUrl"];
        public string JustcoinApiUrl => ConfigurationManager.AppSettings["JustcoinApiUrl"];
        public string PersistanceProfileName => "xxCURRENTxx";
    }
}
