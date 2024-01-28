using Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Persistence.Identity
{
    public interface IAppUserChangePasswordReasonRepository
    {
        Task<AppUserChangePasswordReason> AddAsync(AppUserChangePasswordReason userReason);
    }
}
