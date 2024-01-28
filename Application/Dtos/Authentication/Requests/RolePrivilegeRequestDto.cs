using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Authentication.Requests
{
    public class RolePrivilegeRequestDto
    {
        [Required]
        public string RoleId { get; set; }
        [Required]
        public List<int> PagesIds { get; set; }
    }
}
