using CryptoChecker.Application.DTO;
using CryptoChecker.Application.DTO.Responses;

namespace CryptoChecker.Application.Intefraces
{
    public interface IHistoricalPriceService
    {
        Task<List<HistoricalPriceDto>> AddListAsync(List<CryptoQuote> response, CancellationToken cancellationToken = default);
    }
}