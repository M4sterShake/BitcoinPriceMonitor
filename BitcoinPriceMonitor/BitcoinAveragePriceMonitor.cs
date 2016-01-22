using RestSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitcoinPriceMonitor
{
    public class BitcoinAveragePriceMonitor : Monitor
    {
        public BitcoinAveragePriceMonitor()
        {
        }

        public BitcoinAveragePriceMonitor(TradePriceType priceType)
            : base(priceType)
        {
        }

        public BitcoinAveragePriceMonitor(TradePriceType priceType, Currency convertToCurrency)
            : base(priceType, convertToCurrency)
        {
        }

        public BitcoinAveragePriceMonitor(TradePriceType priceType, long frequency)
            : base(priceType, frequency)
        {
        }

        public BitcoinAveragePriceMonitor(TradePriceType priceType, Currency convertToCurrency, long frequency)
            : base(priceType, convertToCurrency, frequency)
        {
        }

        protected override double checkPrice()
        {
            var apiClient = new RestClient("https://api.bitcoinaverage.com");
            var request = new RestRequest("ticker/global/{currency}/{priceType}", Method.GET);
            request.AddUrlSegment("currency", Enum.GetName(typeof(Currency), this.ConvertToCurrency));
            request.AddUrlSegment("priceType", Enum.GetName(typeof(TradePriceType), this.PriceType).ToLower());
            IRestResponse response = apiClient.Execute(request);
            double result;
            
            return double.TryParse(response.Content, out result) == true ? result : -1;
        }
    }
}
