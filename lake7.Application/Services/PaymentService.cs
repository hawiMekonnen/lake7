using lake7.Domain.Entities;
using lake7.Application.Interface;

namespace lake7.Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentService(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task<Payment> ProcessPaymentAsync(Payment payment)
        {
            payment.Status = "Processed";
            payment.CreatedAt = DateTime.UtcNow;
            return await _paymentRepository.AddAsync(payment);
        }

        public async Task<List<Payment>> GetAllPaymentsAsync()
        {
            return (await _paymentRepository.GetAllAsync()).ToList();
        }

        public async Task<Payment?> GetPaymentByIdAsync(Guid id)
        {
            return await _paymentRepository.GetByIdAsync(id);
        }

        public async Task<Payment?> UpdatePaymentStatusAsync(Guid id, string status)
        {
            var payment = await _paymentRepository.GetByIdAsync(id);
            if (payment == null) return null;

            payment.Status = status;
            payment.UpdatedAt = DateTime.UtcNow;
            return await _paymentRepository.UpdateAsync(payment);
        }
    }
}
