namespace BitcoinPriceMonitor.ContextMenu
{
    using Sections;

    public interface ITradePriceMenuSections
    {
        ITradePriceMonitorContextMenuSection DatasourceSection { get; }
        ITradePriceMonitorContextMenuSection CurrencySection { get; }
        ITradePriceMonitorContextMenuSection PriceTypeSection { get; }
        ITradePriceMonitorContextMenuSection FrequencySection { get; }
        ITradePriceMonitorContextMenuSection LoadProfileSection { get; }
        ITradePriceMonitorContextMenuSection SaveProfileSection { get; }
    }
}
