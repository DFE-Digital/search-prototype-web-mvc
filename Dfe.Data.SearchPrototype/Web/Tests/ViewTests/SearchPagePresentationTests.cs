using AngleSharp.Dom;
using Dfe.Data.SearchPrototype.SearchForEstablishments.ByKeyword.Usecase;
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
    private SearchPageModel _searchPage;

    public SearchPagePresentationTests(WebApplicationFactory<Program> factory) : base(factory)
    {
        _searchPage = new SearchPageModel(_context);
    }

    [Fact]
    public async Task Search_WithNoEstablishmentResultsAndNoFacets_ShowsNoResultsText()
    {
        // arrange
        var useCaseResponse = SearchByKeywordResponseTestDouble.CreateWithNoResults();
        _useCase.Setup(useCase => useCase.HandleRequest(It.IsAny<SearchByKeywordRequest>()))
            .ReturnsAsync(useCaseResponse);

        // act
        await _searchPage.NavigateToPage($"{homeUri}?searchKeyword=anything");

        // assert
        _searchPage.Results!.NoResultsText.Should().Contain("Sorry no results found please amend your search criteria");
        _searchPage.Results!.ResultsText.Should().BeNull();
        _searchPage.FilterSection.Should().BeNull();
    }

    [Fact]
    public async Task Search_ByKeyword_WithSingleEstablishmentResult_Shows1ResultText()
    {
        // arrange
        var useCaseResponse = SearchByKeywordResponseTestDouble.CreateWithOneResult();
        _useCase.Setup(useCase => useCase.HandleRequest(It.IsAny<SearchByKeywordRequest>()))
            .ReturnsAsync(useCaseResponse);

        // act
        await _searchPage.NavigateToPage($"{homeUri}?searchKeyword=anything");

        // assert
        _searchPage.Results.Should().NotBeNull();
        _searchPage.Results!.ResultsText.Should().Be("1 Result");
        _searchPage.Results!.SearchResultLinks!.Count().Should().Be(1);
    }

    [Fact]
    public async Task Search_ByKeyword_WithMultipleEstablishmentResults_ShowsResults()
    {
        // arrange
        var useCaseResponse = SearchByKeywordResponseTestDouble.Create();
        _useCase.Setup(useCase => useCase.HandleRequest(It.IsAny<SearchByKeywordRequest>()))
            .ReturnsAsync(useCaseResponse);

        // act
        await _searchPage.NavigateToPage($"{homeUri}?searchKeyword=anything");

        // assert
        _searchPage.Results.Should().NotBeNull();
        _searchPage.Results!.ResultsText.Should().Contain("Results");
        _searchPage.Results!.SearchResultLinks!.Count().Should().Be(useCaseResponse.EstablishmentResults!.Establishments.Count);
    }

    [Fact]
    public async Task Search_ByKeyword_WithFacetedResults_ShowsFacets()
    {
        // arrange
        var useCaseResponse = SearchByKeywordResponseTestDouble.Create();
        _useCase.Setup(useCase => useCase.HandleRequest(It.IsAny<SearchByKeywordRequest>()))
            .ReturnsAsync(useCaseResponse);

        // act
        await _searchPage.NavigateToPage($"{homeUri}?searchKeyword=anything");

        // assert
        _searchPage.FilterSection.Should().NotBeNull();
        _searchPage.FilterSection!.FiltersHeading.Should().Be("Filters");

        var expectedFacets = useCaseResponse
            .EstablishmentFacetResults!
            .Facets
            .SelectMany(facet => facet.Results.Select(facetResult => new KeyValuePair<string, string>(facet.Name, facetResult.Value)));

        var pageFacets = _searchPage
            .FilterSection
            .Filters
            .SelectMany(filter => filter.CheckBoxValues.Select(checkBoxValue => new KeyValuePair<string, string>(filter.Name, checkBoxValue)));

        pageFacets.Should().BeEquivalentTo(expectedFacets);
    }

    [Fact]
    public async Task UseCaseResponse_WithSelectedFacets_ShowsFacetsAsSelected()
    {
        // arrange
        var useCaseResponse = SearchByKeywordResponseTestDouble.Create();
        _useCase.Setup(useCase => useCase.HandleRequest(It.IsAny<SearchByKeywordRequest>()))
            .ReturnsAsync(useCaseResponse);

        // act
        await _searchPage.NavigateToPage($"{homeUri}?searchKeyword=anything");
        var selectionFilters = new Dictionary<string, string>() {
            {
                useCaseResponse
                    .EstablishmentFacetResults!
                    .Facets
                    .First().Name,
                useCaseResponse
                    .EstablishmentFacetResults
                    .Facets
                    .First().Results.First().Value
            }
        };
         _searchPage.FilterSection!.SelectFilters(selectionFilters);
        await _searchPage.Form!.SubmitAsync();

        var resultsPageSelectedFacets = _searchPage.FilterSection!.Filters.SelectMany(filter => filter.SelectedCheckBoxValues);
        resultsPageSelectedFacets.Should().BeEquivalentTo(selectionFilters.Select(filter => filter.Value));
    }
}