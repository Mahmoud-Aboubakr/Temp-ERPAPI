using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Authentication.Responses
{
    public record AppUserResponseDto(string Id, string UserName, string Email, string FullName);
}
