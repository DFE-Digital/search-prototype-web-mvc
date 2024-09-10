using Dfe.Data.SearchPrototype.SearchForEstablishments.Models;

namespace Dfe.Data.SearchPrototype.Tests.SearchForEstablishments.TestDoubles;

public static class SearchByKeywordCriteriaTestDouble
{
    public static SearchByKeywordCriteria Create()
    {
        return new SearchByKeywordCriteria()
        {
            Facets = new List<string>() { },
            SearchFields = new List<string>() { }
        };
    }
}
