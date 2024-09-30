using Dfe.Data.SearchPrototype.SearchForEstablishments.Models;

namespace Dfe.Data.SearchPrototype.Web.Tests.Shared.TestDoubles;

public static class EstablishmentFacetsTestDouble
{
    public static EstablishmentFacets Create()
    {
        var establishmentFacets = new List<EstablishmentFacet>();

        for (int i = 0; i < new Bogus.Faker().Random.Int(1, 10); i++)
        {
            establishmentFacets.Add(
                EstablishmentFacetTestDouble.Create());
        }
        return new EstablishmentFacets(establishmentFacets);
    }

    public static EstablishmentFacets CreateWithNoResults() => new();

    public static EstablishmentFacets CreateWith(List<EstablishmentFacet> establishmentFacets) => new(establishmentFacets);
}
