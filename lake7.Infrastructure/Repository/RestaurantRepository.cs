using lake7.Application.Interface;
using lake7.Domain.Entities;
using lake7.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace lake7.Infrastructure.Repository
{
    public class RestaurantRepository : IRestaurantRepository
    {
        private readonly Lake7DbContext _context;

        public RestaurantRepository(Lake7DbContext context)
        {
            _context = context;
        }

        public async Task<Restaurant> AddAsync(Restaurant restaurant)
        {
            await _context.Restaurants.AddAsync(restaurant);
            await _context.SaveChangesAsync();
            return restaurant;
        }

        public async Task<IEnumerable<Restaurant>> GetAllAsync()
        {
            return await _context.Restaurants.Where(r => r.IsActive).ToListAsync();
        }

        public async Task<Restaurant?> GetByIdAsync(Guid id)
        {
            return await _context.Restaurants.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Restaurant?> GetByEmailAsync(string email)
        {
            return await _context.Restaurants.FirstOrDefaultAsync(r => r.Email == email);
        }

        public async Task<Restaurant?> UpdateAsync(Restaurant restaurant)
        {
            _context.Restaurants.Update(restaurant);
            await _context.SaveChangesAsync();
            return restaurant;
        }

        public async Task DeleteAsync(Guid id)
        {
            var restaurant = await GetByIdAsync(id);
            if (restaurant != null)
            {
                restaurant.IsActive = false;
                await UpdateAsync(restaurant);
            }
        }
    }
}
