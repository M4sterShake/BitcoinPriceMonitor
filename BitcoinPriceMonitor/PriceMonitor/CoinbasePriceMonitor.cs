namespace BitcoinPriceMonitor.PriceMonitor
{
    using System;
    using Config;
    using RestSharp;
    using System.Collections.Generic;
    using RestSharp.Deserializers;

    public class CoinbasePriceMonitor : RestTradePriceMonitor
    {
        public CoinbasePriceMonitor(IRestClient apiClient, ISettings settings)
            : base(apiClient, settings)
        {
            ApiClient.BaseUrl = new Uri(Settings.CoinbaseApiUrl);
        }

        protected override double CheckPrice()
        {
            var request = new RestRequest("/products/BTC-{currency}/ticker", Method.GET);
            request.AddUrlSegment("currency", Enum.GetName(typeof(Currency), TargetCurrency));
            request.RequestFormat = DataFormat.Json;
            var response = ApiClient.Execute(request);
            var deserializer = new JsonDeserializer();
            var deserializedResponse = deserializer.Deserialize<Dictionary<string, string>>(response);
            var apiPriceType = PriceType == TradePriceType.Last
                ? "price"
                : Enum.GetName(typeof (TradePriceType), PriceType).ToLower();
            string resultString;
            var gotValue = deserializedResponse.TryGetValue(apiPriceType, out resultString);
            double result;

            return gotValue && double.TryParse(resultString, out result) ? result : -1;
        }
    }
}
