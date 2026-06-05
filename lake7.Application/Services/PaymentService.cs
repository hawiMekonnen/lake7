using lake7.Application.Interface;
using lake7.Domain.Entities;
using lake7.Domain.Enums;
using Microsoft.Extensions.Logging;

public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly ILogger<PaymentService> _logger;

    public PaymentService(IPaymentRepository paymentRepository, ILogger<PaymentService> logger)
    {
        _paymentRepository = paymentRepository;
        _logger = logger;
    }

    // Main payment processing (simulate gateway)
    public async Task<Payment> ProcessPaymentAsync(Guid userId, Guid? orderId, Guid? rideId, decimal amount, string method)
    {
        var payment = new Payment
        {
            UserId = userId,
            OrderId = orderId,
            RideId = rideId,
            Amount = amount,
            Method = method,
            Status = PaymentStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        try
        {
            // simulate external gateway call
            payment.Status = PaymentStatus.Completed;
            payment.UpdatedAt = DateTime.UtcNow;
            return await _paymentRepository.AddAsync(payment);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Payment failed");
            payment.Status = PaymentStatus.Failed;
            payment.UpdatedAt = DateTime.UtcNow;
            return await _paymentRepository.AddAsync(payment);
        }
    }

    public async Task<Payment?> GetPaymentByIdAsync(Guid id)
    {
        return await _paymentRepository.GetByIdAsync(id);
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

    public async Task<Payment?> UpdatePaymentStatusAsync(Guid id, PaymentStatus status)
    {
        var payment = await _paymentRepository.GetByIdAsync(id);
        if (payment == null) return null;

        payment.Status = status;
        payment.UpdatedAt = DateTime.UtcNow;
        return await _paymentRepository.UpdateAsync(payment);
    }
}

