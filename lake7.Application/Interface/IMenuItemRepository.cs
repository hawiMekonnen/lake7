using lake7.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace lake7.Application.Interface
{
    public interface IMenuItemRepository
    {
        Task<MenuItem> AddAsync(MenuItem item);
        Task<IEnumerable<MenuItem>> GetByRestaurantIdAsync(Guid restaurantId);
        Task<MenuItem?> GetByIdAsync(Guid id);
        Task<MenuItem?> UpdateAsync(MenuItem item);
        Task DeleteAsync(Guid id);
    }
}
