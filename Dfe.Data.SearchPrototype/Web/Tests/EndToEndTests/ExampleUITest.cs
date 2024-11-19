using Dfe.Data.SearchPrototype.Web.Tests.Shared.Pages;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.Pages.Components;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.Pages.Components.ValueObject;
using Dfe.Testing.Pages;
using Dfe.Testing.Pages.DocumentQueryClient.Provider.WebDriver;
using Dfe.Testing.Pages.Pages;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Dfe.Data.SearchPrototype.Web.Tests.EndToEndTests;

public sealed class ExampleUITest : BaseEndToEndTest
{
    public ExampleUITest(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
    {
    }

    // TODO option->navigateToUri = true to dictate to the provider if it should create a document or reuse an existing document
    // internal logic across providers to have a CachedDocumentFactory?

    [Fact]
    public async Task Search_Submission_Displays_Search_Results()
    {
        HttpRequestMessage request = new()
        {
            RequestUri = new("https://localhost:7042/")
        };

        var homePage = await GetTestService<IPageFactory>().CreatePageAsync<HomePage>(request);
        homePage.Search.SearchForEstablishmentWith("Col");
        homePage.Search.SubmitSearch();
        IEnumerable<EstablishmentSearchResult> searchResults = homePage.Search.SearchResults.GetResults();
        searchResults.Should().NotBeNullOrEmpty();
        searchResults.ToList().ForEach((result) =>
        {
            result.Name.Should().NotBeNullOrEmpty();
            result.Urn.Should().NotBeNullOrEmpty();
            result.TypeOfEstablishment.Should().NotBeNullOrEmpty();
            result.Status.Should().NotBeNullOrEmpty();
            result.Phase.Should().NotBeNullOrEmpty();
        });
    }

    [Fact]
    public async Task Can_Apply_A_Facet_Value_In_A_Facet()
    {
        // Arrange
        HttpRequestMessage request = new()
        {
            RequestUri = new("https://localhost:7042/?searchKeyWord=Col")
        };

        var homePage = await GetTestService<IPageFactory>().CreatePageAsync<HomePage>(request);

        // Act
        List<Facet> facetsAvailable = homePage.Filters.GetDisplayedFacets().ToList();
        Facet facetBeingApplied = facetsAvailable.First();
        List<Facet> facetsNotBeingApplied = facetsAvailable.Where(t => t.Name != facetBeingApplied.Name).ToList();
        FacetValue facetValueToApply = facetBeingApplied.FacetValues.First();

        homePage.Filters.ApplyFacet(facetValueToApply).SubmitFilters();

        // Assert

        var displayedFacetsAfterApplying = homePage.Filters.GetDisplayedFacets();
        displayedFacetsAfterApplying.Count().Should().Be(facetsAvailable.Count());
        displayedFacetsAfterApplying.Select(t => t.Name).Should().BeEquivalentTo(facetsAvailable.Select(t => t.Name));

        // the selected facet should be applied and equivalen
        homePage.Filters.GetDisplayedFacets()
            .Where(t => t.Name == facetBeingApplied.Name)
            .Single()
            .FacetValues
            .Should().BeEquivalentTo(new[] { facetValueToApply });

        // the unselected facets counts will be updated on their label, but their values will remain
        // TODO maybe some work to separate the count displayed from the label of the facet
        var notBeingAppliedFacetsDisplayed = homePage.Filters.GetDisplayedFacets()
            .Where(t => t.Name != facetBeingApplied.Name)
            .SelectMany(t => t.FacetValues)
            .Select(t => t.Value)
            .Should()
            .BeEquivalentTo(
                facetsNotBeingApplied.SelectMany(t => t.FacetValues).Select(t => t.Value));
    }
}

