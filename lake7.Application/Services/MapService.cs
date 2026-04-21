using System;
using System.Collections.Generic;
using System.Text;

using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using lake7.Application.Interface;

public class MapService : IMapService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;

    public MapService(IHttpClientFactory factory, IConfiguration config)
    {
        _httpClient = factory.CreateClient("MapsClient");
        _config = config;
    }

    public async Task<string> GetDirectionsAsync(string origin, string destination)
    {
        var apiKey = _config["GoogleMaps:ApiKey"];

        var url = $"maps/api/directions/json?origin={origin}&destination={destination}&key={apiKey}";

        var response = await _httpClient.GetAsync(url);

        return await response.Content.ReadAsStringAsync();
    }
}