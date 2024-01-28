using Domain.Entities.Setup;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration.Setup
{
	public class UserBranchConfig : IEntityTypeConfiguration<UserBranch>
	{
		public void Configure(EntityTypeBuilder<UserBranch> builder)
		{
			builder.HasKey(ub => new { ub.AppUserId, ub.BranchId });

			//builder.HasOne(ub => ub.User)
			//.WithMany(ub => ub.Branches)
			//.HasForeignKey(ub => ub.UserId)
			//.OnDelete(DeleteBehavior.NoAction);

			builder.HasOne(ub => ub.Branch)
			.WithMany(ub => ub.UserBranches)
			.HasForeignKey(ub => ub.BranchId)
			.OnDelete(DeleteBehavior.NoAction);
		}
	}
}