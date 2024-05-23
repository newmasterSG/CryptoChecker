using CryptoChecker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CryptoChecker.Infrastructure.Db
{
    public class CryptoCheckerDb(DbContextOptions<CryptoCheckerDb> options) : DbContext(options)
    {
        public DbSet<CryptoCurrency> Currencies { get; set; }
        public DbSet<ChainAddress> Chains { get; set; }

        public DbSet<CryptoSymbol> CryptoSymbols { get; set; }

        public DbSet<HistoricalPrice> HistoricalPrices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
