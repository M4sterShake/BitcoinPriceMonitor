namespace BitcoinPriceMonitor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Forms;

    public class TradePriceMonitorContextMenu : ITradePriceMonitorContextMenu
    {
        public ContextMenu Menu { get; private set; }

        private ITradePriceMonitor _tradePriceMonitor;

        public TradePriceMonitorContextMenu(ITradePriceMonitor tradePriceMonitor)
        {
            _tradePriceMonitor = tradePriceMonitor;
            Menu = GetMenu();
        }

        public Guid ObserverId { get; } = Guid.NewGuid();

        private ContextMenu GetMenu()
        {
            var contextMenu = new ContextMenu();
            var settingsMenuItem = new MenuItem("Settings");

            var priceTypeMenuItem = new MenuItem("Trade Price Type");
            priceTypeMenuItem.MenuItems.AddRange(GetTradePriceTypeMenuItems());

            var currencyMenuItem = new MenuItem("Currency");
            currencyMenuItem.MenuItems.AddRange(GetCurrencyMenuItems());
            
            var frequencyMenuItem = new MenuItem("Price Check Frequency");
            frequencyMenuItem.MenuItems.AddRange(GetFrequencyMenuItems());

            settingsMenuItem.MenuItems.Add(priceTypeMenuItem);
            settingsMenuItem.MenuItems.Add(currencyMenuItem);
            settingsMenuItem.MenuItems.Add(frequencyMenuItem);

            contextMenu.MenuItems.Add(new MenuItem("Getting bitcoin price...")
            {
                Name = "BitcoinPrice"
            });
            contextMenu.MenuItems.Add("-");
            contextMenu.MenuItems.Add(settingsMenuItem);
            contextMenu.MenuItems.Add("Exit", ExitEventHandler);

            return contextMenu;
        }

        private MenuItem[] GetCurrencyMenuItems()
        {
            List<MenuItem> menuItems = new List<MenuItem>();
            foreach (var currency in Enum.GetValues(typeof(Currency)).Cast<Currency>())
            {
                menuItems.Add(new MenuItem(currency.ToString(),
                    (sender, e) => CurrencyEventHandler(currency, (MenuItem)sender))
                {
                    RadioCheck = true,
                    Checked = _tradePriceMonitor.ConvertToCurrency == currency
                });
            }

            return menuItems.ToArray();
        }

        private MenuItem[] GetTradePriceTypeMenuItems()
        {
            List<MenuItem> menuItems = new List<MenuItem>();
            foreach (var tradePriceType in Enum.GetValues(typeof(TradePriceType)).Cast<TradePriceType>())
            {
                menuItems.Add(new MenuItem(tradePriceType.ToString(),
                    (sender, e) => TradePriceTypeEventHandler(tradePriceType, (MenuItem)sender))
                {
                    RadioCheck = true,
                    Checked = _tradePriceMonitor.PriceType == tradePriceType
                });
            }

            return menuItems.ToArray();
        }

        private MenuItem[] GetFrequencyMenuItems()
        {
            List<MenuItem> menuItems = new List<MenuItem>();
            return menuItems.ToArray();
        }

        private void UncheckMenuItems(Menu.MenuItemCollection menuItems)
        {
            foreach (MenuItem m in menuItems)
            {
                m.Checked = false;
            }
        }

        #region Event Handlers
        private void TradePriceTypeEventHandler(TradePriceType tradePriceType, MenuItem sourceItem)
        {
            _tradePriceMonitor.PriceType = tradePriceType;
            UncheckMenuItems(sourceItem.Parent.MenuItems);
            sourceItem.Checked = !sourceItem.Checked;
        }

        private void CurrencyEventHandler(Currency currency, MenuItem sourceItem)
        {
            _tradePriceMonitor.ConvertToCurrency = currency;
            UncheckMenuItems(sourceItem.Parent.MenuItems);
            sourceItem.Checked = !sourceItem.Checked;
        }

        private void ExitEventHandler(object sender, EventArgs e)
        {
            Application.Exit();
        }
        #endregion

        public void Update(double value)
        {
            var foundMenuItems  = Menu.MenuItems.Find("BitcoinPrice", false);
            if (foundMenuItems?.Length > 0)
            {
                foundMenuItems[0].Text = $"{value} {_tradePriceMonitor.ConvertToCurrency}";
            }
        }
    }
}
