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

        public ITradePriceMonitor Get(string monitorType)
        {
            switch (monitorType)
            {
                case "BitcoinAveragePriceMonitor":
                    return new BitcoinAveragePriceMonitor(new RestClient(), _settings);
                case "CoinbasePriceMonitor":
                    return new CoinbasePriceMonitor(new RestClient(), _settings);
                default:
                    return null;
            }
        }

        public ITradePriceMonitor Get(IProfile profile)
        {
            var monitor = Get(profile.MonitorType);
            monitor.PriceType = profile.PriceType;
            monitor.TargetCurrency = profile.TargetCurrency;
            monitor.Frequency = profile.Frequency;

            return monitor;
        }
    }
}
