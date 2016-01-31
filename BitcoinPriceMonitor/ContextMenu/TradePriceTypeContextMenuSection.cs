using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BitcoinPriceMonitor.PriceMonitor;
using BitcoinPriceMonitor.Profile;

namespace BitcoinPriceMonitor.ContextMenu
{
    class TradePriceTypeContextMenuSection : TradePriceMonitorContextMenuSection
    {
        private const string PriceTypeMenuItemName = "PriceType";

        public TradePriceTypeContextMenuSection(ITradePriceMonitor tradePriceMonitor, IProfileStore profileStore)
            : base(tradePriceMonitor, profileStore)
        {
        }

        public override MenuItem GetMenuItem()
        {
            var subMenuItems =
                Enum.GetValues(typeof(TradePriceType))
                    .Cast<TradePriceType>()
                    .Select(
                        tradePriceType =>
                            new MenuItem(tradePriceType.ToString(),
                                (sender, e) => ClickEventHandler(tradePriceType, (MenuItem)sender))
                            {
                                RadioCheck = true
                            }).ToArray();
            ContextMenuItem = CreateMenuItem("Trade Price Type", PriceTypeMenuItemName, subMenuItems);
            InitMenuItem();

            return ContextMenuItem;
        }

        public override void InitMenuItem()
        {
            foreach (MenuItem item in ContextMenuItem.MenuItems)
            {
                item.Checked = item.Text == TradePriceMonitor.PriceType.ToString();
            }
        }

        private void ClickEventHandler(TradePriceType tradePriceType, MenuItem sourceItem)
        {
            TradePriceMonitor.PriceType = tradePriceType;
            MenuItemCheckedEventHandler(sourceItem);
        }
    }
}
