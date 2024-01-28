using Domain.Entities.Cashier.Setup;
using Domain.Entities.Inventory.Setup;
using Domain.Entities.Setup;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Persistence.Configuration.Inventory.Setup
{
    public class PaymentModesTypeConfig : IEntityTypeConfiguration<PaymentModesType>
    {
        public void Configure(EntityTypeBuilder<PaymentModesType> builder)
        {
            builder
                .HasMany(c => c.PaymentModes)
                .WithOne(g => g.PaymentModesType)
                .HasForeignKey(g => g.PaymentModesTypeId)
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
