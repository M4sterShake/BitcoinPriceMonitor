namespace BitcoinPriceMonitor
{
    using RestSharp;
    using System;

    public class BitcoinAveragePriceMonitor : TradePriceMonitor
    {
        private IRestClient _apiClient;
        public BitcoinAveragePriceMonitor(IRestClient apiClient)
        {
            _apiClient = apiClient;
        }

        protected override double checkPrice()
        {
            var request = new RestRequest("ticker/global/{currency}/{priceType}", Method.GET);
            request.AddUrlSegment("currency", Enum.GetName(typeof(Currency), this.ConvertToCurrency));
            request.AddUrlSegment("priceType", Enum.GetName(typeof(TradePriceType), this.PriceType).ToLower());
            IRestResponse response = _apiClient.Execute(request);
            double result;
            
            return double.TryParse(response.Content, out result) == true ? result : -1;
        }
    }
}
