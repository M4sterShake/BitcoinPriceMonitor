namespace BitcoinPriceMonitor
{
    public interface ITradePriceMonitor : ITradePriceObservable
    {
        double CurrentPrice { get; }
        TradePriceType PriceType { get; set; }
        Currency ConvertToCurrency { get; set; }
        long Frequency { get; set; }

        void StartMonitoring();
        void StopMonitoring();
    }
}
