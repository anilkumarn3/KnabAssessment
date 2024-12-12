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

        public async Task FetchBitcoinValue(string cryptocurrencyCode, List<string> currencies)
        {
            var apiKey = coinMarketCapSettings.ApiKey;
            var apiUrl = coinMarketCapSettings.ApiUrl;

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("X-CMC_PRO_API_KEY", apiKey);
                client.DefaultRequestHeaders.Add("Accept", "application/json");

                string currencyList = string.Join(",", currencies);

                string url = $"{apiUrl}?symbol={cryptocurrencyCode}&convert={currencyList}";

                try
                {
                    //HttpResponseMessage response = await client.GetAsync(url);

                    //response.EnsureSuccessStatusCode();

                    //string responseBody = await response.Content.ReadAsStringAsync();
                    string responseBody = string.Empty;
                    using (StreamReader reader = new StreamReader(@"C:\Users\AKN061\Downloads\test.json"))
                    {
                        responseBody = reader.ReadToEnd();
                    }
                    
                    dynamic data = JsonConvert.DeserializeObject(responseBody);

                    foreach (var currency in currencies)
                    {
                        var price = data.data[cryptocurrencyCode][0].quote[currency]?.price;
                        if (price != null)
                        {
                            Console.WriteLine($"Bitcoin Value in {currency}: {price} {currency}");
                        }
                        else
                        {
                            Console.WriteLine($"Price for {currency} not available.");
                        }
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error fetching data: {ex.Message}");
                }
            }
        }
    }
}
