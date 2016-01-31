namespace BitcoinPriceMonitor.ContextMenu.Sections
{
    using System.Windows.Forms;
    using PriceMonitor;
    using Profile;

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
