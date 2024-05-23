using CryptoChecker.Application.DTO;
using CryptoChecker.Application.DTO.Responses;
using CryptoChecker.Application.Intefraces;
using CryptoChecker.Domain.Entities;
using CryptoChecker.Infrastructure.Db;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CryptoChecker.Application.Services
{
    public class CoinRestApiService(
        IHttpClientFactory httpClientFactory, 
        ICryptoCurrencyService currencyService, 
        IChainAddressService chainAddressService,
        ICryptoSymbolService cryptoSymbolService,
        IHistoricalPriceService historicalPriceService) : ICoinRestApiService
    {
        private readonly IHttpClientFactory _clientFactory = httpClientFactory;

        public async Task GetAllCryptoAsync(CancellationToken cancellation = default)
        {
            var client = _clientFactory.CreateClient("CoinApi");
            var response = await client.GetAsync("assets", cancellation);

            response.EnsureSuccessStatusCode();

            var body = await response.Content.ReadAsStringAsync(cancellation);

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            };

            var assets = JsonSerializer.Deserialize<List<CurrencyResponse>>(body, options);

            var crypto = assets.Where(x => x.TypeIsCrypto == 1 && x.PriceUsd > 0.0).ToList();

            var chainAddresses = crypto
                                    .Where(c => c.ChainAddresses != null)
                                    .SelectMany(c => c.ChainAddresses)
                                    .ToList();

            await chainAddressService.AddListChainAsync(chainAddresses, cancellation);

            await currencyService.AddListCurrencyAsync(crypto, cancellation);
        }

        public async Task GetSymbolsAsync(CancellationToken cancellation = default)
        {
            var client = _clientFactory.CreateClient("CoinApi");
            client.Timeout = TimeSpan.FromMinutes(10);
            var response = await client.GetAsync("symbols", cancellation);

            response.EnsureSuccessStatusCode();

            var body = await response.Content.ReadAsStringAsync(cancellation);

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            };

            var symbols = JsonSerializer.Deserialize<List<CryptoSymbolResponse>>(body, options);

            await cryptoSymbolService.AddListAsync(symbols, cancellation);
        }

        public async Task<List<HistoricalPriceDto>> GetHistoricPriceByCryptoNameAsync(string name, CancellationToken cancellationToken = default)
        {
            var symbols = await cryptoSymbolService.GetByCryptoNameAsync(name, cancellationToken);
            var tasks = new List<Task<string>>();

            foreach (var symbol in symbols)
            {
                tasks.Add(GetQuoteHistoryAsync(symbol, cancellationToken));
            }

            var results = await Task.WhenAll(tasks);

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            };

            var combinedResult = string.Join(",", results);

            var prices = JsonSerializer.Deserialize<List<CryptoQuote>>(combinedResult, options);

            return await historicalPriceService.AddListAsync(prices, cancellationToken);
        }

        private async Task<string> GetQuoteHistoryAsync(string symbolId, CancellationToken cancellationToken = default)
        {
            DateTime startTime = new DateTime(DateTime.Now.Year, 1, 1, 0, 0, 0);
            DateTime endTime = new DateTime(DateTime.Now.Year, 12, 31, 23, 59, 59);

            var url = $"quotes/{symbolId}/history?period_id=1DAY" +
                  $"&time_start={startTime:yyyy-MM-ddTHH:mm:ss}" +
                  $"&time_end={endTime:yyyy-MM-ddTHH:mm:ss}";

            var client = _clientFactory.CreateClient("CoinApi");
            var response = await client.GetAsync(url, cancellationToken);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync(cancellationToken);
        }
    }
}
