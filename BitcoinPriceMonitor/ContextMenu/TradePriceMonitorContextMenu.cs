using BitcoinPriceMonitor.Observer;

namespace BitcoinPriceMonitor.ContextMenu
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Forms;
    using Microsoft.VisualBasic;
    using PriceMonitor;
    using Profile;
    using System.Collections;

    public class TradePriceMonitorContextMenu : ITradePriceMonitorContextMenu
    {
        public ContextMenu Menu { get; }

        private const string BitcoinPriceMenuItemName = "BitcoinPrice";
        private const string CurrencyMenuItemName = "Currecy";
        private const string PriceTypeMenuItemName = "PriceType";
        private const string FrequencyMenuItemName = "Frequency";
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
            InitMenuOptions();
        }

        public Guid ObserverId { get; } = Guid.NewGuid();

        private ContextMenu GetMenu()
        {
            var contextMenu = new ContextMenu();

            var priceTypeMenuItem = new MenuItem("Trade Price Type")
            {
                Name = PriceTypeMenuItemName
            };
            priceTypeMenuItem.MenuItems.AddRange(GetTradePriceTypeMenuItems());

            var currencyMenuItem = new MenuItem("Currency")
            {
                Name = CurrencyMenuItemName
            };
            currencyMenuItem.MenuItems.AddRange(GetCurrencyMenuItems());
            
            var frequencyMenuItem = new MenuItem("Price Check Frequency")
            {
                Name = FrequencyMenuItemName
            };
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
                RadioCheck = true
            }).ToArray();
        }

        private MenuItem[] GetTradePriceTypeMenuItems()
        {
            return Enum.GetValues(typeof (TradePriceType)).Cast<TradePriceType>().Select(tradePriceType => new MenuItem(tradePriceType.ToString(), (sender, e) => TradePriceTypeEventHandler(tradePriceType, (MenuItem) sender))
            {
                RadioCheck = true
            }).ToArray();
        }

        private MenuItem[] GetFrequencyMenuItems()
        {
            return _availableFrequencies.Select(frequency => new MenuItem(frequency.Key, (sender, e) => FrequencyEventHander(frequency.Value, (MenuItem) sender))
            {
                RadioCheck = true
            }).ToArray();
        }

        private MenuItem[] GetLoadProfileMenuItems()
        {
            return _profileStore.Profiles?.Select(p =>
            {
                var profileMenuItem = new MenuItem(p);
                profileMenuItem.MenuItems.Add(new MenuItem("Load", (sender, e) => LoadProfileEventHandler(p)));
                profileMenuItem.MenuItems.Add(new MenuItem("Remove", (sender, e) => RemoveProfileEventHandler(p)));
                return profileMenuItem;
            }).ToArray();
        }

        private void InitMenuOptions()
        {
            InitCurrencyMenuItems();
            InitPriceTypeMenuItems();
            InitFrequencyMenuItems();
        }

        private void InitCurrencyMenuItems()
        {
            var currencyMenuItem = Menu.MenuItems.Find(CurrencyMenuItemName, true)[0];
            foreach (MenuItem item in currencyMenuItem.MenuItems)
            {
                item.Checked = item.Text == _tradePriceMonitor.TargetCurrency.ToString();
            }
        }

        private void InitPriceTypeMenuItems()
        {
            var priceTypeMenuItem = Menu.MenuItems.Find(PriceTypeMenuItemName, true)[0];
            foreach (MenuItem item in priceTypeMenuItem.MenuItems)
            {
                item.Checked = item.Text == _tradePriceMonitor.PriceType.ToString();
            }
        }

        private void InitFrequencyMenuItems()
        {
            var frequencyMenuItem = Menu.MenuItems.Find(FrequencyMenuItemName, true)[0];
            foreach (MenuItem item in frequencyMenuItem.MenuItems)
            {
                item.Checked = item.Text ==
                               _availableFrequencies.Where(f => f.Value == _tradePriceMonitor.Frequency)
                                   .Select(f => f.Key)
                                   .FirstOrDefault();
            }
        }

        private void RefreshLoadProfileMenuItems()
        {
            var loadProfileMenuItem = Menu.MenuItems.Find(LoadProfileMenuItemName, true);
            loadProfileMenuItem[0]?.MenuItems.Clear();
            loadProfileMenuItem[0]?.MenuItems.AddRange(GetLoadProfileMenuItems());
        }

        private void UncheckMenuItems(IEnumerable menuItems)
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
            MenuItemCheckedEventHandler(sourceItem);
        }

        private void CurrencyEventHandler(Currency currency, MenuItem sourceItem)
        {
            _tradePriceMonitor.TargetCurrency = currency;
            MenuItemCheckedEventHandler(sourceItem);
        }

        private void FrequencyEventHander(int frequency, MenuItem sourceItem)
        {
            _tradePriceMonitor.Frequency = frequency;
            MenuItemCheckedEventHandler(sourceItem);
        }

        private void MenuItemCheckedEventHandler(MenuItem sourceItem)
        {
            RefreshPriceMonitor();
            UncheckMenuItems(sourceItem.Parent.MenuItems);
            sourceItem.Checked = !sourceItem.Checked;
            _profileStore.SavePersistenceProfile(_tradePriceMonitor);
        }

        private void LoadProfileEventHandler(string profileName)
        {
            var newTradePriceMonitor = _profileStore.LoadProfile(profileName);
            _tradePriceMonitor.TrasferSubscription(newTradePriceMonitor);
            _tradePriceMonitor.Dispose();
            _tradePriceMonitor = newTradePriceMonitor;
            InitMenuOptions();
            _tradePriceMonitor.StartMonitoring();
        }

        private void RemoveProfileEventHandler(string profileName)
        {
            _profileStore.RemoveProfile(profileName);
            RefreshLoadProfileMenuItems();
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

        public void Update(TradePrice price)
        {
            var foundMenuItems  = Menu.MenuItems.Find("BitcoinPrice", false);
            if (foundMenuItems.Length > 0)
            {
                foundMenuItems[0].Text = $"{price.Price} {price.Currency}";
            }
        }

        private void RefreshPriceMonitor()
        {
            _tradePriceMonitor.StopMonitoring();
            _tradePriceMonitor.StartMonitoring();
        }
    }
}
