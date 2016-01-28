using Microsoft.VisualBasic;

namespace BitcoinPriceMonitor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Forms;

    public class TradePriceMonitorContextMenu : ITradePriceMonitorContextMenu
    {
        public ContextMenu Menu { get; private set; }

        private const string BitcoinPriceMenuItemName = "BitcoinPrice";
        private const string LoadProfileMenuItemName = "LoadProfiles";

        private ITradePriceMonitor _tradePriceMonitor;
        private IProfileStore _profileStore;
        
        public TradePriceMonitorContextMenu(ITradePriceMonitor tradePriceMonitor, IProfileStore profileStore)
        {
            _tradePriceMonitor = tradePriceMonitor;
            _profileStore = profileStore;
            Menu = GetMenu();
        }

        public Guid ObserverId { get; } = Guid.NewGuid();

        private ContextMenu GetMenu()
        {
            var contextMenu = new ContextMenu();

            var priceTypeMenuItem = new MenuItem("Trade Price Type");
            priceTypeMenuItem.MenuItems.AddRange(GetTradePriceTypeMenuItems());

            var currencyMenuItem = new MenuItem("Currency");
            currencyMenuItem.MenuItems.AddRange(GetCurrencyMenuItems());
            
            var frequencyMenuItem = new MenuItem("Price Check Frequency");
            frequencyMenuItem.MenuItems.AddRange(GetFrequencyMenuItems());

            var loadProfileMenuItem = new MenuItem("Load Settings")
            {
                Name = LoadProfileMenuItemName
            };
            loadProfileMenuItem.MenuItems.AddRange(GetLoadProfileMenuItems());
            var saveProfileMenuItem = new MenuItem("Save Settings...", (sender, e) => SaveProfileEventHandler());

            contextMenu.MenuItems.Add(new MenuItem("Getting bitcoin price...")
            {
                Name = BitcoinPriceMenuItemName
            });
            contextMenu.MenuItems.Add("-");
            contextMenu.MenuItems.Add(priceTypeMenuItem);
            contextMenu.MenuItems.Add(currencyMenuItem);
            contextMenu.MenuItems.Add(frequencyMenuItem);
            contextMenu.MenuItems.Add("-");
            contextMenu.MenuItems.Add(loadProfileMenuItem);
            contextMenu.MenuItems.Add(saveProfileMenuItem);
            contextMenu.MenuItems.Add("-");
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

        private MenuItem[] GetLoadProfileMenuItems()
        {
            return _profileStore.Profiles?.Select(p => new MenuItem(p, (sender, e) => LoadProfileEventHandler(p))).ToArray();
        }

        private void RefreshLoadProfileMenuItems()
        {
            var loadProfileMenuItem = Menu.MenuItems.Find(LoadProfileMenuItemName, true);
            loadProfileMenuItem?[0]?.MenuItems.Clear();
            loadProfileMenuItem?[0]?.MenuItems.AddRange(GetLoadProfileMenuItems());
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
            RefreshPriceMonitor();
            UncheckMenuItems(sourceItem.Parent.MenuItems);
            sourceItem.Checked = !sourceItem.Checked;
        }

        private void CurrencyEventHandler(Currency currency, MenuItem sourceItem)
        {
            _tradePriceMonitor.ConvertToCurrency = currency;
            RefreshPriceMonitor();
            UncheckMenuItems(sourceItem.Parent.MenuItems);
            sourceItem.Checked = !sourceItem.Checked;
        }

        private void LoadProfileEventHandler(string profileName)
        {
            _tradePriceMonitor.StopMonitoring();
            var newTradePriceMonitor = _profileStore.LoadProfile(profileName);
            _tradePriceMonitor.TrasferSubscription(newTradePriceMonitor);
            _tradePriceMonitor = newTradePriceMonitor;
            _tradePriceMonitor.StartMonitoring();
        }

        private void SaveProfileEventHandler()
        {
            string profileName = Interaction.InputBox("Please enter a name for the profile", "Save Profile", string.Empty);
            if (profileName != string.Empty)
            {
                _profileStore.SaveProfile(_tradePriceMonitor, profileName);
                RefreshLoadProfileMenuItems();
            }
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

        private void RefreshPriceMonitor()
        {
            _tradePriceMonitor.StopMonitoring();
            _tradePriceMonitor.StartMonitoring();
        }
    }
}
