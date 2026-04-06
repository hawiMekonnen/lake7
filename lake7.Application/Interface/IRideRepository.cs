using lake7.Domain.Entities;

namespace lake7.Application.Interface
{
    public interface IRideRepository
    {
        Task<Ride> AddAsync(Ride ride);
        Task<IEnumerable<Ride>> GetAllAsync();
        Task<Ride?> GetByIdAsync(Guid id);
        Task<Ride?> UpdateAsync(Ride ride);
    }
}
