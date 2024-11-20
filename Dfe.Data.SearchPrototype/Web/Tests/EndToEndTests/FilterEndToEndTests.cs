using Dfe.Data.SearchPrototype.Web.Tests.EndToEndTests.Options;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.Pages;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.Pages.Components.ValueObject;
using Dfe.Testing.Pages.DocumentQueryClient.Pages;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Dfe.Data.SearchPrototype.Web.Tests.EndToEndTests;

public sealed class FilterEndToEndTests : BaseEndToEndTest
{
    public FilterEndToEndTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
    {
    }

    [Fact]
    public async Task Can_Apply_A_Facet_Value_In_A_Facet()
    {
        // Arrange
        ApplicationOptions options = GetTestService<ApplicationOptions>();
        HttpRequestMessage request = new()
        {
            RequestUri = new Uri(options.BaseUrl, "?searchKeyWord=Col")
        };

        var homePage = await GetTestService<IPageFactory>().CreatePageAsync<HomePage>(request);

        List<Facet> facetsAvailable = homePage.Filters.GetDisplayedFacets().ToList();
        Facet facetBeingApplied = facetsAvailable.First();
        FacetValue facetValueToApply = facetBeingApplied.FacetValues.First();
        List<Facet> facetsNotBeingApplied = facetsAvailable.Where(t => t.Name != facetBeingApplied.Name).ToList();

        // Act
        homePage.Filters.ApplyFacet(facetValueToApply).SubmitFilters();

        // Assert

        // Top level facets should have the same count and labels
        var displayedFacetsAfterApplying = homePage.Filters.GetDisplayedFacets();
        displayedFacetsAfterApplying.Count().Should().Be(facetsAvailable.Count()); // assumption a single facet value in each remains
        displayedFacetsAfterApplying.Select(t => t.Name).Should().BeEquivalentTo(facetsAvailable.Select(t => t.Name));

        // the selected facet should have been applied and be the only facetValue displayed in the facet
        var actualFacetBeingApplied = homePage.Filters.GetDisplayedFacets().Where(t => t.Name == facetBeingApplied.Name).ToList();
        actualFacetBeingApplied.Single().FacetValues.Should().BeEquivalentTo(new[] { facetValueToApply });

        homePage.Search.SearchResults.GetResults().Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Clear_Facets_And_Applied_FacetValues()
    {
        // Arrange
        ApplicationOptions options = GetTestService<ApplicationOptions>();
        HttpRequestMessage request = new()
        {
            RequestUri = new Uri(options.BaseUrl, "?searchKeyWord=Col")
        };

        var homePage = await GetTestService<IPageFactory>().CreatePageAsync<HomePage>(request);

        List<Facet> facetsAvailable = homePage.Filters.GetDisplayedFacets().ToList();
        FacetValue firstFacetWithAnyFacetValue = facetsAvailable.First().FacetValues.First();
        FacetValue secondFacetWithAnyFacetValue = facetsAvailable[1].FacetValues.First();

        homePage.Filters
            .ApplyFacet(firstFacetWithAnyFacetValue)
            .ApplyFacet(secondFacetWithAnyFacetValue)
            .SubmitFilters();

        // Guard to check the facetValues are selected
        homePage.Filters.GetDisplayedFacets()
            .SelectMany(t => t.FacetValues)
            .Should().HaveCount(2);

        // Act
        homePage.Filters.ClearFilters();

        // Assert
        homePage.Filters.GetDisplayedFacets().Should().BeEquivalentTo(facetsAvailable);
        homePage.Search.SearchResults.GetResults().Should().NotBeNullOrEmpty();
    }
}
