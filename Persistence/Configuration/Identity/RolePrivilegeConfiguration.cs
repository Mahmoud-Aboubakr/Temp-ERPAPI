using Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Configuration.Identity
{
    public class RolePrivilegeConfiguration : IEntityTypeConfiguration<AppUserRolePrivilege>
    {
        public void Configure(EntityTypeBuilder<AppUserRolePrivilege> builder)
        {
            builder.HasKey(x => new { x.AppUserRoleId, x.PageId });
        }
    }
}
