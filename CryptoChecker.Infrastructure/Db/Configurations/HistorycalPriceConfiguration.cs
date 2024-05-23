using CryptoChecker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CryptoChecker.Infrastructure.Db.Configurations
{
    public class HistorycalPriceConfiguration : IEntityTypeConfiguration<HistoricalPrice>
    {
        public void Configure(EntityTypeBuilder<HistoricalPrice> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(cs => cs.AskPrice)
                    .HasColumnType("decimal(38, 10)");

            builder.Property(cs => cs.BidPrice)
                   .HasColumnType("decimal(38, 10)");
        }
    }
}
