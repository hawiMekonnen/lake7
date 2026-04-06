using lake7.Application.Interface;
using lake7.Domain.Entities;
using lake7.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace lake7.Infrastructure.Repository
{
    public class RideRepository : IRideRepository
    {
        private readonly Lake7DbContext _context;

        public RideRepository(Lake7DbContext context)
        {
            _context = context;
        }

        public async Task<Ride> AddAsync(Ride ride)
        {
            _context.Rides.Add(ride);
            await _context.SaveChangesAsync();
            return ride;
        }

        public async Task<IEnumerable<Ride>> GetAllAsync()
        {
            return await _context.Rides.ToListAsync();
        }

        public async Task<Ride?> GetByIdAsync(Guid id)
        {
            return await _context.Rides.FindAsync(id);
        }

        public async Task<Ride?> UpdateAsync(Ride ride)
        {
            _context.Rides.Update(ride);
            await _context.SaveChangesAsync();
            return ride;
        }
    }
}
