using AngleSharp;
using AngleSharp.Dom;
using Dfe.Data.SearchPrototype.SearchForEstablishments.ByKeyword.Usecase;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.Helpers;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.Pages;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.TestDoubles;
using Dfe.Data.SearchPrototype.Web.Tests.ViewTests;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using Xunit;

namespace Dfe.Data.SearchPrototype.Web.Tests.PresentationLayerTests;

public class SearchPagePresentationTests : SharedTestFixture
{
    private const string homeUri = "http://localhost";

    public SearchPagePresentationTests(WebApplicationFactory<Program> factory) :base(factory)
    {
        
    }

    [Fact]
    public async Task Search_WithNoEstablishmentResultsAndNoFacets_ShowsNoResultsText()
    {
        // arrange
        var useCaseResponse = SearchByKeywordResponseTestDouble.CreateWithNoResults();
        _useCase.Setup(useCase => useCase.HandleRequest(It.IsAny<SearchByKeywordRequest>()))
            .ReturnsAsync(useCaseResponse);

        // act
        var resultsPage = await _context.OpenAsync($"{homeUri}?searchKeyword=anything");

        // assert
        resultsPage.QuerySelector(HomePage.SearchNoResultText.Criteria)!
            .TextContent.Should().Contain("Sorry no results found please amend your search criteria");
        resultsPage.QuerySelector(HomePage.SearchResultsNumber.Criteria)!
            .Should().BeNull();
        resultsPage.GetElementById("filters-container").Should().BeNull();
    }

    [Fact]
    public async Task Search_ByKeyword_WithSingleEstablishmentResult_Shows1ResultText()
    {
        // arrange
        var useCaseResponse = SearchByKeywordResponseTestDouble.CreateWithOneResult();
        _useCase.Setup(useCase => useCase.HandleRequest(It.IsAny<SearchByKeywordRequest>()))
            .ReturnsAsync(useCaseResponse);

        // act
        var resultsPage = await _context.OpenAsync($"{homeUri}?searchKeyword=anything");

        // assert
        resultsPage.QuerySelector(HomePage.SearchResultsNumber.Criteria)!
            .TextContent.Should().Be("1 Result");
        resultsPage.QuerySelector(HomePage.SearchResultsContainer.Criteria)!
            .GetMultipleElements(HomePage.SearchResultLinks.Criteria)
            .Count().Should().Be(1);
    }

    [Fact]
    public async Task Search_ByKeyword_WithMultipleEstablishmentResults_ShowsResults()
    {
        // arrange
        var useCaseResponse = SearchByKeywordResponseTestDouble.Create();
        _useCase.Setup(useCase => useCase.HandleRequest(It.IsAny<SearchByKeywordRequest>()))
            .ReturnsAsync(useCaseResponse);

        // act
        var resultsPage = await _context.OpenAsync($"{homeUri}?searchKeyword=anything");

        // assert
        resultsPage.QuerySelector(HomePage.SearchResultsNumber.Criteria)!
            .TextContent.Should().Contain("Results");
        resultsPage.QuerySelector(HomePage.SearchResultsContainer.Criteria)!
            .GetMultipleElements(HomePage.SearchResultLinks.Criteria)
            .Count().Should().Be(useCaseResponse.EstablishmentResults!.Establishments.Count);
    }

    [Fact]
    public async Task Search_ByKeyword_WithFacetedResults_ShowsFacets()
    {
        // arrange
        var useCaseResponse = SearchByKeywordResponseTestDouble.Create();
        _useCase.Setup(useCase => useCase.HandleRequest(It.IsAny<SearchByKeywordRequest>()))
            .ReturnsAsync(useCaseResponse);

        // act
        var resultsPage = await _context.OpenAsync($"{homeUri}?searchKeyword=anything");

        // assert
        var filtersHeading = resultsPage.QuerySelector(HomePage.FiltersHeading.Criteria);
        filtersHeading.Should().NotBeNull();
        filtersHeading!.TextContent.Should().Be("Filters");

        var expectedFacets = useCaseResponse.EstablishmentFacetResults!.Facets;
        foreach (var expectedFacet in expectedFacets)
        {
            // get the page element and its child nodes for each expected facet - this bit could be done with a page model
            var matchingFacetPageElement = resultsPage.GetFacet(expectedFacet.Name);
            var facetLegend = matchingFacetPageElement.GetLegend();
            var facetInputElements = matchingFacetPageElement.GetCheckBoxes();

            // assert the facet name is on the page
            facetLegend.TextContent.Trim().Should().Be(expectedFacet.Name);
            foreach (var expectedFacetValue in expectedFacet.Results)
            {
                // assert that each expected facet value appears on the page under the correct facet name
                var matchedFacet = facetInputElements.Single(inputElement => inputElement!.Value == expectedFacetValue.Value);
                matchedFacet.Should().NotBeNull();
            }
        }
    }

    [Fact]
    public async Task UseCaseResponse_WithSelectedFacets_ShowsFacetsAsSelected()
    {
        // arrange
        var useCaseResponse = SearchByKeywordResponseTestDouble.Create();
        _useCase.Setup(useCase => useCase.HandleRequest(It.IsAny<SearchByKeywordRequest>()))
            .ReturnsAsync(useCaseResponse);

        // act
        var resultsPage = await _context.OpenAsync($"{homeUri}?searchKeyword=anything");
        var checkedBoxes = resultsPage.SelectFilters();
        IDocument filteredResultsPage = await resultsPage.SubmitSearchAsync();

        var resultsPageSelectedFacets = filteredResultsPage.GetFacets().SelectMany(facet => facet.GetCheckBoxes().Where(checkBox => checkBox.IsChecked));
        resultsPageSelectedFacets.Select(facet => facet.Value).Should().BeEquivalentTo(checkedBoxes.Select(facet => facet.Value));
    }
}