namespace BitcoinPriceMonitor.ContextMenu
{
    using Observer;

    public interface ITradePriceMonitorContextMenu : ITradePriceObserver
    {
        System.Windows.Forms.ContextMenu Menu { get; }
    }
}
