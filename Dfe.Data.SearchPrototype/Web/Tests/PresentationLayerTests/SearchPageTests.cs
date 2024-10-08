﻿using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
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
    private const string uri = "http://localhost";
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
    public async Task Search_ByKeyword_WithNoEstablishmentResultsAndNoFacets_ShowsNoResultsText()
    {
        var useCaseResponse = SearchByKeywordResponseTestDouble.CreateWithNoResults();
        _useCase.Setup(useCase => useCase.HandleRequest(It.IsAny<SearchByKeywordRequest>()))
            .ReturnsAsync(useCaseResponse);

        var document = await _context.OpenAsync(uri);

        IHtmlFormElement form = document!.QuerySelector<IHtmlFormElement>(HomePage.SearchForm.Criteria)!;
        IDocument resultsPage = await form.SubmitAsync(new
        {
            searchKeyWord = "anything - I've mocked the response from the use-case regardless of the request"
        });

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

        var document = await _context.OpenAsync(uri);

        IHtmlFormElement form = document!.QuerySelector<IHtmlFormElement>(HomePage.SearchForm.Criteria)!;
        IDocument resultsPage = await form.SubmitAsync(new
        {
            searchKeyWord = "anything - I've mocked the response from the use-case regardless of the request"
        });

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

        var document = await _context.OpenAsync(uri);

        IHtmlFormElement form = document!.QuerySelector<IHtmlFormElement>(HomePage.SearchForm.Criteria)!;
        IDocument resultsPage = await form.SubmitAsync(new
        {
            searchKeyWord = "anything - I've mocked the response from the use-case regardless of the request"
        });

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

        var document = await _context.OpenAsync(uri);

        IHtmlFormElement form = document!.QuerySelector<IHtmlFormElement>(HomePage.SearchForm.Criteria)!;
        IDocument resultsPage = await form.SubmitAsync(new
        {
            searchKeyWord = "anything - I've mocked the response from the use-case regardless of the request"
        });

        var filtersHeading = resultsPage.QuerySelector(HomePage.FiltersHeading.Criteria);
        filtersHeading.Should().NotBeNull();
        filtersHeading!.TextContent.Should().Be("Filters");

        var facetContainer = resultsPage.QuerySelector(HomePage.FiltersContainer.Criteria);

        var expectedFacets = useCaseResponse.EstablishmentFacetResults!.Facets;
        foreach (var expectedFacet in expectedFacets)
        {
            // get the page element and its child nodes for each expected facet - this bit could be done with a page model
            var matchingFacetPageElement = facetContainer!
                .GetNodes<IHtmlFieldSetElement>()
                .Single(element => element.Id != null && element.Id == $"FacetName-{expectedFacet.Name}");
            var facetLegend = matchingFacetPageElement.GetNodes<IHtmlLegendElement>().Single();
            var facetInputElements = matchingFacetPageElement.GetNodes<IHtmlInputElement>();

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