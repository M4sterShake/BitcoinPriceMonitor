using System.Linq;

namespace BitcoinPriceMonitor.PriceMonitor
{
    using Config;
    using Profile;
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
                case "BtcePriceMonitor":
                    return new BtcePriceMonitor(new RestClient(), _settings);
                case "BitstampPriceMonitor":
                    return new BitstampPriceMonitor(new RestClient(), _settings);
                case "BitfinexPriceMonitor":
                    return new BitfinexPriceMonitor(new RestClient(), _settings);
                case "JustcoinPriceMonitor":
                    return new BitfinexPriceMonitor(new RestClient(), _settings);
                default:
                    return null;
            }
        }

        public ITradePriceMonitor Get(IProfile profile)
        {
            var monitor = Get(profile.MonitorType);
            monitor.PriceType = profile.PriceType;
            monitor.TargetCurrency = monitor.SupportedCurrencies.Contains(profile.TargetCurrency)
                ? profile.TargetCurrency
                : monitor.SupportedCurrencies.First();
            monitor.Frequency = profile.Frequency;

            return monitor;
        }
    }
}
