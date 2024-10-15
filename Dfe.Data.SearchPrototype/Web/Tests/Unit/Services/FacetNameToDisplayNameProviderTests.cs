using Dfe.Data.SearchPrototype.Web.Services;
using FluentAssertions;
using Xunit;

namespace Dfe.Data.SearchPrototype.Web.Tests.Unit.Services;

public class FacetNameToDisplayNameProviderTests
{
    [Theory]
    [InlineData("FACET1", "nice name 1")]
    [InlineData("FACET2", "nice name 2")]
    [InlineData("somethingelse", "somethingelse")]
    public void GetDisplayName_ReturnsDisplayName(string facetName, string expected)
    {
        // arrange
        Dictionary<string, string> stubNiceNameDictionary = new Dictionary<string, string>()
        {
            {"FACET1", "nice name 1"},
            {"FACET2", "nice name 2"}
        };

        var provider = new FacetNameToDisplayNameProvider(stubNiceNameDictionary);

        // act
        var result = provider.GetDisplayName(facetName);

        // assert
        result.Should().Be(expected);
    }
}
