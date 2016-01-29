namespace BitcoinPriceMonitor.Config
{
    public interface ISettings
    {
        string ProfileStoreDirectory { get; }
        string BitcoinAverageApiUrl { get; }
        string CoinbaseApiUrl { get; }
        string PersistanceProfileName { get; }
    }
}
