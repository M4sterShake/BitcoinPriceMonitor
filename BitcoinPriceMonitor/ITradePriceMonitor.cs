using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitcoinPriceMonitor
{
    public interface ITradePriceMonitor
    {
        double CurrentPrice { get; }
        TradePriceType PriceType { get; set; }
        long Frequency { get; set; }

        void StartMonitoring(Action<double> callback = null);
        void StopMonitoring();
    }
}
