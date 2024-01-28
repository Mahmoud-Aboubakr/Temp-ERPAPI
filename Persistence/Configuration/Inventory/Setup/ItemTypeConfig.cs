using Domain.Entities.Inventory.Setup;
using Domain.Entities.Setup;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Persistence.Configuration.Inventory.Setup
{
    public class ItemTypeConfig : IEntityTypeConfiguration<ItemType>
    {
        public void Configure(EntityTypeBuilder<ItemType> builder)
        {
            builder
                .HasMany(c => c.Items)
                .WithOne(g => g.ItemType)
                .HasForeignKey(g => g.ItemTypeId)
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
