using lake7.Application.DTOs;
using lake7.Application.Interface;
using lake7.Domain.Entities;
using lake7.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace lake7.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDeliveryRepository _deliveryRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IPaymentService _paymentService;
        private readonly INotificationService _notificationService;
        private readonly IDriverLocationRepository _driverLocationRepository;
        private readonly IDriverRepository _driverRepository;

        public OrderService(
            IUnitOfWork unitOfWork,
            IDeliveryRepository deliveryRepository,
            IOrderRepository orderRepository,
            IPaymentService paymentService,
            INotificationService notificationService,
            IDriverLocationRepository driverLocationRepository,
            IDriverRepository driverRepository)
        {
            _unitOfWork = unitOfWork;
            _deliveryRepository = deliveryRepository;
            _orderRepository = orderRepository;
            _paymentService = paymentService;
            _notificationService = notificationService;
            _driverLocationRepository = driverLocationRepository;
            _driverRepository = driverRepository;
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
                    newOrder.Status = OrderStatus.Confirmed;
                    newDelivery.Status = RideStatus.Pending; 
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

                // Notify Restaurant
                await _notificationService.NotifyOrderCreatedAsync(newOrder);

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
            return await _orderRepository.GetAllAsync();
        }

        public async Task<Order?> AssignDriverAsync(Guid orderId, Guid driverId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null) return null;

            if (order.DeliveryId.HasValue)
            {
                var delivery = await _deliveryRepository.GetByIdAsync(order.DeliveryId.Value);
                if (delivery != null)
                {
                    delivery.DriverId = driverId;
                    delivery.Status = RideStatus.Pending;
                    await _deliveryRepository.UpdateAsync(delivery);
                }
            }

            order.Status = OrderStatus.Prepared;
            await _orderRepository.UpdateAsync(order);

            // Notify Driver
            await _notificationService.NotifyOrderAssignedAsync(driverId, order);
            
            // Notify User
            await _notificationService.NotifyOrderStatusChangedAsync(order.UserId, order.Status.ToString());

            return order;
        }

        public async Task<Order?> UpdateOrderStatusAsync(Guid orderId, OrderStatus status)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null) return null;

            order.Status = status;
            
            // Sync with delivery status if applicable
            if (order.DeliveryId.HasValue)
            {
                var delivery = await _deliveryRepository.GetByIdAsync(order.DeliveryId.Value);
                if (delivery != null)
                {
                    if (status == OrderStatus.OutForDelivery) delivery.Status = RideStatus.Accepted;
                    else if (status == OrderStatus.Delivered) delivery.Status = RideStatus.Completed;
                    else if (status == OrderStatus.Cancelled) delivery.Status = RideStatus.Cancelled;
                    
                    await _deliveryRepository.UpdateAsync(delivery);
                }
            }

            await _orderRepository.UpdateAsync(order);
            
            // Automatically assign nearby cyclist if order is Received
            if (status == OrderStatus.Received && order.DeliveryId.HasValue)
            {
                var delivery = await _deliveryRepository.GetByIdAsync(order.DeliveryId.Value);
                if (delivery != null)
                {
                    // Find nearby cyclists (Delivery vehicle type)
                    var nearbyDrivers = await _driverLocationRepository.GetNearbyDriversAsync(delivery.PickupLatitude, delivery.PickupLongitude, 10.0); // 10km radius
                    var cyclist = nearbyDrivers.FirstOrDefault(d => d.VehicleType.ToLower() == "delivery");

                    if (cyclist != null)
                    {
                        delivery.DriverId = cyclist.DriverId;
                        delivery.Status = RideStatus.Pending;
                        await _deliveryRepository.UpdateAsync(delivery);

                        // Notify Driver
                        await _notificationService.NotifyOrderAssignedAsync(cyclist.DriverId, order);

                        // Fetch Driver Profile for User Notification
                        var driverInfo = await _driverRepository.GetByIdAsync(cyclist.DriverId);
                        if (driverInfo != null)
                        {
                            var driverData = new
                            {
                                type = "OrderDriverAssigned",
                                driverName = driverInfo.Name,
                                driverPhoneNumber = driverInfo.PhoneNumber,
                                driverVehicleInfo = driverInfo.VehicleInfo,
                                driverLicensePlate = driverInfo.LicensePlate,
                                orderId = order.Id
                            };
                            await _notificationService.NotifyUserAsync(order.UserId, driverData);
                        }
                    }
                }
            }

            // Notify User
            await _notificationService.NotifyOrderStatusChangedAsync(order.UserId, order.Status.ToString());

            // Notify Drivers if Order is Prepared
            if (status == OrderStatus.Prepared)
            {
                await _notificationService.NotifyOrderPreparedAsync(order);
            }

            return order;
        }

        public async Task<List<Order>> GetOrdersByStatusAsync(OrderStatus status)
        {
            return await _orderRepository.GetByStatusAsync(status);
        }

        public async Task<Order?> GetActiveOrderByDriverIdAsync(Guid driverId)
        {
            var orders = await _orderRepository.GetAllAsync();
            return orders.FirstOrDefault(o => o.Delivery?.DriverId == driverId && 
                                              o.Status != OrderStatus.Completed && 
                                              o.Status != OrderStatus.Cancelled &&
                                              o.Status != OrderStatus.Delivered);
        }
    }
}

