using Domain.Entities.Inventory.Setup;
using Domain.Entities.Setup;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Persistence.Configuration.Inventory.Setup
{
    public class ItemCategoryConfig : IEntityTypeConfiguration<ItemCategory>
    {
        public void Configure(EntityTypeBuilder<ItemCategory> builder)
        {
            builder
                .HasMany(c => c.Items)
                .WithOne(g => g.ItemCategory)
                .HasForeignKey(g => g.ItemCategoryId)
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
