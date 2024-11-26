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
using ViewModels = Dfe.Data.SearchPrototype.Web.Models.ViewModels;
using Moq;
using Xunit;

namespace Dfe.Data.SearchPrototype.Web.Tests.Unit.Controllers;

public class HomeControllerTests
{
    [Fact]
    public async Task Index_SearchKeyword_CallUseCase()
    {
        Mock<ILogger<HomeController>> mockLogger = LoggerTestDouble.MockLogger();

        Mock<ISearchResultsFactory> mockSearchResultsFactory = SearchResultsFactoryTestDouble.MockFor(new ViewModels.SearchResults());

        Mock<IMapper<Dictionary<string, List<string>>?, IList<FilterRequest>>> mockRequestMapper =
            ViewModelSelectedFacetsToFilterRequestMapperTestDouble.MockFor([]);

        SearchByKeywordResponse response = new(status: SearchResponseStatus.Success)
        {
            EstablishmentResults = new EstablishmentResults([])
        };

        IUseCase<SearchByKeywordRequest, SearchByKeywordResponse> mockUseCase =
            new SearchByKeywordUseCaseMockBuilder().WithHandleRequestReturnValue(response).Create();

        HomeController controller =
            new(mockLogger.Object, mockUseCase,
                mockSearchResultsFactory.Object,
                mockRequestMapper.Object);

        await controller.Index(new SearchRequest() { SearchKeyword = "KDM" });

        Mock.Get(mockUseCase).Verify(useCase => useCase.HandleRequest(It.IsAny<SearchByKeywordRequest>()), Times.Once());
    }

    [Fact]
    public async Task Index_SearchKeyword_CallsViewModelFactory()
    {
        Mock<ILogger<HomeController>> mockLogger = LoggerTestDouble.MockLogger();

        Mock<ISearchResultsFactory> mockSearchResultsFactory = SearchResultsFactoryTestDouble.MockFor(new ViewModels.SearchResults());

        Mock<IMapper<Dictionary<string, List<string>>?, IList<FilterRequest>>> mockRequestMapper =
            ViewModelSelectedFacetsToFilterRequestMapperTestDouble.MockFor([]);

        SearchByKeywordResponse response =
            new(status: SearchResponseStatus.Success) { EstablishmentResults = new([]) };

        IUseCase<SearchByKeywordRequest, SearchByKeywordResponse> mockUseCase =
            new SearchByKeywordUseCaseMockBuilder().WithHandleRequestReturnValue(response).Create();

        HomeController controller =
            new(mockLogger.Object, mockUseCase,
               mockSearchResultsFactory.Object,
                mockRequestMapper.Object);

        await controller.Index(new SearchRequest() { SearchKeyword = "KDM" });

        mockSearchResultsFactory.Verify(factory =>
            factory.CreateViewModel(
                It.IsAny<EstablishmentResults?>(),
                It.IsAny<FacetsAndSelectedFacets>(),
                It.IsAny<int>(),
                It.IsAny<int>()), Times.Once());
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

        Mock<ISearchResultsFactory> mockSearchResultsFactory = SearchResultsFactoryTestDouble.MockFor(new ViewModels.SearchResults());

        Mock<IMapper<Dictionary<string, List<string>>?, IList<FilterRequest>>> mockRequestMapper =
            ViewModelSelectedFacetsToFilterRequestMapperTestDouble.MockFor([]);

        SearchRequest searchRequest =
            new()
            {
                SelectedFacets = new Dictionary<string, List<string>>() {
                    { "Facet_1", ["Facet_1_Value"] }
                }
            };

        HomeController controller =
            new(mockLogger.Object, mockUseCase,
                mockSearchResultsFactory.Object,
                mockRequestMapper.Object);

        // act
        IActionResult result = await controller.Index(searchRequest);

        // assert
        ViewResult viewResult = Assert.IsType<ViewResult>(result);
        viewResult.Model.Should().BeNull();
    }
}
