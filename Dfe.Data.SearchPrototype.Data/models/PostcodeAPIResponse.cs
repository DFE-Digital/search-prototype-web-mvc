
using System.Text.Json.Serialization;

namespace Dfe.Data.SearchPrototype.Data.Models;

public class APIResult
{
    //public string? query { get; set; }
    [JsonPropertyName("result")]
    public GeoLocation? Result { get; set; }
}

public class PostcodeApiResponse
{
    [JsonPropertyName("status")]
    public int Status { get; set; }
    [JsonPropertyName("result")]
    public List<APIResult>? Result { get; set; }
}