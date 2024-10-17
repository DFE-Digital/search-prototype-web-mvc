using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Io;
using AngleSharp.Io.Network;
using Dfe.Data.SearchPrototype.Common.CleanArchitecture.Application.UseCase;
using Dfe.Data.SearchPrototype.SearchForEstablishments.ByKeyword.Usecase;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.Pages;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.TestDoubles;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using Xunit;

namespace Dfe.Data.SearchPrototype.Web.Tests.ViewTests;

public class SearchPageFormSubmissionTests : IClassFixture<WebApplicationFactory<Dfe.Data.SearchPrototype.Web.Program>>
{
    private const string homeUri = "http://localhost";
    private Mock<IUseCase<SearchByKeywordRequest, SearchByKeywordResponse>> _useCase = new();
    private readonly HttpClient _client;
    private readonly IBrowsingContext _context;

    private readonly WebApplicationFactory<Program> _factory;
    public SearchPageFormSubmissionTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = CreateHost().CreateClient();
        _context = CreateBrowsingContext(_client);
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

        //var clearedCheckedBoxes = document.GetFirstFacetCheckBoxes();

        //Assert.Empty(clearedCheckedBoxes);

        // assert
        var usecaseSelectedFacets = capturedUsecaseRequest!
            .FilterRequests.Should().BeNull();
    }

    private IBrowsingContext CreateBrowsingContext(HttpClient httpClient)
    {
        var config = AngleSharp.Configuration.Default
            .WithRequester(new HttpClientRequester(httpClient))
            .WithDefaultLoader(new LoaderOptions { IsResourceLoadingEnabled = true });

        return BrowsingContext.New(config);
    }

    private WebApplicationFactory<Program> CreateHost()
    {
        return _factory.WithWebHostBuilder(
            (IWebHostBuilder builder) =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.RemoveAll<IUseCase<SearchByKeywordRequest, SearchByKeywordResponse>>();
                    services.AddScoped<IUseCase<SearchByKeywordRequest, SearchByKeywordResponse>>(provider => _useCase.Object);
                });
            }
        );
    }
}
