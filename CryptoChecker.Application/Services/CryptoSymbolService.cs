using CryptoChecker.Application.DTO.Responses;
using CryptoChecker.Application.Intefraces;
using CryptoChecker.Application.Mapping;
using CryptoChecker.Domain.Entities;
using CryptoChecker.Infrastructure.Db;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;
using System.Text.Json;
using System.Threading;

namespace CryptoChecker.Application.Services
{
    public class CryptoSymbolService(CryptoCheckerDb dbContext) : ICryptoSymbolService
    {
        public async Task AddListAsync(List<CryptoSymbolResponse> responses, CancellationToken cancellationToken = default)
        {
            var cryptoSymbols = new ConcurrentBag<CryptoSymbol>();

            var assetIdBases = responses.Select(r => r.AssetIdBase).Distinct().ToList();

            var cryptoCurrencies = await dbContext.Currencies
                                .Where(c => assetIdBases.Contains(c.AssetName))
                                .ToListAsync(cancellationToken);

            var cryptoCurrencyDict = new ConcurrentDictionary<string, CryptoCurrency>(
                                        cryptoCurrencies
                                        .Where(c => !string.IsNullOrEmpty(c.AssetName))
                                        .ToDictionary(c => c.AssetName, c => c));

            var symbolNames = responses.Select(r => r.SymbolId).Distinct().ToList();

            var existingCryptoSymbols = await GetAllSymbolsAsync(symbolNames, assetIdBases, cancellationToken);

            var cryptoSymbolsDict = existingCryptoSymbols
                                            .ToDictionary(cs => (cs.SymbolName, cs.CryptoId.Value), cs => cs);

            var existingCryptoSymbolDict = new ConcurrentDictionary<(string SymbolName, int CryptoId), CryptoSymbol>(cryptoSymbolsDict);

            Parallel.ForEach(responses, response =>
            {
                if (string.IsNullOrEmpty(response.AssetIdBase) || !cryptoCurrencyDict.TryGetValue(response.AssetIdBase, out var cryptoCurrency) || cryptoCurrency.Id == 0)
                {
                    return;
                }

                (string SymbolName, int CryptoId) key = (response.SymbolId, cryptoCurrency.Id);

                if ((string.IsNullOrEmpty(key.SymbolName) || key.CryptoId == null) && existingCryptoSymbolDict.ContainsKey(key))
                {
                    return;
                }

                var cryptoSymbol = Mappers.DTOToSymbol(response);
                cryptoSymbol.Price = response.Price > decimal.MaxValue ? decimal.MaxValue : response.Price;

                cryptoSymbol.CryptoId = cryptoCurrency.Id;
                cryptoSymbol.Crypto = cryptoCurrency;

                cryptoSymbols.Add(cryptoSymbol);
            });

            await dbContext.AddRangeAsync(cryptoSymbols, cancellationToken);

            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<string?>> GetByCryptoNameAsync(string cryptoName, CancellationToken cancellationToken = default)
        {
            return await dbContext.CryptoSymbols.AnyAsync(cancellationToken) ?
                    await dbContext.CryptoSymbols.AsTracking()
                                    .Include(x => x.Crypto)
                                    .Where(x => x.Crypto.AssetName.Equals(cryptoName) && x.TradeExchange.Equals("USD"))
                                    .Select(x => x.SymbolName)
                                    .Take(10) //Only because for btc it's about 20000 objects.
                                    .ToListAsync(cancellationToken) : ["BITSTAMP_SPOT_BTC_USD"];
        }

        private async Task<List<CryptoSymbol>> GetAllSymbolsAsync(List<string> symbolNames, List<string> assetIdBases, CancellationToken cancellationToken = default)
        {
            int batch = 5000;
            var existingCryptoSymbols = new List<CryptoSymbol>();

            for (int i = 0; i < symbolNames.Count; i += batch)
            {
                var pagedSymbols = symbolNames.Skip(i).Take(batch);
                var pagedResult = await dbContext.CryptoSymbols
                                        .AsNoTracking()
                                        .Where(cs => pagedSymbols.Contains(cs.SymbolName) && assetIdBases.Contains(cs.Crypto.AssetName) && cs.CryptoId.HasValue)
                                        .ToListAsync(cancellationToken);

                existingCryptoSymbols.AddRange(pagedResult);
            }

            return existingCryptoSymbols;
        }
    }
}
