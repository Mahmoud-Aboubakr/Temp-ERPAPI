using Application.Contracts.Persistence.Identity;
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
    public class ChangePasswordReasonRepository : IChangePasswordReasonRepository
    {
        private readonly ApplicationDbContext _context;

        public ChangePasswordReasonRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ChangePasswordReason> AddAsync(ChangePasswordReason newReason)
        {
            await _context.ChangePasswordReasons.AddAsync(newReason);
            await _context.SaveChangesAsync();

            return newReason;
        }

        public async Task DeleteAsync(ChangePasswordReason reason)
        {
            _context.ChangePasswordReasons.Remove(reason);
            await _context.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<ChangePasswordReason>> GetAllAsync()
        {
            return await _context.ChangePasswordReasons.ToListAsync();
        }

        public async Task<ChangePasswordReason> GetByIdAsync(Guid reasonId)
        {
            return await _context.ChangePasswordReasons.SingleOrDefaultAsync(x => x.Id == reasonId);
        }

        public async Task<ChangePasswordReason> UpdateAsync(ChangePasswordReason newReason)
        {
            _context.ChangePasswordReasons.Update(newReason);
            await _context.SaveChangesAsync();

            return newReason;
        }
    }
}
