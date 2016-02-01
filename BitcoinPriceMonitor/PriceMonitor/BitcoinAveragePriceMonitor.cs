using System.Collections.Generic;

namespace BitcoinPriceMonitor.PriceMonitor
{
    using System;
    using RestSharp;
    using Config;

    public class BitcoinAveragePriceMonitor : RestTradePriceMonitor
    {
        public BitcoinAveragePriceMonitor(IRestClient apiClient, ISettings settings)
            : base(apiClient, settings)
        {
            ApiClient.BaseUrl = new Uri(Settings.BitcoinAverageApiUrl);
        }

        public override IEnumerable<Currency> SupportedCurrencies => new List<Currency>
        {
            Currency.ARS,
            Currency.AUD,
            Currency.BRL,
            Currency.CAD,
            Currency.CNY,
            Currency.DKK,
            Currency.EUR,
            Currency.HKD,
            Currency.INR,
            Currency.ILS,
            Currency.JPY,
            Currency.MXN,
            Currency.NZD,
            Currency.NOK,
            Currency.PLN,
            Currency.GBP,
            Currency.RUB,
            Currency.ZAR,
            Currency.KRW,
            Currency.SEK,
            Currency.CHF,
            Currency.SGD,
            Currency.THB,
            Currency.USD
        };

        protected override double CheckPrice()
        {
            var request = new RestRequest("ticker/global/{currency}/{priceType}", Method.GET);
            request.AddUrlSegment("currency", Enum.GetName(typeof(Currency), TargetCurrency));
            request.AddUrlSegment("priceType", Enum.GetName(typeof(TradePriceType), PriceType).ToLower());
            IRestResponse response = ApiClient.Execute(request);
            double result;
            
            return double.TryParse(response.Content, out result) ? result : -1;
        }
    }
}
