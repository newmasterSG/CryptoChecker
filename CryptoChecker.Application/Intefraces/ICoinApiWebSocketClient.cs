using CryptoChecker.Application.DTO;

namespace CryptoChecker.Application.Intefraces
{
    public interface ICoinApiWebSocketClient
    {
        Task GetInformationToken(PostSocketRequest assets, CancellationToken cancellationToken = default);
    }
}