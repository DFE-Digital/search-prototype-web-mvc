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

public class SearchPageTests : IClassFixture<WebApplicationFactory<Program>>
{
    private const string uri = "http://localhost:5000";
    private Mock<IUseCase<SearchByKeywordRequest, SearchByKeywordResponse>> _useCase = new();

    private readonly WebApplicationFactory<Program> _factory;

    public SearchPageTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Search_ByKeyword_WithNoEstablishmentResultsAndNoFacets()
    {
        var useCaseResponse = SearchByKeywordResponseTestDouble.CreateWithNoResults();
        _useCase.Setup(useCase => useCase.HandleRequest(It.IsAny<SearchByKeywordRequest>()))
            .ReturnsAsync(useCaseResponse);
        var client = CreateHost().CreateClient();

        var response = await client.GetAsync(uri);
        var document = await HtmlHelpers.GetDocumentAsync(response);

        var formElement = document.QuerySelector<IHtmlFormElement>(SearchPage.SearchForm.Criteria);
        var formButton = document.QuerySelector<IHtmlButtonElement>(SearchPage.SearchButton.Criteria);

        var formResponse = await client.SendAsync(
            formElement!,
            formButton!,
            new Dictionary<string, string>
            {
                ["searchKeyWord"] = "anything - I've mocked the response from the use-case regardless of the request"
            });

        var resultsPage = await HtmlHelpers.GetDocumentAsync(formResponse);

        resultsPage.QuerySelector(SearchPage.SearchNoResultText.Criteria)!
            .TextContent.Should().Contain("Sorry no results found please amend your search criteria");
        resultsPage.QuerySelector(SearchPage.SearchResultsNumber.Criteria)!
            .Should().BeNull();
        resultsPage.GetElementById("filters-container").Should().BeNull();
    }

    [Fact]
    public async Task Search_ByKeyword_WithSingleEstablishmentResult()
    {
        var useCaseResponse = SearchByKeywordResponseTestDouble.CreateWithOneResult();
        _useCase.Setup(useCase => useCase.HandleRequest(It.IsAny<SearchByKeywordRequest>()))
            .ReturnsAsync(useCaseResponse);
        var client = CreateHost().CreateClient();

        var response = await client.GetAsync(uri);
        var document = await HtmlHelpers.GetDocumentAsync(response);

        var formElement = document.QuerySelector<IHtmlFormElement>(SearchPage.SearchForm.Criteria);
        var formButton = document.QuerySelector<IHtmlButtonElement>(SearchPage.SearchButton.Criteria);

        var formResponse = await client.SendAsync(
            formElement!,
            formButton!,
            new Dictionary<string, string>
            {
                ["searchKeyWord"] = "anything - I've mocked the response from the use-case regardless of the request"
            });

        var resultsPage = await HtmlHelpers.GetDocumentAsync(formResponse);

        resultsPage.QuerySelector(SearchPage.SearchResultsNumber.Criteria)!
            .TextContent.Should().Be("1 Result");
        resultsPage.QuerySelector(SearchPage.SearchResultsContainer.Criteria)!
            .GetMultipleElements(SearchPage.SearchResultLinks.Criteria)
            .Count().Should().Be(1);
    }

    [Fact]
    public async Task Search_ByKeyword_WithMultipleEstablishmentResults()
    {
        var useCaseResponse = SearchByKeywordResponseTestDouble.Create();
        _useCase.Setup(useCase => useCase.HandleRequest(It.IsAny<SearchByKeywordRequest>()))
            .ReturnsAsync(useCaseResponse);
        var client = CreateHost().CreateClient();

        var response = await client.GetAsync(uri);
        var document = await HtmlHelpers.GetDocumentAsync(response);

        var formElement = document.QuerySelector<IHtmlFormElement>(SearchPage.SearchForm.Criteria);
        var formButton = document.QuerySelector<IHtmlButtonElement>(SearchPage.SearchButton.Criteria);

        var formResponse = await client.SendAsync(
            formElement!,
            formButton!,
            new Dictionary<string, string>
            {
                ["searchKeyWord"] = "anything - I've mocked the response from the use-case regardless of the request"
            });

        var resultsPage = await HtmlHelpers.GetDocumentAsync(formResponse);

        resultsPage.QuerySelector(SearchPage.SearchResultsNumber.Criteria)!
            .TextContent.Should().Contain("Results");
        resultsPage.QuerySelector(SearchPage.SearchResultsContainer.Criteria)!
            .GetMultipleElements(SearchPage.SearchResultLinks.Criteria)
            .Count().Should().Be(useCaseResponse.EstablishmentResults!.Establishments.Count);
    }

    //[Fact]
    //public async Task Search_ByKeyword_WithFacetedResults()
    //{
    //    var useCaseResponse = SearchByKeywordResponseTestDouble.Create();
    //    _useCase.Setup(useCase => useCase.HandleRequest(It.IsAny<SearchByKeywordRequest>()))
    //        .ReturnsAsync(useCaseResponse);
    //    var client = CreateHost().CreateClient();

    //    var response = await client.GetAsync(uri);
    //    var document = await HtmlHelpers.GetDocumentAsync(response);

    //    var formElement = document.QuerySelector<IHtmlFormElement>(SearchPage.SearchForm.Criteria);
    //    var formButton = document.QuerySelector<IHtmlButtonElement>(SearchPage.SearchButton.Criteria);

    //    var formResponse = await client.SendAsync(
    //        formElement!,
    //        formButton!,
    //        new Dictionary<string, string>
    //        {
    //            ["searchKeyWord"] = "anything - I've mocked the response from the use-case regardless of the request"
    //        });

    //    var resultsPage = await HtmlHelpers.GetDocumentAsync(formResponse);

    //    var filtersHeading = resultsPage.QuerySelector(SearchPage.FiltersHeading.Criteria);
    //    filtersHeading.Should().NotBeNull();
    //    filtersHeading!.TextContent.Should().Be("Filters");

    //    var expectedFacetNames = useCaseResponse.EstablishmentFacetResults!.Facets.Select(f => f.Name).ToArray();
    //    var resultFacetNames = resultsPage.GetElementsByTagName("legend").Select(x => x.InnerHtml.Trim());

    //    foreach (var facetName in expectedFacetNames)
    //    {
    //        resultFacetNames.Where(x => x == facetName).First().Should().NotBeNull();
    //    }
    //}

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