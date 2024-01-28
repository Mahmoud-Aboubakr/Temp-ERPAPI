using Domain.Entities.Inventory.Setup;
using Domain.Entities.Setup;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Persistence.Configuration.Inventory.Setup
{
    public class ItemClassificationConfig : IEntityTypeConfiguration<ItemClassification>
    {
        public void Configure(EntityTypeBuilder<ItemClassification> builder)
        {
            builder
                .HasMany(c => c.Items)
                .WithOne(g => g.ItemClassification)
                .HasForeignKey(g => g.ItemClassificationId)
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
