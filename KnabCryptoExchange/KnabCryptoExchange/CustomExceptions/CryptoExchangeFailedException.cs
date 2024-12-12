namespace KnabCryptoExchange.CustomExceptions
{
    [Serializable]
    public class CryptoExchangeFailedException : Exception
    {
        public CryptoExchangeFailedException(string? message) : base(message)
        {
        }
    }
}