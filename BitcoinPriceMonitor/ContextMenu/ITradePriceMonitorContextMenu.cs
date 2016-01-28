using BitcoinPriceMonitor.Observer;

namespace BitcoinPriceMonitor.ContextMenu
{
    public interface ITradePriceMonitorContextMenu : ITradePriceObserver
    {
        System.Windows.Forms.ContextMenu Menu { get; }
    }
}
