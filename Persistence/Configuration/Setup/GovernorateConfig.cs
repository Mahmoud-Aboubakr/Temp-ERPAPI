using Domain.Entities.Setup;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Persistence.Configurations
{
    public class GovernorateConfig : IEntityTypeConfiguration<Governorate>
    {
        public void Configure(EntityTypeBuilder<Governorate> builder)
        {
            builder
                .HasMany(c => c.Cities)
                .WithOne(g => g.Governorate)
                .HasForeignKey(g => g.GovernorateId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
