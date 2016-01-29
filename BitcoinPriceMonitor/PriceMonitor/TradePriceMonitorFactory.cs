using BitcoinPriceMonitor.Config;
using BitcoinPriceMonitor.Profile;

namespace BitcoinPriceMonitor.PriceMonitor
{
    using RestSharp;

    public class TradePriceMonitorFactory : ITradePriceMonitorFactory
    {
        private readonly ISettings _settings;

        public TradePriceMonitorFactory(ISettings settings)
        {
            _settings = settings;
        }

        public ITradePriceMonitor Get(IProfile profile)
        {
            switch (profile.MonitorType)
            {
                case "BitcoinAveragePriceMonitor":
                    return new BitcoinAveragePriceMonitor(new RestClient(), _settings)
                    {
                        ConvertToCurrency = profile.ConvertToCurrency,
                        Frequency = profile.Frequency,
                        PriceType = profile.PriceType
                    };
                default:
                    return null;
            }
        }
    }
}
