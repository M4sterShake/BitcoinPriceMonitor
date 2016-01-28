namespace BitcoinPriceMonitor
{
    public interface ITradePriceMonitorFactory
    {
        ITradePriceMonitor Get(IProfile profile);
    }
}
