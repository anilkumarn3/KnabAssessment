using KnabCryptoExchange.Models;

namespace KnabCryptoExchange.Domain
{
    public interface ICryptocurrencyReader
    {
        Task<CryptoValue> FetchBitcoinValue(string cryptocurrencyCode, string currency);
    }
}