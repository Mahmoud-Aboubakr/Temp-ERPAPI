using Domain.Entities.Identity;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Configuration.Identity
{
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.Property(x => x.Gender)
                .HasConversion(v => v.ToString(),
                    v => (Gender)Enum.Parse(typeof(Gender), v));

            builder.Property(x => x.Status)
                .HasConversion(v => v.ToString(),
                    v => (UserStatus)Enum.Parse(typeof(UserStatus), v));

            builder.Property(x => x.Language)
                .HasConversion(v => v.ToString(),
                    v => (Language)Enum.Parse(typeof(Language), v));
        }
    }
}
