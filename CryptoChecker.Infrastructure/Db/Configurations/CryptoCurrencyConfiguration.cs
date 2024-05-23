using CryptoChecker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CryptoChecker.Infrastructure.Db.Configurations
{
    public class CryptoCurrencyConfiguration : IEntityTypeConfiguration<CryptoCurrency>
    {
        public void Configure(EntityTypeBuilder<CryptoCurrency> builder)
        {
            //By default ef understands that field with name 'Id' must be key, but I used to do manaul write
            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.AssetName).IsUnique();
        }
    }
}
