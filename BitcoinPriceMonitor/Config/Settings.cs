﻿namespace BitcoinPriceMonitor.Config
{
    using System.Configuration;
    using System.IO;
    using System.Reflection;

    public class Settings : ISettings
    {
        public string ProfileStoreDirectory => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        public string BitcoinAverageApiUrl => ConfigurationManager.AppSettings["BitcoinAverageApiUrl"];
        public string PersistanceProfileName => "xxCURRENTxx";
    }
}
