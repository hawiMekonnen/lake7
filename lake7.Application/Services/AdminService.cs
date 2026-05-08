using lake7.Application.Interface;
using lake7.Domain.Entities;
using System.Security.Cryptography;
using System.Text;

namespace lake7.Application.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;

        public AdminService(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        public async Task<AdminAccount?> LoginAsync(string email, string password)
        {
            var admin = await _adminRepository.GetByEmailAsync(email);
            if (admin == null) return null;

            // In a real app, use BCrypt or similar. For now, simple check.
            if (admin.Password != password) return null;

            admin.LastLogin = DateTime.UtcNow;
            // Save last login if repo supported it, but we'll just return for now
            return admin;
        }

        public async Task<AdminAccount> RegisterAsync(AdminAccount admin)
        {
            return await _adminRepository.CreateAsync(admin);
        }

        public async Task<AdminAccount?> GetAdminByIdAsync(Guid id)
        {
            return await _adminRepository.GetByIdAsync(id);
        }

        public async Task<List<AdminAccount>> GetAllAdminsAsync()
        {
            return await _adminRepository.GetAllAsync();
        }
    }
}
