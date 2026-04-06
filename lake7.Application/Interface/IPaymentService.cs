using lake7.Domain.Entities;

namespace lake7.Application.Interface
{
    public interface IPaymentService
    {
        Task<Payment> ProcessPaymentAsync(Payment payment);
        Task<List<Payment>> GetAllPaymentsAsync();
        Task<Payment?> GetPaymentByIdAsync(Guid id);
        Task<Payment?> UpdatePaymentStatusAsync(Guid id, string status);
    }
}
