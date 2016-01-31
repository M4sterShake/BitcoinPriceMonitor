namespace BitcoinPriceMonitor.PriceMonitor
{
    using System;
    using System.Collections.Generic;
    using Config;
    using Newtonsoft.Json.Linq;
    using RestSharp;
    using RestSharp.Deserializers;

    class BtcePriceMonitor : RestTradePriceMonitor
    {
        public BtcePriceMonitor(IRestClient apiClient, ISettings settings)
            : base(apiClient, settings)
        {
            ApiClient.BaseUrl = new Uri(Settings.BtceApiUrl);
        }

        protected override double CheckPrice()
        {
            var priceConversionString = $"btc_{Enum.GetName(typeof (Currency), TargetCurrency)?.ToLower()}";
            var request = new RestRequest($"/ticker/{priceConversionString}", Method.GET)
            {
                RequestFormat = DataFormat.Json
            };
            var response = ApiClient.Execute(request);
            var deserializer = new JsonDeserializer();
            var deserializedResponse = deserializer.Deserialize<Dictionary<string, string>>(response);
            var apiPriceType = ToBtcePriceTypeString(PriceType);
            string responseContents;
            var gotValue = deserializedResponse.TryGetValue(priceConversionString, out responseContents);
            string resultString = string.Empty;
            if (gotValue)
            {
                var deserializedResponseContents = JObject.Parse(responseContents);
                resultString = deserializedResponseContents[apiPriceType].ToString();
            }
            double result;

            return gotValue && double.TryParse(resultString, out result) ? result : -1;
        }

        private string ToBtcePriceTypeString(TradePriceType priceType)
        {
            string priceTypeString;

            switch (priceType)
            {
                case TradePriceType.Ask:
                    priceTypeString = "sell";
                    break;
                case TradePriceType.Bid:
                    priceTypeString = "buy";
                    break;
                case TradePriceType.Last:
                    priceTypeString = "last";
                    break;
                default:
                    priceTypeString = string.Empty;
                    break;
            }

            return priceTypeString;
        }
    }
}
