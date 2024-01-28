using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Setup
{
    public class Module: BaseEntity
    {
        public string NameAr { get; set; }
        public string NameEn { get; set; }

        public virtual ICollection<UserModule> UserModules { get; set;}
        = new HashSet<UserModule>();
    }
}
