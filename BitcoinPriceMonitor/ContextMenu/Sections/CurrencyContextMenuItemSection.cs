namespace BitcoinPriceMonitor.ContextMenu.Sections
{
    using System;
    using System.Linq;
    using System.Windows.Forms;
    using PriceMonitor;
    using Profile;

    class CurrencyContextMenuItemSection : TradePriceMonitorContextMenuSection
    {
        private const string CurrencyMenuItemName = "Currecy";

        public CurrencyContextMenuItemSection(ITradePriceMonitor tradePriceMonitor, IProfileStore profileStore) 
            : base(tradePriceMonitor, profileStore)
        {
        }

        public override MenuItem GetMenuItem()
        {
            var subMenuItems =
                Enum.GetValues(typeof(Currency))
                    .Cast<Currency>()
                    .Select(
                        currency =>
                            new MenuItem(currency.ToString(),
                                (sender, e) => ClickEventHandler(currency, (MenuItem)sender))
                            {
                                RadioCheck = true
                            }).ToArray();
            ContextMenuItem = CreateMenuItem("Currency", CurrencyMenuItemName, subMenuItems);
            InitMenuItem();

            return ContextMenuItem;
        }

        public override void InitMenuItem()
        {
            foreach (MenuItem item in ContextMenuItem.MenuItems)
            {
                item.Checked = item.Text == TradePriceMonitor.TargetCurrency.ToString();
            }
        }

        private void ClickEventHandler(Currency currency, MenuItem sourceItem)
        {
            TradePriceMonitor.TargetCurrency = currency;
            MenuItemCheckedEventHandler(sourceItem);
        }
    }
}
