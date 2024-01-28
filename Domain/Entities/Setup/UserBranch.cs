using Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities.Setup
{
    public class UserBranch : BaseEntity
    {
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public int BranchId { get; set; }
        public Branch Branch { get; set; }
    }
}