using lake7.Domain.Entities;

namespace lake7.Application.Interface
{
    public interface IUserRepository
    {
        Task<User> AddAsync(User user);
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByIdAsync(Guid id);
        Task<User?> GetByEmailAsync(string email);
        Task<User?> UpdateAsync(User user);
    }
}
