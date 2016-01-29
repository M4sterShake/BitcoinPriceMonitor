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
