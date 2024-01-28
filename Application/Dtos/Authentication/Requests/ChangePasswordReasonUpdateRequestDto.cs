using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Authentication.Requests
{
    public class ChangePasswordReasonUpdateRequestDto
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string ReasonAr { get; set; }
        [Required]
        public string ReasonEn { get; set; }
    }

}
