using Domain.Entities.Inventory;
using Domain.Entities.Setup;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Persistence.Configurations
{
    public class CurrencyConfig : IEntityTypeConfiguration<CurrencySetup>
    {
        public void Configure(EntityTypeBuilder<CurrencySetup> builder)
        {
            builder
                .HasMany(c => c.Wholesales)
                .WithOne(g => g.WholesaleCurrency)
                .HasForeignKey(g => g.WholesaleCurrencyId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasMany(c => c.Retails)
                .WithOne(g => g.RetailCurrency)
                .HasForeignKey(g => g.RetailCurrencyId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasMany(c => c.CostPrices)
                .WithOne(g => g.CostPriceCurrency)
                .HasForeignKey(g => g.CostPriceCurrencyId)
                .OnDelete(DeleteBehavior.NoAction);
            builder
               .Property(x => x.Rate)
               .HasColumnType("decimal(18,3)");
        }
    }
}
