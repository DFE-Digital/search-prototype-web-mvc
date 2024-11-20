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

    public async Task<IEnumerable<GeoLocation?>?> LookupPostcodes(IEnumerable<string> postcodes)
    {
        var client = _httpClientFactory.CreateClient("PostcodeLookupAPI");
        var jsonPayload = JsonSerializer.Serialize(new { postcodes = postcodes });
        HttpResponseMessage response = await client.PostAsync("postcodes", new StringContent(jsonPayload, Encoding.UTF8, "application/json"));

        PostcodeApiResponse? result = null;
        try
        {
            response.EnsureSuccessStatusCode();
            if(response.Content is object)
            {
                result = await response.Content.ReadFromJsonAsync<PostcodeApiResponse>();
            }
        }
        finally
        {
            response.Dispose();
        }
        return result?.Result?.Select(x => x.Result);
    }
}
