namespace BitcoinPriceMonitor.Profile
{
    using System.Collections.Generic;
    using PriceMonitor;

    public interface IProfileStore
    {
        IEnumerable<string> Profiles { get; }
        ITradePriceMonitor LoadProfile(string profileName);
        void SaveProfile(ITradePriceMonitor profile, string profileName);
        void SavePersistenceProfile(ITradePriceMonitor profile);
    }
}
