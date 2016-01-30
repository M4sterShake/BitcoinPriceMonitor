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
            TargetCurrency = priceMonitor.TargetCurrency;
            Frequency = priceMonitor.Frequency;
            MonitorType = priceMonitor.GetType().Name;
        }

        public TradePriceType PriceType { get; set; }
        public Currency TargetCurrency { get; set; }
        public int Frequency { get; set; }
        public string MonitorType { get; set; }
    }
}
