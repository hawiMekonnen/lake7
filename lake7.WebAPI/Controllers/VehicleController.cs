using lake7.Application.Interface;
using Microsoft.AspNetCore.Mvc;

namespace lake7.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehicleController : ControllerBase
    {
        private readonly IDriverLocationService _driverLocationService;

        public VehicleController(IDriverLocationService driverLocationService)
        {
            _driverLocationService = driverLocationService;
        }

        [HttpGet("available")]
        public async Task<IActionResult> GetAvailableVehicles(double lat, double lon, double radiusKm = 3)
        {
            var drivers = await _driverLocationService.GetNearbyDriversAsync(lat, lon, radiusKm);
            return Ok(drivers.Select(d => new {
                d.DriverId,
                d.VehicleType,
                d.LicensePlate,
                d.Latitude,
                d.Longitude
            }));
        }
    }

}
