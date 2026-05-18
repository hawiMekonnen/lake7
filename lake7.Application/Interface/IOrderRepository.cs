using lake7.Domain.Entities;
using lake7.Domain.Enums;

public interface IOrderRepository
{
    Task<Order> AddAsync(Order order);
    Task<Order?> GetByIdAsync(Guid id);
    Task<Order> UpdateAsync(Order order);
    Task<List<Order>> GetAllAsync();
    Task<List<Order>> GetByStatusAsync(OrderStatus status);
}

