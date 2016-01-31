namespace BitcoinPriceMonitor.ContextMenu
{
    using Sections;

    public struct TradePriceMenuSections : ITradePriceMenuSections
    {
        public TradePriceMenuSections(ITradePriceMonitorContextMenuSection datasourceSection,
            ITradePriceMonitorContextMenuSection currencySection,
            ITradePriceMonitorContextMenuSection priceTypeSection,
            ITradePriceMonitorContextMenuSection frequencySection,
            ITradePriceMonitorContextMenuSection loadProfileSection,
            ITradePriceMonitorContextMenuSection saveProfileSection)
        {
            DatasourceSection = datasourceSection;
            CurrencySection = currencySection;
            PriceTypeSection = priceTypeSection;
            FrequencySection = frequencySection;
            LoadProfileSection = loadProfileSection;
            SaveProfileSection = saveProfileSection;
        }

        public ITradePriceMonitorContextMenuSection DatasourceSection { get; }
        public ITradePriceMonitorContextMenuSection CurrencySection { get; }
        public ITradePriceMonitorContextMenuSection PriceTypeSection { get; }
        public ITradePriceMonitorContextMenuSection FrequencySection { get; }
        public ITradePriceMonitorContextMenuSection LoadProfileSection { get; }
        public ITradePriceMonitorContextMenuSection SaveProfileSection { get; }
    }
}
