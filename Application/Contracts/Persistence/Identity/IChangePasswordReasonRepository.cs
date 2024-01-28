using Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Persistence.Identity
{
    public interface IChangePasswordReasonRepository
    {
        Task<ChangePasswordReason> AddAsync(ChangePasswordReason newReason);
        Task<ChangePasswordReason> UpdateAsync(ChangePasswordReason newReason);
        Task<ChangePasswordReason> GetByIdAsync(Guid reasonId);
        Task<IReadOnlyList<ChangePasswordReason>> GetAllAsync();
        Task DeleteAsync(ChangePasswordReason reason);
    }
}
