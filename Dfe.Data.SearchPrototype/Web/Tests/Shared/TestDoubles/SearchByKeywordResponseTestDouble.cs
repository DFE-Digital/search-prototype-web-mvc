using Azure.Search.Documents.Models;
using Dfe.Data.SearchPrototype.SearchForEstablishments.ByKeyword.Usecase;
using Dfe.Data.SearchPrototype.SearchForEstablishments.Models;

namespace Dfe.Data.SearchPrototype.Web.Tests.Shared.TestDoubles;

public static class SearchByKeywordResponseTestDouble
{
    public static SearchByKeywordResponse Create() => CreateWith(new Bogus.Faker().Random.Int(11, 66));

    public static SearchByKeywordResponse CreateWith(int establishmentResultsCount, int pageNumber = 0)
    {
        List<Establishment> establishmentResults = new();
        Dictionary<int, List<PageSequence>> pageChunks = new();
        List<PageSequence> currentPageSequences = new();
        if (pageNumber > 0)
        {
            List<int> rawRecordCount = Enumerable.Range(1, establishmentResultsCount).ToList();

            pageChunks = rawRecordCount.Select((x, i) => new PageSequence{RecordNumber = i})
                .GroupBy(x => x.RecordNumber / 10).ToDictionary(x => x.Key, y => y.ToList());
        }

        if (pageChunks.Any())
        {
            currentPageSequences = pageChunks[pageNumber - 1];
        }

        int numberOfResultsPerPage = (currentPageSequences.Any()) ? currentPageSequences.Count : 10;

        for (int i = 0; i < numberOfResultsPerPage; i++)
        {
            establishmentResults.Add(EstablishmentTestDouble.Create());
        }

        List<EstablishmentFacet> facetResults = new();

        for (int i = 0; i < numberOfResultsPerPage; i++)
        {
            facetResults.Add(EstablishmentFacetTestDouble.Create(i.ToString()));

        }
        return new SearchByKeywordResponse(status: SearchResponseStatus.Success)
        {
            EstablishmentResults = new EstablishmentResults(establishmentResults, establishmentResultsCount),
            EstablishmentFacetResults = new EstablishmentFacets(facetResults)
        };
    }
    internal class PageSequence
    {
        public int RecordNumber { get; set; }
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
