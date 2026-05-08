using lake7.Domain.Entities;

namespace lake7.Application.Interface
{
    public interface IAdminRepository
    {
        Task<AdminAccount?> GetByEmailAsync(string email);
        Task<AdminAccount?> GetByIdAsync(Guid id);
        Task<AdminAccount> CreateAsync(AdminAccount admin);
        Task<List<AdminAccount>> GetAllAsync();
    }
}
