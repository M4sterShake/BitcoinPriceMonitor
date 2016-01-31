using BitcoinPriceMonitor.PriceMonitor;

namespace BitcoinPriceMonitor.Profile
{
    public interface ILoadProfileListener
    {
        void ProfileLoaded(ITradePriceMonitor loadedPriceMonitor);
    }
}
