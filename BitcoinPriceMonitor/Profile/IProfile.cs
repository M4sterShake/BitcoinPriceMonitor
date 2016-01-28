namespace BitcoinPriceMonitor.Profile
{
    using PriceMonitor;

    public interface IProfile
    {
        TradePriceType PriceType { get; set; }
        Currency ConvertToCurrency { get; set; }
        long Frequency { get; set; }
        string MonitorType { get; set; }
    }
}
