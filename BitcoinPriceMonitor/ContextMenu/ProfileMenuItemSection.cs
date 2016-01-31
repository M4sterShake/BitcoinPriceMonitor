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
    public abstract class ProfileMenuItemSection : TradePriceMonitorContextMenuSection
    {
        public ProfileMenuItemSection(ITradePriceMonitor tradePriceMonitor, IProfileStore profileStore)
            : base(tradePriceMonitor, profileStore)
        {    
        }

        protected void RefreshLoadProfileMenuItems()
        {
            ContextMenuItem.MenuItems.Clear();
            var refreshedLoadProfileMenuItem = GetMenuItem();
            MenuItem[] profileMenuItems = new MenuItem[refreshedLoadProfileMenuItem.MenuItems.Count];
            refreshedLoadProfileMenuItem.MenuItems.CopyTo(profileMenuItems, 0);
            ContextMenuItem.MenuItems.AddRange(profileMenuItems);
        }
    }
}
