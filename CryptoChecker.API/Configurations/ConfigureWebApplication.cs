using CryptoChecker.Application.DTO;
using CryptoChecker.Application.Intefraces;
using CryptoChecker.Infrastructure.Db;

namespace CryptoChecker.API.Configurations
{
    public static class ConfigureWebApplication
    {
        public static WebApplication ConfigureApp(this WebApplication app)
        {

            var webSocketOptions = new WebSocketOptions
            {
                KeepAliveInterval = TimeSpan.FromMinutes(2)
            };

            app.UseWebSockets(webSocketOptions);

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.ConfigureEndpoints();

            app.CreateFirstDb();

            return app;
        }

        public static WebApplication CreateFirstDb(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<CryptoCheckerDb>();

                dbContext.Database.EnsureCreated();
            }

            return app;
        }

        public static IEndpointRouteBuilder ConfigureEndpoints(this IEndpointRouteBuilder builder)
        {
            builder.MapPost("/cryptocurrency", async (ICoinApiWebSocketClient websocketService, PostSocketRequest assets, CancellationToken cancellationToken) =>
            {
                websocketService.GetInformationToken(assets, cancellationToken).ConfigureAwait(false);
                return Results.Ok();
            });

            builder.MapGet("/cryptocurrency{pageNumber:int}", async (ICryptoCurrencyService cryptoCurrencyService, int pageNumber) =>
            {
                var result = await cryptoCurrencyService.GetListAsync(pageNumber);
                return Results.Ok(result);
            });

            builder.MapGet("/cryptocurrency{cryptoName:alpha}", async (ICoinRestApiService restApiService, string cryptoName, CancellationToken cancellationToken) =>
            {
                var result = await restApiService.GetHistoricPriceByCryptoNameAsync(cryptoName, cancellationToken);
                return Results.Ok(result);
            });


            return builder;
        }
    }
}
