using Newtonsoft.Json;
using KnabCryptoExchange.Models;
using KnabCryptoExchange.CustomExceptions;

namespace KnabCryptoExchange.Domain
{
    public class CryptocurrencyReader : ICryptocurrencyReader
    {
        private readonly CoinMarketCapConfig coinMarketCapSettings;
        private readonly ILogger<CryptocurrencyReader> logger;

        public CryptocurrencyReader(CoinMarketCapConfig coinMarketCapSettings, ILogger<CryptocurrencyReader> logger)
        {
            this.coinMarketCapSettings = coinMarketCapSettings;
            this.logger = logger;
        }

        public async Task<CryptoValue> FetchBitcoinValue(string cryptocurrencyCode, string currency)
        {
            var apiKey = coinMarketCapSettings.ApiKey;
            var apiUrl = coinMarketCapSettings.ApiUrl;

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("X-CMC_PRO_API_KEY", apiKey);
                client.DefaultRequestHeaders.Add("Accept", "application/json");

                string url = $"{apiUrl}?symbol={cryptocurrencyCode}&convert={currency}";

                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);

                    response.EnsureSuccessStatusCode();

                    string responseBody = await response.Content.ReadAsStringAsync();

                    dynamic responseContent = JsonConvert.DeserializeObject(responseBody);

                    var price = responseContent?.data[cryptocurrencyCode][0].quote[currency]?.price;
                    if (price != null)
                    {
                        var cryptoValue = new CryptoValue
                        {
                            Currency = currency,
                            Price = price
                        };
                        logger.LogInformation("Bitcoin Value in {Currency}: {Price}", currency, cryptoValue.Price);
                        return cryptoValue;
                    }

                    logger.LogError("Price for {Currency} not available.", currency);
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