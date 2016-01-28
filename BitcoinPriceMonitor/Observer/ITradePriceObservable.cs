using System.Collections.Generic;
using BitcoinPriceMonitor.Observer;

namespace BitcoinPriceMonitor
{
    public interface ITradePriceObservable
    {
        void Subscribe(ITradePriceObserver observer);
        void Unsubscribe(ITradePriceObserver observer);
        void TrasferSubscription(ITradePriceObservable observable);
    }
}
