using CsvHelper.Configuration.Attributes;
using System.Text.Json.Serialization;

namespace Dfe.Data.SearchPrototype.Data.models;

public class GeographyPoint
{
    public GeographyPoint(double latitude, double longitude)
    {
        coordinates = new List<double>() { longitude, latitude };
    }
    public string type => "Point";
    public List<double> coordinates { get; }
}

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


public class Establishment
{
    [Name("URN")]
    public string? id { get; set; }
    [Name("EstablishmentName")]
    public string? ESTABLISHMENTNAME { get; set; }
    [Name("Street")]
    public string? STREET { get; set; }
    [Name("Locality")]
    public string? LOCALITY { get; set; }
    [Name("Address3")]
    public string? ADDRESS3 { get; set; }
    [Name("Town")]
    public string? TOWN { get; set; }
    [Name("Postcode")]
    public string? POSTCODE { get; set; }
    [Name("TypeOfEstablishment (name)")]
    public string? TYPEOFESTABLISHMENTNAME { get; set; }
    [Name("PhaseOfEducation (name)")]
    public string? PHASEOFEDUCATION { get; set; }
    [Name("EstablishmentStatus (name)")]
    public string? ESTABLISHMENTSTATUSNAME { get; set; }
}
