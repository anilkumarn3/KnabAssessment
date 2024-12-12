using Newtonsoft.Json;
using KnabCryptoExchange.Models;
using KnabCryptoExchange.CustomExceptions;

namespace KnabCryptoExchange.Domain
{
    public class MultipleCryptocurrencyReader
    {
        private readonly CoinMarketCapConfig coinMarketCapConfig;
        private readonly ILogger<MultipleCryptocurrencyReader> logger;

        public MultipleCryptocurrencyReader(CoinMarketCapConfig coinMarketCapConfig, ILogger<MultipleCryptocurrencyReader> logger)
        {
            this.coinMarketCapConfig = coinMarketCapConfig;
            this.logger = logger;
        }

        public async Task<List<CryptoValue>> FetchBitcoinValue(string cryptocurrencyCode, List<string> currencies)
        {
            var apiKey = coinMarketCapConfig.ApiKey;
            var apiUrl = coinMarketCapConfig.ApiUrl;

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("X-CMC_PRO_API_KEY", apiKey);
                client.DefaultRequestHeaders.Add("Accept", "application/json");

                string currencyList = string.Join(",", currencies);
                string url = $"{apiUrl}?symbol={cryptocurrencyCode}&convert={currencyList}";

                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);

                    response.EnsureSuccessStatusCode();

                    string responseBody = await response.Content.ReadAsStringAsync();

                    dynamic responseContent = JsonConvert.DeserializeObject(responseBody);

                    List<CryptoValue> cryptoValues = new List<CryptoValue>();
                    foreach (var currency in currencies)
                    {
                        var price = responseContent?.data[cryptocurrencyCode][0].quote[currency]?.price;
                        if (price != null)
                        {
                            var cryptoValue = new CryptoValue
                            {
                                Currency = currency,
                                Price = price
                            };
                            cryptoValues.Add(cryptoValue);
                            logger.LogInformation("Value in {Currency}: {Price}", currency, cryptoValue.Price);

                            return cryptoValues;
                        }
                    }
                    throw new CryptoExchangeFailedException("Price is not available");
                }
                catch (Exception ex)
                {
                    throw new CryptoExchangeFailedException(ex.Message);
                }
            }
        }
    }
}