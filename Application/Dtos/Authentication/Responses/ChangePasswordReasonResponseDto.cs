using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Authentication.Responses
{
    public record ChangePasswordReasonResponseDto(Guid Id, string ReasonAr, string ReasonEn);
}
