using Dfe.Data.SearchPrototype.Infrastructure.Tests.TestDoubles.Shared;
using Dfe.Data.SearchPrototype.SearchForEstablishments.ByKeyword.ServiceAdapters;
using Dfe.Data.SearchPrototype.SearchForEstablishments.ByKeyword.Usecase;
using Dfe.Data.SearchPrototype.SearchForEstablishments.Models;
using Dfe.Data.SearchPrototype.Tests.SearchForEstablishments.TestDoubles;
using Dfe.Data.SearchPrototype.Web.Controllers;
using Dfe.Data.SearchPrototype.Web.Mappers;
using Dfe.Data.SearchPrototype.Web.Tests.PartialIntegrationTests.TestDoubles;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.TestDoubles;
using Dfe.Data.SearchPrototype.Web.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Dfe.Data.SearchPrototype.Web.Models.Factories;

namespace Dfe.Data.SearchPrototype.Web.Tests.PartialIntegrationTests;

public class HomeControllerTests
{
    private readonly Mock<ILogger<HomeController>> _logger = new();

    [Fact]
    public async Task Index_WithSearchTerm_ReturnsModel()
    {
        // arrange
        SearchForEstablishments.Models.SearchResults stubSearchResults = new() {
            Establishments = EstablishmentResultsTestDouble.Create() };

        ISearchServiceAdapter mockSearchServiceAdapter =
            SearchServiceAdapterTestDouble.MockFor(stubSearchResults).Object;

        SearchByKeywordUseCase useCase = new (
            mockSearchServiceAdapter,
            IOptionsTestDouble.IOptionsMockFor(SearchByKeywordCriteriaTestDouble.Create()));

        HomeController controller =
            new(_logger.Object, useCase,
                new SearchResultsFactory(
                    new EstablishmentResultsToEstablishmentsViewModelMapper(),
                    new FacetsAndSelectedFacetsToFacetsViewModelMapper()
                ),
                new SelectedFacetsToFilterRequestsMapper());

        // act
        IActionResult result = await controller.Index(new SearchRequest() { SearchKeyword = "searchTerm" });

        // assert
        ViewResult viewResult = Assert.IsType<ViewResult>(result);
        Models.ViewModels.SearchResults viewModel = Assert.IsType<Models.ViewModels.SearchResults>(viewResult.Model);

        viewModel.SearchItems.Should().NotBeEmpty();
        viewModel.HasResults.Should().BeTrue();
        viewModel.SearchResultsCount.Should().Be(stubSearchResults.Establishments.Establishments.Count);
    }

    [Fact]
    public async Task Index_WithNoResults_ReturnsNoSearchResultsOnModel()
    {
        // arrange
        SearchForEstablishments.Models.SearchResults stubSearchResults = new() {
            Establishments = EstablishmentResultsTestDouble.CreateWithNoResults() };

        ISearchServiceAdapter mockSearchServiceAdapter =
             SearchServiceAdapterTestDouble.MockFor(stubSearchResults).Object;

        SearchByKeywordUseCase useCase =
            new (mockSearchServiceAdapter,
                IOptionsTestDouble.IOptionsMockFor(SearchByKeywordCriteriaTestDouble.Create()));

        HomeController controller =
            new(_logger.Object, useCase,
                new SearchResultsFactory(
                    new EstablishmentResultsToEstablishmentsViewModelMapper(),
                    new FacetsAndSelectedFacetsToFacetsViewModelMapper()
                ),
                new SelectedFacetsToFilterRequestsMapper());

        // act
        IActionResult result = await controller.Index(new SearchRequest() { SearchKeyword = "searchTerm" });

        // assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var viewModel = Assert.IsType<Models.ViewModels.SearchResults>(viewResult.Model);

        viewModel.SearchItems.Should().BeNull();
        viewModel.HasResults.Should().BeFalse();
        viewModel.SearchResultsCount.Should().Be(0);
    }

    [Fact]
    public async Task SearchWithFilters_WithSearchRequestAndNoMatchingSelectedFacets_ReturnsModelNoFacetsSelected()
    {
        // arrange
        SearchForEstablishments.Models.SearchResults stubSearchResults =
            new () {
                Establishments = EstablishmentResultsTestDouble.Create(),
                Facets = EstablishmentFacetsTestDouble.Create()
            };

        ISearchServiceAdapter mockSearchServiceAdapter =
             SearchServiceAdapterTestDouble.MockFor(stubSearchResults).Object;

        SearchByKeywordUseCase useCase =
            new (mockSearchServiceAdapter,
                IOptionsTestDouble.IOptionsMockFor(SearchByKeywordCriteriaTestDouble.Create()));
        
        var controller =
            new HomeController(_logger.Object, useCase,
            new SearchResultsFactory(
                    new EstablishmentResultsToEstablishmentsViewModelMapper(),
                    new FacetsAndSelectedFacetsToFacetsViewModelMapper()
            ),
            new SelectedFacetsToFilterRequestsMapper());

        // act
        IActionResult result =
            await controller.Index(
                new SearchRequest()
                {
                    SearchKeyword = "searchTerm",
                    SelectedFacets = new Dictionary<string, List<string>> { { "Facet_1", [] } }
                });

        // assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var viewModel = Assert.IsType<Models.ViewModels.SearchResults>(viewResult.Model);
        viewModel.SearchItems.Should().NotBeEmpty();
        viewModel.HasResults.Should().BeTrue();
        viewModel.SearchResultsCount.Should().Be(stubSearchResults.Establishments.Establishments.Count);
        viewModel.Facets.Should().NotContain(facet => facet.Values.Any(facetValue => facetValue.IsSelected));
    }

