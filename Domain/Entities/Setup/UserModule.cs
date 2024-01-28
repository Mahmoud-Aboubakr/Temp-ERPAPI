using Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Setup
{
    public class UserModule: BaseEntity
    {
        public string AppUserId { get; set; }
        public int ModuleId { get; set; }

        public virtual AppUser AppUser { get; set; }
        public virtual Module Module { get; set; }  
    }
}
