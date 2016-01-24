namespace BitcoinPriceMonitor
{
    public interface ITradePriceObservable
    {
        void Subscribe(ITradePriceObserver observer);
        void Unsubscribe(ITradePriceObserver observer);
    }
}
