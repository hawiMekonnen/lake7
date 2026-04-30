using lake7.Application.Interface;
using System.Text.Json;

namespace lake7.Application.Services
{
    public class MapService : IMapService
    {
        public async Task<List<object>> GetDirectionsAsync(string origin, string destination)
        {
            var client = new HttpClient();

            var url = $"http://router.project-osrm.org/route/v1/driving/" +
                      $"{origin.Replace(",", "%2C")};{destination.Replace(",", "%2C")}" +
                      "?overview=full&geometries=geojson";

            var response = await client.GetStringAsync(url);

            var json = JsonDocument.Parse(response);

            var coords = json
                .RootElement
                .GetProperty("routes")[0]
                .GetProperty("geometry")
                .GetProperty("coordinates");

            var result = coords.EnumerateArray()
                .Select(c => new
                {
                    latitude = c[1].GetDouble(),
                    longitude = c[0].GetDouble()
                }).ToList<object>();

            return result;
        }
    }
}