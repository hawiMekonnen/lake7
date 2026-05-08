using lake7.Application.Interface;
using lake7.Domain.Entities;
using lake7.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace lake7.Infrastructure.Repository
{
    public class AdminRepository : IAdminRepository
    {
        private readonly Lake7DbContext _context;

        public AdminRepository(Lake7DbContext context)
        {
            _context = context;
        }

        public async Task<AdminAccount?> GetByEmailAsync(string email)
        {
            return await _context.AdminAccounts.FirstOrDefaultAsync(a => a.Email == email);
        }

        public async Task<AdminAccount?> GetByIdAsync(Guid id)
        {
            return await _context.AdminAccounts.FindAsync(id);
        }

        public async Task<AdminAccount> CreateAsync(AdminAccount admin)
        {
            await _context.AdminAccounts.AddAsync(admin);
            await _context.SaveChangesAsync();
            return admin;
        }

        public async Task<List<AdminAccount>> GetAllAsync()
        {
            return await _context.AdminAccounts.ToListAsync();
        }
    }
}
