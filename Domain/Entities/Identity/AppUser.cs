using Domain.Entities.Setup;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Identity
{
    public class AppUser : IdentityUser
    {
        public override string PhoneNumber { get => base.PhoneNumber; set => base.PhoneNumber = value; }
        public int EmployeeId { get; set; }
        public string FullNameAR { get; set; }
        public string FullNameEn { get; set; }
        public Gender Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public int? NationalityId { get; set; }
        public int? DepartmentId { get; set; }
        public int? UserTypeId { get; set; }
        public UserStatus? Status { get; set; }
        public int? DefaultModule { get; set; }
        public Language? Language { get; set; }
        public bool HasDiscount { get; set; } = false;
        public double? DiscountPercentage { get; set; }
        public double? DiscountLimit { get; set; }

        public virtual ICollection<AppUserChangePasswordReason> UsersReasons { get; set; }
        = new HashSet<AppUserChangePasswordReason>();
        public virtual ICollection<UserBranch> UserBranches { get; set; }
        = new HashSet<UserBranch>();
        public virtual ICollection<UserModule> UserModules { get; set; }
        = new HashSet<UserModule>();
        public virtual Department Department { get; set; }  
    }
}
