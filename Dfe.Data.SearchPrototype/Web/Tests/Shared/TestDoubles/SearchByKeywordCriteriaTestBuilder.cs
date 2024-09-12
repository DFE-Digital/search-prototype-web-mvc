using Dfe.Data.SearchPrototype.SearchForEstablishments.Models;

namespace Dfe.Data.SearchPrototype.Tests.SearchForEstablishments.TestDoubles;

public static class SearchByKeywordCriteriaTestDouble
{
    public static SearchByKeywordCriteria Create()
    {
        return new SearchByKeywordCriteria()
        {
            Facets = ["FACET1", "FACET2", "FACET3"],
            SearchFields = ["FIELD1", "FIELD2", "FILED3"]
        };
    }
}
