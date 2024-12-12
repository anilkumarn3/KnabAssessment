using Newtonsoft.Json;

namespace KnabCryptoViewer.Controllers
{
    public class CryptocurrencyReader
    {
        private readonly CoinMarketCapConfig coinMarketCapSettings;

        public CryptocurrencyReader(CoinMarketCapConfig coinMarketCapSettings)
        {
            this.coinMarketCapSettings = coinMarketCapSettings;
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
                        Console.WriteLine($"Bitcoin Value in {currency}: {price} {currency}");
                        return cryptoValue;
                    }

                    Console.WriteLine($"Price for {currency} not available.");
                    return null;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error fetching data: {ex.Message}");
                    return null;
                }
            }
        }
    }
}