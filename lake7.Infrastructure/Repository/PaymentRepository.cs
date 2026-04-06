using lake7.Application.Interface;
using lake7.Domain.Entities;
using lake7.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace lake7.Infrastructure.Repository
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly Lake7DbContext _context;

        public PaymentRepository(Lake7DbContext context)
        {
            _context = context;
        }

        public async Task<Payment> AddAsync(Payment payment)
        {
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();
            return payment;
        }

        public async Task<IEnumerable<Payment>> GetAllAsync()
        {
            return await _context.Payments.ToListAsync();
        }

        public async Task<Payment?> GetByIdAsync(Guid id)
        {
            return await _context.Payments.FindAsync(id);
        }

        public async Task<Payment?> UpdateAsync(Payment payment)
        {
            _context.Payments.Update(payment);
            await _context.SaveChangesAsync();
            return payment;
        }
    }
}
