using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace BitcoinPriceMonitor
{
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
