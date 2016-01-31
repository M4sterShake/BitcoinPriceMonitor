using BitcoinPriceMonitor.PriceMonitor;

namespace BitcoinPriceMonitor.ContextMenu
{
    using System.Windows.Forms;

    public interface ITradePriceMonitorContextMenuSection
    {
        MenuItem GetMenuItem();
        void InitMenuItem();
        void UpdateTradePriceMonitor(ITradePriceMonitor updatedTradePriceMonitor);
    }
}
