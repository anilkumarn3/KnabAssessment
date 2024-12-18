﻿using Moq;
using Microsoft.Extensions.Logging;
using KnabCryptoExchange.Models;
using KnabCryptoExchange.Domain;
using KnabCryptoExchange.Service;
using FluentAssertions;

namespace Crypto.UnitTest;

public class CryptoExchangeServiceTests
{
    private readonly Mock<ICryptocurrencyReader> _mockReader;
    private readonly Mock<ILogger<CryptoExchangeService>> _mockLogger;
    private readonly ICryptoExchangeService _service;

    public CryptoExchangeServiceTests()
    {
        _mockReader = new Mock<ICryptocurrencyReader>();
        _mockLogger = new Mock<ILogger<CryptoExchangeService>>();

        _service = new CryptoExchangeService(_mockReader.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task Fetch_ReturnsCorrectCryptoValues_ForAllCurrencies()
    {
        // Arrange
        var cryptoCode = "BTC";
        var mockCryptoValues = new List<CryptoValue>
        {
            new CryptoValue { Currency = "USD", Price = 30000.23 },
            new CryptoValue { Currency = "EUR", Price = 25000.45 },
            new CryptoValue { Currency = "BRL", Price = 15000.23 },
            new CryptoValue { Currency = "GBP", Price = 23000.57 },
            new CryptoValue { Currency = "AUD", Price = 21000.34 }
        };

        _mockReader.Setup(reader => reader.FetchBitcoinValue(cryptoCode, It.IsAny<string>()))
            .Returns<string, string>((code, currency) =>
            {
                var result = mockCryptoValues.FirstOrDefault(x => x.Currency == currency);
                return Task.FromResult(result);
            });

        // Act
        var result = await _service.Fetch(cryptoCode);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(5);
        result.Should().Contain(x => x.Currency == "USD" && x.Price == 30000.23);
        result.Should().Contain(x => x.Currency == "EUR" && x.Price == 25000.45);
        result.Should().Contain(x => x.Currency == "BRL" && x.Price == 15000.23);
        result.Should().Contain(x => x.Currency == "GBP" && x.Price == 23000.57);
        result.Should().Contain(x => x.Currency == "AUD" && x.Price == 21000.34);

        _mockReader.Verify(reader => reader.FetchBitcoinValue(cryptoCode, It.IsAny<string>()), Times.Exactly(5));
    }

    [Fact]
    public async Task Fetch_ReturnsEmptyList_WhenFetchBitcoinValueReturnsNull()
    {
        // Arrange
        var cryptoCode = "BTC";

        _mockReader.Setup(reader => reader.FetchBitcoinValue(cryptoCode, It.IsAny<string>()))
            .Returns<string, string>((code, currency) => Task.FromResult<CryptoValue>(null));

        // Act
        var result = await _service.Fetch(cryptoCode);

        // Assert
        Assert.NotNull(result);
        result.Should().NotBeNull();
        result[0].Should().BeNull();
    }

    [Fact]
    public async Task Fetch_ReturnsPartialList_WhenFetchBitcoinValueThrowsException()
    {
        // Arrange
        var cryptoCode = "BTC";

        var mockCryptoValues = new List<CryptoValue>
        {
            new CryptoValue { Currency = "USD", Price = 30000.23 },
            new CryptoValue { Currency = "EUR", Price = 25000.45 },
            new CryptoValue { Currency = "BRL", Price = 15000.23 },
            new CryptoValue { Currency = "GBP", Price = 23000.57 },
            new CryptoValue { Currency = "AUD", Price = 21000.34 }
        };

        _mockReader.Setup(reader => reader.FetchBitcoinValue(cryptoCode, "USD"))
            .Returns(Task.FromResult(mockCryptoValues[0]));
        _mockReader.Setup(reader => reader.FetchBitcoinValue(cryptoCode, "EUR"))
            .Returns(Task.FromResult(mockCryptoValues[1]));
        _mockReader.Setup(reader => reader.FetchBitcoinValue(cryptoCode, "BRL"))
            .Returns(Task.FromResult(mockCryptoValues[2]));

        _mockReader.Setup(reader => reader.FetchBitcoinValue(cryptoCode, "GBP"))
            .ThrowsAsync(new Exception("Network error"));

        _mockReader.Setup(reader => reader.FetchBitcoinValue(cryptoCode, "AUD"))
            .Returns(Task.FromResult(mockCryptoValues[4]));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(async () => await _service.Fetch(cryptoCode));
    }
}
