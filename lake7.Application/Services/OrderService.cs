using lake7.Application.DTOs;
using lake7.Application.Interface;
using lake7.Domain.Entities;
using lake7.Domain.Enums;

namespace lake7.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDeliveryRepository _deliveryRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IPaymentService _paymentService;

        public OrderService(
            IUnitOfWork unitOfWork,
            IDeliveryRepository deliveryRepository,
            IOrderRepository orderRepository,
            IPaymentService paymentService)
        {
            _unitOfWork = unitOfWork;
            _deliveryRepository = deliveryRepository;
            _orderRepository = orderRepository;
            _paymentService = paymentService;
        }

        public async Task<Order> PlaceDeliveryOrderAsync(Guid userId, PlaceDeliveryOrderDto dto)
        {
            // 1. Start Transaction
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                // 2. Create Delivery
                var delivery = new Delivery
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    SenderName = dto.SenderName,
                    SenderPhone = dto.SenderPhone,
                    ReceiverName = dto.ReceiverName,
                    ReceiverPhone = dto.ReceiverPhone,
                    PickupAddress = dto.PickupAddress,
                    DropoffAddress = dto.DropoffAddress,
                    PickupLatitude = dto.PickupLatitude,
                    PickupLongitude = dto.PickupLongitude,
                    DropoffLatitude = dto.DropoffLatitude,
                    DropoffLongitude = dto.DropoffLongitude,
                    ItemDescription = dto.ItemDescription,
                    Status = RideStatus.Pending,
                    RequestedAt = DateTime.UtcNow
                };

                var newDelivery = await _deliveryRepository.AddAsync(delivery);

                // 3. Create Order
                var order = new Order
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    DeliveryId = newDelivery.Id,
                    TotalAmount = dto.PaymentAmount,
                    Status = OrderStatus.Pending,
                    CreatedAt = DateTime.UtcNow
                };

                var newOrder = await _orderRepository.AddAsync(order);

                // 4. Process Payment
                // Since PaymentService.ProcessPaymentAsync saves to DB, 
                // it will participate in the current transaction.
                var payment = await _paymentService.ProcessPaymentAsync(
                    userId, 
                    newOrder.Id,
                    null, // rideId
                    dto.PaymentAmount, 
                    dto.PaymentMethod
                );
                
                // 5. Finalize status

                if (payment.Status == PaymentStatus.Completed)
                {
                    newOrder.Status = OrderStatus.Completed;
                    newDelivery.Status = RideStatus.Accepted; 
                }
                else
                {
                    newOrder.Status = OrderStatus.Cancelled; 
                    newDelivery.Status = RideStatus.Cancelled;
                }

                await _orderRepository.UpdateAsync(newOrder);
                await _deliveryRepository.UpdateAsync(newDelivery);

                // 6. Commit Transaction
                await _unitOfWork.CommitTransactionAsync();

                return newOrder;
            }
            catch (Exception)
            {
                // 7. Rollback on error
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<Order?> GetOrderByIdAsync(Guid id)
        {
            return await _orderRepository.GetByIdAsync(id);
        }

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            // Note: Ideally IOrderRepository should have GetAllAsync
            // For now return empty or implement if needed
            return new List<Order>();
        }
    }
}
