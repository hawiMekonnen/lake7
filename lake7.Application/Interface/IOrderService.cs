using lake7.Application.DTOs;
using lake7.Domain.Entities;
using lake7.Domain.Enums;

namespace lake7.Application.Interface
{
    public interface IOrderService
    {
        Task<Order> PlaceDeliveryOrderAsync(Guid userId, PlaceDeliveryOrderDto dto);
        Task<Order?> GetOrderByIdAsync(Guid id);
        Task<List<Order>> GetAllOrdersAsync();
        Task<Order?> AssignDriverAsync(Guid orderId, Guid driverId);
        Task<Order?> UpdateOrderStatusAsync(Guid orderId, OrderStatus status);
        Task<List<Order>> GetOrdersByStatusAsync(OrderStatus status);
    }
}

