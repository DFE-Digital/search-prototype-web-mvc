using Dfe.Data.SearchPrototype.Common.CleanArchitecture.Application.UseCase;
using Dfe.Data.SearchPrototype.SearchForEstablishments;
using Moq;

namespace Dfe.Data.SearchPrototype.Web.Tests.Shared.TestDoubles;

public class SearchByKeywordUseCaseMockBuilder
{
    private Mock<IUseCase<SearchByKeywordRequest, SearchByKeywordResponse>> _usecase = new ();

    public SearchByKeywordUseCaseMockBuilder WithHandleRequestReturnValue(SearchByKeywordResponse response)
    {
        _usecase.Setup(x => x.HandleRequest(It.IsAny<SearchByKeywordRequest>()))
            .ReturnsAsync(response);
        return this;
    }

    public IUseCase<SearchByKeywordRequest, SearchByKeywordResponse> Create()
    {
        return _usecase.Object;
    }
}
