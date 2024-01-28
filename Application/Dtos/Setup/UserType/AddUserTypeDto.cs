
using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.UserType
{
    public class AddUserTypeDto
    {
        [Required]
        public string TypeName { get; set; }
        [Required]
        public string DescNameAr { get; set; }
        [Required]
        public string DescNameEn { get; set; }
        public string FullDesc { get; set; }
    }
}
