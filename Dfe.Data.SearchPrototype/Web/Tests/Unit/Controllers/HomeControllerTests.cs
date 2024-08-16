using Dfe.Data.SearchPrototype.Common.CleanArchitecture.Application.UseCase;
using Dfe.Data.SearchPrototype.Common.Mappers;
using Dfe.Data.SearchPrototype.SearchForEstablishments;
using Dfe.Data.SearchPrototype.SearchForEstablishments.Models;
using Dfe.Data.SearchPrototype.Web.Controllers;
using Dfe.Data.SearchPrototype.Web.Models;
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
        Mock<IMapper<SearchByKeywordResponse, SearchResultsViewModel>> mockMapper =
            SearchResultsToViewModelMapperTestDouble.MockFor(new SearchResultsViewModel());
        SearchByKeywordResponse response = new(new List<Establishment>().AsReadOnly());
        Mock<IUseCase<SearchByKeywordRequest, SearchByKeywordResponse>> mockUseCase =
            SearchByKeywordUseCaseTestDouble.MockFor(response);

        HomeController controller = new(mockLogger.Object, mockUseCase.Object, mockMapper.Object);

        IActionResult result = await controller.Index("KDM");

        mockUseCase.Verify(useCase => useCase.HandleRequest(It.IsAny<SearchByKeywordRequest>()), Times.Once());
    }

    [Fact]
    public async Task Index_SearchKeyword_CallMapper()
    {
        Mock<ILogger<HomeController>> mockLogger = LoggerTestDouble.MockLogger();
        Mock<IMapper<SearchByKeywordResponse, SearchResultsViewModel>> mockMapper =
            SearchResultsToViewModelMapperTestDouble.MockFor(new SearchResultsViewModel());
        SearchByKeywordResponse response = new(new List<Establishment>().AsReadOnly());
        Mock<IUseCase<SearchByKeywordRequest, SearchByKeywordResponse>> mockUseCase =
            SearchByKeywordUseCaseTestDouble.MockFor(response);

        HomeController controller = new(mockLogger.Object, mockUseCase.Object, mockMapper.Object);

        IActionResult result = await controller.Index("KDM");

        mockMapper.Verify(mapper => mapper.MapFrom(It.IsAny<SearchByKeywordResponse>()), Times.Once());
    }

    [Fact]
    public async Task Index_NoSearchKeyword_NullViewModel()
    {
        // arrange
        Mock<ILogger<HomeController>> mockLogger = LoggerTestDouble.MockLogger();
        Mock<IUseCase<SearchByKeywordRequest, SearchByKeywordResponse>> mockUseCase =
            SearchByKeywordUseCaseTestDouble.DefaultMock();
        Mock<IMapper<SearchByKeywordResponse, SearchResultsViewModel>> mockMapper =
            SearchResultsToViewModelMapperTestDouble.DefaultMock();

        //act
        HomeController controller = new HomeController(mockLogger.Object, mockUseCase.Object, mockMapper.Object);
        IActionResult result = await controller.Index(null!);

        // assert
        result.Should().NotBeNull();
        ViewResult viewResult = Assert.IsType<ViewResult>(result);
        viewResult.Model.Should().BeNull();
    }
}
