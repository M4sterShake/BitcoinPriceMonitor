namespace BitcoinPriceMonitor.Profile
{
    using PriceMonitor;

    public class Profile : IProfile
    {
        public Profile()
        {
            
        }

        public Profile(ITradePriceMonitor priceMonitor)
        {
            PriceType = priceMonitor.PriceType;
            ConvertToCurrency = priceMonitor.TargetCurrency;
            Frequency = priceMonitor.Frequency;
            MonitorType = priceMonitor.GetType().Name;
        }

        public TradePriceType PriceType { get; set; }
        public Currency ConvertToCurrency { get; set; }
        public int Frequency { get; set; }
        public string MonitorType { get; set; }
    }
}
