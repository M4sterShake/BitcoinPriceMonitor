using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using RestSharp;

namespace BitcoinPriceMonitor
{
    public abstract class TradePriceMonitor : ITradePriceMonitor
    {
        public Currency ConvertToCurrency { get; set; } = Currency.USD;
        public TradePriceType PriceType { get; set; } = TradePriceType.Last;
        public double CurrentPrice { get; private set; }
        public long Frequency { get; set; } = 2000;

        private Timer _priceCheckTimer;
        private List<ITradePriceObserver> observers = new List<ITradePriceObserver>();

        public TradePriceMonitor()
        {
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

        abstract protected double checkPrice();

        private void Notify(double price)
        {
            foreach (var observer in observers)
            {
                observer.Update(price);
            }
        }

        public void Subscribe(ITradePriceObserver observer)
        {
            observers.Add(observer);
        }

        public void Unsubscribe(ITradePriceObserver observer)
        {
            observers.Remove((from o in observers
                              where o.ObserverId == observer.ObserverId
                              select o).First());
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
