﻿using BitcoinPriceMonitor.ContextMenu.Sections;
using BitcoinPriceMonitor.Observer;
using BitcoinPriceMonitor.Profile;

namespace BitcoinPriceMonitorTests
{
    using System.Windows.Forms;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using BitcoinPriceMonitor.ContextMenu;
    using BitcoinPriceMonitor.PriceMonitor;

    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class TradePriceMonitorContextMenuTests
    {
        [TestMethod]
        public void TradePriceMonitorContextMenu_ConstructorSubscribesMenuWithProfileLoaders()
        {
            // Arrange
            var mockTradePriceMonitor = new Mock<ITradePriceMonitor>();
            var mockMenuSections = new Mock<ITradePriceMenuSections>();
            var menuItem = new MenuItem();
            var mockLoadProfileMenuSection = new Mock<ITradePriceMonitorContextMenuSection>();
            var mockDatasourceMenuSection = new Mock<ITradePriceMonitorContextMenuSection>();
            var mockDatasourceMenuSectionProfileLoader = mockDatasourceMenuSection.As<IProfileLoader>();
            var mockLoadProfileMenuSectionProfileLoader = mockLoadProfileMenuSection.As<IProfileLoader>();
            var mockEveryOtherMenuSection = new Mock<ITradePriceMonitorContextMenuSection>();
            mockLoadProfileMenuSection.Setup(m => m.GetMenuItem()).Returns(menuItem);
            mockDatasourceMenuSection.Setup(m => m.GetMenuItem()).Returns(menuItem);
            mockEveryOtherMenuSection.Setup(m => m.GetMenuItem()).Returns(menuItem);
            mockMenuSections.Setup(m => m.LoadProfileSection).Returns(mockLoadProfileMenuSection.Object);
            mockMenuSections.Setup(m => m.DatasourceSection).Returns(mockDatasourceMenuSection.Object);
            mockMenuSections.Setup(m => m.SaveProfileSection).Returns(mockEveryOtherMenuSection.Object);
            mockMenuSections.Setup(m => m.CurrencySection).Returns(mockEveryOtherMenuSection.Object);
            mockMenuSections.Setup(m => m.FrequencySection).Returns(mockEveryOtherMenuSection.Object);
            mockMenuSections.Setup(m => m.PriceTypeSection).Returns(mockEveryOtherMenuSection.Object);

            // Act
            var target = new TradePriceMonitorContextMenu(mockTradePriceMonitor.Object, mockMenuSections.Object);

            // Assert
            mockDatasourceMenuSectionProfileLoader.Verify(m => m.Subscribe(target), Times.Once);
            mockLoadProfileMenuSectionProfileLoader.Verify(m => m.Subscribe(target), Times.Once);
        }

        [TestMethod]
        public void TradePriceMonitorContextMenu_MenuSanityCheck()
        {
            // Arrange
            var mockTradePriceMonitor = new Mock<ITradePriceMonitor>();
            var mockMenuSections = new Mock<ITradePriceMenuSections>();

            // Act
            var target = new TradePriceMonitorContextMenu(mockTradePriceMonitor.Object, mockMenuSections.Object);
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
            var mockMenuSections = new Mock<ITradePriceMenuSections>();
            var target = new TradePriceMonitorContextMenu(mockTradePriceMonitor.Object, mockMenuSections.Object);

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
            var mockMenuSections = new Mock<ITradePriceMenuSections>();
            var target = new TradePriceMonitorContextMenu(mockTradePriceMonitor.Object, mockMenuSections.Object);
            var originalPriceItemText = target.Menu.MenuItems.Find("BitcoinPrice", false)[0].Text;
            var updatePrice = 2.5;

            // Act
            target.Update(new TradePrice(updatePrice, Currency.GBP));
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
            var mockMenuSections = new Mock<ITradePriceMenuSections>();
            var target = new TradePriceMonitorContextMenu(mockTradePriceMonitor.Object, mockMenuSections.Object);
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
            var mockMenuSections = new Mock<ITradePriceMenuSections>();
            var target = new TradePriceMonitorContextMenu(mockTradePriceMonitor.Object, mockMenuSections.Object);
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
