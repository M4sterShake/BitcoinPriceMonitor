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
    public class FrequencyContextMenuSection : TradePriceMonitorContextMenuSection
    {
        private const string FrequencyMenuItemName = "Frequency";
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

        public FrequencyContextMenuSection(ITradePriceMonitor tradePriceMonitor, IProfileStore profileStore)
            : base(tradePriceMonitor, profileStore)
        {
        }

        public override MenuItem GetMenuItem()
        {
            var subMenuItems =
                _availableFrequencies.Select(
                    frequency =>
                        new MenuItem(frequency.Key,
                            (sender, e) => ClickEventHander(frequency.Value, (MenuItem)sender))
                        {
                            RadioCheck = true
                        }).ToArray();
            ContextMenuItem = CreateMenuItem("Price Check Frequency", FrequencyMenuItemName, subMenuItems);
            InitMenuItem();
            return ContextMenuItem;
        }

        public override void InitMenuItem()
        {
            foreach (MenuItem item in ContextMenuItem.MenuItems)
            {
                item.Checked = item.Text ==
                               _availableFrequencies.Where(f => f.Value == TradePriceMonitor.Frequency)
                                   .Select(f => f.Key)
                                   .FirstOrDefault();
            }
        }

        private void ClickEventHander(int frequency, MenuItem sourceItem)
        {
            TradePriceMonitor.Frequency = frequency;
            MenuItemCheckedEventHandler(sourceItem);
        }
    }
}
