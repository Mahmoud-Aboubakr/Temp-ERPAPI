using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Identity
{
    public class AppUserRole : IdentityRole
    {
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
        public string FullDescription { get; set; }

        public virtual ICollection<AppUserRolePrivilege> RolePrivileges { get; set; }
        = new List<AppUserRolePrivilege>();
    }
}
