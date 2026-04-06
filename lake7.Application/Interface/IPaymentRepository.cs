using lake7.Domain.Entities;

namespace lake7.Application.Interface
{
    public interface IPaymentRepository
    {
        Task<Payment> AddAsync(Payment payment);
        Task<IEnumerable<Payment>> GetAllAsync();
        Task<Payment?> GetByIdAsync(Guid id);
        Task<Payment?> UpdateAsync(Payment payment);
    }
}
