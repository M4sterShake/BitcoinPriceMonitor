using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace BitcoinPriceMonitor
{
    public abstract class TradePriceMonitor : ITradePriceMonitor
    {
        public Currency ConvertToCurrency { get; set; } = Currency.USD;
        public TradePriceType PriceType { get; set; }
        public double CurrentPrice { get; private set; }
        public long Frequency { get; set; } = 60000;

        private Timer _priceCheckTimer;

        public TradePriceMonitor()
            : this(TradePriceType.Last)
        {
        }

        public TradePriceMonitor(TradePriceType priceType)
        {
            this.PriceType = priceType;
        }

        public TradePriceMonitor(TradePriceType priceType, Currency convertToCurrency)
            : this(priceType)
        {
            this.ConvertToCurrency = convertToCurrency;
        }

        public TradePriceMonitor(TradePriceType priceType, long frequency)
            : this(priceType)
        {
            this.Frequency = frequency;
        }

        public TradePriceMonitor(TradePriceType priceType, Currency convertToCurrency, long frequency)
            : this(priceType, convertToCurrency)
        {
            this.Frequency = frequency;
        }

        /// <summary>
        /// Starts monitoring the price at a set frequency
        /// </summary>
        /// <param name="callback">An optional callback to be called each time the price is updated.</param>
        public void StartMonitoring(Action<double> callback = null)
        {
            _priceCheckTimer = new Timer(state => 
            {
                CurrentPrice = checkPrice();
                if(callback != null)
                {
                    callback(CurrentPrice);
                }
            }, null, 0, Frequency);
        }

        public void StopMonitoring()
        {
            _priceCheckTimer.Dispose();
        }

        abstract protected double checkPrice();
    }

    public enum TradePriceType
    {
        Last,
        Ask,
        Bid
    }

    public enum Currency
    {
        GBP,
        EUR,
        USD
    }
}
