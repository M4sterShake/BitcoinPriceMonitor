namespace BitcoinPriceMonitor
{
    using System.Windows.Forms;

    public interface ITradePriceMonitorContextMenu : ITradePriceObserver
    {
        ContextMenu Menu { get; }
    }
}
