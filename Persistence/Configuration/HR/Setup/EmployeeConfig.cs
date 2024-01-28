using Domain.Entities.HR.Setup;
using Domain.Entities.Setup;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Persistence.Configurations
{
    public class EmployeeConfig : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder
            .HasMany(b => b.EmployeeFiles)
            .WithOne(c => c.Employee)
            .HasForeignKey(b => b.EmployeeId)
            .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
