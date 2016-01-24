using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using StructureMap;

namespace BitcoinPriceMonitor
{
    public class IocRegistry : Registry
    {
        public IocRegistry()
        {
            For<ITradePriceMonitor>().Singleton().Use<BitcoinAveragePriceMonitor>()
                .Ctor<IRestClient>().Is(new RestClient("https://api.bitcoinaverage.com"))
                .Name = "DefaultPriceMonitor";
            For<INotificationTrayIcon>().Use<NotificationTrayIcon>();
            For<ITradePriceMonitorContextMenu>().Use<TradePriceMonitorContextMenu>()
                .Ctor<ITradePriceMonitor>().IsNamedInstance("DefaultPriceMonitor");
            For<IBitcoinPriceMonitorApp>().Use<BitcoinPriceMonitorApp>()
                .Ctor<ITradePriceMonitor>().IsNamedInstance("DefaultPriceMonitor");
        }
    }
}
