using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitcoinPriceMonitor
{
    class BitcoinPriceMonitorApp : IBitcoinPriceMonitorApp
    {
        private ITradePriceMonitor _priceMonitor;
        private ITradePriceMonitorContextMenu _contextMenu;
        private INotificationTrayIcon _notifyIcon;

        public BitcoinPriceMonitorApp(ITradePriceMonitor priceMonitor, 
            ITradePriceMonitorContextMenu contextMenu,
            INotificationTrayIcon notifyIcon)
        {
            _priceMonitor = priceMonitor;
            _contextMenu = contextMenu;
            _notifyIcon = notifyIcon;
            AppDomain.CurrentDomain.ProcessExit += (object sender, EventArgs e) => _notifyIcon.Close();
        }

        public void Start()
        {
            var observablePriceMonitor = _priceMonitor as IObservable<double>;
            var observerContextMenu = _contextMenu as IObserver<double>;
            var observerIcon = _notifyIcon as IObserver<double>;
            observablePriceMonitor.Subscribe(observerContextMenu);
            observablePriceMonitor.Subscribe(observerIcon);
            _priceMonitor.StartMonitoring();
        }
    }
}
