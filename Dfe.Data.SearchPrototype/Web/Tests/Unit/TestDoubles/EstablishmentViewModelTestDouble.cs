using Bogus;
using Dfe.Data.SearchPrototype.Web.Models;

namespace Dfe.Data.SearchPrototype.Web.Tests.Unit.TestDoubles;

public static class EstablishmentViewModelTestDouble
{
    public static string GetEstablishmentNameFake() =>
             new Faker().Company.CompanyName();

    public static string GetEstablishmentIdentifierFake() =>
        new Faker().Random.Int(100000, 999999).ToString();

    private static string GetEstablishmentStreetFake() =>
        new Faker().Address.StreetName();

    private static string GetEstablishmentLocalityFake() =>
        new Faker().Address.City();

    private static string GetEstablishmentAddress3Fake() =>
        new Faker().Address.City();

    private static string GetEstablishmentTownFake() =>
    new Faker().Address.City();

    private static string GetEstablishmentPostcodeFake() =>
        new Faker().Address.ZipCode();
    private static string GetEstablishmentPhaseOfEducation() =>
        new Faker().Random.Word();
    private static string GetEstablishmentStatusNameFake() =>
       new Faker().Random.Word();
    public static Establishment Create()
    {
        return new()
        {
            Urn = GetEstablishmentIdentifierFake(),
            Name = GetEstablishmentNameFake(),
            PhaseOfEducation = GetEstablishmentPhaseOfEducation(),
            Address = new()
            {
                Street = GetEstablishmentStreetFake(),
                Locality = GetEstablishmentLocalityFake(),
                Address3 = GetEstablishmentAddress3Fake(),
                Town = GetEstablishmentTownFake(),
                Postcode = GetEstablishmentPostcodeFake()
            },
            EstablishmentStatusName = GetEstablishmentStatusNameFake()
        };
    }
}
