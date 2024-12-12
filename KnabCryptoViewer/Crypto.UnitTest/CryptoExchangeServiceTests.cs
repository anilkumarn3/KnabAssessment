﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Xunit;
using Microsoft.Extensions.Logging;
using KnabCryptoViewer.Controllers;

namespace Crypto.UnitTest;

public class CryptoExchangeServiceTests
{
    private readonly Mock<CryptocurrencyReader> _mockReader;
    private readonly Mock<ILogger<CryptoExchangeService>> _mockLogger;
    private readonly CryptoExchangeService _service;

    public CryptoExchangeServiceTests()
    {
        // Initialize mocks
        _mockReader = new Mock<CryptocurrencyReader>();
        _mockLogger = new Mock<ILogger<CryptoExchangeService>>();

        // Initialize the service with mocked dependencies
        _service = new CryptoExchangeService(_mockReader.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task Fetch_ReturnsCorrectCryptoValues_ForAllCurrencies()
    {
        // Arrange
        var cryptoCode = "BTC";  // Example cryptocurrency code
        var mockCryptoValues = new List<CryptoValue>
        {
            new CryptoValue { Currency = "USD", Price = 30000.ToString() },
            new CryptoValue { Currency = "EUR", Price = 25000.ToString() },
            new CryptoValue { Currency = "BRL", Price = 150000.ToString() },
            new CryptoValue { Currency = "GBP", Price = 23000.ToString() },
            new CryptoValue { Currency = "AUD", Price = 21000.ToString() }
        };

        // Setup the mock to return a Task with the expected values
        _mockReader.Setup(reader => reader.FetchBitcoinValue(cryptoCode, It.IsAny<string>()))
            .Returns<string, string>((code, currency) =>
            {
                var result = mockCryptoValues.FirstOrDefault(x => x.Currency == currency);
                return Task.FromResult(result);
            });

        // Act
        var result = await _service.Fetch(cryptoCode);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(5, result.Count); // We expect 5 values, one for each currency
        Assert.Contains(result, x => x.Currency == "USD" && x.Price == 30000.ToString());
        Assert.Contains(result, x => x.Currency == "EUR" && x.Price == 25000.ToString());
        Assert.Contains(result, x => x.Currency == "BRL" && x.Price == 150000.ToString());
        Assert.Contains(result, x => x.Currency == "GBP" && x.Price == 23000.ToString());
        Assert.Contains(result, x => x.Currency == "AUD" && x.Price == 21000.ToString());

        // Verify that FetchBitcoinValue was called 5 times (once for each currency)
        _mockReader.Verify(reader => reader.FetchBitcoinValue(cryptoCode, It.IsAny<string>()), Times.Exactly(5));

        // Optionally, you can verify logging calls, though it's not always necessary for functionality tests
        _mockLogger.Verify(logger => logger.LogInformation(It.IsAny<string>(), It.IsAny<object[]>()), Times.Once);
    }
}