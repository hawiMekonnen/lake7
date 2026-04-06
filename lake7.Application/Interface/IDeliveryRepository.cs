using lake7.Domain.Entities;

namespace lake7.Application.Interface
{
    public interface IDeliveryRepository
    {
        Task<Delivery> AddAsync(Delivery delivery);
        Task<IEnumerable<Delivery>> GetAllAsync();
        Task<Delivery?> GetByIdAsync(Guid id);
        Task<Delivery?> UpdateAsync(Delivery delivery);
    }
}
