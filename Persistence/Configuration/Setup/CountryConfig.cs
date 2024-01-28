using Domain.Entities.Setup;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Persistence.Configurations
{
    public class CountryConfig : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder
                .HasMany(c => c.Governorates)
                .WithOne(g => g.Country)
                .HasForeignKey(g => g.CountryId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasMany(c => c.Currencies)
                .WithOne(g => g.Country)
                .HasForeignKey(g => g.CountryId)
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
