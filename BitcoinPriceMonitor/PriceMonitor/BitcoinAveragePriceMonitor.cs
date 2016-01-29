namespace BitcoinPriceMonitor.PriceMonitor
{
    using System;
    using RestSharp;
    using Config;

    public class BitcoinAveragePriceMonitor : TradePriceMonitor
    {
        private readonly IRestClient _apiClient;
        private readonly ISettings _settings;

        public BitcoinAveragePriceMonitor(IRestClient apiClient, ISettings settings)
        {
            _apiClient = apiClient;
            _settings = settings;
            apiClient.BaseUrl = new Uri(_settings.BitcoinAverageApiUrl);
        }

        public BitcoinAveragePriceMonitor(IRestClient apiClient, ISettings settings, TradePriceType priceType,
            Currency convertToCurrency, int frequency)
            : this(apiClient, settings)
        {
            PriceType = priceType;
            ConvertToCurrency = convertToCurrency;
            Frequency = frequency;
        }
        
        protected override double CheckPrice()
        {
            var request = new RestRequest("ticker/global/{currency}/{priceType}", Method.GET);
            request.AddUrlSegment("currency", Enum.GetName(typeof(Currency), ConvertToCurrency));
            request.AddUrlSegment("priceType", Enum.GetName(typeof(TradePriceType), PriceType).ToLower());
            IRestResponse response = _apiClient.Execute(request);
            double result;
            
            return double.TryParse(response.Content, out result) ? result : -1;
        }
    }
}
