namespace BitcoinPriceMonitorTests
{
    using System.Windows.Forms;
    using BitcoinPriceMonitor;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class TradePriceMonitorContextMenuTests
    {
        [TestMethod]
        public void TradePriceMonitorContextMenu_MenuSanityCheck()
        {
            // Arrange
            var mockTradePriceMonitor = new Mock<ITradePriceMonitor>();
            var mockProfileStore = new Mock<IProfileStore>();

            // Act
            var target = new TradePriceMonitorContextMenu(mockTradePriceMonitor.Object, mockProfileStore.Object);
            MenuItem currencyMenuItem = null;
            MenuItem exitMenuItem = null;
            foreach (MenuItem item in target.Menu.MenuItems)
            {
                if (item.Text == "Currency")
                {
                    currencyMenuItem = item;
                }
                if (item.Text == "Exit")
                {
                    exitMenuItem = item;
                }
            }

            // Assert
            Assert.IsTrue(target.Menu.MenuItems.Count > 1);
            Assert.IsNotNull(currencyMenuItem);
            Assert.IsNotNull(exitMenuItem);
        }

        [TestMethod]
        public void TradePriceMonitorContextMenu_ObserverIdTest()
        {
            // Arrange
            var mockTradePriceMonitor = new Mock<ITradePriceMonitor>();
            var mockProfileStore = new Mock<IProfileStore>();
            var target = new TradePriceMonitorContextMenu(mockTradePriceMonitor.Object, mockProfileStore.Object);

            //Act
            var observerId = target.ObserverId;

            //Assert
            Assert.IsNotNull(observerId);
            Assert.IsTrue(observerId.ToString().Length >= 32);
        }

        [TestMethod]
        public void TradePriceMonitorContextMenu_UpdateTest()
        {
            // Arrange
            var mockTradePriceMonitor = new Mock<ITradePriceMonitor>();
            var mockProfileStore = new Mock<IProfileStore>();
            var target = new TradePriceMonitorContextMenu(mockTradePriceMonitor.Object, mockProfileStore.Object);
            var originalPriceItemText = target.Menu.MenuItems.Find("BitcoinPrice", false)[0].Text;
            var updatePrice = 2.5;

            // Act
            target.Update(updatePrice);
            var priceItemTextAfterUpdate = target.Menu.MenuItems.Find("BitcoinPrice", false)[0].Text;

            // Assert
            Assert.IsFalse(originalPriceItemText.Contains(updatePrice.ToString()));
            Assert.IsTrue(priceItemTextAfterUpdate.Contains(updatePrice.ToString()));
        }

        [TestMethod]
        public void TradePriceMonitorContextMenu_CurrencyEventHandlerTest()
        {
            // Arrange
            var mockTradePriceMonitor = new Mock<ITradePriceMonitor>();
            var mockProfileStore = new Mock<IProfileStore>();
            var target = new TradePriceMonitorContextMenu(mockTradePriceMonitor.Object, mockProfileStore.Object);
            Menu.MenuItemCollection currencyMenuItems = null;
            MenuItem euroMenuItem = null;
            bool euroMenuItemOriginallyChecked = true;
            var currencyToSelect = Currency.EUR.ToString();
            foreach (MenuItem item in target.Menu.MenuItems)
            {
                if (item.Text == "Currency")
                {
                    currencyMenuItems = item.MenuItems;
                }
            }

            // Act
            foreach (MenuItem item in currencyMenuItems)
            {
                if (item.Text == currencyToSelect)
                {
                    euroMenuItemOriginallyChecked = item.Checked;
                    euroMenuItem = item;
                    item.PerformClick();
                }
            }

            // Assert
            Assert.IsFalse(euroMenuItemOriginallyChecked);
            Assert.IsTrue(euroMenuItem.Checked);
            foreach (MenuItem item in currencyMenuItems)
            {
                if (item.Text != currencyToSelect)
                {
                    Assert.IsFalse(item.Checked);
                }
            }
        }

        [TestMethod]
        public void TradePriceMonitorContextMenu_PriceTypeEventHandlerTest()
        {
            // Arrange
            var mockTradePriceMonitor = new Mock<ITradePriceMonitor>();
            var mockProfileStore = new Mock<IProfileStore>();
            var target = new TradePriceMonitorContextMenu(mockTradePriceMonitor.Object, mockProfileStore.Object);
            Menu.MenuItemCollection tradePriceTypeMenuItems = null;
            MenuItem bidMenuItem = null;
            bool bidMenuItemOriginallyChecked = true;
            var priceTypeToSelect = TradePriceType.Bid.ToString();
            foreach (MenuItem item in target.Menu.MenuItems)
            {
                if (item.Text == "Trade Price Type")
                {
                    tradePriceTypeMenuItems = item.MenuItems;
                }
            }

            // Act
            foreach (MenuItem item in tradePriceTypeMenuItems)
            {
                if (item.Text == priceTypeToSelect)
                {
                    bidMenuItemOriginallyChecked = item.Checked;
                    bidMenuItem = item;
                    item.PerformClick();
                }
            }

            // Assert
            Assert.IsFalse(bidMenuItemOriginallyChecked);
            Assert.IsTrue(bidMenuItem.Checked);
            foreach (MenuItem item in tradePriceTypeMenuItems)
            {
                if (item.Text != priceTypeToSelect)
                {
                    Assert.IsFalse(item.Checked);
                }
            }
        }
    }
}
