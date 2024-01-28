using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Authentication.Requests
{
    public class RegistrationRequestDto
    {
        [Required]
        public string UserName {  get; set; }
        [Required]
        public int EmployeeId { get; set; }
        [Required]
        public string FullNameAR { get; set; }
        [Required]
        public string FullNameEn { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public int? NationalityId { get; set; }
        public int? DepartmentId { get; set; }
        public int? UserTypeId { get; set; }
        
        [Required]
        public List<int> Branches { get; set; }
        [Required]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$", ErrorMessage = "Password must have 1 Uppercase, 1 Lowercase, 1 number, 1 non alphanumeric and at least 8 characters.")]
        [StringLength(256, MinimumLength = 8, ErrorMessage = "Password must have 1 Uppercase, 1 Lowercase, 1 number, 1 non alphanumeric and at least 8 characters.")]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        public string? Status { get; set; }
        public int? DefaultModule { get; set; }
        public string? Language { get; set; }
        public bool HasDiscount { get; set; } = false;
        public double? DiscountPercentage { get; set; }
        public double? DiscountLimit { get; set; }

        [Required]
        public List<string> Roles { get; set; }
    }
}
