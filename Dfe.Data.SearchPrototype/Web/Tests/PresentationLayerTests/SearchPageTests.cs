using AngleSharp;
using AngleSharp.Dom;
using Dfe.Data.SearchPrototype.Common.CleanArchitecture.Application.UseCase;
using Dfe.Data.SearchPrototype.SearchForEstablishments.ByKeyword.Usecase;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.TestDoubles;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using Xunit;

namespace Dfe.Data.SearchPrototype.Web.Tests.PresentationLayerTests;

public class SearchPageTests : IClassFixture<WebApplicationFactory<Dfe.Data.SearchPrototype.Web.Program>>
{
    private Mock<IUseCase<SearchByKeywordRequest, SearchByKeywordResponse>> _useCase = new();
    private HttpClient _httpClient;
    private readonly IBrowsingContext _browsingContext;

    private readonly WebApplicationFactory<Program> _factory;
    
    public SearchPageTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _httpClient = ConfigureHost().CreateClient();
        _browsingContext = CreateBrowsingContext(_httpClient);
    }

    [Fact]
    public async Task Search_ByKeyword_WithNoEstablishmentResultsAndNoFacets_ShowsNoResultsText()
    {
        var useCaseResponse = SearchByKeywordResponseTestDouble.CreateWithNoResults();
        _useCase.Setup(useCase => useCase.HandleRequest(It.IsAny<SearchByKeywordRequest>()))
            .ReturnsAsync(useCaseResponse);

        // act 
        var document = await NavigateToPage("Home");

        var searchPage = new SearchPageModel(document);

        var formResponse = await new SearchPageModel(document)
                .Form
                .SubmitSearch(_httpClient, new Dictionary<string, string>
                {
                    ["searchKeyWord"] = "anything - I've mocked the response from the use-case regardless of the request"
                });

        var searchResponse = await _browsingContext.OpenAsync(req => req.Content(formResponse));
        var resultsPage = new SearchPageModel(searchResponse);

        resultsPage.Results.NoResultsText!.Should().Contain("Sorry no results found please amend your search criteria");
        resultsPage.Results.ResultsText!.Should().BeNull();
        searchResponse.GetElementById("filters-container").Should().BeNull();
    }

    [Fact]
    public async Task Search_ByKeyword_WithSingleEstablishmentResult_Shows1ResultText()
    {
        var useCaseResponse = SearchByKeywordResponseTestDouble.CreateWithOneResult();
        _useCase.Setup(useCase => useCase.HandleRequest(It.IsAny<SearchByKeywordRequest>()))
            .ReturnsAsync(useCaseResponse);

        // act 
        var document = await NavigateToPage("Home");
        var formResponse = await new SearchPageModel(document)
                .Form
                .SubmitSearch(_httpClient, new Dictionary<string, string>
                {
                    ["searchKeyWord"] = "anything - I've mocked the response from the use-case regardless of the request"
                });

        var searchResponse = await _browsingContext.OpenAsync(req => req.Content(formResponse));

        var resultsPage = new SearchPageModel(searchResponse);

        resultsPage.Results.ResultsText.Should().Be("1 Result");
        resultsPage.Results.SearchResultLinks!.Count().Should().Be(1);
    }

    [Fact]
    public async Task Search_ByKeyword_WithMultipleEstablishmentResults_ShowsResults()
    {
        var useCaseResponse = SearchByKeywordResponseTestDouble.Create();
        _useCase.Setup(useCase => useCase.HandleRequest(It.IsAny<SearchByKeywordRequest>()))
            .ReturnsAsync(useCaseResponse);

        // act 
        var document = await NavigateToPage("Home");
        var formResponse = await new SearchPageModel(document)
                .Form
                .SubmitSearch(_httpClient, new Dictionary<string, string>
                {
                    ["searchKeyWord"] = "anything - I've mocked the response from the use-case regardless of the request"
                });

        var searchResponse = await _browsingContext.OpenAsync(req => req.Content(formResponse));

        var resultsPage = new SearchPageModel(searchResponse);

        resultsPage.Results.ResultsText.Should().Contain("Results");
        resultsPage.Results.SearchResultLinks!.Count().Should().Be(useCaseResponse.EstablishmentResults!.Establishments.Count);
    }

    [Fact]
    public async Task Search_ByKeyword_WithFacetedResults_ShowsFacets()
    {
        var useCaseResponse = SearchByKeywordResponseTestDouble.Create();
        _useCase.Setup(useCase => useCase.HandleRequest(It.IsAny<SearchByKeywordRequest>()))
            .ReturnsAsync(useCaseResponse);

        // act 
        var document = await NavigateToPage("Home");
        var formResponse = await new SearchPageModel(document)
                .Form
                .SubmitSearch(_httpClient, new Dictionary<string, string>
                {
                    ["searchKeyWord"] = "anything - I've mocked the response from the use-case regardless of the request"
                });

        var searchResponse = await _browsingContext.OpenAsync(req => req.Content(formResponse));

        var resultsPage = new SearchPageModel(searchResponse);

        // assert
        var filtersHeading = resultsPage.Filters.FiltersHeading.Should().Be("Filters");

        var expectedFacets = useCaseResponse.EstablishmentFacetResults!.Facets;
        var resultFacetNames = resultsPage.Filters.FacetNames;

        foreach (var expectedFacet in expectedFacets)
        {
            var filterBoxesForFacet = resultsPage.Filters.FilterBoxesForFacet(expectedFacet.Name);
            foreach(var expectedFacetValue in expectedFacet.Results)
            {
                var matchedFacetValue = filterBoxesForFacet.Single(inputElement => inputElement!.Value == expectedFacetValue.Value);
                matchedFacetValue.Should().NotBeNull();
            }
        }
    }

    private WebApplicationFactory<Program> ConfigureHost()
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

    protected async Task<IDocument> NavigateToPage(string webPage)
    {
        var response = await _httpClient.GetAsync(webPage);

        var DOM = await response.Content.ReadAsStringAsync();

        return await _browsingContext.OpenAsync(req => req.Content(DOM));
    }

    private IBrowsingContext CreateBrowsingContext(HttpClient httpClient)
    {
        var config = AngleSharp.Configuration.Default;
        return BrowsingContext.New(config);
    }
}