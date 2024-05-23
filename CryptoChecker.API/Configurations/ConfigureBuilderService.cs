using CryptoChecker.API.BackgroundServices;
using CryptoChecker.Application.DTO;
using CryptoChecker.Application.Intefraces;
using CryptoChecker.Application.Services;
using CryptoChecker.Infrastructure.Db;
using Microsoft.EntityFrameworkCore;

namespace CryptoChecker.API.Configurations
{
    public static class ConfigureBuilderService
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen();

            services.AddAuthorization();

            services.AddDb(configuration);

            services.AddOptions(configuration);
            services.AddRequestClients(configuration);

            services.AddOwnServices();
            services.AddBackgroundServices();

            return services;
        }

        public static IServiceCollection AddDb(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = bool.Parse(configuration["LocalDevelopment:IsLocal"]!) 
                ? configuration.GetConnectionString("DefaultConnection") 
                : configuration.GetConnectionString("DockerConnection");

            services.AddDbContext<CryptoCheckerDb>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            return services;
        }

        public static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<CoinApiOptions>(
               configuration.GetSection(CoinApiOptions.Position));

            return services;
        }

        public static IServiceCollection AddRequestClients(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient("CoinApi", client =>
            {
                client.BaseAddress = new Uri(configuration["CoinApiHttp:URL"]!);
                client.DefaultRequestHeaders.Add("X-CoinAPI-Key", configuration["CoinApiHttp:ApiKey"]!);
            });

            return services;
        }

        public static IServiceCollection AddOwnServices(this IServiceCollection services)
        {
            services.AddScoped<IChainAddressService, ChainAddressService>();
            services.AddScoped<ICryptoCurrencyService, CryptoCurrencyService>();
            services.AddScoped<ICryptoSymbolService, CryptoSymbolService>();
            services.AddScoped<IHistoricalPriceService, HistoricalPriceService>();

            services.AddScoped<ICoinRestApiService, CoinRestApiService>();
            services.AddSingleton<ICoinApiWebSocketClient, CoinApiWebSocketClient>();

            return services;
        }

        public static IServiceCollection AddBackgroundServices(this IServiceCollection services)
        {
            services.AddHostedService<CryptoHostedService>();

            return services;
        }
    }
}
