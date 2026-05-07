using lake7.Application.DTOs;
using lake7.Application.Helpers;
using lake7.Application.Interface;
using lake7.Domain.Entities;
using lake7.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
namespace lake7.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RideController : ControllerBase
    {
        private readonly IRideService _rideService;
        private readonly IMapService _mapService;

        public RideController(IRideService rideService, IMapService mapService)
        {
            _rideService = rideService;
            _mapService = mapService;
        }

        [Authorize]
        [HttpPost("request")]
        public async Task<IActionResult> RequestRide([FromBody] RideRequestDto dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userIdClaim == null)
                return Unauthorized("Invalid token");

            var ride = new Ride
            {
                UserId = Guid.Parse(userIdClaim),
                PickupLocation = dto.PickupLocation,
                PickupLatitude = dto.PickupLatitude,
                PickupLongitude = dto.PickupLongitude,
                DropoffLocation = dto.DropoffLocation,
                DropoffLatitude = dto.DropoffLatitude,
                DropoffLongitude = dto.DropoffLongitude
            };

            var (newRide, nearbyDrivers) = await _rideService.RequestRideWithMatchingAsync(ride, 3);

            return Ok(new
            {
                Ride = RideMapper.ToDto(newRide),
                NearbyDrivers = nearbyDrivers
            });
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
        public async Task<IActionResult> UpdateRideStatus(Guid id, [FromQuery] RideStatus status)
        {
            var updatedRide = await _rideService.UpdateRideStatusAsync(id, status);
            if (updatedRide == null) return NotFound();
            return Ok(RideMapper.ToDto(updatedRide));
        }

        [HttpGet("directions")]
        public async Task<IActionResult> GetDirections(string origin, string destination)
        {
            var result = await _mapService.GetDirectionsAsync(origin, destination);
            return Ok(result);
        }
        [Authorize]
        [HttpPost("{rideId}/accept")]
        public async Task<IActionResult> AcceptRide(Guid rideId, [FromQuery] Guid driverId)
        {
            var acceptedRide = await _rideService.AcceptRideAsync(rideId, driverId);
            if (acceptedRide == null) return BadRequest("Ride already accepted or not found.");

            return Ok(RideMapper.ToDto(acceptedRide));
        }

        [HttpPatch("{rideId}/transition")]
        public async Task<IActionResult> TransitionRide(Guid rideId, [FromQuery] RideStatus newStatus)
        {
            var updatedRide = await _rideService.TransitionRideStatusAsync(rideId, newStatus);
            if (updatedRide == null) return BadRequest("Invalid transition or ride not found.");

            return Ok(RideMapper.ToDto(updatedRide));
        }


        [HttpGet("nearby-pending")]
        public async Task<IActionResult> GetNearbyPendingRides([FromQuery] double latitude, [FromQuery] double longitude, [FromQuery] double radiusKm = 5)
        {
            var rides = await _rideService.GetNearbyPendingRidesAsync(latitude, longitude, radiusKm);
            return Ok(rides.Select(RideMapper.ToDto));
        }


    }
}
