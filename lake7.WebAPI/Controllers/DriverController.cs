using lake7.Application.DTOs;
using lake7.Application.Helpers;
using lake7.Application.Interface;
using lake7.Application.Services;
using lake7.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace lake7.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DriverController : ControllerBase
    {
        private readonly IDriverService _driverService;
        private readonly IConfiguration _config;
        private readonly IDriverLocationService _driverLocationService;

        public DriverController(IDriverService driverService, IConfiguration config, IDriverLocationService driverLocationService)
        {
            _driverService = driverService;
            _config = config;
            _driverLocationService = driverLocationService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterDriver([FromBody] Driver driver)
        {
            var newDriver = await _driverService.RegisterDriverAsync(driver);
            return CreatedAtAction(nameof(GetDriverById), new { id = newDriver.Id }, DriverMapper.ToDto(newDriver));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var driver = await _driverService.ValidateDriverAsync(dto.Email, dto.Password);

            if (driver == null)
                return Unauthorized("Invalid email or password");

            var token = JwtHelper.GenerateToken(
                driver.Id,
                driver.Email,
                _config["Jwt:Key"]!,
                _config["Jwt:Issuer"]!,
                _config["Jwt:Audience"]!);

            return Ok(new { token });
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetDrivers()
        {
            var drivers = await _driverService.GetDriversAsync();
            var driverDtos = drivers.Select(d => DriverMapper.ToDto(d)).ToList();
            return Ok(driverDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDriverById(Guid id)
        {
            var driver = await _driverService.GetDriverByIdAsync(id);
            if (driver == null) return NotFound();
            return Ok(DriverMapper.ToDto(driver));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDriver(Guid id, [FromBody] Driver driver)
        {
            var updatedDriver = await _driverService.UpdateDriverAsync(driver, id);
            if (updatedDriver == null) return NotFound();
            return Ok(DriverMapper.ToDto(updatedDriver));
        }

        [HttpPatch("{id}/availability")]
        public async Task<IActionResult> SetAvailability(Guid id, [FromQuery] bool isAvailable)
        {
            var success = await _driverService.SetAvailabilityAsync(id, isAvailable);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpPost("{id}/location")]
        public async Task<IActionResult> UpdateLocation(Guid id, [FromBody] LocationDto dto)
        {
            var updatedLocation = await _driverLocationService.UpdateLocationAsync(id, dto.Latitude, dto.Longitude);
            if (updatedLocation == null) return NotFound();
            return Ok(updatedLocation);

        }

        [HttpGet("nearby")]
        public async Task<IActionResult> GetNearbyDrivers([FromQuery] double latitude, [FromQuery] double longitude, [FromQuery] double radiusKm)
        {
            var drivers = await _driverLocationService.GetNearbyDriversAsync(latitude, longitude, radiusKm);
            if (drivers == null || !drivers.Any()) return NotFound("No drivers found nearby.");
            return Ok(drivers);
        }

    }
}
