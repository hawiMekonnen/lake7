using lake7.Application.Interface;
using lake7.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace lake7.Application.Services
{
    public class RestaurantService : IRestaurantService
    {
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IMenuItemRepository _menuItemRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RestaurantService(
            IRestaurantRepository restaurantRepository,
            IMenuItemRepository menuItemRepository,
            IUnitOfWork unitOfWork)
        {
            _restaurantRepository = restaurantRepository;
            _menuItemRepository = menuItemRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Restaurant>> GetAllRestaurantsAsync()
        {
            return await _restaurantRepository.GetAllAsync();
        }

        public async Task<Restaurant?> GetRestaurantByIdAsync(Guid id)
        {
            return await _restaurantRepository.GetByIdAsync(id);
        }

        public async Task<Restaurant?> GetRestaurantByEmailAsync(string email)
        {
            return await _restaurantRepository.GetByEmailAsync(email);
        }

        public async Task<Restaurant> RegisterRestaurantAsync(Restaurant restaurant)
        {
            restaurant.CreatedAt = DateTime.UtcNow;
            restaurant.UpdatedAt = DateTime.UtcNow;
            return await _restaurantRepository.AddAsync(restaurant);
        }

        public async Task<Restaurant> UpdateRestaurantAsync(Restaurant restaurant)
        {
            restaurant.UpdatedAt = DateTime.UtcNow;
            return await _restaurantRepository.UpdateAsync(restaurant);
        }

        public async Task<IEnumerable<MenuItem>> GetMenuItemsAsync(Guid restaurantId)
        {
            return await _menuItemRepository.GetByRestaurantIdAsync(restaurantId);
        }

        public async Task<MenuItem?> GetMenuItemByIdAsync(Guid id)
        {
            return await _menuItemRepository.GetByIdAsync(id);
        }

        public async Task<MenuItem> AddMenuItemAsync(MenuItem item)
        {
            item.CreatedAt = DateTime.UtcNow;
            item.UpdatedAt = DateTime.UtcNow;
            return await _menuItemRepository.AddAsync(item);
        }

        public async Task<MenuItem> UpdateMenuItemAsync(MenuItem item)
        {
            item.UpdatedAt = DateTime.UtcNow;
            return await _menuItemRepository.UpdateAsync(item);
        }

        public async Task DeleteMenuItemAsync(Guid id)
        {
            await _menuItemRepository.DeleteAsync(id);
        }
    }
}
