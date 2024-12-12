
using KnabCryptoViewer.Models;

namespace KnabCryptoViewer.Controllers
{
    public interface ICryptocurrencyReader
    {
        Task<CryptoValue> FetchBitcoinValue(string cryptocurrencyCode, string currency);
    }
}