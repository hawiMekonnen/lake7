using lake7.Application.Interface;
using Microsoft.AspNetCore.Mvc;

namespace lake7.WebAPI.Controllers
{
    [ApiController]
    [Route("api/map")]
    public class MapController : ControllerBase
    {
        private readonly IMapService _mapService;

        public MapController(IMapService mapService)
        {
            _mapService = mapService;
        }

        [HttpGet("directions")]
        public async Task<IActionResult> GetDirections(
            [FromQuery] string origin,
            [FromQuery] string destination)
        {
            if (string.IsNullOrEmpty(origin) || string.IsNullOrEmpty(destination))
                return BadRequest("Origin and destination are required");

            var result = await _mapService.GetDirectionsAsync(origin, destination);

            return Ok(result);
        }
    }
}