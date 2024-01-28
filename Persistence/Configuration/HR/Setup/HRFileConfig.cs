using Domain.Entities.HR.Setup;
using Domain.Entities.Setup;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Persistence.Configurations
{
    public class HRFileConfig : IEntityTypeConfiguration<HRFile>
    {
        public void Configure(EntityTypeBuilder<HRFile> builder)
        {
            builder
            .HasMany(b => b.EmployeeFiles)
            .WithOne(c => c.HRFile)
            .HasForeignKey(b => b.HRFileId)
            .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
