using Application.Contracts.Persistence.Identity;
using Application.Dtos.Authentication.Responses;
using Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories.Identity
{
    public class AppUserRoleRepository : IAppUserRoleRepository
    {
        private readonly ApplicationDbContext _context;

        public AppUserRoleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> GetAllAppuserRolesCountAsync()
        {
            return await _context.Roles.CountAsync();
        }

        public async Task<IReadOnlyList<AppUserRole>> GetAllAppUserRolesPaginatedAsync(
            int pageIndex = 1, int pageSize = 25)
        {
            pageSize = pageSize > 500 ? 500 : pageSize;

            return await _context.Roles
                 .Skip((pageIndex - 1) * pageSize)
                 .Take(pageSize)
                 .ToListAsync();
        }

        public async Task<AppUserRole> GetRoleWithPrivilegesAsync(string roleId)
        {
            return await _context.Roles
                .Include(x => x.RolePrivileges)
                .FirstOrDefaultAsync(x => x.Id == roleId);
        }
    }
}
