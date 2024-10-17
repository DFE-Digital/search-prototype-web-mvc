using AngleSharp;
using AngleSharp.Dom;
using Dfe.Data.SearchPrototype.SearchForEstablishments.ByKeyword.Usecase;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.Pages;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.TestDoubles;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using Xunit;

namespace Dfe.Data.SearchPrototype.Web.Tests.ViewTests;

public class SearchPageFormSubmissionTests : SharedTestFixture
{
    private const string homeUri = "http://localhost";

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

        // act
        // navigate to results page with search keyword)
        var document = await _context.OpenAsync($"{homeUri}?searchKeyword={searchTerm}");
        // select some filters
        var checkedBoxes = document.SelectFilters();
        // submit filtered search
        IDocument resultsPage = await document.SubmitSearchAsync();

        // assert
        var usecaseSelectedFacets = capturedUsecaseRequest!
            .FilterRequests!
            .SelectMany(request => request
                .FilterValues
                .Where(filterValue => checkedBoxes.Select(checkbox => checkbox.Value).Contains(filterValue.ToString()))
                );

        checkedBoxes.Select(element => element.Value).Should().BeEquivalentTo(usecaseSelectedFacets.Select(facet => facet.ToString()));
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

        // act
        // navigate to results page with search keyword)
        var document = await _context.OpenAsync($"{homeUri}?searchKeyword={searchTerm}");

        // select some filters
        var checkedBoxes = document.SelectFilters();
        IDocument resultsPage = await document.SubmitSearchAsync();
        Assert.NotEmpty(checkedBoxes);

        // once form submitted with filters we want to clear them
        IDocument clearedFiltersPage = await document.SubmitClearAsync();

        // assert
        var usecaseSelectedFacets = capturedUsecaseRequest!
            .FilterRequests.Should().BeEmpty();
    }
}
