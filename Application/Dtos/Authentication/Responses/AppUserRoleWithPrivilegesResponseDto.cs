using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Authentication.Responses
{
    public class AppUserRoleWithPrivilegesResponseDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
        public string FulDescription { get; set; }  

        public IReadOnlyList<int> RolePrivileges { get; set; }
    }
}
