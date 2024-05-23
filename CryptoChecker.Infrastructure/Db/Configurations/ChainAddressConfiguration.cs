using CryptoChecker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace CryptoChecker.Infrastructure.Db.Configurations
{
    public class ChainAddressConfiguration : IEntityTypeConfiguration<ChainAddress>
    {
        public void Configure(EntityTypeBuilder<ChainAddress> builder)
        {
            //By default ef understands that field with name 'Id' must be key, but I used to do manaul write
            builder.HasKey(x => x.Id);
        }
    }
}
