namespace BitcoinPriceMonitor.Observer
{
    public interface ITradePriceObservable
    {
        void Subscribe(ITradePriceObserver observer);
        void Unsubscribe(ITradePriceObserver observer);
        void TrasferSubscription(ITradePriceObservable observable);
    }
}
