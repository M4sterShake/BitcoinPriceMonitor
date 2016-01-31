using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BitcoinPriceMonitor.ContextMenu.Sections;
using BitcoinPriceMonitor.PriceMonitor;
using BitcoinPriceMonitor.Profile;

namespace BitcoinPriceMonitor.ContextMenu
{
    class LoadProfileContextMenuSection : ProfileMenuItemSection, IProfileLoader
    {
        private const string LoadProfileMenuItemName = "LoadProfiles";
        private List<ILoadProfileListener> _loadProfileListeners = new List<ILoadProfileListener>();

        public LoadProfileContextMenuSection(ITradePriceMonitor tradePriceMonitor, IProfileStore profileStore) : base(tradePriceMonitor, profileStore)
        {
        }

        public override MenuItem GetMenuItem()
        {
            var subMenuItems = ProfileStore.Profiles?.Select(p =>
            {
                var profileMenuItem = new MenuItem(p);
                profileMenuItem.MenuItems.Add(new MenuItem("Load", (sender, e) => LoadProfileEventHandler(p)));
                profileMenuItem.MenuItems.Add(new MenuItem("Remove", (sender, e) => RemoveProfileEventHandler(p)));
                return profileMenuItem;
            }).ToArray();

            ContextMenuItem = CreateMenuItem("Load Settings", LoadProfileMenuItemName, subMenuItems);

            return ContextMenuItem;
        }

        public override void InitMenuItem()
        {
            throw new NotImplementedException();
        }

        private void LoadProfileEventHandler(string profileName)
        {
            var newTradePriceMonitor = ProfileStore.LoadProfile(profileName);
            TradePriceMonitor.TrasferSubscription(newTradePriceMonitor);
            TradePriceMonitor.Dispose();
            TradePriceMonitor = newTradePriceMonitor;
            Notify(newTradePriceMonitor);
            TradePriceMonitor.StartMonitoring();
        }

        private void RemoveProfileEventHandler(string profileName)
        {
            ProfileStore.RemoveProfile(profileName);
            RefreshLoadProfileMenuItems();
        }

        private void Notify(ITradePriceMonitor newPriceMonitor)
        {
            foreach (var listener in _loadProfileListeners)
            {
                listener.ProfileLoaded(newPriceMonitor);
            }
        }

        public void Subscribe(ILoadProfileListener listener)
        {
            _loadProfileListeners.Add(listener);
        }
    }
}
