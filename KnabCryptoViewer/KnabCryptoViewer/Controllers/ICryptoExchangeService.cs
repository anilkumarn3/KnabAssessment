﻿using KnabCryptoViewer.Models;

public interface ICryptoExchangeService
{
    Task<List<CryptoValue>> Fetch(string cryptoCode);
}
