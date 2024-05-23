using CryptoChecker.Application.DTO.Responses;
using CryptoChecker.Application.Intefraces;
using CryptoChecker.Domain.Entities;
using CryptoChecker.Infrastructure.Db;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;

namespace CryptoChecker.Application.Services
{
    public class ChainAddressService(CryptoCheckerDb dbcontext) : IChainAddressService
    {
        public async Task AddListChainAsync(List<ChainAddressDTo> chains, CancellationToken cancellation = default)
        {
            var uniqueChainAddresses = new ConcurrentDictionary<string, ChainAddressDTo>();

            Parallel.ForEach(chains, chains =>
            {
                uniqueChainAddresses.TryAdd(chains.ChainId, chains);
            });

            foreach (var chainDto in uniqueChainAddresses.Values)
            {
                var existingChain = await dbcontext.Chains.FirstOrDefaultAsync(c => c.ChainName == chainDto.ChainId, cancellation);

                if (existingChain != null)
                {
                    continue;
                }

                var newChain = new ChainAddress
                {
                    ChainName = chainDto.ChainId,
                    Address = chainDto.Address,
                    NetworkId = chainDto.NetworkId,
                };

                await dbcontext.Chains.AddAsync(newChain, cancellation);
            }

            await dbcontext.SaveChangesAsync(cancellation);
        }
    }
}
