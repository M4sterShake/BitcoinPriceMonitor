namespace BitcoinPriceMonitor
{
    using System.Configuration;

    class Settings : ISettings
    {
        public string Get(string name)
        {
            return ConfigurationManager.AppSettings.Get(name);
        }

        public void Set(string name, string value)
        {
            ConfigurationManager.AppSettings.Set(name, value);
        }
    }
}
