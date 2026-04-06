using lake7.Application.Interface;
using lake7.Application.DTOs;
using lake7.Application.Helpers;
using lake7.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace lake7.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DriverController : ControllerBase
    {
        private readonly IDriverService _driverService;

        public DriverController(IDriverService driverService)
        {
            _driverService = driverService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterDriver([FromBody] Driver driver)
        {
            var newDriver = await _driverService.RegisterDriverAsync(driver);
            return CreatedAtAction(nameof(GetDriverById), new { id = newDriver.Id }, DriverMapper.ToDto(newDriver));
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
    }
}
