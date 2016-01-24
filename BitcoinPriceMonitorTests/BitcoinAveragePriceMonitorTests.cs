using System;
using System.Text;
using System.Collections.Generic;
using System.Threading;
using BitcoinPriceMonitor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RestSharp;

namespace BitcoinPriceMonitorTests
{
    [TestClass]
    public class BitcoinAveragePriceMonitorTests
    {

        [TestMethod]
        public void BitcoinAveragePriceMonitor_ConstructorTest()
        {
            // Arrange/Act
            var target = new BitcoinAveragePriceMonitor(new RestClient());

            // Assert
            Assert.AreEqual(Currency.USD, target.ConvertToCurrency);
            Assert.AreEqual(2000, target.Frequency);
            Assert.AreEqual(TradePriceType.Last, target.PriceType);
        }

        [TestMethod]
        public void BitcoinAveragePriceMonitor_StartMonitoringTest()
        {
            // Arrange
            var expectedResult = 2.5; ;
            var expectedResultString = expectedResult.ToString();

            var mockResponseWithExpectedResult = new Mock<IRestResponse>();
            mockResponseWithExpectedResult.SetupGet(m => m.Content).Returns(expectedResultString);
            var mockRestClientThatReturnsMockResponse = new Mock<IRestClient>();
            mockRestClientThatReturnsMockResponse.Setup(m => m.Execute(It.IsAny<IRestRequest>()))
                .Returns(mockResponseWithExpectedResult.Object);
            var target = new BitcoinAveragePriceMonitor(mockRestClientThatReturnsMockResponse.Object);
            target.Frequency = 100;

            // Act
            Thread thread = new Thread(delegate ()
            {
                target.StartMonitoring();
            });
            thread.Start();
            Thread.Sleep(110);

            // Assert
            Assert.AreEqual(expectedResult, target.CurrentPrice);
        }

        [TestMethod]
        public void BitcoinAveragePriceMonitor_StopMonitoringTest()
        {
            // Arrange
            var expectedResult = 2.5;
            var mockResponseWithExpectedResult = new Mock<IRestResponse>();
            mockResponseWithExpectedResult.SetupGet(m => m.Content)
                .Returns((expectedResult).ToString());
            var mockRestClientThatReturnsMockResponse = new Mock<IRestClient>();
            mockRestClientThatReturnsMockResponse.Setup(m => m.Execute(It.IsAny<IRestRequest>()))
                .Returns(mockResponseWithExpectedResult.Object);
            var target = new BitcoinAveragePriceMonitor(mockRestClientThatReturnsMockResponse.Object);
            target.Frequency = 100;

            // Act
            Thread thread = new Thread(delegate ()
            {
                target.StartMonitoring();
            });
            thread.Start();
            Thread.Sleep(110);
            target.StopMonitoring();
            Thread.Sleep(110);

            // Assert
            Assert.AreEqual(expectedResult, target.CurrentPrice);
            mockRestClientThatReturnsMockResponse.Verify(m => m.Execute(It.IsAny<IRestRequest>()), Times.Exactly(2));
        }

        [TestMethod]
        public void BitcoinAveragePriceMonitor_ObserversUpdatedTest()
        {
            // Arrange
            var expectedResult = 2.5; ;
            var expectedResultString = expectedResult.ToString();

            var mockResponseWithExpectedResult = new Mock<IRestResponse>();
            mockResponseWithExpectedResult.SetupGet(m => m.Content).Returns(expectedResultString);
            var mockRestClientThatReturnsMockResponse = new Mock<IRestClient>();
            mockRestClientThatReturnsMockResponse.Setup(m => m.Execute(It.IsAny<IRestRequest>()))
                .Returns(mockResponseWithExpectedResult.Object);
            var mockObserver = new Mock<ITradePriceObserver>();
            var target = new BitcoinAveragePriceMonitor(mockRestClientThatReturnsMockResponse.Object);
            target.Subscribe(mockObserver.Object);
            target.Frequency = 100;

            // Act
            Thread thread = new Thread(delegate ()
            {
                target.StartMonitoring();
            });
            thread.Start();
            Thread.Sleep(120);
            target.StopMonitoring();

            // Assert
            mockObserver.Verify(m => m.Update(expectedResult), Times.Exactly(2));
        }

        [TestMethod]
        public void BitcoinAveragePriceMonitor_UnsubscribedObserversNotNotifiedTest()
        {
            // Arrange
            var expectedResult = 2.5; ;
            var expectedResultString = expectedResult.ToString();
            var observerGuid = Guid.NewGuid();

            var mockResponseWithExpectedResult = new Mock<IRestResponse>();
            mockResponseWithExpectedResult.SetupGet(m => m.Content).Returns(expectedResultString);
            var mockRestClientThatReturnsMockResponse = new Mock<IRestClient>();
            mockRestClientThatReturnsMockResponse.Setup(m => m.Execute(It.IsAny<IRestRequest>()))
                .Returns(mockResponseWithExpectedResult.Object);
            var mockObserver = new Mock<ITradePriceObserver>();
            mockObserver.SetupGet(m => m.ObserverId).Returns(observerGuid);
            var target = new BitcoinAveragePriceMonitor(mockRestClientThatReturnsMockResponse.Object);
            target.Subscribe(mockObserver.Object);
            target.Frequency = 100;

            // Act
            Thread thread = new Thread(delegate ()
            {
                target.StartMonitoring();
            });
            thread.Start();
            Thread.Sleep(110);
            target.Unsubscribe(mockObserver.Object);
            Thread.Sleep(110);
            target.StopMonitoring();

            // Assert
            mockObserver.Verify(m => m.Update(expectedResult), Times.Exactly(2));
        }

        [TestMethod]
        public void BitcoinAveragePriceMonitor_ConvertToCurrencyTest()
        {
            // Arrange
            var target = new BitcoinAveragePriceMonitor(new RestClient());
            var originalCurrency = target.ConvertToCurrency;
            var expectedCurrency = Currency.GBP;
            
            // Act
            target.ConvertToCurrency = expectedCurrency;

            // Assert
            Assert.AreEqual(Currency.USD, originalCurrency);
            Assert.AreEqual(expectedCurrency, target.ConvertToCurrency);
        }

        [TestMethod]
        public void BitcoinAveragePriceMonitor_PriceTypeTest()
        {
            // Arrange
            var target = new BitcoinAveragePriceMonitor(new RestClient());
            var originalPriceType = target.PriceType;
            var expectedPriceType = TradePriceType.Bid;

            // Act
            target.PriceType = expectedPriceType;

            // Assert
            Assert.AreEqual(TradePriceType.Last, originalPriceType);
            Assert.AreEqual(expectedPriceType, target.PriceType);
        }

        [TestMethod]
        public void BitcoinAveragePriceMonitor_FrequencyTest()
        {
            // Arrange
            var target = new BitcoinAveragePriceMonitor(new RestClient());
            var originalFrequency = target.Frequency;
            var expectedFrequency = 55000;

            // Act
            target.Frequency = expectedFrequency;

            // Assert
            Assert.AreEqual(2000, originalFrequency);
            Assert.AreEqual(expectedFrequency, target.Frequency);
        }
    }
}
