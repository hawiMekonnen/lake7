using lake7.Domain.Entities;
using lake7.Domain.Enums;
using lake7.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

public class OrderRepository : IOrderRepository
{
    private readonly Lake7DbContext _context;

    public OrderRepository(Lake7DbContext context)
    {
        _context = context;
    }

    public async Task<Order> AddAsync(Order order)
    {
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
        return order;
    }

    public async Task<Order?> GetByIdAsync(Guid id)
    {
        return await _context.Orders
            .Include(o => o.User)
            .Include(o => o.Ride)
            .Include(o => o.Delivery)
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<Order> UpdateAsync(Order order)
    {
        _context.Orders.Update(order);
        await _context.SaveChangesAsync();
        return order;
    }

    public async Task<List<Order>> GetAllAsync()
    {
        return await _context.Orders
            .Include(o => o.User)
            .Include(o => o.Ride)
            .Include(o => o.Delivery)
            .ToListAsync();
    }

    public async Task<List<Order>> GetByStatusAsync(OrderStatus status)
    {
        return await _context.Orders
            .Include(o => o.User)
            .Include(o => o.Ride)
            .Include(o => o.Delivery)
            .Where(o => o.Status == status)
            .ToListAsync();
    }
}

