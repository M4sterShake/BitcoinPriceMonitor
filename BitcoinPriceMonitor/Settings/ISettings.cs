﻿namespace BitcoinPriceMonitor
{
    public interface ISettings
    {
        string ProfileStoreDirectory { get; }
        string BitcoinAverageApiUrl { get; }
    }
}
