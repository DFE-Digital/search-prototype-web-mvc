using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Dfe.Data.SearchPrototype.Data.Models;

namespace Dfe.Data.SearchPrototype.Data;

public class PostcodeLookupService
{
    private IHttpClientFactory _httpClientFactory;
    public PostcodeLookupService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IEnumerable<GeoLocation>> LookupPostcodes(IEnumerable<string> postcodes)
    {
        var client = _httpClientFactory.CreateClient("PostcodeLookupAPI");
        var jsonPayload = JsonSerializer.Serialize(new { postcodes = postcodes });
        using (HttpResponseMessage response = await client.PostAsync("postcodes", new StringContent(jsonPayload, Encoding.UTF8, "application/json")))
        {
            var geoLocations = new List<GeoLocation>();

            response.EnsureSuccessStatusCode();
            if (response.Content is object)
            {
                PostcodeApiResponse? result = await response.Content.ReadFromJsonAsync<PostcodeApiResponse>();
                geoLocations = result?.Result?
                    .Where(apiResult => apiResult.Result != null)
                    .Select(apiResult => apiResult.Result!)
                    .ToList() ?? new List<GeoLocation>();
            }
            return geoLocations;
        }
    }
}
