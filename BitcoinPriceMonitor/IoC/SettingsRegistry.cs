namespace BitcoinPriceMonitor.IoC
{
    using Config;
    using StructureMap;

    class SettingsRegistry : Registry
    {
        public SettingsRegistry()
        {
            For<ISettings>().Use<Settings>();
        }
    }
}
