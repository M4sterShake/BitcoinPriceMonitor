using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BitcoinPriceMonitor.PriceMonitor;

namespace BitcoinPriceMonitor
{
    public interface IProfile
    {
        TradePriceType PriceType { get; set; }
        Currency ConvertToCurrency { get; set; }
        long Frequency { get; set; }
        string MonitorType { get; set; }
    }
}
