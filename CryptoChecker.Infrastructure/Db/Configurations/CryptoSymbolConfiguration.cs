using CryptoChecker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CryptoChecker.Infrastructure.Db.Configurations
{
    public class CryptoSymbolConfiguration : IEntityTypeConfiguration<CryptoSymbol>
    {
        public void Configure(EntityTypeBuilder<CryptoSymbol> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(cs => cs.Price)
                    .HasColumnType("decimal(38, 10)");
        }
    }
}
