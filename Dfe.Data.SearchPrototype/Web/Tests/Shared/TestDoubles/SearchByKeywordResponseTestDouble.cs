using Dfe.Data.SearchPrototype.SearchForEstablishments.ByKeyword.Usecase;
using Dfe.Data.SearchPrototype.SearchForEstablishments.Models;

namespace Dfe.Data.SearchPrototype.Web.Tests.Shared.TestDoubles;

public static class SearchByKeywordResponseTestDouble
{
    public static SearchByKeywordResponse Create() => CreateWith(new Bogus.Faker().Random.Int(11, 66));

    public static SearchByKeywordResponse CreateWith(int establishmentResultsCount)
    {
        List<Establishment> establishmentResults = Enumerable.Range(0, 10)
            .Select(_ => EstablishmentTestDouble.Create())
            .ToList();

        List<EstablishmentFacet> facetResults = Enumerable.Range(0, 10)
            .Select(i => EstablishmentFacetTestDouble.Create(i.ToString()))
            .ToList();

        return new SearchByKeywordResponse(status: SearchResponseStatus.Success)
        {
            EstablishmentResults = new EstablishmentResults(establishmentResults),
            EstablishmentFacetResults = new EstablishmentFacets(facetResults),
            TotalNumberOfEstablishments = establishmentResultsCount
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
            EstablishmentResults = new EstablishmentResults(establishmentResults),
            EstablishmentFacetResults = new EstablishmentFacets(facetResults),
            TotalNumberOfEstablishments = 1
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
