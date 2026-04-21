using lake7.Domain.Entities;
using lake7.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace lake7.Application.Interface
{
    public interface IPaymentService
    {
        Task<Payment> CreatePaymentAsync(Payment payment);
        Task<List<Payment>> GetAllPaymentsAsync();
        Task<Payment?> GetPaymentByIdAsync(Guid id);
        Task<Payment?> UpdatePaymentStatusAsync(Guid id, PaymentStatus status);
    }
}
