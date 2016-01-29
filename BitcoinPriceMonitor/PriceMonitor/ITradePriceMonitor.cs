namespace BitcoinPriceMonitor.PriceMonitor
{
    using System;
    using Observer;

    public interface ITradePriceMonitor : ITradePriceObservable, IDisposable
    {
        double CurrentPrice { get; }
        TradePriceType PriceType { get; set; }
        Currency TargetCurrency { get; set; }
        int Frequency { get; set; }

        void StartMonitoring();
        void StopMonitoring();
    }
}
