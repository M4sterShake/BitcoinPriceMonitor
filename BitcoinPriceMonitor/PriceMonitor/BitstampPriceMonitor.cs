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
    public class BitstampPriceMonitor : RestTradePriceMonitor
    {
        public BitstampPriceMonitor(IRestClient apiClient, ISettings settings) : base(apiClient, settings)
        {
            ApiClient.BaseUrl = new Uri(Settings.BitstampApiUrl);
        }

        public override IEnumerable<Currency> SupportedCurrencies => new List<Currency>
        {
            Currency.USD
        };

        public override Currency TargetCurrency => Currency.USD;

        protected override double CheckPrice()
        {
            var request = new RestRequest("/ticker/", Method.GET);
            request.RequestFormat = DataFormat.Json;
            var response = ApiClient.Execute(request);
            var deserializer = new JsonDeserializer();
            var deserializedResponse = deserializer.Deserialize<Dictionary<string, string>>(response);
            var apiPriceType = PriceType.ToString().ToLower();
            string resultString;
            var gotValue = deserializedResponse.TryGetValue(apiPriceType, out resultString);
            double result;

            return gotValue && double.TryParse(resultString, out result) ? result : -1;
        }
    }
}
