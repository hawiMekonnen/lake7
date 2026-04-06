using lake7.Application.DTOs;
using lake7.Application.Helpers;
using lake7.Application.Interface;
using lake7.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace lake7.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(User user)
        {
            var newUser = await _userService.CreateUserAsync(user);
            return Ok(UserMapper.ToDto(newUser));
        }

        [Authorize]
        [HttpGet("getUsers")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetUserAsync();
            return Ok(users.Select(UserMapper.ToDto));
        }

        [HttpGet("getByID")]
        public async Task<IActionResult> GetByID(Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(UserMapper.ToDto(user));
        }

        [HttpGet("getByEmail")]
        public async Task<IActionResult> GetByEmail(string email)
        {
            var user = await _userService.GetUserByEmailAsync(email);
            if (user == null) return NotFound();
            return Ok(UserMapper.ToDto(user));
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update(User user, Guid id)
        {
            var updatedUser = await _userService.UpdateUserAsync(user, id);
            if (updatedUser == null) return NotFound();
            return Ok(UserMapper.ToDto(updatedUser));
        }
    }
}
