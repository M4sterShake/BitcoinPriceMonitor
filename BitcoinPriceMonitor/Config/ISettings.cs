namespace BitcoinPriceMonitor.Config
{
    public interface ISettings
    {
        string ProfileStoreDirectory { get; }
        string BitcoinAverageApiUrl { get; }
        string CoinbaseApiUrl { get; }
        string BtceApiUrl { get; }
        string BitstampApiUrl { get; }
        string BitfinexApiUrl { get; }
        string JustcoinApiUrl { get; }
        string PersistanceProfileName { get; }
    }
}
