using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Authentication.Responses
{
    public record AppUserRoleResponseDto(string Id, string Name, string DescriptionAr, string DescriptionEn,
        string FulDescription);
}
