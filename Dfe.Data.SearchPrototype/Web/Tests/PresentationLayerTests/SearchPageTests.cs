using AngleSharp.Dom;
using AngleSharp.Html.Dom;
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
    private const string uri = "http://localhost:5000";
    private Mock<IUseCase<SearchByKeywordRequest, SearchByKeywordResponse>> _useCase = new();
    private readonly HttpClient _client;

    private readonly WebApplicationFactory<Program> _factory;
        public SearchPageTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = CreateHost().CreateClient();
    }

    [Fact]
    public async Task Search_ByKeyword_WithNoEstablishmentResultsAndNoFacets_ShowsNoResultsText()
    {
        var useCaseResponse = SearchByKeywordResponseTestDouble.CreateWithNoResults();
        _useCase.Setup(useCase => useCase.HandleRequest(It.IsAny<SearchByKeywordRequest>()))
            .ReturnsAsync(useCaseResponse);

        var response = await _client.GetAsync(uri);
        var document = await HtmlHelpers.GetDocumentAsync(response);

        var formElement = document.QuerySelector<IHtmlFormElement>(HomePage.SearchForm.Criteria);
        var formButton = document.QuerySelector<IHtmlButtonElement>(HomePage.SearchButton.Criteria);

        var formResponse = await _client.SendAsync(
            formElement!,
            formButton!,
            new Dictionary<string, string>
            {
                ["searchKeyWord"] = "anything - I've mocked the response from the use-case regardless of the request"
            });

        var resultsPage = await HtmlHelpers.GetDocumentAsync(formResponse);

        resultsPage.QuerySelector(HomePage.SearchNoResultText.Criteria)!
            .TextContent.Should().Contain("Sorry no results found please amend your search criteria");
        resultsPage.QuerySelector(HomePage.SearchResultsNumber.Criteria)!
            .Should().BeNull();
        resultsPage.GetElementById("filters-container").Should().BeNull();
    }

    [Fact]
    public async Task Search_ByKeyword_WithSingleEstablishmentResult_Shows1ResultText()
    {
        var useCaseResponse = SearchByKeywordResponseTestDouble.CreateWithOneResult();
        _useCase.Setup(useCase => useCase.HandleRequest(It.IsAny<SearchByKeywordRequest>()))
            .ReturnsAsync(useCaseResponse);

        var response = await _client.GetAsync(uri);
        var document = await HtmlHelpers.GetDocumentAsync(response);

        var formElement = document.QuerySelector<IHtmlFormElement>(HomePage.SearchForm.Criteria);
        var formButton = document.QuerySelector<IHtmlButtonElement>(HomePage.SearchButton.Criteria);

        var formResponse = await _client.SendAsync(
            formElement!,
            formButton!,
            new Dictionary<string, string>
            {
                ["searchKeyWord"] = "anything - I've mocked the response from the use-case regardless of the request"
            });

        var resultsPage = await HtmlHelpers.GetDocumentAsync(formResponse);

        resultsPage.QuerySelector(HomePage.SearchResultsNumber.Criteria)!
            .TextContent.Should().Be("1 Result");
        resultsPage.QuerySelector(HomePage.SearchResultsContainer.Criteria)!
            .GetMultipleElements(HomePage.SearchResultLinks.Criteria)
            .Count().Should().Be(1);
    }

    [Fact]
    public async Task Search_ByKeyword_WithMultipleEstablishmentResults_ShowsResults()
    {
        var useCaseResponse = SearchByKeywordResponseTestDouble.Create();
        _useCase.Setup(useCase => useCase.HandleRequest(It.IsAny<SearchByKeywordRequest>()))
            .ReturnsAsync(useCaseResponse);

        var response = await _client.GetAsync(uri);
        var document = await HtmlHelpers.GetDocumentAsync(response);

        var formElement = document.QuerySelector<IHtmlFormElement>(HomePage.SearchForm.Criteria);
        var formButton = document.QuerySelector<IHtmlButtonElement>(HomePage.SearchButton.Criteria);

        var formResponse = await _client.SendAsync(
            formElement!,
            formButton!,
            new Dictionary<string, string>
            {
                ["searchKeyWord"] = "anything - I've mocked the response from the use-case regardless of the request"
            });

        var resultsPage = await HtmlHelpers.GetDocumentAsync(formResponse);

        resultsPage.QuerySelector(HomePage.SearchResultsNumber.Criteria)!
            .TextContent.Should().Contain("Results");
        resultsPage.QuerySelector(HomePage.SearchResultsContainer.Criteria)!
            .GetMultipleElements(HomePage.SearchResultLinks.Criteria)
            .Count().Should().Be(useCaseResponse.EstablishmentResults!.Establishments.Count);
    }

    [Fact]
    public async Task Search_ByKeyword_WithFacetedResults_ShowsFacets()
    {
        var useCaseResponse = SearchByKeywordResponseTestDouble.Create();
        _useCase.Setup(useCase => useCase.HandleRequest(It.IsAny<SearchByKeywordRequest>()))
            .ReturnsAsync(useCaseResponse);

        var response = await _client.GetAsync(uri);
        var document = await HtmlHelpers.GetDocumentAsync(response);

        var formElement = document.QuerySelector<IHtmlFormElement>(HomePage.SearchForm.Criteria);
        var formButton = document.QuerySelector<IHtmlButtonElement>(HomePage.SearchButton.Criteria);

        var formResponse = await _client.SendAsync(
            formElement!,
            formButton!,
            new Dictionary<string, string>
            {
                ["searchKeyWord"] = "anything - I've mocked the response from the use-case regardless of the request"
            });

        var resultsPage = await HtmlHelpers.GetDocumentAsync(formResponse);

        var filtersHeading = resultsPage.QuerySelector(HomePage.FiltersHeading.Criteria);
        filtersHeading.Should().NotBeNull();
        filtersHeading!.TextContent.Should().Be("Filters");

        var expectedFacets = useCaseResponse.EstablishmentFacetResults!.Facets;
        var resultFacetNames = resultsPage.GetElementsByTagName("legend").Select(x => x.InnerHtml.Trim());

        foreach (var expectedFacet in expectedFacets)
        {
            var facetInputElements = resultsPage.All
                .Where(element => element.Id != null && element.Id.Contains($"selectedFacets_{expectedFacet.Name}"))
                .Select(e => e as IHtmlInputElement);

            foreach(var expectedFacetValue in expectedFacet.Results)
            {
                var matchedFacet = facetInputElements.Single(inputElement => inputElement!.Value == expectedFacetValue.Value);
                matchedFacet.Should().NotBeNull();
            }
        }
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