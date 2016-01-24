namespace BitcoinPriceMonitor
{
    public interface INotificationTrayIcon : ITradePriceObserver
    {
        void Close();
    }
}
