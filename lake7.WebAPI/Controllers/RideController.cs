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
    public class RideController : ControllerBase
    {
        private readonly IRideService _rideService;

        public RideController(IRideService rideService)
        {
            _rideService = rideService;
        }

        [Authorize]
        [HttpPost("request")]
        public async Task<IActionResult> RequestRide([FromBody] Ride ride)
        {
            var newRide = await _rideService.RequestRideAsync(ride);
            return Ok(RideMapper.ToDto(newRide));
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllRides()
        {
            var rides = await _rideService.GetAllRidesAsync();
            return Ok(rides.Select(RideMapper.ToDto));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRideById(Guid id)
        {
            var ride = await _rideService.GetRideByIdAsync(id);
            if (ride == null) return NotFound();
            return Ok(RideMapper.ToDto(ride));
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateRideStatus(Guid id, [FromQuery] string status)
        {
            var updatedRide = await _rideService.UpdateRideStatusAsync(id, status);
            if (updatedRide == null) return NotFound();
            return Ok(RideMapper.ToDto(updatedRide));
        }
    }
}
