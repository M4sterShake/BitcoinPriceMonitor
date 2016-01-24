namespace BitcoinPriceMonitor
{
    interface ISettings
    {
        string Get(string name);
        void Set(string name, string value);
    }
}