    [Fact]
    public async Task SearchWithFilters_WithSearchRequestAndNoSelectedFacets_ReturnsModelNoFacetsSelected()
    {
        // arrange
        const string FacetValueKey = "Facet_1";

        SearchForEstablishments.Models.SearchResults stubSearchResults =
            new(){
                Establishments = EstablishmentResultsTestDouble.Create(),
                Facets = EstablishmentFacetsTestDouble.CreateWithNoResults()
            };

        ISearchServiceAdapter mockSearchServiceAdapter =
             SearchServiceAdapterTestDouble.MockFor(stubSearchResults).Object;

        SearchByKeywordUseCase useCase =
            new (mockSearchServiceAdapter,
                IOptionsTestDouble.IOptionsMockFor(SearchByKeywordCriteriaTestDouble.Create()));

        HomeController controller =
            new (_logger.Object, useCase,
                new SearchResultsFactory(
                    new EstablishmentResultsToEstablishmentsViewModelMapper(),
                    new FacetsAndSelectedFacetsToFacetsViewModelMapper()
                ),
                new SelectedFacetsToFilterRequestsMapper());

        // act
        IActionResult result =
            await controller.Index(
                new SearchRequest()
                {
                    SearchKeyword = "searchTerm",
                    SelectedFacets = new Dictionary<string, List<string>> { { FacetValueKey, [FacetValueKey] } }
                });

        // assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var viewModel = Assert.IsType<Models.ViewModels.SearchResults>(viewResult.Model);
        viewModel.SearchItems.Should().NotBeEmpty();
        viewModel.HasResults.Should().BeTrue();
        viewModel.SearchResultsCount.Should().Be(stubSearchResults.Establishments.Establishments.Count);
        viewModel.Facets.Should().NotContain(facet => facet.Values.Any(facetValue => facetValue.IsSelected));
    }

    [Fact]
    public async Task SearchWithFilters_WithSearchRequestAndMatchingSelectedFacets_ReturnsModelWithMatchingFacetSelected()
{
        // arrange
        const string FacetValueKey = "Facet_1";

        EstablishmentFacet testEstablishmentFacet =
            EstablishmentFacetTestDouble.CreateWith(
                facetName: FacetValueKey, facetResultValue: FacetValueKey, facetResultCount: 1);

        SearchForEstablishments.Models.SearchResults stubSearchResults =
            new(){
                Establishments = EstablishmentResultsTestDouble.Create(),
                Facets = EstablishmentFacetsTestDouble.CreateWith([testEstablishmentFacet])
            };

        ISearchServiceAdapter mockSearchServiceAdapter =
             SearchServiceAdapterTestDouble.MockFor(stubSearchResults).Object;

        SearchByKeywordUseCase useCase = new(
            mockSearchServiceAdapter,
            IOptionsTestDouble.IOptionsMockFor(SearchByKeywordCriteriaTestDouble.Create()));

        var controller =
            new HomeController(_logger.Object, useCase,
            new SearchResultsFactory(
                    new EstablishmentResultsToEstablishmentsViewModelMapper(),
                    new FacetsAndSelectedFacetsToFacetsViewModelMapper()
                ),
            new SelectedFacetsToFilterRequestsMapper());

        // act
        IActionResult result =
            await controller.Index(
                new SearchRequest()
                {
                    SearchKeyword = "searchTerm",
                    SelectedFacets = new Dictionary<string, List<string>> { { FacetValueKey, [FacetValueKey] } }
                });

        // assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var viewModel = Assert.IsType<Models.ViewModels.SearchResults>(viewResult.Model);
        
        viewModel.SearchItems.Should().NotBeEmpty();
        viewModel.HasResults.Should().BeTrue();
        viewModel.SearchResultsCount.Should().Be(stubSearchResults.Establishments.Establishments.Count);
        viewModel.Facets.Should().Contain(facet => facet.Values.Any(facetValue => facetValue.IsSelected));
    }
}
