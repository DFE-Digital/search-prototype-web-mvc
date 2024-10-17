using Dfe.Data.SearchPrototype.Web.Models;
using Scriban.Functions;
using Xunit;

namespace Dfe.Data.SearchPrototype.Web.Tests.Unit.Models;

public class SearchRequestModelTests
{
    [Fact]
    public void ClearFiltersTrue_ReturnsNullSelectedFacets()
    {
        SearchRequest searchRequestModel = new()
        {
            ClearFilters = true,
            SearchKeyword = "word",
            SelectedFacets = new Dictionary<string, List<string>>
            {
                { "facet", new List<string> { "filter1", "filter2" } }
            }
        };

        Assert.Null(searchRequestModel.SelectedFacets);

    }

    [Fact]
    public void ClearFiltersFalse_ReturnsSelectedFacetsDictionary()
    {
        SearchRequest searchRequestModel = new()
        {
            ClearFilters = false,
            SearchKeyword = "word",
            SelectedFacets = new Dictionary<string, List<string>>
            {
                { "facet", new List<string> { "filter1", "filter2" } }
            }
        };
        var expected = new Dictionary<string, List<string>>
            {
                { "facet", new List<string> { "filter1", "filter2" } }
            };
        Assert.NotNull(searchRequestModel.SelectedFacets);
        Assert.Equal(expected, searchRequestModel.SelectedFacets);

    }
}



