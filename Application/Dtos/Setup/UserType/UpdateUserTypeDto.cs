
using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.UserType
{
    public class UpdateUserTypeDto
    {
        public int Id { get; set; }
        [Required]
        public string TypeName { get; set; }
        [Required]
        public string DescNameAr { get; set; }
        [Required]
        public string DescNameEn { get; set; }
        public string FullDesc { get; set; }
        public bool IsActive { get; set; }
    }
}
