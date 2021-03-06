﻿using BitcoinPriceMonitor.Profile;

namespace BitcoinPriceMonitor.PriceMonitor
{
    public interface ITradePriceMonitorFactory
    {
        ITradePriceMonitor Get(IProfile profile);
    }
}
