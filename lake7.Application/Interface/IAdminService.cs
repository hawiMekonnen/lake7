using lake7.Domain.Entities;

namespace lake7.Application.Interface
{
    public interface IAdminService
    {
        Task<AdminAccount?> LoginAsync(string email, string password);
        Task<AdminAccount> RegisterAsync(AdminAccount admin);
        Task<AdminAccount?> GetAdminByIdAsync(Guid id);
        Task<List<AdminAccount>> GetAllAdminsAsync();
    }
}
