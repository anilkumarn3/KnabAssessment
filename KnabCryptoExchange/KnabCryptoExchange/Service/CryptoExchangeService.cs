using KnabCryptoViewer.Domain;
using KnabCryptoViewer.Models;

namespace KnabCryptoViewer.Service;

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

        foreach (var currency in currencies)
        {
            tasks.Add(reader.FetchBitcoinValue(cryptoCode, currency));
        }
        logger.LogInformation("Fetching Bitcoin value for {CryptocurrencyCode}", cryptoCode);
        var results = await Task.WhenAll(tasks);

        return results.ToList();
    }
}