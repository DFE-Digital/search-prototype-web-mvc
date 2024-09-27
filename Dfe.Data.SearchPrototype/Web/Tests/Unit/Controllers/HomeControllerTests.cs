using Dfe.Data.SearchPrototype.Common.CleanArchitecture.Application.UseCase;
using Dfe.Data.SearchPrototype.Common.Mappers;
using Dfe.Data.SearchPrototype.SearchForEstablishments.ByKeyword.Usecase;
using Dfe.Data.SearchPrototype.SearchForEstablishments.Models;
using Dfe.Data.SearchPrototype.Web.Controllers;
using Dfe.Data.SearchPrototype.Web.Mappers;
using Dfe.Data.SearchPrototype.Web.Models;
using Dfe.Data.SearchPrototype.Web.Models.Factories;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.TestDoubles;
using Dfe.Data.SearchPrototype.Web.Tests.Unit.TestDoubles;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Dfe.Data.SearchPrototype.Web.Tests.Unit.Controllers;

public class HomeControllerTests
{
    [Fact]
    public async Task Index_SearchKeyword_CallUseCase()
    {
        Mock<ILogger<HomeController>> mockLogger = LoggerTestDouble.MockLogger();

        Mock<ISearchResultsFactory> mockSearchResultsFactory = SearchResultsFactoryTestDouble.MockFor(new Web.Models.SearchResults());

        Mock<IMapper<Dictionary<string, List<string>>, IList<FilterRequest>>> mockRequestMapper =
            ViewModelSelectedFacetsToFilterRequestMapperTestDouble.MockFor([]);

        SearchByKeywordResponse response = new(status: SearchResponseStatus.Success)
        {
            EstablishmentResults =
            new EstablishmentResults([])
        };

        IUseCase<SearchByKeywordRequest, SearchByKeywordResponse> mockUseCase =
            new SearchByKeywordUseCaseMockBuilder().WithHandleRequestReturnValue(response).Create();

        HomeController controller =
            new(mockLogger.Object, mockUseCase,
                mockSearchResultsFactory.Object,
                mockRequestMapper.Object);

        await controller.Index("KDM");

        Mock.Get(mockUseCase).Verify(useCase => useCase.HandleRequest(It.IsAny<SearchByKeywordRequest>()), Times.Once());
    }

    [Fact]
    public async Task Index_SearchKeyword_CallsViewModelFactory()
    {
        Mock<ILogger<HomeController>> mockLogger = LoggerTestDouble.MockLogger();

        Mock<ISearchResultsFactory> mockSearchResultsFactory = SearchResultsFactoryTestDouble.MockFor(new Web.Models.SearchResults());

        Mock<IMapper<Dictionary<string, List<string>>, IList<FilterRequest>>> mockRequestMapper =
            ViewModelSelectedFacetsToFilterRequestMapperTestDouble.MockFor([]);

        SearchByKeywordResponse response =
            new(status: SearchResponseStatus.Success) { EstablishmentResults = new([]) };

        IUseCase<SearchByKeywordRequest, SearchByKeywordResponse> mockUseCase =
            new SearchByKeywordUseCaseMockBuilder().WithHandleRequestReturnValue(response).Create();

        HomeController controller =
            new(mockLogger.Object, mockUseCase,
               mockSearchResultsFactory.Object,
                mockRequestMapper.Object);

        await controller.Index("KDM");

        mockSearchResultsFactory.Verify(factory => factory.CreateViewModel(It.IsAny<EstablishmentResults?>(), It.IsAny<FacetsAndSelectedFacets>()), Times.Once());
    }

    [Fact]
    public async Task Index_NoSearchKeyword_NullViewModel()
    {
        // arrange
        Mock<ILogger<HomeController>> mockLogger = LoggerTestDouble.MockLogger();

        IUseCase<SearchByKeywordRequest, SearchByKeywordResponse> mockUseCase =
            new SearchByKeywordUseCaseMockBuilder().Create();

        Mock<ISearchResultsFactory> mockSearchResultsFactory = SearchResultsFactoryTestDouble.MockFor(new Web.Models.SearchResults());

        Mock<IMapper<Dictionary<string, List<string>>, IList<FilterRequest>>> mockRequestMapper =
            ViewModelSelectedFacetsToFilterRequestMapperTestDouble.MockFor([]);

        //act
        HomeController controller =
            new(mockLogger.Object, mockUseCase,
                mockSearchResultsFactory.Object,
                mockRequestMapper.Object);

        IActionResult result = await controller.Index(null!);

        // assert
        result.Should().NotBeNull();
        ViewResult viewResult = Assert.IsType<ViewResult>(result);
        viewResult.Model.Should().BeNull();
    }

    [Fact]
    public async Task SearchWithFilters_NoSearckKeywordOnSearchRequest_NullViewModel()
    {
        // arrange
        Mock<ILogger<HomeController>> mockLogger = LoggerTestDouble.MockLogger();

        SearchByKeywordResponse response =
           new(status: SearchResponseStatus.Success) { EstablishmentResults = new([]) };

        IUseCase<SearchByKeywordRequest, SearchByKeywordResponse> mockUseCase =
            new SearchByKeywordUseCaseMockBuilder().WithHandleRequestReturnValue(response).Create();

        Mock<ISearchResultsFactory> mockSearchResultsFactory = SearchResultsFactoryTestDouble.MockFor(new Web.Models.SearchResults());

        Mock<IMapper<Dictionary<string, List<string>>, IList<FilterRequest>>> mockRequestMapper =
            ViewModelSelectedFacetsToFilterRequestMapperTestDouble.MockFor([]);

        SearchRequest searchRequest =
            new()
            {
                SearchKeyword = null!,
                SelectedFacets = new Dictionary<string, List<string>>() {
                    { "Facet_1", ["Facet_1_Value"] }
                }
            };

        //act
        HomeController controller =
            new(mockLogger.Object, mockUseCase,
                mockSearchResultsFactory.Object,
                mockRequestMapper.Object);

        // assert/verify
        IActionResult result = await controller.SearchWithFilters(searchRequest);

        // assert
        ViewResult viewResult = Assert.IsType<ViewResult>(result);
        viewResult.Model.Should().BeNull();
    }
}
