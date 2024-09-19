﻿using Dfe.Data.SearchPrototype.Common.CleanArchitecture.Application.UseCase;
using Dfe.Data.SearchPrototype.Common.Mappers;
using Dfe.Data.SearchPrototype.SearchForEstablishments.ByKeyword.Usecase;
using Dfe.Data.SearchPrototype.SearchForEstablishments.Models;
using Dfe.Data.SearchPrototype.Web.Controllers;
using Dfe.Data.SearchPrototype.Web.Mappers;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.TestDoubles;
using Dfe.Data.SearchPrototype.Web.Tests.Unit.TestDoubles;
using Dfe.Data.SearchPrototype.Web.ViewModels;
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

        Mock<IMapper<EstablishmentResults?, List<ViewModels.Establishment>?>> mockEstablishmentResultsToEstablishmentsViewModelMapper =
            EstablishmentResultsToEstablishmentsViewModelMapperTestDouble.MockFor([]);

        Mock<IMapper<EstablishmentFacetsMapperRequest, List<Facet>?>> mockEstablishmentFacetsToFacetsViewModelMapper =
            EstablishmentFacetsToFacetsViewModelMapperTestDouble.MockFor([]);

        Mock<IMapper<Dictionary<string, List<string>>, IList<FilterRequest>>> mockRequestMapper =
            ViewModelSelectedFacetsToFilterRequestMapperTestDouble.MockFor([]);

        SearchByKeywordResponse response = new(status: SearchResponseStatus.Success) {EstablishmentResults =
            new EstablishmentResults([])};

        IUseCase<SearchByKeywordRequest, SearchByKeywordResponse> mockUseCase =
            new SearchByKeywordUseCaseMockBuilder().WithHandleRequestReturnValue(response).Create();

        HomeController controller =
            new(mockLogger.Object, mockUseCase,
                mockEstablishmentResultsToEstablishmentsViewModelMapper.Object,
                mockEstablishmentFacetsToFacetsViewModelMapper.Object,
                mockRequestMapper.Object);

        await controller.Index("KDM");

        Mock.Get(mockUseCase).Verify(useCase => useCase.HandleRequest(It.IsAny<SearchByKeywordRequest>()), Times.Once());
    }

    [Fact]
    public async Task Index_SearchKeyword_CallMapper()
    {
        Mock<ILogger<HomeController>> mockLogger = LoggerTestDouble.MockLogger();

        Mock<IMapper<EstablishmentResults?, List<ViewModels.Establishment>?>> mockEstablishmentResultsToEstablishmentsViewModelMapper =
           EstablishmentResultsToEstablishmentsViewModelMapperTestDouble.MockFor([]);

        Mock<IMapper<EstablishmentFacetsMapperRequest, List<Facet>?>> mockEstablishmentFacetsToFacetsViewModelMapper =
            EstablishmentFacetsToFacetsViewModelMapperTestDouble.MockFor([]);

        Mock<IMapper<Dictionary<string, List<string>>, IList<FilterRequest>>> mockRequestMapper =
            ViewModelSelectedFacetsToFilterRequestMapperTestDouble.MockFor([]);

        SearchByKeywordResponse response =
            new(status: SearchResponseStatus.Success) {EstablishmentResults = new([]) };

        IUseCase<SearchByKeywordRequest, SearchByKeywordResponse> mockUseCase =
            new SearchByKeywordUseCaseMockBuilder().WithHandleRequestReturnValue(response).Create();

        HomeController controller =
            new(mockLogger.Object, mockUseCase,
                mockEstablishmentResultsToEstablishmentsViewModelMapper.Object,
                mockEstablishmentFacetsToFacetsViewModelMapper.Object,
                mockRequestMapper.Object);

        await controller.Index("KDM");

        mockEstablishmentResultsToEstablishmentsViewModelMapper.Verify(mapper => mapper.MapFrom(It.IsAny<EstablishmentResults?>()), Times.Once());
    }

    [Fact]
    public async Task Index_NoSearchKeyword_NullViewModel()
    {
        // arrange
        Mock<ILogger<HomeController>> mockLogger = LoggerTestDouble.MockLogger();

        IUseCase<SearchByKeywordRequest, SearchByKeywordResponse> mockUseCase =
            new SearchByKeywordUseCaseMockBuilder().Create();

        Mock<IMapper<EstablishmentResults?, List<ViewModels.Establishment>?>> mockEstablishmentResultsToEstablishmentsViewModelMapper =
            EstablishmentResultsToEstablishmentsViewModelMapperTestDouble.MockFor([]);

        Mock<IMapper<EstablishmentFacetsMapperRequest, List<Facet>?>> mockEstablishmentFacetsToFacetsViewModelMapper =
            EstablishmentFacetsToFacetsViewModelMapperTestDouble.MockFor([]);

        Mock<IMapper<Dictionary<string, List<string>>, IList<FilterRequest>>> mockRequestMapper =
            ViewModelSelectedFacetsToFilterRequestMapperTestDouble.MockFor([]);

        //act
        HomeController controller =
            new(mockLogger.Object, mockUseCase,
                mockEstablishmentResultsToEstablishmentsViewModelMapper.Object,
                mockEstablishmentFacetsToFacetsViewModelMapper.Object,
                mockRequestMapper.Object);

        IActionResult result = await controller.Index(null!);

        // assert
        result.Should().NotBeNull();
        ViewResult viewResult = Assert.IsType<ViewResult>(result);
        viewResult.Model.Should().BeNull();
    }
}
