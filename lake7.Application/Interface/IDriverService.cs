using lake7.Domain.Entities;

namespace lake7.Application.Interface
{
    public interface IDriverService
    {
        Task<Driver> RegisterDriverAsync(Driver driver);
        Task<List<Driver>> GetDriversAsync();
        Task<Driver?> ValidateDriverAsync(string email, string password);
        Task<Driver?> GetDriverByIdAsync(Guid id);
        Task<Driver?> SetApprovalStatusAsync(Guid id, bool isApproved);
        Task<Driver?> UpdateDriverAsync(Driver driver, Guid id);
        Task<bool> SetAvailabilityAsync(Guid id, bool isAvailable);
    }
}
