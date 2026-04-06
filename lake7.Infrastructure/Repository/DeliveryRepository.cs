using lake7.Application.Interface;
using lake7.Domain.Entities;
using lake7.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace lake7.Infrastructure.Repository
{
    public class DeliveryRepository : IDeliveryRepository
    {
        private readonly Lake7DbContext _context;

        public DeliveryRepository(Lake7DbContext context)
        {
            _context = context;
        }

        public async Task<Delivery> AddAsync(Delivery delivery)
        {
            _context.Deliveries.Add(delivery);
            await _context.SaveChangesAsync();
            return delivery;
        }

        public async Task<IEnumerable<Delivery>> GetAllAsync()
        {
            return await _context.Deliveries.ToListAsync();
        }

        public async Task<Delivery?> GetByIdAsync(Guid id)
        {
            return await _context.Deliveries.FindAsync(id);
        }

        public async Task<Delivery?> UpdateAsync(Delivery delivery)
        {
            _context.Deliveries.Update(delivery);
            await _context.SaveChangesAsync();
            return delivery;
        }
    }
}
