using BitcoinPriceMonitor.Observer;
using BitcoinPriceMonitor.PriceMonitor;

namespace BitcoinPriceMonitor.ApplicationEntryPoint
{
    using System;
    using ContextMenu;
    using NotifyIcon;

    public class BitcoinPriceMonitorApp : IBitcoinPriceMonitorApp
    {
        private readonly ITradePriceMonitor _priceMonitor;
        private readonly ITradePriceMonitorContextMenu _contextMenu;
        private readonly INotificationTrayIcon _notifyIcon;

        public BitcoinPriceMonitorApp(ITradePriceMonitor priceMonitor, 
            ITradePriceMonitorContextMenu contextMenu,
            INotificationTrayIcon notifyIcon)
        {
            _priceMonitor = priceMonitor;
            _contextMenu = contextMenu;
            _notifyIcon = notifyIcon;
            AppDomain.CurrentDomain.ProcessExit += (sender, e) => _notifyIcon.Close();
        }

        public void Start()
        {
            var observablePriceMonitor = _priceMonitor as ITradePriceObservable;
            var observerContextMenu = _contextMenu as ITradePriceObserver;
            var observerIcon = _notifyIcon as ITradePriceObserver;
            observablePriceMonitor.Subscribe(observerContextMenu);
            observablePriceMonitor.Subscribe(observerIcon);
            _priceMonitor.StartMonitoring();
        }
    }
}
