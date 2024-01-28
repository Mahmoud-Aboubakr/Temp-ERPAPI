using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Identity
{
    public class AppUserChangePasswordReason
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ReasonId { get; set; }

        public virtual AppUser AppUser { get; set; }    
        public virtual ChangePasswordReason ChangePasswordReason { get; set; }
    }
}
