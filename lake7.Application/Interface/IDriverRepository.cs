using System;
using System.Collections.Generic;
using System.Text;

using lake7.Domain.Entities;

namespace lake7.Application.Interface
{
    public interface IDriverRepository
    {
        Task<Driver> AddAsync(Driver driver);
        Task<IEnumerable<Driver>> GetAllAsync();
        Task<Driver?> GetByEmailAsync(string email);
        Task<Driver?> GetByIdAsync(Guid id);
        Task<Driver?> UpdateAsync(Driver driver);
    }
}

