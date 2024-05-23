using CryptoChecker.Application.DTO;
using CryptoChecker.Application.DTO.Responses;
using CryptoChecker.Application.Intefraces;
using CryptoChecker.Application.Mapping;
using CryptoChecker.Domain.Entities;
using CryptoChecker.Infrastructure.Db;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;

namespace CryptoChecker.Application.Services
{
    public class HistoricalPriceService(CryptoCheckerDb db) : IHistoricalPriceService
    {
        public async Task<List<HistoricalPriceDto>> AddListAsync(List<CryptoQuote> response, CancellationToken cancellationToken = default)
        {
            var historyPrice = new ConcurrentBag<HistoricalPrice>();
            var updateHistoryPrice = new ConcurrentBag<HistoricalPrice>();

            var symbolsIdBases = response.Select(r => r.SymbolId).Distinct().ToList();

            var existCryptoSymbols = await db.CryptoSymbols
            .Where(c => symbolsIdBases.Contains(c.SymbolName) && !string.IsNullOrEmpty(c.SymbolName))
                                .ToListAsync(cancellationToken);

            var dictWithoutNull = existCryptoSymbols.ToDictionary(c => c.SymbolName, c => c);

            var cryptoCurrencyDict = new ConcurrentDictionary<string, CryptoSymbol>(dictWithoutNull);

            var existingHistoryPrices = await db.HistoricalPrices
                    .Where(hp => symbolsIdBases.Contains(hp.CryptoSymbol.SymbolName) &&
                     hp.UpdateTime.Year == DateTime.UtcNow.Year)
                    .ToListAsync(cancellationToken);

            var existingHistoryPriceDict = new ConcurrentDictionary<(string SymbolName, DateTime UpdateTime), HistoricalPrice>(
                existingHistoryPrices.ToDictionary(hp => (hp.CryptoSymbol.SymbolName, hp.UpdateTime))
            );

            Parallel.ForEach(response, (cryptoQuote) =>
            {
                if (string.IsNullOrEmpty(cryptoQuote.SymbolId) || !cryptoCurrencyDict.TryGetValue(cryptoQuote.SymbolId, out var cryptoSymbol))
                {
                    return;
                }

                var key = (cryptoQuote.SymbolId, cryptoQuote.TimeExchange);
                if (existingHistoryPriceDict.TryGetValue(key, out var history))
                {
                    if (history.AskPrice != cryptoQuote.AskPrice)
                    {
                        history.AskPrice = cryptoQuote.AskPrice;
                    }

                    if (history.BidPrice != cryptoQuote.BidPrice)
                    {
                        history.BidPrice = cryptoQuote.BidPrice;
                    }

                    history.UpdateTime = DateTime.UtcNow;
                    updateHistoryPrice.Add(history);
                }

                var historicalPrice = new HistoricalPrice
                {
                    AskPrice = cryptoQuote.AskPrice,
                    BidPrice = cryptoQuote.BidPrice,
                    UpdateTime = cryptoQuote.TimeExchange,
                    CryptoSymbol = cryptoSymbol
                };

                historyPrice.Add(historicalPrice);

            });

            if (!updateHistoryPrice.IsEmpty)
            {
                db.HistoricalPrices.UpdateRange(updateHistoryPrice);
            }

            await db.HistoricalPrices.AddRangeAsync(historyPrice, cancellationToken);

            await db.SaveChangesAsync(cancellationToken);

            var allHistoryPrices = historyPrice.ToList();
            allHistoryPrices.AddRange(updateHistoryPrice);

            List<HistoricalPriceDto> dtos = new List<HistoricalPriceDto>();
            foreach(var price in allHistoryPrices)
            {
                dtos.Add(price.ToDto());
            }

            return dtos;
        }
    }
}
