using Dfe.Data.SearchPrototype.SearchForEstablishments.ByKeyword.Usecase;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.TestDoubles;
using FluentAssertions;
using Moq;
using Xunit;

namespace Dfe.Data.SearchPrototype.Web.Tests.ViewTests;

public class SearchPageTestsUsingPageModelInterface : AngleSharpIntegrationTestsBase
{
    private const string _homeUri = "http://localhost";

    public SearchPageTestsUsingPageModelInterface(IntegrationTestingWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task HomePage_HeadingIsThere()
    {
        await _searchPage.NavigateToPage(_homeUri);

        var heading = _searchPage.PageHeading;
        _searchPage.PageHeading.Should().Be("Search");
    }

    [Fact]
    public async Task Search_WithNoEstablishmentResultsAndNoFacets_ShowsNoResultsText()
    {
        // arrange
        var useCaseResponse = SearchByKeywordResponseTestDouble.CreateWithNoResults();
        _factory._useCase.Setup(useCase => useCase.HandleRequest(It.IsAny<SearchByKeywordRequest>()))
            .ReturnsAsync(useCaseResponse);

        // act
        await _searchPage.NavigateToPage($"{_homeUri}?searchKeyword=anything");

        // assert
        _searchPage.NoResultsText.Should().Contain("Sorry no results found please amend your search criteria");
        _searchPage.ResultsText.Should().BeNull();
        _searchPage.FilterSectionIsNullOrEmpty.Should().BeTrue();
    }

    [Fact]
    public async Task Search_ByKeyword_WithFacetedResults_ShowsFacets()
    {
        // arrange
        var useCaseResponse = SearchByKeywordResponseTestDouble.Create();
        _factory._useCase.Setup(useCase => useCase.HandleRequest(It.IsAny<SearchByKeywordRequest>()))
            .ReturnsAsync(useCaseResponse);

        // act
        await _searchPage.NavigateToPage($"{_homeUri}?searchKeyword=anything");

        // assert
        _searchPage.FilterSectionIsNullOrEmpty.Should().BeFalse();
        _searchPage.FilterSectionHeading.Should().Be("Filters");
        var expectedFacets = useCaseResponse
            .EstablishmentFacetResults!
            .Facets
            .SelectMany(facet => facet.Results.Select(facetResult => new KeyValuePair<string, string>(facet.Name, facetResult.Value)));
        _searchPage.Filters.Should().BeEquivalentTo(expectedFacets);
    }

    // Form submission tests
    // submit the form and check that the use-case is called with the data from the form submission
    [Fact]
    public async Task Search_SelectClearFiltersButton_SendsExpectedRequestToUseCase()
    {
        // arrange
        var searchTerm = "e.g. School name";
        SearchByKeywordRequest? capturedUsecaseRequest = default;

        var useCaseResponse = SearchByKeywordResponseTestDouble.Create();
        _factory._useCase.Setup(useCase => useCase.HandleRequest(It.IsAny<SearchByKeywordRequest>()))
            .Callback<SearchByKeywordRequest>((x) => capturedUsecaseRequest = x)
            .ReturnsAsync(useCaseResponse);

        // choose a filter to apply from those mocked in the use-case response 
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

        // act
        // navigate to results page with search keyword
        await _searchPage.NavigateToPage($"{_homeUri}?searchKeyword={searchTerm}");

        // select some filters
        _searchPage.SelectFilters(selectionFilters);
        await _searchPage.SubmitAsync();

        // assert checkboxes have been selected
        _searchPage.SelectedFilters.Should().NotBeEmpty();

        // clear the filters
        await _searchPage.SubmitClearAsync();

        // assert checkboxes are no longer selected
        var usecaseSelectedFacets = capturedUsecaseRequest!
            .FilterRequests.Should().BeEmpty();
    }
}
