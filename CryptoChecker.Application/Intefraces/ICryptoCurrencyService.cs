using CryptoChecker.Application.DTO;
using CryptoChecker.Application.DTO.Responses;

namespace CryptoChecker.Application.Intefraces
{
    public interface ICryptoCurrencyService
    {
        Task AddListCurrencyAsync(List<CurrencyResponse> currency, CancellationToken cancellationToken = default);
        Task<GetCrypto> GetListAsync(int pageNumber = 1, int pageSize = 10);
    }
}