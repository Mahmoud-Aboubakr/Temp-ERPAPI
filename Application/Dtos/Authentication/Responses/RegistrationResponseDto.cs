using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Authentication.Responses
{
    public class RegistrationResponseDto
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string FullNameAr { get; set; }
        public string FullNameEn { get; set; }
    }
}
