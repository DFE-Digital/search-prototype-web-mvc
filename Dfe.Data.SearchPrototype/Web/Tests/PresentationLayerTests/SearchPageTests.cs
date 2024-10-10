using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Io;
using AngleSharp.Io.Network;
using Dfe.Data.SearchPrototype.Common.CleanArchitecture.Application.UseCase;
using Dfe.Data.SearchPrototype.SearchForEstablishments.ByKeyword.Usecase;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.Helpers;
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

namespace Dfe.Data.SearchPrototype.Web.Tests.PresentationLayerTests;

public class SearchPageTests : IClassFixture<WebApplicationFactory<Dfe.Data.SearchPrototype.Web.Program>>
{
    private const string homeUri = "http://localhost";
    private Mock<IUseCase<SearchByKeywordRequest, SearchByKeywordResponse>> _useCase = new();
    private readonly HttpClient _client;
    private readonly IBrowsingContext _context;

    private readonly WebApplicationFactory<Program> _factory;
        public SearchPageTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = CreateHost().CreateClient();
        _context = CreateBrowsingContext(_client);
    }

    [Fact]
    public async Task Search_WithNoEstablishmentResultsAndNoFacets_ShowsNoResultsText()
    {
        // arrange
        var useCaseResponse = SearchByKeywordResponseTestDouble.CreateWithNoResults();
        _useCase.Setup(useCase => useCase.HandleRequest(It.IsAny<SearchByKeywordRequest>()))
            .ReturnsAsync(useCaseResponse);

        // act
        var landingPage = await _context.OpenAsync($"{homeUri}");
        landingPage.TypeIntoSearchBox("search terms");
        var resultsPage = await landingPage.SubmitSearchAsync();

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
        var landingPage = await _context.OpenAsync($"{homeUri}");
        landingPage.TypeIntoSearchBox("search terms");
        var resultsPage = await landingPage.SubmitSearchAsync();

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
        var landingPage = await _context.OpenAsync($"{homeUri}");
        landingPage.TypeIntoSearchBox("search terms");
        var resultsPage = await landingPage.SubmitSearchAsync();

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
        var landingPage = await _context.OpenAsync($"{homeUri}");
        landingPage.TypeIntoSearchBox("search terms");
        var resultsPage = await landingPage.SubmitSearchAsync();

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
    public async Task Search_ByKeyword_WithSelectedFacets_ShowsFacetsAsSelected()
    {
        // arrange
        var useCaseResponse = SearchByKeywordResponseTestDouble.Create();
        _useCase.Setup(useCase => useCase.HandleRequest(It.IsAny<SearchByKeywordRequest>()))
            .ReturnsAsync(useCaseResponse);

        // act
        var landingPage = await _context.OpenAsync($"{homeUri}");
        landingPage.TypeIntoSearchBox("search terms");
        var resultsPage = await landingPage.SubmitSearchAsync();
        var checkedBoxes = resultsPage.SelectFilters();
        IDocument filteredResultsPage = await resultsPage.SubmitSearchAsync();

        var resultsPageSelectedFacets = filteredResultsPage.GetFacets().SelectMany(facet => facet.GetCheckBoxes().Where(checkBox => checkBox.IsChecked));
        resultsPageSelectedFacets.Select(facet => facet.Value).Should().BeEquivalentTo(checkedBoxes.Select(facet => facet.Value));
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