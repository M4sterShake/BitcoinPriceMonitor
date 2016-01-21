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
            For<ITradePriceMonitor>().Singleton().Use<BitcoinAveragePriceMonitor>()
                .Ctor<TradePriceType>().Is(TradePriceType.Last)
                .Ctor<long>().Is(2000)
                .Name = "DefaultPriceMonitor";
            For<INotificationTrayIcon>().Use<NotificationTrayIcon>();
            For<ITradePriceMonitorContextMenu>().Use<TradePriceMonitorContextMenu>()
                .Ctor<ITradePriceMonitor>().IsNamedInstance("DefaultPriceMonitor");
            For<IBitcoinPriceMonitorApp>().Use<BitcoinPriceMonitorApp>()
                .Ctor<ITradePriceMonitor>().IsNamedInstance("DefaultPriceMonitor");
        }
    }
}
