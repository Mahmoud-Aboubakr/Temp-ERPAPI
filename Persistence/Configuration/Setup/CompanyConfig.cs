using Domain.Entities.Setup;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Persistence.Configurations
{
    public class CompanyConfig : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder
            .HasMany(b => b.Branches)
            .WithOne(c => c.Company)
            .HasForeignKey(b => b.CompanyId)
            .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
