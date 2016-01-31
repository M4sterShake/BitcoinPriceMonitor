namespace BitcoinPriceMonitor.ContextMenu.Sections
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Forms;
    using PriceMonitor;
    using Profile;

    public class DatasourceContextMenuSection : TradePriceMonitorContextMenuSection
    {
        private const string DatasourceMenuItemName = "DatasourceMenuItemName";
        private readonly Dictionary<string, Type> _datasources = new Dictionary<string, Type>()
        {
            { "Bitcoin Average", typeof(BitcoinAveragePriceMonitor) },
            { "Coinbase", typeof(CoinbasePriceMonitor) },
            { "BTC-e", typeof(BtcePriceMonitor)}
        };

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
            TradePriceMonitor.StartMonitoring();
            MenuItemCheckedEventHandler(sourceItem);
        }
    }
}
