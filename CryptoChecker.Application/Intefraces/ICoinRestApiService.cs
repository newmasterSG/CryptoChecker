using CryptoChecker.Application.DTO;

namespace CryptoChecker.Application.Intefraces
{
    public interface ICoinRestApiService
    {
        Task GetAllCryptoAsync(CancellationToken cancellation = default);

        Task GetSymbolsAsync(CancellationToken cancellation = default);

        Task<List<HistoricalPriceDto>> GetHistoricPriceByCryptoNameAsync(string name, CancellationToken cancellationToken = default);
    }
}