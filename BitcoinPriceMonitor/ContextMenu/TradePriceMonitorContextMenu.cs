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
        private readonly ITradePriceMenuSections _menuSections;

        public TradePriceMonitorContextMenu(ITradePriceMonitor tradePriceMonitor, ITradePriceMenuSections menuSections)
        {
            _tradePriceMonitor = tradePriceMonitor;
            _menuSections = menuSections;
            (_menuSections.LoadProfileSection as IProfileLoader)?.Subscribe(this);
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
            _menuSections.DatasourceSection.UpdateTradePriceMonitor(_tradePriceMonitor);
            _menuSections.PriceTypeSection.UpdateTradePriceMonitor(_tradePriceMonitor);
            _menuSections.CurrencySection.UpdateTradePriceMonitor(_tradePriceMonitor);
            _menuSections.FrequencySection.UpdateTradePriceMonitor(_tradePriceMonitor);
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
            contextMenu.MenuItems.Add(_menuSections.DatasourceSection.GetMenuItem());
            contextMenu.MenuItems.Add(_menuSections.PriceTypeSection.GetMenuItem());
            contextMenu.MenuItems.Add(_menuSections.CurrencySection.GetMenuItem());
            contextMenu.MenuItems.Add(_menuSections.FrequencySection.GetMenuItem());
            contextMenu.MenuItems.Add("-");
            contextMenu.MenuItems.Add(_menuSections.LoadProfileSection.GetMenuItem());
            contextMenu.MenuItems.Add(_menuSections.SaveProfileSection.GetMenuItem());
            contextMenu.MenuItems.Add("-");
            contextMenu.MenuItems.Add("Exit", ExitEventHandler);

            return contextMenu;
        }

        private void InitMenuOptions()
        {
            _menuSections.DatasourceSection.InitMenuItem();
            _menuSections.PriceTypeSection.InitMenuItem();
            _menuSections.CurrencySection.InitMenuItem();
            _menuSections.FrequencySection.InitMenuItem();
        }

        #region Event Handlers
        private void ExitEventHandler(object sender, EventArgs e)
        {
            Application.Exit();
        }
        #endregion
    }
}