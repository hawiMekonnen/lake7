using lake7.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace lake7.Application.Interface
{
    public interface IRestaurantService
    {
        Task<IEnumerable<Restaurant>> GetAllRestaurantsAsync();
        Task<Restaurant?> GetRestaurantByIdAsync(Guid id);
        Task<Restaurant?> GetRestaurantByEmailAsync(string email);
        Task<Restaurant> RegisterRestaurantAsync(Restaurant restaurant);
        Task<Restaurant> UpdateRestaurantAsync(Restaurant restaurant);
        
        // Menu Items
        Task<IEnumerable<MenuItem>> GetMenuItemsAsync(Guid restaurantId);
        Task<MenuItem?> GetMenuItemByIdAsync(Guid id);
        Task<MenuItem> AddMenuItemAsync(MenuItem item);
        Task<MenuItem> UpdateMenuItemAsync(MenuItem item);
        Task DeleteMenuItemAsync(Guid id);
    }
}
