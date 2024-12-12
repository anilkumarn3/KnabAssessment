using KnabCryptoExchange.Domain;
using KnabCryptoExchange.Models;

namespace KnabCryptoExchange.Service;

public class CryptoExchangeService : ICryptoExchangeService
{
    private readonly ICryptocurrencyReader reader;
    private readonly ILogger<CryptoExchangeService> logger;

    public CryptoExchangeService(ICryptocurrencyReader reader, ILogger<CryptoExchangeService> logger)
    {
        this.reader = reader;
        this.logger = logger;
    }

    public async Task<List<CryptoValue>> Fetch(string cryptoCode)
    {
        List<string> currencies = new List<string> { "USD", "EUR", "BRL", "GBP", "AUD" };

        var tasks = new List<Task<CryptoValue>>();
        logger.LogInformation("Fetching different equivalent currenies for {CryptocurrencyCode}", cryptoCode);

        foreach (var currency in currencies)
        {
            tasks.Add(reader.FetchBitcoinValue(cryptoCode, currency));
        }
        var results = await Task.WhenAll(tasks);

        return results.ToList();
    }
}