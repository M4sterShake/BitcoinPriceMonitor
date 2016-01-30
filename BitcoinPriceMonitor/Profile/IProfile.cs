namespace BitcoinPriceMonitor.Profile
{
    using PriceMonitor;

    public interface IProfile
    {
        TradePriceType PriceType { get; set; }
        Currency TargetCurrency { get; set; }
        int Frequency { get; set; }
        string MonitorType { get; set; }
    }
}
