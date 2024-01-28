using Application.Contracts.Persistence.Identity;
using Domain.Entities.Identity;
using Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories.Identity
{
    public class RolePrivilegeRepository : IRolePrivilegeRepository
    {
        private readonly ApplicationDbContext _context;

        public RolePrivilegeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(string roleId, List<int> PageIds)
        {
            var rolePrivileges = new List<AppUserRolePrivilege>();

            foreach (var pageId in PageIds)
            {
                rolePrivileges.Add(new AppUserRolePrivilege
                {
                    AppUserRoleId = roleId,
                    PageId = pageId
                });
            };

            await _context.AppUserRolePrivileges.AddRangeAsync(rolePrivileges);
            await _context.SaveChangesAsync();
        }
    }
}
