using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitcoinPriceMonitor
{
    interface INotificationTrayIcon
    {
        void Update(string iconText);
    }
}
