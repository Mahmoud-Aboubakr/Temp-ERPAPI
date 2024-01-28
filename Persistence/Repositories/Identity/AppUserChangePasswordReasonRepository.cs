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
    public class AppUserChangePasswordReasonRepository : IAppUserChangePasswordReasonRepository
    {
        private readonly ApplicationDbContext _context;

        public AppUserChangePasswordReasonRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<AppUserChangePasswordReason> AddAsync(AppUserChangePasswordReason userReason)
        {
            await _context.AppUserChangePasswordReasons.AddAsync(userReason);
            await _context.SaveChangesAsync();

            return userReason;
        }
    }
}
