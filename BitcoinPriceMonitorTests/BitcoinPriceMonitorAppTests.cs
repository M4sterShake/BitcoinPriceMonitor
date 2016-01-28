namespace BitcoinPriceMonitorTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using BitcoinPriceMonitor.ApplicationEntryPoint;
    using BitcoinPriceMonitor.ContextMenu;
    using BitcoinPriceMonitor.NotifyIcon;
    using BitcoinPriceMonitor.PriceMonitor;

    [TestClass]
    public class BitcoinPriceMonitorAppTests
    {
        [TestMethod]
        public void BitcoinPriceMonitorApp_Start_RegistersObservers()
        {
            // Arrange
            var mockTradePriceMonitorContextMenu = new Mock<ITradePriceMonitorContextMenu>();
            var mockNotificationIcon = new Mock<INotificationTrayIcon>();
            var tradePriceMonitorThatMustHaveSubscribeCalled = new Mock<ITradePriceMonitor>();

            var target = new BitcoinPriceMonitorApp(tradePriceMonitorThatMustHaveSubscribeCalled.Object,
                mockTradePriceMonitorContextMenu.Object,
                mockNotificationIcon.Object);

            // Act
            target.Start();

            // Assert
            tradePriceMonitorThatMustHaveSubscribeCalled.Verify(
                m => m.Subscribe(mockTradePriceMonitorContextMenu.Object), Times.Once);
            tradePriceMonitorThatMustHaveSubscribeCalled.Verify(
                m => m.Subscribe(mockNotificationIcon.Object), Times.Once);
        }

        [TestMethod]
        public void BitcoinPriceMonitorApp_Start_StartsMonitor()
        {
            // Arrange
            var mockTradePriceMonitorContextMenu = new Mock<ITradePriceMonitorContextMenu>();
            var mockNotificationIcon = new Mock<INotificationTrayIcon>();
            var mockTradePriceMonitorThatMustHaveStartMonitoringCalled = new Mock<ITradePriceMonitor>();

            var target = new BitcoinPriceMonitorApp(mockTradePriceMonitorThatMustHaveStartMonitoringCalled.Object,
                mockTradePriceMonitorContextMenu.Object,
                mockNotificationIcon.Object);

            // Act
            target.Start();

            // Assert
            mockTradePriceMonitorThatMustHaveStartMonitoringCalled.Verify(m => m.StartMonitoring(), Times.Once);
        }
    }
}
