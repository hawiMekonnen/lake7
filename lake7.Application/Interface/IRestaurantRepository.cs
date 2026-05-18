using lake7.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace lake7.Application.Interface
{
    public interface IRestaurantRepository
    {
        Task<Restaurant> AddAsync(Restaurant restaurant);
        Task<IEnumerable<Restaurant>> GetAllAsync();
        Task<Restaurant?> GetByIdAsync(Guid id);
        Task<Restaurant?> GetByEmailAsync(string email);
        Task<Restaurant?> UpdateAsync(Restaurant restaurant);
        Task DeleteAsync(Guid id);
    }
}
