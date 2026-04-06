using lake7.Application.Interface;
using lake7.Domain.Entities;
using lake7.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace lake7.Infrastructure.Repository
{
    public class DriverRepository : IDriverRepository
    {
        private readonly Lake7DbContext _context;

        public DriverRepository(Lake7DbContext context)
        {
            _context = context;
        }

        public async Task<Driver> AddAsync(Driver driver)
        {
            _context.Drivers.Add(driver);
            await _context.SaveChangesAsync();
            return driver;
        }

        public async Task<IEnumerable<Driver>> GetAllAsync()
        {
            return await _context.Drivers.ToListAsync();
        }

        public async Task<Driver?> GetByIdAsync(Guid id)
        {
            return await _context.Drivers.FindAsync(id);
        }

        public async Task<Driver?> UpdateAsync(Driver driver)
        {
            _context.Drivers.Update(driver);
            await _context.SaveChangesAsync();
            return driver;
        }
    }
}
