using KnabCryptoExchange.Models;

namespace KnabCryptoExchange.Service;

public interface ICryptoExchangeService
{
    Task<List<CryptoValue>> Fetch(string cryptoCode);
}
