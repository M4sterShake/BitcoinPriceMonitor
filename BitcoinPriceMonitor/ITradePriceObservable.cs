using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitcoinPriceMonitor
{
    interface ITradePriceObservable
    {
        void Subscribe(ITradePriceObserver observer);
        void Unsubscribe(ITradePriceObserver observer);
    }
}
