using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Authentication.Responses
{
    public class LoginResponseDto
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string AccessToken { get; set; }
    }
}
