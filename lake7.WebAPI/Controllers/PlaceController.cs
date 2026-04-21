using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

namespace lake7.WebAPI.Controllers
{
    [ApiController]
    [Route("api/places")]
    public class PlacesController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public PlacesController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet("autocomplete")]
        public async Task<IActionResult> Autocomplete([FromQuery] string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return Ok(new { predictions = new object[0] });
            }

            try
            {
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Add("User-Agent", "lake7-rideshare-app"); // ✅ required

                var url = $"https://nominatim.openstreetmap.org/search?" +
                           $"q={Uri.EscapeDataString(input)}" +
                           "&format=json" +
                           "&addressdetails=1" +
                           "&limit=5" +
                           "&countrycodes=et" + //restrict to Ethiopia
                           "&viewbox=38.6,9.1,38.9,9.05" + //bounding box around Addis Ababa
                           "&bounded=1"; //force results inside the box


                var response = await client.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return StatusCode((int)response.StatusCode, content);
                }

                var results = System.Text.Json.JsonSerializer.Deserialize<object>(content);

                return Ok(new { predictions = results }); // ✅ wrap in predictions
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
