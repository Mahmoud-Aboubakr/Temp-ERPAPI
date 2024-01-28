using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Authentication.Requests
{
    public class AppUserRoleUpdateRequestDto
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string DescriptionAr { get; set; }
        [Required]
        public string DescriptionEn { get; set; }
        
        public string FulDescription { get; set; }
    }
}
