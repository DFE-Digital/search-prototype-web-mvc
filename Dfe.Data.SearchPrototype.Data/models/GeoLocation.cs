namespace Dfe.Data.SearchPrototype.Data.Models;

/// <summary>
/// defines the relevant response from the postcode API
/// </summary>
public class GeoLocation
{
    public string? postcode { get; set; }
    public double longitude { get; set; }
    public double latitude { get; set; }
}