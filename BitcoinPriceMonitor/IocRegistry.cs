namespace BitcoinPriceMonitor
{
    using RestSharp;
    using StructureMap;

    public class IocRegistry : Registry
    {
        public IocRegistry(ISettings settings)
        {
            For<ITradePriceMonitor>().Use<BitcoinAveragePriceMonitor>()
                .Ctor<IRestClient>().Is(new RestClient())
                .Name = "DefaultPriceMonitor";
            For<INotificationTrayIcon>().Use<NotificationTrayIcon>();
            For<ITradePriceMonitorContextMenu>().Use<TradePriceMonitorContextMenu>()
                .Ctor<ITradePriceMonitor>().IsNamedInstance("DefaultPriceMonitor");
            For<IBitcoinPriceMonitorApp>().Use<BitcoinPriceMonitorApp>()
                .Ctor<ITradePriceMonitor>().IsNamedInstance("DefaultPriceMonitor");
            For<IProfileStore>().Use<ProfileStore>();
            For<ISettings>().Use(settings);
            For<ITradePriceMonitorFactory>().Singleton().Use<TradePriceMonitorFactory>();
        }
    }
}
