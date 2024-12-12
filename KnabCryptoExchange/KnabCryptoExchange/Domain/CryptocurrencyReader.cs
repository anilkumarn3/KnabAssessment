using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using KnabCryptoViewer.Models;

namespace KnabCryptoViewer.Domain
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
                    //string responseBody = string.Empty;
                    //using (StreamReader reader = new StreamReader(@"C:\Users\AKN061\Downloads\test.json"))
                    //{
                    //    responseBody = reader.ReadToEnd();
                    //}

                    dynamic responseContent = JsonConvert.DeserializeObject(responseBody);

                    var price = responseContent.data[cryptocurrencyCode][0].quote[currency]?.price;
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

                    logger.LogError($"Price for {currency} not available.");
                    return null;
                }
                catch (Exception ex)
                {
                    logger.LogError($"Error fetching data: {ex.Message}");
                    return null;
                }
            }
        }
    }
}