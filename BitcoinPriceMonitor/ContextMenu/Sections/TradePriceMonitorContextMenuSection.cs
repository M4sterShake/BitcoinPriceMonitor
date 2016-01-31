namespace BitcoinPriceMonitor.ContextMenu.Sections
{
    using System.Collections;
    using System.Windows.Forms;
    using PriceMonitor;
    using Profile;

    public abstract class TradePriceMonitorContextMenuSection : ITradePriceMonitorContextMenuSection
    {
        protected ITradePriceMonitor TradePriceMonitor;
        protected IProfileStore ProfileStore;
        protected MenuItem ContextMenuItem;

        public TradePriceMonitorContextMenuSection(ITradePriceMonitor tradePriceMonitor, IProfileStore profileStore)
        {
            TradePriceMonitor = tradePriceMonitor;
            ProfileStore = profileStore;
        }

        public abstract MenuItem GetMenuItem();

        protected MenuItem CreateMenuItem(string text, string name, MenuItem[] subMenuItems)
        {
            var menuItem = new MenuItem(text)
            {
                Name = name
            };

            menuItem.MenuItems.AddRange(subMenuItems);

            return menuItem;
        }

        protected void MenuItemCheckedEventHandler(MenuItem sourceItem)
        {
            RefreshPriceMonitor();
            UncheckMenuItems(sourceItem.Parent.MenuItems);
            sourceItem.Checked = !sourceItem.Checked;
            ProfileStore.SavePersistenceProfile(TradePriceMonitor);
        }

        private void RefreshPriceMonitor()
        {
            TradePriceMonitor.StopMonitoring();
            TradePriceMonitor.StartMonitoring();
        }

        private void UncheckMenuItems(IEnumerable menuItems)
        {
            foreach (MenuItem m in menuItems)
            {
                m.Checked = false;
            }
        }

        public abstract void InitMenuItem();
        public void UpdateTradePriceMonitor(ITradePriceMonitor updatedTradePriceMonitor)
        {
            TradePriceMonitor = updatedTradePriceMonitor;
            InitMenuItem();
        }
    }
}
