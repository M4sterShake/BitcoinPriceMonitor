namespace BitcoinPriceMonitor.IoC
{
    using ApplicationEntryPoint;
    using Config;
    using ContextMenu;
    using NotifyIcon;
    using PriceMonitor;
    using Profile;
    using StructureMap;

    public class IocRegistry : Registry
    {
        public IocRegistry()
        {
            var settingsContainer = new Container(new SettingsRegistry());
            var settings = settingsContainer.GetInstance<ISettings>();
            var profileContainer = new Container(new ProfileRegistry(settings));
            var profileStore = profileContainer.GetInstance<IProfileStore>();
            var persistenceProfile = profileStore.LoadProfile(settings.PersistanceProfileName);

            For<ITradePriceMonitorContextMenuSection>().Use<DatasourceContextMenuSection>()
                .Named("DatasourceMenuSection");
            For<ITradePriceMonitorContextMenuSection>().Add<CurrencyContextMenuItemSection>()
                .Named("CurrencyMenuSection");
            For<ITradePriceMonitorContextMenuSection>().Add<TradePriceTypeContextMenuSection>()
                .Named("PriceTypeMenuSection");
            For<ITradePriceMonitorContextMenuSection>().Add<FrequencyContextMenuSection>()
                .Named("FrequencyMenuSection");
            For<ITradePriceMonitorContextMenuSection>().Add<LoadProfileContextMenuSection>()
                .Named("LoadProfileMenuSection");
            For<ITradePriceMonitorContextMenuSection>().Add<SaveProfileContextMenuSection>()
                .Named("SaveProfileMenuSection");

            For<ITradePriceMenuSections>().Use<TradePriceMenuSections>()
                .Ctor<ITradePriceMonitorContextMenuSection>("datasourceSection").IsNamedInstance("DatasourceMenuSection")
                .Ctor<ITradePriceMonitorContextMenuSection>("currencySection").IsNamedInstance("CurrencyMenuSection")
                .Ctor<ITradePriceMonitorContextMenuSection>("priceTypeSection").IsNamedInstance("PriceTypeMenuSection")
                .Ctor<ITradePriceMonitorContextMenuSection>("frequencySection").IsNamedInstance("FrequencyMenuSection")
                .Ctor<ITradePriceMonitorContextMenuSection>("loadProfileSection").IsNamedInstance("LoadProfileMenuSection")
                .Ctor<ITradePriceMonitorContextMenuSection>("saveProfileSection").IsNamedInstance("SaveProfileMenuSection");
            
            For<ITradePriceMonitor>().Use(persistenceProfile)
                .Name = "DefaultPriceMonitor";
            For<INotificationTrayIcon>().Use<NotificationTrayIcon>();
            For<ITradePriceMonitorContextMenu>().Use<TradePriceMonitorContextMenu>()
                .Ctor<ITradePriceMonitor>().IsNamedInstance("DefaultPriceMonitor")
                .Named("DefaultContextMenu");
            For<IBitcoinPriceMonitorApp>().Use<BitcoinPriceMonitorApp>()
                .Ctor<ITradePriceMonitor>().IsNamedInstance("DefaultPriceMonitor");
            For<IProfileStore>().Use(profileStore);
            For<ISettings>().Use(settings);
            For<ITradePriceMonitorFactory>().Singleton().Use<TradePriceMonitorFactory>();
        }
    }
}
