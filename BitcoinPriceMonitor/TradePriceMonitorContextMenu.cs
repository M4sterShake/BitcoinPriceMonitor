using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BitcoinPriceMonitor
{
    public class TradePriceMonitorContextMenu : ITradePriceMonitorContextMenu
    {
        private ITradePriceMonitor _tradePriceMonitor;

        public TradePriceMonitorContextMenu(ITradePriceMonitor tradePriceMonitor)
        {
            _tradePriceMonitor = tradePriceMonitor;
        }

        public ContextMenu GetMenu()
        {
            var contextMenu = new ContextMenu();
            var settingsMenuItem = new MenuItem("Settings");

            var priceTypeMenuItem = new MenuItem("Trade Price Type");
            foreach (var tradePriceType in Enum.GetValues(typeof(TradePriceType)).Cast<TradePriceType>())
            {
                priceTypeMenuItem.MenuItems.Add(new MenuItem(tradePriceType.ToString(),
                    (sender, e) => TradePriceTypeEventHandler(tradePriceType)));
            }

            var currencyMenuItem = new MenuItem("Currency");
            foreach (var currency in Enum.GetValues(typeof(Currency)).Cast<Currency>())
            {
                currencyMenuItem.MenuItems.Add(new MenuItem(currency.ToString(),
                    (sender, e) => CurrencyEventHandler(currency)));
            }

            var frequencyMenuItem = new MenuItem("Price Check Frequency");


            settingsMenuItem.MenuItems.Add(priceTypeMenuItem);
            settingsMenuItem.MenuItems.Add(currencyMenuItem);

            contextMenu.MenuItems.Add(settingsMenuItem);
            contextMenu.MenuItems.Add("Exit", ExitEventHandler);

            return contextMenu;
        }

        #region Event Handlers
        private void TradePriceTypeEventHandler(TradePriceType tradePriceType)
        {
            MessageBox.Show(tradePriceType.ToString());
        }

        private void CurrencyEventHandler(Currency currency)
        {
            MessageBox.Show(currency.ToString());
        }

        private void ExitEventHandler(object sender, EventArgs e)
        {
            Application.Exit();
        }
        #endregion
    }
}
