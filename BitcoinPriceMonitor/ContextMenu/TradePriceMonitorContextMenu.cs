namespace BitcoinPriceMonitor.ContextMenu
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Forms;
    using Microsoft.VisualBasic;
    using PriceMonitor;
    using Profile;

    public class TradePriceMonitorContextMenu : ITradePriceMonitorContextMenu
    {
        public ContextMenu Menu { get; }

        private const string BitcoinPriceMenuItemName = "BitcoinPrice";
        private const string LoadProfileMenuItemName = "LoadProfiles";

        private ITradePriceMonitor _tradePriceMonitor;
        private readonly IProfileStore _profileStore;
        private readonly Dictionary<string, int> _availableFrequencies = new Dictionary<string, int>()
        {
            { "5 Seconds", 1000 * 5 },
            { "10 Seconds", 1000 * 10 },
            { "30 Seconds", 1000 * 30 },
            { "1 Minute", 1000 * 60 },
            { "2 Minute", 11000 * 60 * 2 },
            { "5 Minute", 1000 * 60 * 5 },
            { "10 Minute", 1000 * 60 * 10 },
            { "15 Minute", 1000 * 60 * 15 },
            { "20 Minute", 1000 * 60 * 20 },
            { "30 Minute", 1000 * 60 * 30 },
            { "45 Minute", 1000 * 60 * 45 },
            { "1 Hour", 1000 * 60 * 60 }
        };

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
            return Enum.GetValues(typeof (Currency)).Cast<Currency>().Select(currency => new MenuItem(currency.ToString(), (sender, e) => CurrencyEventHandler(currency, (MenuItem) sender))
            {
                RadioCheck = true, Checked = _tradePriceMonitor.ConvertToCurrency == currency
            }).ToArray();
        }

        private MenuItem[] GetTradePriceTypeMenuItems()
        {
            return Enum.GetValues(typeof (TradePriceType)).Cast<TradePriceType>().Select(tradePriceType => new MenuItem(tradePriceType.ToString(), (sender, e) => TradePriceTypeEventHandler(tradePriceType, (MenuItem) sender))
            {
                RadioCheck = true, Checked = _tradePriceMonitor.PriceType == tradePriceType
            }).ToArray();
        }

        private MenuItem[] GetFrequencyMenuItems()
        {
            return _availableFrequencies.Select(frequency => new MenuItem(frequency.Key, (sender, e) => FrequencyEventHander(frequency.Value, (MenuItem) sender))
            {
                RadioCheck = true, Checked = frequency.Key == "5 Seconds"
            }).ToArray();
        }

        private MenuItem[] GetLoadProfileMenuItems()
        {
            return _profileStore.Profiles?.Select(p => new MenuItem(p, (sender, e) => LoadProfileEventHandler(p))).ToArray();
        }

        private void RefreshLoadProfileMenuItems()
        {
            var loadProfileMenuItem = Menu.MenuItems.Find(LoadProfileMenuItemName, true);
            loadProfileMenuItem[0]?.MenuItems.Clear();
            loadProfileMenuItem[0]?.MenuItems.AddRange(GetLoadProfileMenuItems());
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

        private void FrequencyEventHander(int frequency, MenuItem sourceItem)
        {
            _tradePriceMonitor.Frequency = frequency;
            RefreshPriceMonitor();
            UncheckMenuItems(sourceItem.Parent.MenuItems);
            sourceItem.Checked = !sourceItem.Checked;
        }

        private void LoadProfileEventHandler(string profileName)
        {
            var newTradePriceMonitor = _profileStore.LoadProfile(profileName);
            _tradePriceMonitor.TrasferSubscription(newTradePriceMonitor);
            _tradePriceMonitor.Dispose();
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
            if (foundMenuItems.Length > 0)
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
