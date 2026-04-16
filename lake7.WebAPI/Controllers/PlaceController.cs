using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace YourNamespace.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlacesController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public PlacesController(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }
        [HttpGet("autocomplete")]
        public async Task<IActionResult> Autocomplete([FromQuery] string input)
        {
            var apiKey = Environment.GetEnvironmentVariable("GOOGLE_API_KEY");
            var url = $"https://places.googleapis.com/v1/places:autocomplete?input={input}&key={apiKey}";

            using var client = new HttpClient();
            var response = await client.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();

            return Content(content, "application/json");
        }

    }
}