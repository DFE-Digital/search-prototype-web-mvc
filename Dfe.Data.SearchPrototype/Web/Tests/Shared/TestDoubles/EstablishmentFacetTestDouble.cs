using Dfe.Data.SearchPrototype.SearchForEstablishments.Models;

namespace Dfe.Data.SearchPrototype.Web.Tests.Shared.TestDoubles;

public static class EstablishmentFacetTestDouble
{
    public static EstablishmentFacet Create(string? uniqueNumber = null)
    {
        var faker = new Bogus.Faker();
        var facetResults = new List<FacetResult>();
        var facetValuesCount = new Bogus.Faker().Random.Int(1, 10);

        for (int i = 0; i < facetValuesCount; i++)
        {
            var facetResult = new FacetResult(faker.Name.JobTitle(), faker.Random.Int(1, 10));
            facetResults.Add(facetResult);
        }
        return new EstablishmentFacet(new Bogus.Faker().Name.JobType() + uniqueNumber, facetResults);
    }
}
