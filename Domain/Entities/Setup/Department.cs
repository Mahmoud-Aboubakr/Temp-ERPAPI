using Domain.Entities.Identity;
using Domain.Entities.Inventory.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Setup
{
    public class Department:BaseEntity
    {
        public string NameAr { get; set; }
        public string NameEn { get; set; }

        public virtual ICollection<AppUser> AppUsers { get; set; }
        = new HashSet<AppUser>();   
        public virtual ICollection<Store> Stores { get; set; }
    }
}
