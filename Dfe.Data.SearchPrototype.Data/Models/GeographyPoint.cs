namespace Dfe.Data.SearchPrototype.Data.Models;

/// <summary>
/// usd to populate Azure AI search
/// </summary>
public class GeographyPoint
{
    public GeographyPoint(double latitude, double longitude)
    {
        coordinates = new List<double>() { longitude, latitude };
    }
    public string type => "Point";
    public List<double> coordinates { get; }
}
