using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Authentication.Requests
{
    public class ChangePasswordRequestDto
    {
        [Required]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$", ErrorMessage = "Password must have 1 Uppercase, 1 Lowercase, 1 number, 1 non alphanumeric and at least 8 characters.")]
        [StringLength(256, MinimumLength = 8, ErrorMessage = "Password must have 1 Uppercase, 1 Lowercase, 1 number, 1 non alphanumeric and at least 8 characters.")]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$", ErrorMessage = "Password must have 1 Uppercase, 1 Lowercase, 1 number, 1 non alphanumeric and at least 8 characters.")]
        [StringLength(256, MinimumLength = 8, ErrorMessage = "Password must have 1 Uppercase, 1 Lowercase, 1 number, 1 non alphanumeric and at least 8 characters.")]
        public string NewPassword { get; set; } = string.Empty;

        [Required]
        [Compare("NewPassword")]
        public string ConfirmNewPassword { get; set; } = string.Empty;

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid ReasonId { get; set; }
    }
}
