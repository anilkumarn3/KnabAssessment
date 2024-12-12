using KnabCryptoViewer.Models;

namespace KnabCryptoViewer.Domain
{
    public interface ICryptocurrencyReader
    {
        Task<CryptoValue> FetchBitcoinValue(string cryptocurrencyCode, string currency);
    }
}