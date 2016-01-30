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
        private const string DatasourceMenuItemName = "DatasourceMenuItemName";
        private const string CurrencyMenuItemName = "Currecy";
        private const string PriceTypeMenuItemName = "PriceType";
        private const string FrequencyMenuItemName = "Frequency";
        private const string LoadProfileMenuItemName = "LoadProfiles";

        private ITradePriceMonitor _tradePriceMonitor;
        private readonly IProfileStore _profileStore;
        private readonly ITradePriceMonitorFactory _monitorFactory;
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
        private readonly Dictionary<string, Type> _datasources = new Dictionary<string, Type>()
        {
            { "Bitcoin Average", typeof(BitcoinAveragePriceMonitor) },
            { "Coinbase", typeof(CoinbasePriceMonitor) }
        }; 

        public TradePriceMonitorContextMenu(ITradePriceMonitor tradePriceMonitor, IProfileStore profileStore, ITradePriceMonitorFactory monitorFactory)
        {
            _tradePriceMonitor = tradePriceMonitor;
            _profileStore = profileStore;
            _monitorFactory = monitorFactory;
            Menu = GetMenu();
            InitMenuOptions();
        }

        public Guid ObserverId { get; } = Guid.NewGuid();

        private ContextMenu GetMenu()
        {
            var contextMenu = new ContextMenu();

            var datasourceMenuItem = GetDatasourceMenuItem();
            var priceTypeMenuItem = GetTradePriceTypeMenuItem();
            var currencyMenuItem = GetCurrencyMenuItem();
            var frequencyMenuItem = GetFrequencyMenuItem();
            var loadProfileMenuItem = GetLoadProfileMenuItem();
            var saveProfileMenuItem = new MenuItem("Save Settings...", (sender, e) => SaveProfileEventHandler());

            contextMenu.MenuItems.Add(new MenuItem("Getting bitcoin price...")
            {
                Name = BitcoinPriceMenuItemName
            });
            contextMenu.MenuItems.Add("-");
            contextMenu.MenuItems.Add(datasourceMenuItem);
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

        private MenuItem GetDatasourceMenuItem()
        {
            var subMenuItems =
                _datasources.Select(
                    datasource =>
                        new MenuItem(datasource.Key,
                            (sender, e) => DatasourceEventHandler(datasource.Value.Name, (MenuItem) sender))
                        {
                            RadioCheck = true
                        }).ToArray();

            return CreateMenuItem("Datasource", DatasourceMenuItemName, subMenuItems);
        }

        private MenuItem GetTradePriceTypeMenuItem()
        {
            var subMenuItems =
                Enum.GetValues(typeof (TradePriceType))
                    .Cast<TradePriceType>()
                    .Select(
                        tradePriceType =>
                            new MenuItem(tradePriceType.ToString(),
                                (sender, e) => TradePriceTypeEventHandler(tradePriceType, (MenuItem) sender))
                            {
                                RadioCheck = true
                            }).ToArray();

            return CreateMenuItem("Trade Price Type", PriceTypeMenuItemName, subMenuItems);
        }

        private MenuItem GetCurrencyMenuItem()
        {
            var subMenuItems =
                Enum.GetValues(typeof (Currency))
                    .Cast<Currency>()
                    .Select(
                        currency =>
                            new MenuItem(currency.ToString(),
                                (sender, e) => CurrencyEventHandler(currency, (MenuItem) sender))
                            {
                                RadioCheck = true
                            }).ToArray();

            return CreateMenuItem("Currency", CurrencyMenuItemName, subMenuItems);
        }

        private MenuItem GetFrequencyMenuItem()
        {
            var subMenuItems =
                _availableFrequencies.Select(
                    frequency =>
                        new MenuItem(frequency.Key,
                            (sender, e) => FrequencyEventHander(frequency.Value, (MenuItem) sender))
                        {
                            RadioCheck = true
                        }).ToArray();

            return CreateMenuItem("Price Check Frequency", FrequencyMenuItemName, subMenuItems);
        }

        private MenuItem GetLoadProfileMenuItem()
        {
            var subMenuItems =  _profileStore.Profiles?.Select(p =>
            {
                var profileMenuItem = new MenuItem(p);
                profileMenuItem.MenuItems.Add(new MenuItem("Load", (sender, e) => LoadProfileEventHandler(p)));
                profileMenuItem.MenuItems.Add(new MenuItem("Remove", (sender, e) => RemoveProfileEventHandler(p)));
                return profileMenuItem;
            }).ToArray();

            return CreateMenuItem("Load Settings", LoadProfileMenuItemName, subMenuItems);
        }

        private MenuItem CreateMenuItem(string text, string name, MenuItem[] subMenuItems)
        {
            var menuItem = new MenuItem(text)
            {
                Name = name
            };

            menuItem.MenuItems.AddRange(subMenuItems);

            return menuItem;
        }

        private void InitMenuOptions()
        {
            InitDatasourceMenuItems();
            InitCurrencyMenuItems();
            InitPriceTypeMenuItems();
            InitFrequencyMenuItems();
        }

        private void InitDatasourceMenuItems()
        {
            var datasourceMenuItem = Menu.MenuItems.Find(DatasourceMenuItemName, true)[0];
            foreach (MenuItem item in datasourceMenuItem.MenuItems)
            {
                item.Checked = item.Text ==
                               _datasources.Where(d => d.Value == _tradePriceMonitor.GetType())
                                   .Select(d => d.Key)
                                   .FirstOrDefault();
            }
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
            MenuItem[] profileMenuItems = new MenuItem[GetLoadProfileMenuItem().MenuItems.Count];
            GetLoadProfileMenuItem().MenuItems.CopyTo(profileMenuItems, 0);
            loadProfileMenuItem[0]?.MenuItems.AddRange(profileMenuItems);
        }

        private void UncheckMenuItems(IEnumerable menuItems)
        {
            foreach (MenuItem m in menuItems)
            {
                m.Checked = false;
            }
        } 

        #region Event Handlers

        private void DatasourceEventHandler(string datasourceName, MenuItem sourceItem)
        {
            var newTradePriceMonitor = _monitorFactory.Get(new Profile()
            {
                MonitorType = datasourceName,
                Frequency = _tradePriceMonitor.Frequency,
                TargetCurrency = _tradePriceMonitor.TargetCurrency,
                PriceType =  _tradePriceMonitor.PriceType
            });
            _tradePriceMonitor.TrasferSubscription(newTradePriceMonitor);
            _tradePriceMonitor.Dispose();
            _tradePriceMonitor = newTradePriceMonitor;
            _tradePriceMonitor.StartMonitoring();
            MenuItemCheckedEventHandler(sourceItem);
        }

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
