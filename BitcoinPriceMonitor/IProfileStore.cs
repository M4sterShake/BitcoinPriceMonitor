namespace BitcoinPriceMonitor
{
    using System.Collections.Generic;

    public interface IProfileStore
    {
        IEnumerable<string> Profiles { get; }
        ITradePriceMonitor LoadProfile(string profileName);
        void SaveProfile(ITradePriceMonitor profile, string profileName);
    }
}
