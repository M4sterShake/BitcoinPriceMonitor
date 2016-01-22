using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitcoinPriceMonitor
{
    public interface ITradePriceObserver
    {
        Guid ObserverId { get; }
        void Update(double price);
    }
}
