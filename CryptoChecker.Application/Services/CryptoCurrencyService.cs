using CryptoChecker.Application.DTO;
using CryptoChecker.Application.DTO.Responses;
using CryptoChecker.Application.Intefraces;
using CryptoChecker.Application.Mapping;
using CryptoChecker.Domain.Entities;
using CryptoChecker.Infrastructure.Db;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoChecker.Application.Services
{
    public class CryptoCurrencyService(CryptoCheckerDb dbcontext) : ICryptoCurrencyService
    {

        public async Task AddListCurrencyAsync(List<CurrencyResponse> currency, CancellationToken cancellationToken = default)
        {
            var crypto = InitializeCryptoCurrencies(currency);
            var dbChains = await GetDbChainsAsync(cancellationToken);
            var dbCurrency = await GetDbCurrenciesAsync(cancellationToken);

            ProcessRelatedCurrencies(currency, crypto, dbChains, dbCurrency);
            await AddNewCurrenciesToDatabaseAsync(crypto, dbCurrency, cancellationToken);
        }

        public async Task<GetCrypto> GetListAsync(int pageNumber = 1, int pageSize = 10)
        {
            int skipElements = (pageNumber - 1) * pageSize;
            var result = await dbcontext.Currencies
                .AsNoTracking()
                .Include(x => x.ChainAddresses)
                .AsSplitQuery()
                .Include(x => x.CryptoSymbols)
                .OrderBy(x => x.Id)
                .Skip(skipElements)
                .Take(pageSize)
                .ToListAsync();

            var cryptoDTOs = result.Select(Mappers.CryptoToDTO).ToList();

            int totalCryptoCount = await dbcontext.Currencies.AsNoTracking().CountAsync();

            int pageCount = (int)Math.Ceiling((double)totalCryptoCount / pageSize);

            return new GetCrypto { Cryptos = cryptoDTOs, PageCount = pageCount };
        }




        private ConcurrentBag<CryptoCurrency> InitializeCryptoCurrencies(List<CurrencyResponse> currency)
        {
            return new ConcurrentBag<CryptoCurrency>(
                currency.Select(x => new CryptoCurrency
                {
                    AssetName = x.AssetId,
                    DataEnd = x.DataEnd,
                    DataStart = x.DataStart,
                    Name = x.Name,
                    PriceUsd = (decimal)x.PriceUsd,
                }));
        }

        private async Task<List<ChainAddress>> GetDbChainsAsync(CancellationToken cancellationToken)
        {
            return await dbcontext.Chains.ToListAsync(cancellationToken);
        }

        private async Task<List<CryptoCurrency>> GetDbCurrenciesAsync(CancellationToken cancellationToken)
        {
            return await dbcontext.Currencies.AsNoTracking().ToListAsync(cancellationToken);
        }

        private void ProcessRelatedCurrencies(List<CurrencyResponse> currency, ConcurrentBag<CryptoCurrency> crypto, List<ChainAddress> dbChains, List<CryptoCurrency> dbCurrency)
        {
            Parallel.ForEach(dbChains, dbChain =>
            {
                var relatedCurrencies = currency
                    .Where(c => c.ChainAddresses != null && c.ChainAddresses.Any(ca => ca.ChainId == dbChain.ChainName))
                    .ToList();

                foreach (var currencyItem in relatedCurrencies)
                {
                    var relatedCrypto = crypto.FirstOrDefault(c => c.AssetName == currencyItem.AssetId);

                    if (relatedCrypto == null && !dbCurrency.Any(db => db.AssetName == currencyItem.AssetId))
                    {
                        continue;
                    }

                    lock (relatedCrypto)
                    {
                        relatedCrypto.ChainAddresses.Add(dbChain);

                        crypto.TryTake(out relatedCrypto);
                        crypto.Add(relatedCrypto);
                    }
                }
            });
        }

        private async Task AddNewCurrenciesToDatabaseAsync(ConcurrentBag<CryptoCurrency> crypto, List<CryptoCurrency> dbCurrency, CancellationToken cancellationToken)
        {
            var cryptoToAdd = new ConcurrentBag<CryptoCurrency>();

            Parallel.ForEach(crypto, cryptoCurrency =>
            {
                if (!dbCurrency.Any(db => db.AssetName == cryptoCurrency.AssetName))
                {
                    cryptoToAdd.Add(cryptoCurrency);
                }
            });

            await dbcontext.Currencies.AddRangeAsync(cryptoToAdd, cancellationToken);
            await dbcontext.SaveChangesAsync(cancellationToken);
        }
    }
}
