using Application.Dtos.Authentication.Responses;
using Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Persistence.Identity
{
    public interface IAppUserRoleRepository
    {
        Task<IReadOnlyList<AppUserRole>> GetAllAppUserRolesPaginatedAsync(int pageIndex,
            int pageSize);
        Task<int> GetAllAppuserRolesCountAsync();
        Task<AppUserRole> GetRoleWithPrivilegesAsync(string roleId);
    }
}
