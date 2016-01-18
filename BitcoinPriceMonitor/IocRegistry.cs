using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StructureMap;

namespace BitcoinPriceMonitor
{
    class IocRegistry : Registry
    {
        public IocRegistry()
        {
            For<ITradePriceMonitor>().Use<BitcoinAveragePriceMonitor>()
                .Ctor<TradePriceType>().Is(TradePriceType.Last)
                .Ctor<long>().Is(2000);

            For<ITradePriceMonitorContextMenu>().Use<TradePriceMonitorContextMenu>();
            For<INotificationTrayIcon>().Use<NotificationTrayIcon>();

        }
    }
}
