using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitcoinPriceMonitor
{
    public interface INotificationTrayIcon : ITradePriceObserver
    {
        void Update(string iconText);
        void Close();
    }
}
