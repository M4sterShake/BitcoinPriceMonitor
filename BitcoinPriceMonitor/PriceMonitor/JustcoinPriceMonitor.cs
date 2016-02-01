using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BitcoinPriceMonitor.Config;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace BitcoinPriceMonitor.PriceMonitor
{
    class JustcoinPriceMonitor : RestTradePriceMonitor
    {
        public JustcoinPriceMonitor(IRestClient apiClient, ISettings settings) : base(apiClient, settings)
        {
            ApiClient.BaseUrl = new Uri(Settings.JustcoinApiUrl);
        }

        public override IEnumerable<Currency> SupportedCurrencies => new List<Currency>
        {
            Currency.USD
        };

        protected override double CheckPrice()
        {
            var marketId = $"BTC{Enum.GetName(typeof(Currency), TargetCurrency)}";
            var request = new RestRequest("/markets", Method.GET)
            {
                RequestFormat = DataFormat.Json
            };
            var response = ApiClient.Execute(request);
            var apiPriceType = PriceType.ToString().ToLower();

            dynamic deserializedResponse;
            string resultString = string.Empty;
            var gotValue = false;

            try
            {
                deserializedResponse = JObject.Parse(response.Content);
                foreach (var market in deserializedResponse)
                {
                    if (market.Id == marketId)
                    {
                        resultString = market[apiPriceType].ToString();
                        gotValue = true;
                    }
                }
            }
            catch (Exception)
            {
                resultString = string.Empty;
            }
            double result;

            return gotValue && double.TryParse(resultString, out result) ? result : -1;
        }
    }
}
