using CryptoChecker.Application.DTO.Responses;

namespace CryptoChecker.Application.Intefraces
{
    public interface IChainAddressService
    {
        Task AddListChainAsync(List<ChainAddressDTo> chains, CancellationToken cancellation = default);
    }
}