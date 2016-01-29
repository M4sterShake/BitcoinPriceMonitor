namespace BitcoinPriceMonitor.Observer
{
    using System;

    public interface ITradePriceObserver
    {
        Guid ObserverId { get; }
        void Update(TradePrice price);
    }
}
