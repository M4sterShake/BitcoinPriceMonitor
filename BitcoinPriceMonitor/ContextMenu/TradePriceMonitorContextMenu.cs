namespace BitcoinPriceMonitor.ContextMenu
{
    using System;
    using System.Windows.Forms;
    using PriceMonitor;
    using Profile;
    using Observer;

    public class TradePriceMonitorContextMenu : ITradePriceMonitorContextMenu, ILoadProfileListener
    {
        public ContextMenu Menu { get; }

        private const string BitcoinPriceMenuItemName = "BitcoinPrice";

        private ITradePriceMonitor _tradePriceMonitor;
        private readonly IProfileStore _profileStore;
        private readonly ITradePriceMonitorFactory _monitorFactory;

        private readonly DatasourceContextMenuSection _datasourceMenuItem;
        private readonly TradePriceTypeContextMenuSection _tradePriceTypeMenuItem;
        private readonly CurrencyContextMenuItemSection _currencyMenuItem;
        private readonly FrequencyContextMenuSection _frequencyMenuItem;
        private readonly LoadProfileContextMenuSection _loadProfileMenuItem;
        private readonly SaveProfileContextMenuSection _saveProfileMenuItem;

        public TradePriceMonitorContextMenu(ITradePriceMonitor tradePriceMonitor, IProfileStore profileStore, ITradePriceMonitorFactory monitorFactory)
        {
            _tradePriceMonitor = tradePriceMonitor;
            _profileStore = profileStore;
            _monitorFactory = monitorFactory;

            _datasourceMenuItem = new DatasourceContextMenuSection(_tradePriceMonitor, _profileStore, _monitorFactory);
            _tradePriceTypeMenuItem = new TradePriceTypeContextMenuSection(_tradePriceMonitor, _profileStore);
            _currencyMenuItem = new CurrencyContextMenuItemSection(_tradePriceMonitor, _profileStore);
            _frequencyMenuItem = new FrequencyContextMenuSection(_tradePriceMonitor, _profileStore);
            _loadProfileMenuItem = new LoadProfileContextMenuSection(_tradePriceMonitor, _profileStore, this);
            _saveProfileMenuItem = new SaveProfileContextMenuSection(_tradePriceMonitor, _profileStore);

            Menu = GetMenu();
            InitMenuOptions();
        }

        public Guid ObserverId { get; } = Guid.NewGuid();

        public void Update(TradePrice price)
        {
            var foundMenuItems = Menu.MenuItems.Find("BitcoinPrice", false);
            if (foundMenuItems.Length > 0)
            {
                foundMenuItems[0].Text = $"{price.Price} {price.Currency}";
            }
        }

        public void ProfileLoaded(ITradePriceMonitor loadedPriceMonitor)
        {
            _tradePriceMonitor = loadedPriceMonitor;
            _datasourceMenuItem.UpdateTradePriceMonitor(_tradePriceMonitor);
            _tradePriceTypeMenuItem.UpdateTradePriceMonitor(_tradePriceMonitor);
            _currencyMenuItem.UpdateTradePriceMonitor(_tradePriceMonitor);
            _frequencyMenuItem.UpdateTradePriceMonitor(_tradePriceMonitor);
            InitMenuOptions();
        }

        private ContextMenu GetMenu()
        {
            var contextMenu = new ContextMenu();

            contextMenu.MenuItems.Add(new MenuItem("Getting bitcoin price...")
            {
                Name = BitcoinPriceMenuItemName
            });
            contextMenu.MenuItems.Add("-");
            contextMenu.MenuItems.Add(_datasourceMenuItem.GetMenuItem());
            contextMenu.MenuItems.Add(_tradePriceTypeMenuItem.GetMenuItem());
            contextMenu.MenuItems.Add(_currencyMenuItem.GetMenuItem());
            contextMenu.MenuItems.Add(_frequencyMenuItem.GetMenuItem());
            contextMenu.MenuItems.Add("-");
            contextMenu.MenuItems.Add(_loadProfileMenuItem.GetMenuItem());
            contextMenu.MenuItems.Add(_saveProfileMenuItem.GetMenuItem());
            contextMenu.MenuItems.Add("-");
            contextMenu.MenuItems.Add("Exit", ExitEventHandler);

            return contextMenu;
        }

        private void InitMenuOptions()
        {
            _datasourceMenuItem.InitMenuItem();
            _currencyMenuItem.InitMenuItem();
            _tradePriceTypeMenuItem.InitMenuItem();
            _frequencyMenuItem.InitMenuItem();
        }

        #region Event Handlers
        private void ExitEventHandler(object sender, EventArgs e)
        {
            Application.Exit();
        }
        #endregion
    }
}