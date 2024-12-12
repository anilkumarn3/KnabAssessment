using KnabCryptoViewer.Models;

namespace KnabCryptoViewer.Service;

public interface ICryptoExchangeService
{
    Task<List<CryptoValue>> Fetch(string cryptoCode);
}
