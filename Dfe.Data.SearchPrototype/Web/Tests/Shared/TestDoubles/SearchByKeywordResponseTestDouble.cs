using Dfe.Data.SearchPrototype.SearchForEstablishments;
using Dfe.Data.SearchPrototype.SearchForEstablishments.Models;

namespace Dfe.Data.SearchPrototype.Web.Tests.Shared.TestDoubles;

public static class SearchByKeywordResponseTestDouble
{
    public static SearchByKeywordResponse Create()
    {
        List<Establishment> establishmentResults = new();
        for (int i = 0; i < new Bogus.Faker().Random.Int(2, 10); i++)
        {
            establishmentResults.Add(EstablishmentTestDouble.Create());
        }

        List<EstablishmentFacet> facetResults = new();
        for (int i = 0; i < new Bogus.Faker().Random.Int(2, 10); i++)
        {
            facetResults.Add(EstablishmentFacetTestDouble.Create());
        }
        return new SearchByKeywordResponse(status: SearchResponseStatus.Success)
        {
            EstablishmentResults = new EstablishmentResults(establishmentResults),
            EstablishmentFacetResults = new EstablishmentFacets(facetResults)
        };
    }

    public static SearchByKeywordResponse CreateWithOneResult()
    {
        List<Establishment> establishmentResults = new() {
           EstablishmentTestDouble.Create()
        };
        List<EstablishmentFacet> facetResults = new() {
            EstablishmentFacetTestDouble.Create()
        };
        return new SearchByKeywordResponse(status: SearchResponseStatus.Success) {
            EstablishmentResults = new EstablishmentResults(establishmentResults),
            EstablishmentFacetResults = new EstablishmentFacets(facetResults)
        };
    }

    public static SearchByKeywordResponse CreateWithNoResults()
    {
        return new SearchByKeywordResponse(status: SearchResponseStatus.Success) { };
    }
}
