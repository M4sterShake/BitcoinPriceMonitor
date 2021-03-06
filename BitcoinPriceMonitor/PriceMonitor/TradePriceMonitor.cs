﻿namespace BitcoinPriceMonitor.PriceMonitor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using Observer;

    public abstract class TradePriceMonitor : ITradePriceMonitor
    {
        public virtual Currency TargetCurrency { get; set; } = Currency.USD;
        public TradePriceType PriceType { get; set; } = TradePriceType.Last;
        public double CurrentPrice { get; private set; }
        public int Frequency { get; set; } = 5000;
        public abstract IEnumerable<Currency> SupportedCurrencies { get; }

        private Timer _priceCheckTimer;
        private readonly IList<ITradePriceObserver> _subscribers = new List<ITradePriceObserver>();

        public void StartMonitoring()
        {
            _priceCheckTimer = new Timer(state => 
            {
                CurrentPrice = CheckPrice();
                Notify(CurrentPrice);
            }, null, 0, Frequency);
        }

        public void StopMonitoring()
        {
            _priceCheckTimer.Dispose();
        }

        protected abstract double CheckPrice();

        private void Notify(double price)
        {
            foreach (var observer in _subscribers)
            {
                observer.Update(new TradePrice(price, TargetCurrency));
            }
        }

        public void Subscribe(ITradePriceObserver observer)
        {
            _subscribers.Add(observer);
        }

        public void Unsubscribe(ITradePriceObserver observer)
        {
            _subscribers.Remove((from o in _subscribers
                              where o.ObserverId == observer.ObserverId
                              select o).First());
        }

        public void TrasferSubscription(ITradePriceObservable observable)
        {
            foreach (var observer in _subscribers.ToArray())
            {
                observable.Subscribe(observer);
                Unsubscribe(observer);
            }
        }

        public void Dispose()
        {
            
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                StopMonitoring();
            }
        }

        ~TradePriceMonitor()
        {
            Dispose(false);
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
        ARS,
        ARSb,
        AUD,
        BRL,
        CAD,
        CNY,
        DKK,
        EUR,
        HKD,
        INR,
        ILS,
        JPY,
        MXN,
        NZD,
        NOK,
        PLN,
        GBP,
        RUB,
        ZAR,
        KRW,
        SEK,
        CHF,
        SGD,
        THB,
        USD
    }
}
