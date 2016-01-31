namespace BitcoinPriceMonitor.ContextMenu.Sections
{
    using System;
    using System.Windows.Forms;
    using PriceMonitor;
    using Profile;
    using Microsoft.VisualBasic;

    class SaveProfileContextMenuSection : ProfileMenuItemSection
    {
        public SaveProfileContextMenuSection(ITradePriceMonitor tradePriceMonitor, IProfileStore profileStore) : base(tradePriceMonitor, profileStore)
        {
        }

        public override MenuItem GetMenuItem()
        {
            ContextMenuItem = new MenuItem("Save Settings...", (sender, e) => ClickEventHandler());
            return ContextMenuItem;
        }

        public override void InitMenuItem()
        {
            throw new NotImplementedException();
        }

        private void ClickEventHandler()
        {
            string profileName = Interaction.InputBox("Please enter a name for the profile", "Save Profile", string.Empty);
            if (profileName != string.Empty)
            {
                ProfileStore.SaveProfile(TradePriceMonitor, profileName);
                RefreshLoadProfileMenuItems();
            }
        }
    }
}
