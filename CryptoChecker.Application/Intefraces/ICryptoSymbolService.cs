using CryptoChecker.Application.DTO.Responses;

namespace CryptoChecker.Application.Intefraces
{
    public interface ICryptoSymbolService
    {
        Task AddListAsync(List<CryptoSymbolResponse> responses, CancellationToken cancellationToken = default);

        Task<List<string?>> GetByCryptoNameAsync(string cryptoName, CancellationToken cancellationToken = default);
    }
}