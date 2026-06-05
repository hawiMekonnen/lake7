using lake7.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace lake7.Infrastructure.Interface
{
    public interface IDriverRepository
    {
        Task<Driver> AddAsync(Driver driver);
        Task<IEnumerable<Driver>> GetAllAsync();
        Task<Driver?> GetByIdAsync(Guid id);
        Task<Driver?> GetByEmailAsync(string email);
        Task<Driver?> UpdateAsync(Driver driver);
        Task<IEnumerable<Driver>> GetAvailableDriversAsync();
        // New delete method
        Task<bool> DeleteAsync(Guid id);
    }
}
