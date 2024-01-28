using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities.Setup;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration.Setup
{
	public class UserConfig : IEntityTypeConfiguration<User>
	{
		public void Configure(EntityTypeBuilder<User> builder)
		{
			builder.HasIndex(user => user.Username).IsUnique();
			builder.HasIndex(user => user.PhoneNumber).IsUnique();
			builder.HasIndex(user => user.Email).IsUnique();

			//builder.HasOne(user => user.Employee)
			//.WithMany(employee => employee.Users)
			//.HasForeignKey(user => user.EmployeeId)
			//.OnDelete(DeleteBehavior.NoAction);

			//builder.HasOne(user => user.Nationality)
			//.WithMany(nat => nat.Users)
			//.HasForeignKey(user => user.NationalityId)
			//.OnDelete(DeleteBehavior.NoAction);

			//builder.HasOne(user => user.UserType)
			//.WithMany(nat => nat.Users)
			//.HasForeignKey(user => user.UserTypeId)
			//.OnDelete(DeleteBehavior.NoAction);
		}
	}
}