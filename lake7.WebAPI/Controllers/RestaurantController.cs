using lake7.Application.Interface;
using lake7.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace lake7.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;

        public RestaurantController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var restaurants = await _restaurantService.GetAllRestaurantsAsync();
            return Ok(restaurants);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var restaurant = await _restaurantService.GetRestaurantByIdAsync(id);
            if (restaurant == null) return NotFound();
            return Ok(restaurant);
        }

        [HttpGet("by-email/{email}")]
        public async Task<IActionResult> GetByEmail(string email)
        {
            var restaurant = await _restaurantService.GetRestaurantByEmailAsync(email);
            if (restaurant == null) return NotFound();
            return Ok(restaurant);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Restaurant restaurant)
        {
            var newRestaurant = await _restaurantService.RegisterRestaurantAsync(restaurant);
            return CreatedAtAction(nameof(GetById), new { id = newRestaurant.Id }, newRestaurant);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Restaurant restaurant)
        {
            if (id != restaurant.Id) return BadRequest();
            var updated = await _restaurantService.UpdateRestaurantAsync(restaurant);
            return Ok(updated);
        }

        // Menu Items
        [HttpGet("{restaurantId}/menu")]
        public async Task<IActionResult> GetMenu(Guid restaurantId)
        {
            var items = await _restaurantService.GetMenuItemsAsync(restaurantId);
            return Ok(items);
        }

        [HttpPost("menu")]
        public async Task<IActionResult> AddMenuItem([FromBody] MenuItem item)
        {
            var newItem = await _restaurantService.AddMenuItemAsync(item);
            return Ok(newItem);
        }

        [HttpPut("menu/{id}")]
        public async Task<IActionResult> UpdateMenuItem(Guid id, [FromBody] MenuItem item)
        {
            if (id != item.Id) return BadRequest();
            var updated = await _restaurantService.UpdateMenuItemAsync(item);
            return Ok(updated);
        }

        [HttpDelete("menu/{id}")]
        public async Task<IActionResult> DeleteMenuItem(Guid id)
        {
            await _restaurantService.DeleteMenuItemAsync(id);
            return NoContent();
        }
    }
}
