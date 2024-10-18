using Dfe.Data.SearchPrototype.Common.CleanArchitecture.Application.UseCase;
using Dfe.Data.SearchPrototype.Common.Mappers;
using Dfe.Data.SearchPrototype.SearchForEstablishments.ByKeyword.Usecase;
using Dfe.Data.SearchPrototype.WebApi.Controllers;
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
        SearchByKeywordRequest? capturedUseCaseRequest;
        // act
        var response = controller.GetEstablishments(searchRequest);

        Mock.Get(_searchByKeywordUseCase)
            .Setup(usecase => usecase.HandleRequest(It.IsAny<SearchByKeywordRequest>()))
            .Callback(x => capturedUseCaseRequest = x);
    }

    [Fact]
    public void GetEstablishments_CallsFiltersMapper()
    {

    }
}
