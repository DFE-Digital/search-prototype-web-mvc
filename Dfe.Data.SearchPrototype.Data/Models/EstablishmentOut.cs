using System.Text.Json.Serialization;

namespace Dfe.Data.SearchPrototype.Data.Models;

/// <summary>
/// Model used to output to Azure AI cognitive search
/// </summary>
public class EstablishmentOut : Establishment
{
    public EstablishmentOut(double latitude, double longitude)
    {
        geoLocation = new GeographyPoint(latitude, longitude);
    }
    public GeographyPoint geoLocation { get; }

    [JsonPropertyName("@search.action")]
    public string action { get => "mergeOrUpload"; }
}

