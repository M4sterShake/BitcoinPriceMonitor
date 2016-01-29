using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BitcoinPriceMonitor.Config;
using RestSharp;

namespace BitcoinPriceMonitor.PriceMonitor
{
    public abstract class RestTradePriceMonitor : TradePriceMonitor
    {
        protected readonly IRestClient ApiClient;
        protected readonly ISettings Settings;

        protected RestTradePriceMonitor(IRestClient apiClient, ISettings settings)
        {
            ApiClient = apiClient;
            Settings = settings;
        }
    }
}
