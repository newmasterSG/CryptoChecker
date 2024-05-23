
using CryptoChecker.Application.Intefraces;
using CryptoChecker.Application.Services;

namespace CryptoChecker.API.BackgroundServices
{
    public class CryptoHostedService(IServiceProvider services) : BackgroundService
    {
        public IServiceProvider Services { get; } = services;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await DoWork(stoppingToken);

                // Wait for 3 days before running the next cycle
                await Task.Delay(TimeSpan.FromDays(3), stoppingToken);
            }
        }

        private async Task DoWork(CancellationToken stoppingToken)
        {
            using (var scope = Services.CreateScope())
            {
                var scopedProcessingService =
                    scope.ServiceProvider
                        .GetRequiredService<ICoinRestApiService>();

                await scopedProcessingService.GetAllCryptoAsync(stoppingToken);

                await scopedProcessingService.GetSymbolsAsync(stoppingToken);
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            await base.StopAsync(stoppingToken);
        }
    }
}
