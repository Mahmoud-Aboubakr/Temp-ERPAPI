using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Identity
{
    public class AppUserRolePrivilege
    {
        public string AppUserRoleId { get; set; }
        public int PageId { get; set; }

        public virtual AppUserRole AppUserRole { get; set; }
    }
}
