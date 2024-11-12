using Dfe.Data.SearchPrototype.SearchForEstablishments.Models;

namespace DfE.Data.SearchPrototype.Web.Tests.Shared.TestDoubles;

public static class SearchByKeywordCriteriaTestDouble
{
    public static SearchByKeywordCriteria Create()
    {
        return new SearchByKeywordCriteria()
        {
            Facets = ["FACET1", "FACET2", "FACET3"],
            SearchFields = ["FIELD1", "FIELD2", "FIELD3"]
        };
    }
}
