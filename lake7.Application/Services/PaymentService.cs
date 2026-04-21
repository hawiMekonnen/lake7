using lake7.Application.Interface;
using lake7.Domain.Entities;
using lake7.Domain.Enums;

namespace lake7.Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentService(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task<Payment> CreatePaymentAsync(Payment payment)
        {
            payment.Status = PaymentStatus.Pending;
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

        public async Task<Payment?> UpdatePaymentStatusAsync(Guid id, PaymentStatus status)
        {
            var payment = await _paymentRepository.GetByIdAsync(id);
            if (payment == null) return null;

            payment.Status = status;
            return await _paymentRepository.UpdateAsync(payment);
        }
    }
}
