using CryptoChecker.API.BackgroundServices;
using CryptoChecker.API.Configurations;
using CryptoChecker.Application.DTO;
using CryptoChecker.Application.Intefraces;
using CryptoChecker.Infrastructure.Db;

namespace CryptoChecker.API
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            try
            {
                var builder = WebApplication.CreateBuilder(args);
        
                builder.Services.AddServices(builder.Configuration);

                var app = builder.Build().ConfigureApp();

                await app.RunAsync();
            }
            catch (Exception ex) when (
                                        ex.GetType().Name is not "StopTheHostException"
                                        && ex.GetType().Name is not "HostAbortedException"
                                    )
            {
                Console.WriteLine(ex);
            }
        }
    }
}
