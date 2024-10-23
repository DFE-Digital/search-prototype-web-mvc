using Dfe.Data.SearchPrototype.Common.CleanArchitecture.Application.UseCase;
using Dfe.Data.SearchPrototype.Common.Mappers;
using Dfe.Data.SearchPrototype.SearchForEstablishments.ByKeyword.Usecase;
using Dfe.Data.SearchPrototype.WebApi.Controllers;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Dfe.Data.SearchPrototype.Web.Tests.APITests.Unit;

public class EstablishmentsControllerTests
{
    private readonly ILogger<EstablishmentsController> _logger 
        = new Mock<ILogger<EstablishmentsController>>().Object;
    private readonly IUseCase<SearchByKeywordRequest, SearchByKeywordResponse> _searchByKeywordUseCase 
        = new Mock<IUseCase<SearchByKeywordRequest, SearchByKeywordResponse>>().Object;
    private readonly IMapper<SearchRequest, IList<FilterRequest>> _searchRequestToFilterRequestsMapper 
        = new Mock<IMapper<SearchRequest, IList<FilterRequest>>>().Object;

    [Fact]
    public async Task GetEstablishments_CallsUseCase()
    {
        // arrange
        SearchRequest searchRequest = new()
        {
            SearchKeyword = "keyword",
            EstablishmentStatus = new List<string> { "status1" }
        };
        var controller = new EstablishmentsController(_logger, _searchByKeywordUseCase, _searchRequestToFilterRequestsMapper);
        SearchByKeywordRequest? capturedUseCaseRequest = default;

        Mock.Get(_searchByKeywordUseCase)
            .Setup(usecase => usecase.HandleRequest(It.IsAny<SearchByKeywordRequest>()))
            .Callback<SearchByKeywordRequest>((request) => capturedUseCaseRequest = request);
        
        // act
        var response = await controller.GetEstablishments(searchRequest);

        // assert
        capturedUseCaseRequest!.SearchKeyword.Should().Be(searchRequest.SearchKeyword);
    }

    [Fact]
    public async Task GetEstablishments_CallsFiltersMapper()
    {
        // arrange
        SearchRequest searchRequest = new()
        {
            SearchKeyword = "keyword",
            EstablishmentStatus = new List<string> { "status1" }
        };

        var controller = new EstablishmentsController(_logger, _searchByKeywordUseCase, _searchRequestToFilterRequestsMapper);
        SearchRequest? capturedMapperRequest = default;

        Mock.Get(_searchRequestToFilterRequestsMapper)
            .Setup(usecase => usecase.MapFrom(It.IsAny<SearchRequest>()))
            .Callback<SearchRequest>((request) => capturedMapperRequest = request);

        // act
        var response = await controller.GetEstablishments(searchRequest);

        // assert
        capturedMapperRequest!.SearchKeyword.Should().Be(searchRequest.SearchKeyword);
        capturedMapperRequest!.EstablishmentStatus!.Should().BeEquivalentTo(searchRequest.EstablishmentStatus);
        capturedMapperRequest!.PhaseOfEducation.Should().BeNull();
    }
}
