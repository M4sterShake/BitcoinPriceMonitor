using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BitcoinPriceMonitor.Config;
using RestSharp;
using RestSharp.Deserializers;

namespace BitcoinPriceMonitor.PriceMonitor
{
    class BitfinexPriceMonitor : RestTradePriceMonitor
    {
        public BitfinexPriceMonitor(IRestClient apiClient, ISettings settings) : base(apiClient, settings)
        {
            ApiClient.BaseUrl = new Uri(Settings.BitfinexApiUrl);
        }

        public override IEnumerable<Currency> SupportedCurrencies => new List<Currency>
        {
            Currency.USD
        };

        protected override double CheckPrice()
        {
            var request = new RestRequest("/pubticker/BTC{currency}", Method.GET);
            request.AddUrlSegment("currency", Enum.GetName(typeof(Currency), TargetCurrency));
            request.RequestFormat = DataFormat.Json;
            var response = ApiClient.Execute(request);
            var deserializer = new JsonDeserializer();
            var deserializedResponse = deserializer.Deserialize<Dictionary<string, string>>(response);
            var apiPriceType = PriceType == TradePriceType.Last
                ? "last_price"
                : Enum.GetName(typeof(TradePriceType), PriceType).ToLower();
            string resultString;
            var gotValue = deserializedResponse.TryGetValue(apiPriceType, out resultString);
            double result;

            return gotValue && double.TryParse(resultString, out result) ? result : -1;
        }
    }
}
