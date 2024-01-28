using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Identity
{
    public class ChangePasswordReason
    {
        public Guid Id { get; set; }
        public string ReasonAr { get; set; }
        public string ReasonEn { get; set; }

        public virtual ICollection<AppUserChangePasswordReason> UsersReasons { get; set; }
            = new HashSet<AppUserChangePasswordReason>();
    }
}
