using lake7.Application.DTOs;
using lake7.Domain.Entities;

namespace lake7.Application.Interface
{
    public interface IOrderService
    {
        Task<Order> PlaceDeliveryOrderAsync(Guid userId, PlaceDeliveryOrderDto dto);
        Task<Order?> GetOrderByIdAsync(Guid id);
        Task<List<Order>> GetAllOrdersAsync();
    }
}
