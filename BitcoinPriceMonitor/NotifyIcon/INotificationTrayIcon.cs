using BitcoinPriceMonitor.Observer;

namespace BitcoinPriceMonitor.NotifyIcon
{
    public interface INotificationTrayIcon : ITradePriceObserver
    {
        void Close();
    }
}
