using BitcoinPriceMonitor.PriceMonitor;

namespace BitcoinPriceMonitor
{
    public class Profile : IProfile
    {
        public Profile()
        {
            
        }

        public Profile(ITradePriceMonitor priceMonitor)
        {
            PriceType = priceMonitor.PriceType;
            ConvertToCurrency = priceMonitor.ConvertToCurrency;
            Frequency = priceMonitor.Frequency;
            MonitorType = priceMonitor.GetType().Name.ToString();
        }

        public TradePriceType PriceType { get; set; }
        public Currency ConvertToCurrency { get; set; }
        public long Frequency { get; set; }
        public string MonitorType { get; set; }
    }
}
