using CsvHelper.Configuration.Attributes;

namespace Dfe.Data.SearchPrototype.Data;

//public class GeographyPoint
//{
//    public string? latitude { get; set; }
//    public string? longitude { get; set; }
//    public string GeographyPointString
//    {
//        get =>
//            $"geography'POINT({longitude} {latitude})'";
//    }
//}

public class EstablishmentOut : Establishment
{
    public EstablishmentOut(double latitude, double longitude)
    {
        GeographyPoint = $"geography'POINT({longitude} {latitude})'";
    }
    public string? GeographyPoint { get; set; }
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
