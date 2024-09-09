using Dfe.Data.SearchPrototype.SearchForEstablishments;
using Dfe.Data.SearchPrototype.SearchForEstablishments.Models;
using Dfe.Data.SearchPrototype.Web.Controllers;
using Dfe.Data.SearchPrototype.Web.Mappers;
using Dfe.Data.SearchPrototype.Web.Models;
using Dfe.Data.SearchPrototype.Web.Tests.PartialIntegration.TestDoubles;
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
        var stubSearchResults = new SearchResults() { Establishments = EstablishmentResultsTestDouble.Create() };
        _searchServiceAdapterMock.Setup(adapter => adapter.SearchAsync(It.IsAny<SearchContext>()))
            .ReturnsAsync(stubSearchResults);
        var useCase = new SearchByKeywordUseCase(_searchServiceAdapterMock.Object);
        var controller = new HomeController(_logger.Object, useCase, new SearchByKeywordResponseToViewModelMapper());

        // act
        IActionResult result = await controller.Index("searchTerm");

        // assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var viewModel = Assert.IsType<SearchResultsViewModel>(viewResult.Model);
        viewModel.SearchItems.Should().NotBeEmpty();
        viewModel.HasResults.Should().BeTrue();
        viewModel.SearchResultsCount.Should().Be(stubSearchResults.Establishments.Establishments.Count);
    }

    [Fact]
    public async Task Index_WithNoResults_ReturnsNoSearchResultsOnModel()
    {
        // arrange
        var stubSearchResults = new SearchResults() { Establishments = EstablishmentResultsTestDouble.CreateWithNoResults() };
        _searchServiceAdapterMock.Setup(adapter => adapter.SearchAsync(It.IsAny<SearchContext>()))
            .ReturnsAsync(stubSearchResults);
        var useCase = new SearchByKeywordUseCase(_searchServiceAdapterMock.Object);
        var controller = new HomeController(_logger.Object, useCase, new SearchByKeywordResponseToViewModelMapper());

        // act
        IActionResult result = await controller.Index("searchTerm");

        // assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var viewModel = Assert.IsType<SearchResultsViewModel>(viewResult.Model);
        viewModel.SearchItems.Should().BeEmpty();
        viewModel.HasResults.Should().BeFalse();
        viewModel.SearchResultsCount.Should().Be(0);
    }
}
