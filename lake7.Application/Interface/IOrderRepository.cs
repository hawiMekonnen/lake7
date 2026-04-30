using lake7.Domain.Entities;

public interface IOrderRepository
{
    Task<Order> AddAsync(Order order);
    Task<Order?> GetByIdAsync(Guid id);
    Task<Order> UpdateAsync(Order order);
}
