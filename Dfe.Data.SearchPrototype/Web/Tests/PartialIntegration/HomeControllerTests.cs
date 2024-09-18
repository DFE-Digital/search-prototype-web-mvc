using Dfe.Data.SearchPrototype.Infrastructure.Tests.TestDoubles.Shared;
using Dfe.Data.SearchPrototype.SearchForEstablishments.ByKeyword.ServiceAdapters;
using Dfe.Data.SearchPrototype.SearchForEstablishments.ByKeyword.Usecase;
using Dfe.Data.SearchPrototype.Tests.SearchForEstablishments.TestDoubles;
using Dfe.Data.SearchPrototype.Web.Controllers;
using Dfe.Data.SearchPrototype.Web.Mappers;
using Dfe.Data.SearchPrototype.Web.Tests.PartialIntegration.TestDoubles;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.TestDoubles;
using Dfe.Data.SearchPrototype.Web.ViewModels;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Dfe.Data.SearchPrototype.Web.Tests.PartialIntegration;

public class HomeControllerTests
{
    private readonly Mock<ILogger<HomeController>> _logger = new();
    private readonly Mock<ISearchServiceAdapter> _searchServiceAdapterMock = new();

    [Fact]
    public async Task Index_WithSearchTerm_ReturnsModel()
    {
        // arrange
        var stubSearchResults = new SearchForEstablishments.Models.SearchResults() { Establishments = EstablishmentResultsTestDouble.Create() };
        
        _searchServiceAdapterMock.Setup(adapter => adapter.SearchAsync(It.IsAny<SearchServiceAdapterRequest>()))
            .ReturnsAsync(stubSearchResults);
        var useCase = new SearchByKeywordUseCase(
            _searchServiceAdapterMock.Object,
            IOptionsTestDouble.IOptionsMockFor(SearchByKeywordCriteriaTestDouble.Create()));
        
        var controller =
            new HomeController(_logger.Object, useCase,
            new EstablishmentResultsToEstablishmentsViewModelMapper(),
            new EstablishmentFacetsToFacetsViewModelMapper(),
            new ViewModelSelectedFacetsToFilterRequestMapper());

        // act
        IActionResult result = await controller.Index("searchTerm");

        // assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var viewModel = Assert.IsType<SearchResults>(viewResult.Model);

        viewModel.SearchItems.Should().NotBeEmpty();
        viewModel.HasResults.Should().BeTrue();
        viewModel.SearchResultsCount.Should().Be(stubSearchResults.Establishments.Establishments.Count);
    }

    [Fact]
    public async Task Index_WithNoResults_ReturnsNoSearchResultsOnModel()
    {
        // arrange
        var stubSearchResults = new SearchForEstablishments.Models.SearchResults() { Establishments = EstablishmentResultsTestDouble.CreateWithNoResults() };
        
        _searchServiceAdapterMock.Setup(adapter => adapter.SearchAsync(It.IsAny<SearchServiceAdapterRequest>()))
            .ReturnsAsync(stubSearchResults);
        
        var useCase = new SearchByKeywordUseCase(
            _searchServiceAdapterMock.Object,
            IOptionsTestDouble.IOptionsMockFor(SearchByKeywordCriteriaTestDouble.Create()));

        var controller =
            new HomeController(_logger.Object, useCase,
            new EstablishmentResultsToEstablishmentsViewModelMapper(),
            new EstablishmentFacetsToFacetsViewModelMapper(),
            new ViewModelSelectedFacetsToFilterRequestMapper());

        // act
        IActionResult result = await controller.Index("searchTerm");

        // assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var viewModel = Assert.IsType<SearchResults>(viewResult.Model);

        viewModel.SearchItems.Should().BeEmpty();
        viewModel.HasResults.Should().BeFalse();
        viewModel.SearchResultsCount.Should().Be(0);
    }

