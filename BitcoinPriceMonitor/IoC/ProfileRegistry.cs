using BitcoinPriceMonitor.Config;
using BitcoinPriceMonitor.PriceMonitor;

namespace BitcoinPriceMonitor.IoC
{
    using StructureMap;
    using Profile;

    public class ProfileRegistry : Registry
    {
        public ProfileRegistry(ISettings settings)
        {
            For<ISettings>().Use(settings);
            For<ITradePriceMonitorFactory>().Use<TradePriceMonitorFactory>();
            For<IProfileStore>().Use<ProfileStore>();
        }
    }
}
