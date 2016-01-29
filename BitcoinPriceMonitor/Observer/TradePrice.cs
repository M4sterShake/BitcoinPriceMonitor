namespace BitcoinPriceMonitor.Observer
{
    using PriceMonitor;

    public struct TradePrice
    {
        public TradePrice(double price, Currency currency)
        {
            Price = price;
            Currency = currency;
        }

        public double Price { get; }
        public Currency Currency { get; }
    }
}
