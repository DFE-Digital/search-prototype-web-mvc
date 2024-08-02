using Bogus;
using Dfe.Data.SearchPrototype.SearchForEstablishments;

namespace Dfe.Data.SearchPrototype.Web.Tests.Shared;

public class EstablishmentTestDouble
{
    private static string GetEstablishmentNameFake() =>
             new Faker().Company.CompanyName();

    private static string GetEstablishmentIdentifierFake() =>
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

    private static string GetEstablishmentTypeFake() =>
        new Faker().Random.Word();

    private static StatusCode GetEstablishmentStatusCodeFake() =>
       (StatusCode)new Faker().Random.Int(0, 1);

    public static Establishment Create()
    {
        Address address = new(
            street: GetEstablishmentStreetFake(),
            locality: GetEstablishmentLocalityFake(),
            address3: GetEstablishmentAddress3Fake(),
            town: GetEstablishmentTownFake(),
            postcode: GetEstablishmentPostcodeFake());

        return new(
            urn: GetEstablishmentIdentifierFake(),
            name: GetEstablishmentNameFake(),
            address: address,
            establishmentType: GetEstablishmentTypeFake(),
            new EducationPhase("", "", ""),
            establishmentStatusCode: GetEstablishmentStatusCodeFake()
            
            );
    }
}

