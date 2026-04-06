using lake7.Application.DTOs;
using lake7.Application.Helpers;
using lake7.Application.Interface;
using lake7.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace lake7.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _config;

        public AuthController(IUserService userService, IConfiguration config)
        {
            _userService = userService;
            _config = config;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _userService.ValidateUserAsync(dto.Email, dto.Password);

            if (user == null)
                return Unauthorized("Invalid email or password");

            var token = JwtHelper.GenerateToken(
                user.Id,
                user.Email,
                _config["Jwt:Key"]!,
                _config["Jwt:Issuer"]!,
                _config["Jwt:Audience"]!);

            return Ok(new { token });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            
            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = dto.FullName,
                Email = dto.Email,
                Password = dto.Password   
            };

            var newUser = await _userService.CreateUserAsync(user);

            return Ok(UserMapper.ToDto(newUser));
        }
    }
}
