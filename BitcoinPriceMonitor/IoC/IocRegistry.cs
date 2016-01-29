namespace BitcoinPriceMonitor
{
    using RestSharp;
    using StructureMap;
    using ApplicationEntryPoint;
    using Config;
    using ContextMenu;
    using IoC;
    using NotifyIcon;
    using PriceMonitor;
    using Profile;

    public class IocRegistry : Registry
    {
        public IocRegistry()
        {
            var settingsContainer = new Container(new SettingsRegistry());
            var settings = settingsContainer.GetInstance<ISettings>();
            var profileContainer = new Container(new ProfileRegistry(settings));
            var profileStore = profileContainer.GetInstance<IProfileStore>();
            var persistenceProfile = profileStore.LoadProfile(settings.PersistanceProfileName);

            For<ITradePriceMonitor>().Use(persistenceProfile)
                .Name = "DefaultPriceMonitor";
            For<INotificationTrayIcon>().Use<NotificationTrayIcon>();
            For<ITradePriceMonitorContextMenu>().Use<TradePriceMonitorContextMenu>()
                .Ctor<ITradePriceMonitor>().IsNamedInstance("DefaultPriceMonitor");
            For<IBitcoinPriceMonitorApp>().Use<BitcoinPriceMonitorApp>()
                .Ctor<ITradePriceMonitor>().IsNamedInstance("DefaultPriceMonitor");
            For<IProfileStore>().Use(profileStore);
            For<ISettings>().Use(settings);
            For<ITradePriceMonitorFactory>().Singleton().Use<TradePriceMonitorFactory>();
        }
    }
}
