using System.Collections.Generic;

namespace BitcoinPriceMonitor
{
    public interface ITradePriceObservable
    {
        void Subscribe(ITradePriceObserver observer);
        void Unsubscribe(ITradePriceObserver observer);
        void TrasferSubscription(ITradePriceObservable observable);
    }
}
