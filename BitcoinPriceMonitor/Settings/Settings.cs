using System.IO;
using System.Reflection;

namespace BitcoinPriceMonitor
{
    using System.Configuration;

    public class Settings : ISettings
    {
        public string ProfileStoreDirectory => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        public string BitcoinAverageApiUrl => ConfigurationManager.AppSettings["BitcoinAverageApiUrl"];
    }
}
