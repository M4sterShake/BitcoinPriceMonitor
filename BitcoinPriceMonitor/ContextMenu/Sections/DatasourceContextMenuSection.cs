namespace BitcoinPriceMonitor.ContextMenu.Sections
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Forms;
    using PriceMonitor;
    using Profile;

    public class DatasourceContextMenuSection : TradePriceMonitorContextMenuSection, IProfileLoader
    {
        private const string DatasourceMenuItemName = "DatasourceMenuItemName";
        private readonly Dictionary<string, Type> _datasources = new Dictionary<string, Type>()
        {
            { "Bitcoin Average (Worldwide)", typeof(BitcoinAveragePriceMonitor) },
            { "Coinbase (USA)", typeof(CoinbasePriceMonitor) },
            { "BTC-e (Russia)", typeof(BtcePriceMonitor)},
            { "Bitstamp (USA - USD Only)", typeof(BitstampPriceMonitor) },
            { "Bitfinex (Hong Kong)", typeof(BitfinexPriceMonitor) }
        };
        private List<ILoadProfileListener> _loadProfileListeners = new List<ILoadProfileListener>();

        private readonly ITradePriceMonitorFactory _monitorFactory;

        public DatasourceContextMenuSection(ITradePriceMonitor tradePriceMonitor, IProfileStore profileStore, ITradePriceMonitorFactory monitorFactory)
            : base(tradePriceMonitor, profileStore)
        {
            _monitorFactory = monitorFactory;
        }

        public override MenuItem GetMenuItem()
        {
            var subMenuItems =
                _datasources.Select(
                    datasource =>
                        new MenuItem(datasource.Key,
                            (sender, e) => ClickEventHandler(datasource.Value.Name, (MenuItem)sender))
                        {
                            RadioCheck = true
                        }).ToArray();
            ContextMenuItem = CreateMenuItem("Datasource", DatasourceMenuItemName, subMenuItems);
            InitMenuItem();

            return ContextMenuItem;
        }

        public override void InitMenuItem()
        {
            foreach (MenuItem item in ContextMenuItem.MenuItems)
            {
                item.Checked = item.Text ==
                               _datasources.Where(d => d.Value == TradePriceMonitor.GetType())
                                   .Select(d => d.Key)
                                   .FirstOrDefault();
            }
        }

        private void ClickEventHandler(string datasourceName, MenuItem sourceItem)
        {
            var newTradePriceMonitor = _monitorFactory.Get(new Profile
            {
                MonitorType = datasourceName,
                Frequency = TradePriceMonitor.Frequency,
                TargetCurrency = TradePriceMonitor.TargetCurrency,
                PriceType = TradePriceMonitor.PriceType
            });
            TradePriceMonitor.TrasferSubscription(newTradePriceMonitor);
            TradePriceMonitor.Dispose();
            TradePriceMonitor = newTradePriceMonitor;
            Notify(TradePriceMonitor);
            TradePriceMonitor.StartMonitoring();
            MenuItemCheckedEventHandler(sourceItem);
        }

        public void Subscribe(ILoadProfileListener listener)
        {
            _loadProfileListeners.Add(listener);
        }

        private void Notify(ITradePriceMonitor newPriceMonitor)
        {
            foreach (var listener in _loadProfileListeners)
            {
                listener.ProfileLoaded(newPriceMonitor);
            }
        }
    }
}
