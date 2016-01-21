using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace BitcoinPriceMonitor
{
    public abstract class TradePriceMonitor : ITradePriceMonitor, IObservable<double>
    {
        public Currency ConvertToCurrency { get; set; } = Currency.USD;
        public TradePriceType PriceType { get; set; }
        public double CurrentPrice { get; private set; }
        public long Frequency { get; set; } = 60000;

        private Timer _priceCheckTimer;
        private List<IObserver<double>> observers = new List<IObserver<double>>();

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

        public void StartMonitoring()
        {
            _priceCheckTimer = new Timer(state => 
            {
                CurrentPrice = checkPrice();
                Notify(CurrentPrice);
            }, null, 0, Frequency);
        }

        public void StopMonitoring()
        {
            _priceCheckTimer.Dispose();
        }

        public IDisposable Subscribe(IObserver<double> observer)
        {
            observers.Add(observer);
            return new Unsubscriber(observers, observer);
        }

        abstract protected double checkPrice();

        private void Notify(double price)
        {
            foreach (var observer in observers)
            {
                observer.OnNext(price);
            }
        }
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
