using Azure.Search.Documents.Models;
using Dfe.Data.SearchPrototype.SearchForEstablishments.ByKeyword.Usecase;
using Dfe.Data.SearchPrototype.SearchForEstablishments.Models;

namespace Dfe.Data.SearchPrototype.Web.Tests.Shared.TestDoubles;

public static class SearchByKeywordResponseTestDouble
{
    public static SearchByKeywordResponse Create()
    {
        List<Establishment> establishmentResults = new();
        for (int i = 0; i < new Bogus.Faker().Random.Int(11, 66); i++)
        {
            establishmentResults.Add(EstablishmentTestDouble.Create());
        }

        List<EstablishmentFacet> facetResults = new();

        long? totalNumberOfEstablishments = 0;

        for (int i = 0; i < new Bogus.Faker().Random.Int(11, 66); i++)
        {
            facetResults.Add(EstablishmentFacetTestDouble.Create(i.ToString()));

            totalNumberOfEstablishments += i;
        }
        return new SearchByKeywordResponse(status: SearchResponseStatus.Success)
        {
            EstablishmentResults = new EstablishmentResults(establishmentResults, totalNumberOfEstablishments),v
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
        return new SearchByKeywordResponse(status: SearchResponseStatus.Success)
        {
            EstablishmentResults = new EstablishmentResults(establishmentResults, totalNumberOfEstablishments: 1),
            EstablishmentFacetResults = new EstablishmentFacets(facetResults)
        };
    }

    public static SearchByKeywordResponse CreateWithNoResults()
    {
        return new SearchByKeywordResponse(status: SearchResponseStatus.Success) { };
    }

    public static SearchByKeywordResponse CreateWithEmptyList()
    {
        return new SearchByKeywordResponse(status: SearchResponseStatus.Success)
        {
            EstablishmentResults = new EstablishmentResults(),
            EstablishmentFacetResults = new EstablishmentFacets()
        };
    }
}
