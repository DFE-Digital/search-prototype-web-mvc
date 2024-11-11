using Dfe.Data.SearchPrototype.Web.Models.ViewModels;
using FluentAssertions;
using Moq;
using Xunit;

namespace Dfe.Data.SearchPrototype.Web.Tests.Unit.Models;

public class SearchResultsTests
{
    [Fact]
    public void Facets_ReturnedInNameAlphatbeticalOrder()
    {
        SearchResults searchResults = new SearchResults()
        {
            Facets = new List<Facet>() {
                    new Facet("c", new List<FacetValue>() { new FacetValue(It.IsAny<string>(), It.IsAny<long>(), It.IsAny<bool>()) }),
                    new Facet("a", new List<FacetValue>() { new FacetValue(It.IsAny<string>(), It.IsAny<long>(), It.IsAny<bool>()) }),
                    new Facet("z", new List<FacetValue>() { new FacetValue(It.IsAny<string>(), It.IsAny<long>(), It.IsAny<bool>()) })
                }
        };

        searchResults.Facets[0].Name.Should().Be("a");
        searchResults.Facets[1].Name.Should().Be("c");
        searchResults.Facets[2].Name.Should().Be("z");
    }
}