    [Fact]
    public async Task SearchWithFilters_WithSearchRequestAndNoMatchingSelectedFacets_ReturnsModelNoFacetsSelected()
    {
        // arrange
        var stubSearchResults =
            new SearchForEstablishments.Models.SearchResults() {
                Establishments = EstablishmentResultsTestDouble.Create(),
                Facets = EstablishmentFacetsTestDouble.Create()
            };
        
        _searchServiceAdapterMock.Setup(adapter => adapter.SearchAsync(It.IsAny<SearchServiceAdapterRequest>()))
            .ReturnsAsync(stubSearchResults);
        
        var useCase = new SearchByKeywordUseCase(
            _searchServiceAdapterMock.Object,
            IOptionsTestDouble.IOptionsMockFor(SearchByKeywordCriteriaTestDouble.Create()));
        
        var controller =
            new HomeController(_logger.Object, useCase,
            new EstablishmentResultsToEstablishmentsViewModelMapper(),
            new EstablishmentFacetsToFacetsViewModelMapper(),
            new ViewModelSelectedFacetsToFilterRequestMapper());

        // act
        IActionResult result =
            await controller.SearchWithFilters(
                new SearchRequest()
                {
                    SearchKeyword = "searchTerm",
                    SelectedFacets = new Dictionary<string, List<string>> { { "Facet_1", [] } }
                });

        // assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var viewModel = Assert.IsType<SearchResults>(viewResult.Model);
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

        var stubSearchResults =
            new SearchForEstablishments.Models.SearchResults()
            {
                Establishments = EstablishmentResultsTestDouble.Create(),
                Facets = EstablishmentFacetsTestDouble.CreateWithNoResults()
            };

        _searchServiceAdapterMock.Setup(adapter => adapter.SearchAsync(It.IsAny<SearchServiceAdapterRequest>()))
            .ReturnsAsync(stubSearchResults);

        var useCase = new SearchByKeywordUseCase(
            _searchServiceAdapterMock.Object,
            IOptionsTestDouble.IOptionsMockFor(SearchByKeywordCriteriaTestDouble.Create()));

        var controller =
            new HomeController(_logger.Object, useCase,
            new EstablishmentResultsToEstablishmentsViewModelMapper(),
            new EstablishmentFacetsToFacetsViewModelMapper(),
            new ViewModelSelectedFacetsToFilterRequestMapper());

        // act
        IActionResult result =
            await controller.SearchWithFilters(
                new SearchRequest()
                {
                    SearchKeyword = "searchTerm",
                    SelectedFacets = new Dictionary<string, List<string>> { { FacetValueKey, [FacetValueKey] } }
                });

        // assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var viewModel = Assert.IsType<SearchResults>(viewResult.Model);
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

        var testEstablishmentFacet =
            EstablishmentFacetTestDouble.CreateWith(
                facetName: FacetValueKey, facetResultValue: FacetValueKey, facetResultCount: 1);

        var stubSearchResults =
            new SearchForEstablishments.Models.SearchResults()
            {
                Establishments = EstablishmentResultsTestDouble.Create(),
                Facets = EstablishmentFacetsTestDouble.CreateWith([testEstablishmentFacet])
            };

        _searchServiceAdapterMock.Setup(adapter => adapter.SearchAsync(It.IsAny<SearchServiceAdapterRequest>()))
            .ReturnsAsync(stubSearchResults);

        var useCase = new SearchByKeywordUseCase(
            _searchServiceAdapterMock.Object,
            IOptionsTestDouble.IOptionsMockFor(SearchByKeywordCriteriaTestDouble.Create()));

        var controller =
            new HomeController(_logger.Object, useCase,
            new EstablishmentResultsToEstablishmentsViewModelMapper(),
            new EstablishmentFacetsToFacetsViewModelMapper(),
            new ViewModelSelectedFacetsToFilterRequestMapper());

        // act
        IActionResult result =
            await controller.SearchWithFilters(
                new SearchRequest()
                {
                    SearchKeyword = "searchTerm",
                    SelectedFacets = new Dictionary<string, List<string>> { { FacetValueKey, [FacetValueKey] } }
                });

        // assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var viewModel = Assert.IsType<SearchResults>(viewResult.Model);
        
        viewModel.SearchItems.Should().NotBeEmpty();
        viewModel.HasResults.Should().BeTrue();
        viewModel.SearchResultsCount.Should().Be(stubSearchResults.Establishments.Establishments.Count);
        viewModel.Facets.Should().Contain(facet => facet.Values.Any(facetValue => facetValue.IsSelected));
    }
}
