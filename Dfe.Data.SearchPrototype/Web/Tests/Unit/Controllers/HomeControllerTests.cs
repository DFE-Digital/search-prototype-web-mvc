using Dfe.Data.SearchPrototype.Common.CleanArchitecture.Application.UseCase;
using Dfe.Data.SearchPrototype.Common.Mappers;
using Dfe.Data.SearchPrototype.SearchForEstablishments.ByKeyword.Usecase;
using Dfe.Data.SearchPrototype.SearchForEstablishments.Models;
using Dfe.Data.SearchPrototype.Web.Controllers;
using Dfe.Data.SearchPrototype.Web.Models;
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
        Mock<IMapper<SearchByKeywordResponse, SearchResultsViewModel>> mockResponseMapper =
            SearchResultsToViewModelMapperTestDouble.MockFor(new SearchResultsViewModel());
        Mock<IMapper<Dictionary<string, List<string>>, IList<FilterRequest>>> mockRequestMapper =
            ViewModelSelectedFacetsToFilterRequestMapperTestDouble.MockFor(new List<FilterRequest>());

        SearchByKeywordResponse response = new(status: SearchResponseStatus.Success) {EstablishmentResults = new EstablishmentResults(new List<Establishment>())};
        IUseCase<SearchByKeywordRequest, SearchByKeywordResponse> mockUseCase =
            new SearchByKeywordUseCaseMockBuilder().WithHandleRequestReturnValue(response).Create();

        HomeController controller = new(mockLogger.Object, mockUseCase, mockResponseMapper.Object, mockRequestMapper.Object);

        IActionResult result = await controller.Index("KDM");

        Mock.Get(mockUseCase).Verify(useCase => useCase.HandleRequest(It.IsAny<SearchByKeywordRequest>()), Times.Once());
    }

    [Fact]
    public async Task Index_SearchKeyword_CallMapper()
    {
        Mock<ILogger<HomeController>> mockLogger = LoggerTestDouble.MockLogger();
        Mock<IMapper<SearchByKeywordResponse, SearchResultsViewModel>> mockResponseMapper =
            SearchResultsToViewModelMapperTestDouble.MockFor(new SearchResultsViewModel());
        Mock<IMapper<Dictionary<string, List<string>>, IList<FilterRequest>>> mockRequestMapper =
            ViewModelSelectedFacetsToFilterRequestMapperTestDouble.MockFor(new List<FilterRequest>());
        SearchByKeywordResponse response = new(status: SearchResponseStatus.Success) {EstablishmentResults = new(new List<Establishment>()) };
        IUseCase<SearchByKeywordRequest, SearchByKeywordResponse> mockUseCase =
            new SearchByKeywordUseCaseMockBuilder().WithHandleRequestReturnValue(response).Create();

        HomeController controller = new(mockLogger.Object, mockUseCase, mockResponseMapper.Object, mockRequestMapper.Object);

        IActionResult result = await controller.Index("KDM");

        mockResponseMapper.Verify(mapper => mapper.MapFrom(It.IsAny<SearchByKeywordResponse>()), Times.Once());
    }

    [Fact]
    public async Task Index_NoSearchKeyword_NullViewModel()
    {
        // arrange
        Mock<ILogger<HomeController>> mockLogger = LoggerTestDouble.MockLogger();
        IUseCase<SearchByKeywordRequest, SearchByKeywordResponse> mockUseCase =
            new SearchByKeywordUseCaseMockBuilder().Create();
        Mock<IMapper<SearchByKeywordResponse, SearchResultsViewModel>> mockResponseMapper =
            SearchResultsToViewModelMapperTestDouble.DefaultMock();
        Mock<IMapper<Dictionary<string, List<string>>, IList<FilterRequest>>> mockRequestMapper =
            ViewModelSelectedFacetsToFilterRequestMapperTestDouble.MockFor(new List<FilterRequest>());

        //act
        HomeController controller = new HomeController(mockLogger.Object, mockUseCase, mockResponseMapper.Object, mockRequestMapper.Object);
        IActionResult result = await controller.Index(null!);

        // assert
        result.Should().NotBeNull();
        ViewResult viewResult = Assert.IsType<ViewResult>(result);
        viewResult.Model.Should().BeNull();
    }
}
