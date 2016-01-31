namespace BitcoinPriceMonitor.ContextMenu.Sections
{
    using System.Windows.Forms;
    using PriceMonitor;

    public interface ITradePriceMonitorContextMenuSection
    {
        MenuItem GetMenuItem();
        void InitMenuItem();
        void UpdateTradePriceMonitor(ITradePriceMonitor updatedTradePriceMonitor);
    }
}
