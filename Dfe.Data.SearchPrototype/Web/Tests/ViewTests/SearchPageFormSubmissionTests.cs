using Dfe.Data.SearchPrototype.SearchForEstablishments.ByKeyword.Usecase;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.TestDoubles;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using Xunit;

namespace Dfe.Data.SearchPrototype.Web.Tests.ViewTests;

/// <summary>
///  Tests that submit the page form and check that the subsequest
///  request to the use-case is correct
/// </summary>
public class SearchPageFormSubmissionTests : SharedTestFixture
{
    public SearchPageFormSubmissionTests(WebApplicationFactory<Program> factory) : base(factory)
    {
    }

    [Fact]
    public async Task Search_SendsKeywordAndSelectedFiltersToUsecase()
    {
        // arrange
        var searchTerm = "e.g. School name";
        SearchByKeywordRequest? capturedUsecaseRequest = default;

        var useCaseResponse = SearchByKeywordResponseTestDouble.Create();
        _useCase.Setup(useCase => useCase.HandleRequest(It.IsAny<SearchByKeywordRequest>()))
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
        _searchPage.FilterSection!.SelectFilters(selectionFilters);
        // submit filtered search
        await _searchPage.Form!.SubmitAsync();

        // assert
        var usecaseSelectedFacets = capturedUsecaseRequest!
            .FilterRequests!
            .SelectMany(request => request
                .FilterValues
                .Where(filterValue => selectionFilters.Select(selectedFilter => selectedFilter.Value).Contains(filterValue.ToString()))
                );

        selectionFilters.Select(element => element.Value).Should().BeEquivalentTo(usecaseSelectedFacets.Select(facet => facet.ToString()));
        capturedUsecaseRequest!.SearchKeyword.Should().Be(searchTerm);
    }

    [Fact]
    public async Task Search_SelectClearFiltersButton_SendsExpectedRequestToUseCase()
    {
        // arrange
        var searchTerm = "e.g. School name";
        SearchByKeywordRequest? capturedUsecaseRequest = default;

        var useCaseResponse = SearchByKeywordResponseTestDouble.Create();
        _useCase.Setup(useCase => useCase.HandleRequest(It.IsAny<SearchByKeywordRequest>()))
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
        _searchPage.FilterSection!.SelectFilters(selectionFilters);
        await _searchPage.Form!.SubmitAsync();

        // assert checkboxes have been selected
        _searchPage.FilterSection!.Filters.SelectMany(filter => filter.SelectedCheckBoxValues).Should().NotBeEmpty();

        // clear the filters
        await _searchPage.Form!.SubmitClearAsync();

        // assert checkboxes are no longer selected
        var usecaseSelectedFacets = capturedUsecaseRequest!
            .FilterRequests.Should().BeEmpty();
    }
}
